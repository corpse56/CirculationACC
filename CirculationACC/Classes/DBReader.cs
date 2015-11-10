using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Circulation
{
    class DBReader:DB
    {
        public DBReader() { }

        public DataRow GetReaderByID(int ID)
        {
            DA.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = "+ID;
            DS = new DataSet();
            int i = DA.Fill(DS, "reader");
            if (i == 0) return null;
            return DS.Tables["reader"].Rows[0];
        }
        public bool Exists(int ID)
        {
            DA.SelectCommand.CommandText = "select 1 from Readers..Main where NumberReader = " + ID;
            DS = new DataSet();
            if (DA.Fill(DS, "reader") > 0) return true; else return false;
        }
    }
}
