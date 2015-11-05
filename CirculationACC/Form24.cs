using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Support;
using Itenso.Rtf;

namespace Circulation
{
    public partial class Form24 : Form
    {
        Form1 f1;
        DBWork.dbReader reader;
        List<int> bold;
        string mailtext;
        public bool canshow = false;
        string rn = System.Environment.NewLine;
        string LiveEmail = "";
        string WorkEmail = "";
        string RegEmail = "";
        string htmltext;
        public Form24(Form1 f1_, DBWork.dbReader reader_)
        {
            InitializeComponent();
            f1 = f1_;
            reader = reader_;
            label1.Text = reader.Surname + " " + reader.Name + " " + reader.SecondName;
            int rownum = 0;
            bold = new List<int>();
            



            LiveEmail = reader.GetLiveemail();
            WorkEmail = reader.GetWorkEmail();
            RegEmail = reader.GetRegEmail();
            //LiveEmail = "debarkader@gmail.com";
            //WorkEmail = "debarkader@gmail.com";

            if ((LiveEmail == "") && (WorkEmail == "") && (RegEmail == ""))
            {
                MessageBox.Show("Email не существует или имеет неверный формат!");
                this.Close();
                return;
            }
            this.canshow = true;
            richTextBox1.Text = "Уважаемый(ая) " + reader.Name + " " + reader.SecondName + "!" + rn +
                "Вы задерживаете книги:" + rn + rn;
            foreach (DataGridViewRow r in f1.Formular.Rows)
            {
                if (((bool)r.Cells[14].Value == true) && (r.Cells[12].Value.ToString() == ""))
                {
                    rownum++;
                    string zag = r.Cells[1].Value.ToString();
                    if (zag.Length > 21)
                        zag.Remove(20);
                    TimeSpan ts = DateTime.Now.AddDays(1) - (DateTime)r.Cells[11].Value;
                    richTextBox1.Text += rownum.ToString() + ". " + r.Cells[3].Value.ToString() +
                        ", " + zag +
                        ", " + r.Cells[9].Value.ToString() +
                        ", ";
                    richTextBox1.Text += ((DateTime)r.Cells[11].Value).ToString("dd.MM.yyyy");
                    bold.Add(richTextBox1.TextLength - 10);
                    richTextBox1.Text += ". Задержано на " + ts.Days.ToString() + " дней." + rn;
                }
            }
            if (rownum == 0)
            {
                MessageBox.Show("За читателем нет задоженностей!");
                this.Close();
                return;
            }
            richTextBox1.Text += rn + "Просим Вас в ближайшее время вернуть литературу в Абонемент Библиотеки иностранной литературы." + rn +
                "С уважением, " + rn +
                "Абонемент ВГБИЛ," + rn +
                "(495) 915-35-47" + rn +
                "пн-пт - 10.00-19.30" + rn +
                "субб - 10.00-17.30";

            foreach (int i in bold)
            {
                richTextBox1.Select(i, 10);
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            }
            htmltext = Form24.ConvertRtfToHtml(richTextBox1.Rtf);

        }
        private static string ConvertRtfToHtml(string rtfText)
        {
            IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc(rtfText);
            RtfHtmlConvertSettings settings = new RtfHtmlConvertSettings();
            settings.ConvertScope = RtfHtmlConvertScope.Content;

            RtfHtmlConverter htmlConverter = new RtfHtmlConverter(rtfDocument, settings);
            return htmlConverter.Convert();
        } // ConvertRtfToHtml
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            SmtpClient client = new SmtpClient("neos.libfl.ru");
            //client.EnableSsl = true;
            client.Credentials = new NetworkCredential("abonement@libfl.ru", "abonement123");
            MailAddress from = new MailAddress("abonement@libfl.ru", "ВГБИЛ", Encoding.UTF8);
            MailAddress to;
            MailMessage message = new MailMessage();
            message.From = from;
            message.IsBodyHtml = true;
            //LiveEmail = "debarkader@gmail.com";
            //WorkEmail = "debarkader@gmail.com";
            if (LiveEmail != "")
            {
                to = new MailAddress(LiveEmail);
                message.To.Add(to);
            }
            if (WorkEmail != "")
            {
                to = new MailAddress(WorkEmail);
                message.To.Add(to);
            }
            if (RegEmail != "")
            {
                to = new MailAddress(RegEmail);
                message.To.Add(to);
            }
            message.Body = htmltext;
            
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = "ВГБИЛ - Абонемент";
            message.SubjectEncoding = Encoding.UTF8;
            
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            message.Dispose();
            MessageBox.Show("Отправлено успешно!");
            f1.dbw.InsertActionEMAIL(reader);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
