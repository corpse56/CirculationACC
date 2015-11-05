using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Circulation
{
    public partial class Form17 : Form
    {
        Form1 f1;
        public Form17( Form1 f1_)
        {
            InitializeComponent();
            f1 = f1_;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(DelAllSpaces(maskedTextBox1.Text));
           

            if (maskedTextBox1.Text == "")
            {
                MessageBox.Show("Введите номер читателя!");
                return;
            }
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = " + DelAllSpaces(maskedTextBox1.Text);
            DataSet DS = new DataSet();
            if (Conn.SQLDA.Fill(DS, "t") == 0)
            {
                MessageBox.Show("Читатель с номером " + DelAllSpaces(maskedTextBox1.Text) + " не найден!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            f1.ReaderRecord = new DBWork.dbReader((int)DS.Tables["t"].Rows[0]["NumberReader"]);
            if (!f1.OldBase(f1.ReaderRecord))
            {
                f1.button2_Click_1(button1, new EventArgs());
                Close();                
            }
            Close();
        }
        public string DelAllSpaces(string str)
        {
            while (str.Contains(" "))
            {
                str = str.Remove(str.IndexOf(" "), 1);
            }
            return str;
        }
    }
}
