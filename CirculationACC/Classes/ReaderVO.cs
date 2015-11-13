using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

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

    }
}
