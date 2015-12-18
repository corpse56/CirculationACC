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
    public partial class EmailSending : Form
    {
        Form1 f1;
        ReaderVO reader;
        List<int> bold;
        string mailtext;
        public bool canshow = false;
        string rn = System.Environment.NewLine;
        string LiveEmail = "";
        string WorkEmail = "";
        string RegEmail = "";
        string htmltext;
        int IDISSUED_ACC;
        public EmailSending(Form1 f1_, ReaderVO reader_)
        {
            InitializeComponent();
            f1 = f1_;
            reader = reader_;
            label1.Text = reader.FIO;
            int rownum = 0;
            bold = new List<int>();
            //IDISSUED_ACC = IDISS;



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
            richTextBox1.Text = "Уважаемый(ая) " + reader.Name + " " + reader.Father + "!" + rn +
                "Вы задерживаете книги:" + rn + rn;
            foreach (DataGridViewRow r in f1.Formular.Rows)
            {
                if (r.DefaultCellStyle.BackColor == Color.Tomato)
                {
                    rownum++;
                    string zag = r.Cells["tit"].Value.ToString();
                    if (zag.Length > 21)
                        zag.Remove(20);
                    TimeSpan ts = DateTime.Now.AddDays(1) - (DateTime)r.Cells["ret"].Value;
                    richTextBox1.Text += rownum.ToString() + ". " + r.Cells["avt"].Value.ToString() +
                        ", " + zag +
                        ", выдано: " + ((DateTime)r.Cells["iss"].Value).ToString("dd.MM.yyyy") +
                        ", дата возврата: ";
                    richTextBox1.Text += ((DateTime)r.Cells["ret"].Value).ToString("dd.MM.yyyy");
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
            richTextBox1.Text += rn + "Просим Вас в ближайшее время вернуть литературу в Центр американской культуры Библиотеки иностранной литературы." + rn +
                "С уважением, " + rn +
                "Центр американской культуры ВГБИЛ," + rn +
                "тел. +7 (495) 926-4554" + rn +
                "пн-пт - с 11:00 до 20:45." + rn +
                "субб - с 11:00 до 18:45";

            foreach (int i in bold)
            {
                richTextBox1.Select(i, 10);
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            }
            htmltext = EmailSending.ConvertRtfToHtml(richTextBox1.Rtf);

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
            DBGeneral dbg = new DBGeneral();
            dbg.InsertSendEmailAction(f1.EmpID,reader.ID);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
