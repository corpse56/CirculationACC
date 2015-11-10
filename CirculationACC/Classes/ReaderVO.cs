using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Circulation
{
    class ReaderVO
    {
        public ReaderVO() { }
        public ReaderVO(int ID)
        {
            DBReader dbr = new DBReader();
            DataRow reader = dbr.GetReaderByID(ID);
            if (reader == null) return;
            ReaderVO ret = new ReaderVO();
            this.ID = (int)reader["ID"];
            this.Family = reader["FamilyName"].ToString();
            this.Father = reader["FatherName"].ToString();
            this.Name = reader["Name"].ToString();

        }
        public int ID;
        public string Family;
        public string Name;
        public string Father;

    }
}
