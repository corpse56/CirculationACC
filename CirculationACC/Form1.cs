using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
//using Test1;
using System.Globalization;
using System.Xml;
using System.Windows.Forms.VisualStyles;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.IO;
//using Circulation.Classes;
namespace Circulation
{

    public partial class Form1 : Form
    {
        Department DEPARTMENT = new Department();

        public int EmpID;
        private Auth f2;
        private Prolong f4;
        SerialPort port;

        public ExtGui.RoundProgress RndPrg;
        public Form1()
        {

            f2 = new Auth(this);
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            f2.ShowDialog();

            this.bConfirm.Enabled = false;
            this.bCancel.Enabled = false;
            label4.Text = "������ ������� " + DateTime.Now.ToShortDateString() + ":";

            port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            port.Handshake = Handshake.RequestToSend;
            port.NewLine = Convert.ToChar(13).ToString();

            try
            {
                port.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Log();

        }
        public delegate void ScanFuncDelegate(string data);

        //public static event ScannedEventHandler Scanned;

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string FromPort = "";
            FromPort = port.ReadLine();
            FromPort = FromPort.Trim();
            port.DiscardInBuffer();
            ScanFuncDelegate ScanDelegate;
            ScanDelegate = new ScanFuncDelegate(Form1_Scanned);
            
            this.Invoke(ScanDelegate, new object[] { FromPort });
        }


        void Form1_Scanned(string fromport)
        {
            string g = tabControl1.SelectedTab.ToString();
            switch (tabControl1.SelectedTab.Text)
            {
                case "�������� ��������":
                    #region formular
                    ReaderVO reader = new ReaderVO(fromport);
                    FillFormular(reader);

                    #endregion
                    break;
                #region old_formular

                ////string _data = ((IOPOSScanner_1_10)sender).ScanData.ToString();
                //string _data = fromport;
                //if (!dbw.isReader(_data))
                //{
                //    MessageBox.Show("�������� �������� ��������!", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                ///*if (_data.Length < 20)
                //    _data = _data.Remove(0, 1);*/
                ////_data = _data.Remove(_data.Length - 1, 1);
                //ReaderRecordFormular = new DBWork.dbReader(_data);

                //if (ReaderRecordFormular.barcode == "notfoundbynumber")
                //{
                //    MessageBox.Show("�������� �� ������, ���� �������� ��������!", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //if (ReaderRecordFormular.barcode == "numsoc")
                //{
                //    MessageBox.Show("�������� �� ������, ���� �������� �������!", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //if (ReaderRecordFormular.barcode == "sersoc")
                //{
                //    MessageBox.Show("�� ������������� ����� ���������� �����!�������� ������� ���������� �����!����� ���������� ����� ������� �������, �� ��������� �����! ����� ���������� ����� ���������� ���������������� � ������������!", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //label20.Text = ReaderRecordFormular.Surname + " " + ReaderRecordFormular.Name + " " + ReaderRecordFormular.SecondName;
                ////textBox6.Text = ReaderRecordFormular.AbonType;
                //label25.Text = ReaderRecordFormular.id;

                ////dbw.SetPenalty(ReaderRecordFormular.id);
                ////this.FormularColumnsForming(ReaderRecordFormular.id);

                ///*Formular.Columns[1].Width = 100;
                //Formular.Columns[2].Visible = false;
                //Formular.Columns[4].Visible = false;
                //Formular.Columns[3].HeaderText = "�����";
                //Formular.Columns[3].Width = 90;
                //Formular.Columns[5].HeaderText = "��� �������";
                //Formular.Columns[5].Width = 110;
                //Formular.Columns[7].Visible = false;
                //Formular.Columns[6].HeaderText = "����� �������";
                //Formular.Columns[6].Width = 170;
                //Formular.Columns[8].HeaderText = "���� ������";
                //Formular.Columns[8].Width = 130;
                //Formular.Columns[9].HeaderText = "�������������� ���� ��������";
                //Formular.Columns[9].Width = 130;
                //Formular.Columns[10].HeaderText = "����������� ���� ��������";
                //Formular.Columns[10].Width = 130;
                //Formular.Columns[11].HeaderText = "���������";
                //Formular.Columns[11].Width = 130;*/


                ////Formular.Columns[8].Visible = false;
                ////Formular.Columns[9].Visible = false;
                //Sorting.WhatStat = Stats.Formular;
                //Sorting.AuthorSort = SortDir.None;
                //Sorting.ZagSort = SortDir.None;
                //break;
                #endregion

                case "����/������ �������":
                    #region priem
                    switch (DEPARTMENT.Circulate(fromport))
                    {
                        case 0:
                            DEPARTMENT.RecieveBook(EmpID);
                            CancelIssue();
                            break;
                        case 1:
                            MessageBox.Show("�������� �� ������ �� � ���� ��������� �� � ���� ����!");
                            break;
                        case 2:
                            MessageBox.Show("�������� �������� ��������, � ������ �������� �������!");
                            break;
                        case 3:
                            MessageBox.Show("�������� �������� �������, � ������ �������� ��������!");
                            break;
                        case 4:
                            lAuthor.Text = DEPARTMENT.ScannedBook.AUTHOR;
                            lTitle.Text = DEPARTMENT.ScannedBook.TITLE;
                            bCancel.Enabled = true;
                            label1.Text = "�������� �������� ��������";
                            break;
                        case 5:
                            lReader.Text = DEPARTMENT.ScannedReader.Family + " " + DEPARTMENT.ScannedReader.Name + " " + DEPARTMENT.ScannedReader.Father;
                            RPhoto.Image = DEPARTMENT.ScannedReader.Photo;
                            bConfirm.Enabled = true;
                            this.AcceptButton = bConfirm;
                            bConfirm.Focus();
                            label1.Text = "����������� ��������";
                            break;

                    }
                    Log();
                    break;
                    #endregion
                #region ���� ������������

                case "���� ������������":

                    AttendanceScan(fromport);

                    break;
                #endregion
            }
        }

        private void AttendanceScan(string fromport)
        {
            if (!ReaderVO.IsReader(fromport))
            {
                MessageBox.Show("�������� �������� ��������!", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ReaderVO reader = new ReaderVO(fromport);

            if (!reader.IsAlreadyMarked())
            {
                DEPARTMENT.AddAttendance(reader);
                label21.Text = "�� ������� ������������ ����������: " + DEPARTMENT.GetAttendance() + " �������(�)";
            }
            else
            {
                MessageBox.Show("���� �������� ��� ������� ������� ��� �������!");
                return;
            }
        }

        public void FillFormular(ReaderVO reader)
        {
            if (reader.ID == 0)
            {
                MessageBox.Show("�������� �� ������!");
                return;
            }
            FillFormularGrid(reader);

        }
        public void FillFormularGrid(ReaderVO reader)
        {
            lFormularName.Text = reader.Family + " " + reader.Name + " " + reader.Father;
            lFromularNumber.Text = reader.ID.ToString();
            Formular.DataSource = reader.GetFormular();
            Formular.Columns["num"].HeaderText = "��";
            Formular.Columns["num"].Width = 40;
            Formular.Columns["bar"].HeaderText = "��������";
            Formular.Columns["bar"].Width = 80;
            Formular.Columns["avt"].HeaderText = "�����";
            Formular.Columns["avt"].Width = 200;
            Formular.Columns["tit"].HeaderText = "��������";
            Formular.Columns["tit"].Width = 400;
            Formular.Columns["iss"].HeaderText = "���� ������";
            Formular.Columns["iss"].Width = 80;
            Formular.Columns["ret"].HeaderText = "�������������� ���� ��������";
            Formular.Columns["ret"].Width = 110;
            Formular.Columns["shifr"].HeaderText = "�������������� ����";
            Formular.Columns["shifr"].Width = 90;
            Formular.Columns["idiss"].Visible = false;
            Formular.Columns["idr"].Visible = false;
            pictureBox2.Image = reader.Photo;
            foreach (DataGridViewRow r in Formular.Rows)
            {
                DateTime ret = (DateTime)r.Cells["ret"].Value;
                if (ret < DateTime.Now)
                {
                    r.DefaultCellStyle.BackColor = Color.Tomato;
                }
            }


        }
        private void bConfirm_Click(object sender, EventArgs e)
        {
            if (DEPARTMENT.ScannedReader.IsAlreadyIssuedMoreThanFourBooks())
            {
                DialogResult res = MessageBox.Show("�������� ��� ������ ����� 4 ������������! �� ����� ������ ������?", "��������", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (res == DialogResult.No)
                {
                    CancelIssue();
                    return;
                }
            }
            switch (DEPARTMENT.ISSUE(EmpID))
            {
                case 0:
                    bConfirm.Enabled = false;
                    bCancel.Enabled = false;
                    CancelIssue();
                    Log();
                    DEPARTMENT = new Department();
                    break;
            }

        }
        private void bCancel_Click(object sender, EventArgs e)
        {
            CancelIssue();
        }
        private void CancelIssue()
        {
            this.lAuthor.Text = "";
            this.lTitle.Text = "";
            this.lReader.Text = "";
            DEPARTMENT = new Department();
            label1.Text = "�������� �������� �������";
            bConfirm.Enabled = false;
            bCancel.Enabled = false;
            RPhoto.Image = null;
        }
        private void Log()
        {
            DBGeneral dbg = new DBGeneral();

            dgvLOG.Columns.Clear();
            dgvLOG.AutoGenerateColumns = true;
            dgvLOG.DataSource = dbg.GetLog();
            dgvLOG.Columns["time"].HeaderText = "�����";
            dgvLOG.Columns["time"].Width = 80;
            dgvLOG.Columns["bar"].HeaderText = "��������";
            dgvLOG.Columns["bar"].Width = 80;
            dgvLOG.Columns["tit"].HeaderText = "�������";
            dgvLOG.Columns["tit"].Width = 600;
            dgvLOG.Columns["idr"].HeaderText = "��������";
            dgvLOG.Columns["idr"].Width = 80;
            dgvLOG.Columns["st"].HeaderText = "��������";
            dgvLOG.Columns["st"].Width = 100;
            foreach (DataGridViewColumn c in dgvLOG.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            ReaderVO reader = new ReaderVO((int)numericUpDown3.Value);
            if (reader.ID == 0)
            {
                MessageBox.Show("�������� �� ������!");
                return;
            }
            FillFormularGrid(reader);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Formular.SelectedRows.Count == 0)
            {
                MessageBox.Show("�������� ������!");
                return;
            }
            Prolong p = new Prolong();
            p.ShowDialog();
            if (p.Days == -99) return;
            DEPARTMENT.Prolong((int)Formular.SelectedRows[0].Cells["idiss"].Value, p.Days, EmpID);
            ReaderVO reader = new ReaderVO((int)Formular.SelectedRows[0].Cells["idr"].Value);
            FillFormularGrid(reader);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //BookRecordWork = new DBWork.dbBook("R00063Y0803");

            f2.textBox2.Text = "";
            f2.textBox3.Text = "";
            f2.ShowDialog();
            //if (f2.Canceled)
            //if ((this.EmpID == "") || (this.EmpID == null))
            //{
            //    MessageBox.Show("�� �� ������������! ��������� ����������� ���� ������", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Close();
            //}
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Visible = !label1.Visible;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Text)
            {
                case "����/������ �������":
                    Log();
                    //CancelIssue();
                    label1.Enabled = true;

                    //label1.Text = "�������� �������� �������";
                    break;
                case "�������":
                    label1.Enabled = false;
                    break;
                case "�������� ��������":
                    lFromularNumber.Text = "";
                    lFormularName.Text = "";
                    Formular.Columns.Clear();
                    AcceptButton = this.button10;
                    pictureBox2.Image = null;
                    break;
                case "���� ������������":
                    label21.Text = "�� ������� ������������ ����������: " + DEPARTMENT.GetAttendance() + " �������(�)";
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (f2.Canceled)
            {
                MessageBox.Show("�� �� ������������! ��������� ����������� ���� ������", "��������!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            button12.Enabled = false;
            int x = this.Left + button7.Left;
            int y = this.Top + button7.Top + tabControl1.Top + 60;
            contextMenuStrip2.Show(x, y);
        }


      


       
        public void autoinc(DataGridView dgv)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = ++i;
            }
        }
       
        private void Statistics_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (label19.Text.Contains("�������"))
                foreach (DataGridViewRow r in Statistics.Rows)
                {
                    if (r.Cells[5].Value.ToString() == "true")
                    {
                        r.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
            if (label19.Text.Contains("�������"))
                foreach (DataGridViewRow r in Statistics.Rows)
                {
                    if (r.Cells[10].Value.ToString() == "true")
                    {
                        r.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
            autoinc(Statistics);
        }

        class Span
        {
            public DateTime start;
            public DateTime end;
        }
        Span MyDateSpan;
        private void button12_Click(object sender, EventArgs e)
        {
            if (Statistics.Rows.Count == 0)
            {
                MessageBox.Show("������ ��������������!");
                return;
            }
            string strExport = "";
            //Loop through all the columns in DataGridView to Set the 
            //Column Heading
            foreach (DataGridViewColumn dc in Statistics.Columns)
            {
                strExport += dc.HeaderText.Replace(";", " ") + "  ; ";
            }
            strExport = strExport.Substring(0, strExport.Length - 3) + Environment.NewLine.ToString();
            //Loop through all the row and append the value with 3 spaces
            foreach (DataGridViewRow dr in Statistics.Rows)
            {
                foreach (DataGridViewCell dc in dr.Cells)
                {
                    if (dc.Value != null)
                    {
                        strExport += dc.FormattedValue.ToString().Replace(";", " ") + " ;  ";
                    }
                }
                strExport += Environment.NewLine.ToString();
            }
            strExport = strExport.Substring(0, strExport.Length - 3) + Environment.NewLine.ToString() + Environment.NewLine.ToString() + DateTime.Now.ToString("dd.MM.yyyy") + "  ����� ���������� " + this.EmpID + " - " + this.textBox1.Text;
            //Create a TextWrite object to write to file, select a file name with .csv extention
            string tmp = label19.Text + "_" + DateTime.Now.ToString("hh:mm:ss.nnn") + ".csv";
            tmp = label19.Text + "_" + DateTime.Now.Ticks.ToString() + ".csv";
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "��������� � ����";
            sd.Filter = "csv files (*.csv)|*.csv";
            sd.FilterIndex = 1;
            TextWriter tw;
            sd.FileName = tmp;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                tmp = sd.FileName;
                tw = new System.IO.StreamWriter(tmp, false, Encoding.UTF8);
                //Write the Text to file
                //tw.Encoding = Encoding.Unicode;
                tw.Write(strExport);
                //Close the Textwrite
                tw.Close();
            }
        }

       
        public string emul;
        public string pass;
        private void button14_Click(object sender, EventArgs e)
        {
            ParolEmulation f20 = new ParolEmulation(this);
            f20.ShowDialog();
            if (pass == "aa")
            {
                pass = "";
                Emulation f19 = new Emulation(this);
                f19.ShowDialog();
                Form1_Scanned(f19.emul);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (lFromularNumber.Text == "")
            {
                MessageBox.Show("������� ����� ��� �������� �������� ��������!");
                return;
            }
            ReaderVO reader = new ReaderVO(int.Parse(lFromularNumber.Text));
            History f7 = new History(reader);
            f7.ShowDialog();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (lFromularNumber.Text == "")
            {
                MessageBox.Show("������� ����� ��� �������� �������� ��������!");
                return;
            }
            ReaderVO reader = new ReaderVO(int.Parse(lFromularNumber.Text));
            ReaderInformation f9 = new ReaderInformation(reader, this);
            f9.ShowDialog();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //����� �������� �� �������
            FindReaderBySurname f16 = new FindReaderBySurname(this);
            f16.ShowDialog();
        }

        private void Statistics_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex == -1) return;
            if ((label19.Text.IndexOf("������ ������������ ���������� �� ������� ������") != -1))
            {
                tabControl1.SelectedIndex = 1;
                numericUpDown3.Value = int.Parse(Statistics.Rows[e.RowIndex].Cells[3].Value.ToString());
                button10_Click(sender, new EventArgs());
            }
            if (label19.Text.Contains("�������"))
            {
                tabControl1.SelectedIndex = 1;
                numericUpDown3.Value = int.Parse(Statistics.Rows[e.RowIndex].Cells[1].Value.ToString());
                button10_Click(sender, new EventArgs());
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DBReader dbr = new DBReader();
            byte[] fotka = File.ReadAllBytes("f://41_1.jpg");
            dbr.AddPhoto(fotka);
        }

        private void RPhoto_Click(object sender, EventArgs e)
        {
            ViewFullSizePhoto fullsize = new ViewFullSizePhoto(RPhoto.Image);
            fullsize.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ViewFullSizePhoto fullsize = new ViewFullSizePhoto(pictureBox2.Image);
            fullsize.ShowDialog();

        }

        private void ����������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            label19.Text = "������ �������� ���������� �� ������� ������ ";
            label18.Text = "";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetAllIssuedBook();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 270;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 140;
            Statistics.Columns[3].HeaderText = "����� ������ ������� ������";
            Statistics.Columns[3].Width = 70;
            Statistics.Columns[4].HeaderText = "�������";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "���";
            Statistics.Columns[5].Width = 90;
            Statistics.Columns[6].HeaderText = "��������";
            Statistics.Columns[6].Width = 100;
            Statistics.Columns[7].HeaderText = "��������";
            Statistics.Columns[7].Width = 80;
            Statistics.Columns[8].HeaderText = "���� ������";
            Statistics.Columns[8].ValueType = typeof(DateTime);
            Statistics.Columns[8].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[8].Width = 85;
            Statistics.Columns[9].HeaderText = "������ �������� ���� ��������";
            Statistics.Columns[9].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[9].Width = 85;
            Statistics.Columns[10].Visible = false;
            Statistics.Columns[11].HeaderText = "�������������� ����";
            Statistics.Columns[11].Width = 100;

            button12.Enabled = true;
        }

        private void �����������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            label19.Text = "������ ������������ ���������� �� ������� ������";
            label18.Text = "";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetAllOverdueBook();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 240;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 120;
            Statistics.Columns[3].HeaderText = "����� ������ ������� ������";
            Statistics.Columns[3].Width = 70;
            Statistics.Columns[4].HeaderText = "�������";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "���";
            Statistics.Columns[5].Width = 80;
            Statistics.Columns[6].HeaderText = "��������";
            Statistics.Columns[6].Width = 80;
            Statistics.Columns[7].HeaderText = "��������";
            Statistics.Columns[7].Width = 75;
            Statistics.Columns[8].HeaderText = "���� ������";
            Statistics.Columns[8].ValueType = typeof(DateTime);
            Statistics.Columns[8].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[8].Width = 85;
            Statistics.Columns[9].HeaderText = "������ �������� ���� ��������";
            Statistics.Columns[9].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[9].Width = 85;
            Statistics.Columns[10].Visible = false;
            Statistics.Columns[10].ValueType = typeof(bool);
            Statistics.Columns[11].HeaderText = "���� ��������� �������� email";
            Statistics.Columns[11].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[11].Width = 85;
            Statistics.Columns[12].HeaderText = "�������������� ����";
            Statistics.Columns[12].Width = 85;
            foreach (DataGridViewRow r in Statistics.Rows)
            {
                object value = r.Cells[10].Value;
                if (Convert.ToBoolean(value) == true)
                {
                    r.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
            button12.Enabled = true;
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (lFromularNumber.Text == "")
            {
                MessageBox.Show("������� ����� ��� �������� �������� ��������!");
                return;
            }
            ReaderVO reader = new ReaderVO(int.Parse(lFromularNumber.Text));
            EmailSending es = new EmailSending(this, reader);
            if (es.canshow)
            {
                es.ShowDialog();
            }
        }

        private void �������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            DatePeriod f3 = new DatePeriod();
            f3.ShowDialog();
            label18.Text = "";
            label19.Text = "";
            label19.Text = "������ �������� ��������� �� ������ � " + f3.StartDate.ToString("dd.MM.yyyy") + " �� " + f3.EndDate.ToString("dd.MM.yyyy") + ": ";
            DBGeneral dbg = new DBGeneral();

            try
            {
                Statistics.DataSource = dbg.GetOperatorActions(f3.StartDate, f3.EndDate, EmpID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[2].HeaderText = "����";
            Statistics.Columns[2].Width = 200;
            autoinc(Statistics);
        }

        private void �������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            DatePeriod f3 = new DatePeriod();
            f3.ShowDialog();
            label18.Text = "";
            label19.Text = "����� ������ �� ������ � " + f3.StartDate.ToString("dd.MM.yyyy") + " �� " + f3.EndDate.ToString("dd.MM.yyyy") + ": ";
            DBGeneral dbg = new DBGeneral();

            try
            {
                Statistics.DataSource = dbg.GetDepReport(f3.StartDate, f3.EndDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[1].HeaderText = "������������";
            Statistics.Columns[2].HeaderText = "����������";
            Statistics.Columns[2].Width = 200;
            autoinc(Statistics);
        }
        private void ������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            DatePeriod f3 = new DatePeriod();
            f3.ShowDialog();
            label18.Text = "";
            label19.Text = "����� �������� ��������� �� ������ � " + f3.StartDate.ToString("dd.MM.yyyy") + " �� " + f3.EndDate.ToString("dd.MM.yyyy") + ": ";
            DBGeneral dbg = new DBGeneral();

            try
            {
                Statistics.DataSource = dbg.GetOprReport(f3.StartDate, f3.EndDate, this.EmpID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[1].HeaderText = "������������";
            Statistics.Columns[2].HeaderText = "����������";
            Statistics.Columns[2].Width = 200;
            autoinc(Statistics);
        }
        private void ����������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            label19.Text = "������ ���� ���������� ������������� ������ ";
            label18.Text = "";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetAllBooks();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 70;
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 200;
            Statistics.Columns[3].HeaderText = "��������";
            Statistics.Columns[3].Width = 100;

            button12.Enabled = true;
        }

        private void ����������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            label19.Text = "������������ ���������� ������������� ������ ";
            label18.Text = "";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetBookNegotiability();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 70;
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 200;
            Statistics.Columns[3].HeaderText = "��������";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "������������";
            Statistics.Columns[4].Width = 100;

            button12.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Formular.SelectedRows.Count == 0)
            {
                MessageBox.Show("�������� ������!");
                return;
            }
            DialogResult dr = MessageBox.Show("�� ������������� ������ ����� ��������������� �� ���������� �����?", "��������!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dr == DialogResult.No) return;
            DEPARTMENT.RemoveResponsibility((int)Formular.SelectedRows[0].Cells["idiss"].Value, EmpID);
            ReaderVO reader = new ReaderVO((int)Formular.SelectedRows[0].Cells["idr"].Value);
            FillFormularGrid(reader);
        }

        private void ��������������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            label19.Text = "������������ ���������� ������������� ������ ";
            label18.Text = "";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetBooksWithRemovedResponsibility();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "��������";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[2].HeaderText = "�����";
            Statistics.Columns[2].Width = 130;
            Statistics.Columns[3].HeaderText = "����� ������ ������� ������";
            Statistics.Columns[3].Width = 70;
            Statistics.Columns[4].HeaderText = "�������";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "���";
            Statistics.Columns[5].Width = 80;
            Statistics.Columns[6].HeaderText = "��������";
            Statistics.Columns[6].Width = 80;
            Statistics.Columns[7].HeaderText = "��������";
            Statistics.Columns[7].Width = 80;
            Statistics.Columns[8].HeaderText = "���� ������";
            Statistics.Columns[8].ValueType = typeof(DateTime);
            Statistics.Columns[8].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[8].Width = 85;
            Statistics.Columns[9].HeaderText = "���� ������ ���������������";
            Statistics.Columns[9].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[9].Width = 85;
            button12.Enabled = true;
        }

        private void ����������������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            //Statistics.Columns.Add("NN", "� �/�");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            //DatePeriod f3 = new DatePeriod();
            //f3.ShowDialog();
            label19.Text = "������ ����������� ������ ����������� ";
            label18.Text = "";
            DBReference dbref = new DBReference();
            Statistics.DataSource = dbref.GetViolators();
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("��� �������� ����!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "��";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "����� ������ ������� ������";
            Statistics.Columns[1].Width = 70;
            Statistics.Columns[2].HeaderText = "�������";
            Statistics.Columns[2].Width = 120;
            Statistics.Columns[3].HeaderText = "���";
            Statistics.Columns[3].Width = 120;
            Statistics.Columns[4].HeaderText = "��������";
            Statistics.Columns[4].Width = 120;
            Statistics.Columns[5].Visible = false;
            Statistics.Columns[6].HeaderText = "���� ��������� �������� email";
            Statistics.Columns[6].Width = 150;
            button12.Enabled = true;
            foreach (DataGridViewRow r in Statistics.Rows)
            {
                object value = r.Cells[5].Value;
                if (Convert.ToBoolean(value) == true)
                {
                    r.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button14_Click(sender, e);
        }

  


    }
}
