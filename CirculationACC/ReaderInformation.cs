using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
namespace Circulation
{
    public partial class ReaderInformation : Form
    {
        DBWork.dbReader reader;
        Form1 f1;
        public ReaderInformation(DBWork.dbReader reader_,Form1 f1_)
        {
            InitializeComponent();
            f1 = f1_;
            reader = reader_;
            label2.Text = reader.FIO;
            MethodsForCurBase.FormTable(reader,dataGridView1);
            DisplayCommNote();
            RegInMos();
            label6.Text = "Дата последней отправки письма: " +f1.dbw.GetLastDateEmail(reader.id);
        }
        void DisplayCommNote()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..AbonementMemo where IDReader = " + reader.id;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "tmp");
            if (c == 0) return;
            textBox1.Text = DS.Tables["tmp"].Rows[0]["Comment"].ToString();
            textBox2.Text = DS.Tables["tmp"].Rows[0]["Note"].ToString();
        }
        void RegInMos()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..AbonementAdd where IDReader = " + reader.id;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            if (c == 0)
            {
                textBox3.Text = "(нет)";
                return;
            }
            object o = DS.Tables["t"].Rows[0]["RegInMoscow"];
            if (DS.Tables["t"].Rows[0]["RegInMoscow"] == DBNull.Value)
            {
                textBox3.Text = "(нет)";
                return;
            }
            textBox3.Text = ((DateTime)DS.Tables["t"].Rows[0]["RegInMoscow"]).ToString("dd.MM.yyyy");

        }
        private void button1_Click(object sender, EventArgs e)
        {
            ChangeComment f11 = new ChangeComment(reader);
            f11.ShowDialog();
            DisplayCommNote();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangeNote f12 = new ChangeNote(reader);
            f12.ShowDialog();
            DisplayCommNote();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.UpdateCommand = new SqlCommand();
            DA.UpdateCommand.Connection = Conn.ReadersCon;
            if (DA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                DA.UpdateCommand.Connection.Open();
            }
            DA.UpdateCommand.CommandText = "update Readers..AbonementAdd set RegInMoscow = null where IDReader = " + this.reader.id;
            DA.UpdateCommand.ExecuteNonQuery();
            DA.UpdateCommand.Connection.Close();
            textBox3.Text = "(нет)";
            MessageBox.Show("Дата окончания регистрации в Москве успешно сброшена!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReaderRegistrationInMoscow f23 = new ReaderRegistrationInMoscow(this.reader);
            f23.ShowDialog();
            RegInMos();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            EmailSending f24 = new EmailSending(f1, this.reader);
            if (f24.canshow)
                f24.ShowDialog();
        }
    }
    public static class MethodsForCurBase
    {
        public static string GetRightBoolValue(string value_)
        {
            string tmp = value_.ToString();
            if (value_.ToString() == "True")
            {
                return "да";
            }
            if (value_.ToString() == "False")
            {
                return "нет";
            }
            return value_;
        }
        public static string GetValueFromList(string colname, string value_)
        {
            DataSet DS = new DataSet();
            int cnt;
            switch (colname)
            {
                case "Document":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..Document where IDDocument = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameDocument"].ToString();
                    }
                case "Education":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..Education where IDEducation = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameEducation"].ToString();
                    }
                case "AcademicDegree":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..AcademicDegree where IDAcademicDegree = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameAcademicDegree"].ToString();
                    }
                case "WorkDepartment":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from BJACC..LIST_8 where ID = " + value_;
                        int c = Conn.SQLDA.Fill(DS, "tmp");
                        if (c == 0)
                        {
                            return "(нет)";
                        }
                        return DS.Tables["tmp"].Rows[0]["NAME"].ToString();
                    }
                case "EducationalInstitution":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..EducationalInstitution where IDEducationalInstitution = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameEducationalInstitution"].ToString();
                    }
                case "ClassInfringer":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..ClassInfringer where IDClassInfringer = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameClassInfringer"].ToString();
                    }
                case "InfringerEditor":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
                case "PenaltyID":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..Penalty where IDPenalty = " + value_;
                        int c = Conn.SQLDA.Fill(DS, "tmp");
                        if (c == 0)
                        {
                            return "(нет)";
                        }
                        return DS.Tables["tmp"].Rows[0]["NamePenalty"].ToString();
                    }
                case "EditorCreate":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
                case "EditorEnd":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
                case "EditorNow":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
            }
            return value_;
        }
        public static void FormTable(DBWork.dbReader reader, DataGridView dataGridView1)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = " + reader.id;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "lll");
            dataGridView1.Columns.Add("value", "");
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 296;
            dataGridView1.Columns[0].Width = 436;
            int i = 0;
            Dictionary<string, string> FieldsCaptions = new Dictionary<string, string>();
            Conn.SQLDA.SelectCommand.CommandText = "      USE Readers;  " +
                                                   "SELECT " +
                                                   "             [Table Name] = OBJECT_NAME(c.object_id),  " +
                                                   "             [Column Name] = c.name,  " +
                                                   "             [Description] = ex.value   " +
                                                   "       FROM   " +
                                                   "             sys.columns c   " +
                                                   "       LEFT OUTER JOIN   " +
                                                   "             sys.extended_properties ex   " +
                                                   "       ON   " +
                                                   "             ex.major_id = c.object_id  " +
                                                   "             AND ex.minor_id = c.column_id   " +
                                                   "             AND ex.name = 'MS_Description'   " +
                                                   "       WHERE   " +
                                                   "             OBJECTPROPERTY(c.object_id, 'IsMsShipped')=0   " +
                                                   "             AND OBJECT_NAME(c.object_id) = 'Main' " +
                                                   "       ORDER  " +
                                                   "             BY OBJECT_NAME(c.object_id), c.column_id;";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "fldcap");
            foreach (DataRow r in DS.Tables["fldcap"].Rows)
            {
                FieldsCaptions.Add(r["Column Name"].ToString(), r["Description"].ToString());
            }
            foreach (DataColumn col in DS.Tables["lll"].Columns)
            {
                if ((col.ColumnName == "AbonementType") || (col.ColumnName == "SheetWithoutCard") || (col.ColumnName == "Password") || (col.ColumnName == "FamilyNameFind") || (col.ColumnName == "NameFind") || (col.ColumnName == "FatherNameFind") || (col.ColumnName == "Interest"))
                {
                    continue;
                }
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = FieldsCaptions[col.ColumnName];
                string value = DS.Tables["lll"].Rows[0][col].ToString();
                value = MethodsForCurBase.GetValueFromList(col.ColumnName, value);
                value = MethodsForCurBase.GetRightBoolValue(value);
                if (DS.Tables["lll"].Rows[0][col].GetType() == typeof(DateTime))
                {
                    value = ((DateTime)DS.Tables["lll"].Rows[0][col]).ToShortDateString();
                }
                dataGridView1.Rows[i].Cells[0].Value = value;
                i++;
            }
            Conn.SQLDA.SelectCommand.CommandText = "select B.NameInterest intr from Readers..Interest A inner join Readers..InterestList B on A.IDInterest = B.IDInterest where IDReader = " + reader.id;
            Conn.SQLDA.Fill(DS, "itrs");
            foreach (DataRow r in DS.Tables["itrs"].Rows)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = "Интерес";
                dataGridView1.Rows[i].Cells[0].Value = r["intr"].ToString();
                i++;
            }
            Conn.SQLDA.SelectCommand.CommandText = "select B.NameLanguage lng from Readers..Language A inner join Readers..LanguageList B on A.IDLanguage = B.IDLanguage where IDReader = " + reader.id;
            Conn.SQLDA.Fill(DS, "lng");
            foreach (DataRow r in DS.Tables["lng"].Rows)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = "Язык";
                dataGridView1.Rows[i].Cells[0].Value = r["lng"].ToString();
                i++;
            }
        }
    }
}
