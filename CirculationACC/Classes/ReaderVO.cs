using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;

namespace Circulation
{
    public class ReaderVO
    {
        public ReaderVO() { }
        public ReaderVO(int ID)
        {
            DBReader dbr = new DBReader();
            DataRow reader = dbr.GetReaderByID(ID);
            if (reader == null) return;
            this.ID = (int)reader["NumberReader"];
            this.Family = reader["FamilyName"].ToString();
            this.Father = reader["FatherName"].ToString();
            this.Name = reader["Name"].ToString();
            this.FIO = this.Family + " " + this.Name + " " + this.Father;
            if (reader["fotka"].GetType() != typeof(System.DBNull))
            {
                byte[] data = (byte[])reader["fotka"];

                if (data != null)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        ms.Write(data, 0, data.Length);
                        ms.Position = 0L;

                        this.Photo = new Bitmap(ms);
                    }
                }
            }
            else
            {
                this.Photo = Properties.Resources.nofoto;
            }


        }
        public ReaderVO(string BAR)
        {
            var dbr = new DBReader();
            DataRow reader = dbr.GetReaderByBAR(BAR);
            if (reader == null) return;
            this.ID = (int)reader["NumberReader"];
            this.Family = reader["FamilyName"].ToString();
            this.Father = reader["FatherName"].ToString();
            this.Name = reader["Name"].ToString();
            this.FIO = this.Family + " " + this.Name + " " + this.Father;
            if (reader["fotka"].GetType() != typeof(System.DBNull))
            {
                object o = reader["fotka"];
                byte[] data = (byte[])reader["fotka"];

                if (data != null)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        ms.Write(data, 0, data.Length);
                        ms.Position = 0L;

                        this.Photo = new Bitmap(ms);
                    }
                }
            }
            else
            {
                this.Photo = Properties.Resources.nofoto;
            }


        }
        public bool IsAlreadyIssuedMoreThanFourBooks()
        {
            DBReader dbr = new DBReader();
            return dbr.IsAlreadyIssuedMoreThanFourBooks(this);
        }
        public DataTable GetFormular()
        {
            DBReader dbr = new DBReader();
            return dbr.GetFormular(this.ID);
        }
        public int ID;
        public string Family;
        public string Name;
        public string Father;
        public Image Photo;
        public string FIO;

        internal string GetLiveemail()
        {
            DBReader dbr = new DBReader();
            return dbr.GetLiveemail(this);
        }

        internal string GetWorkEmail()
        {
            DBReader dbr = new DBReader();
            return dbr.GetWorkEmail(this);
        }

        internal string GetRegEmail()
        {
            DBReader dbr = new DBReader();
            return dbr.GetRegEmail(this);
        }


        internal string GetLastDateEmail()
        {
            DBReader dbr = new DBReader();
            return dbr.GetLastDateEmail(this);

        }
    }
}
