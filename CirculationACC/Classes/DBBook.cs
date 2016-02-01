using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Circulation
{
    class DBBook : DB
    {
        //BJACCRecord rec;// = new BJACCRecord();
        public DBBook()
        {
            
        }
        public List<BJACCRecord> GetBookByIDMAIN(int IDMAIN)
        {
            DA.SelectCommand.CommandText = "select A.*,B.PLAIN from BJACC..DATAEXT A "+
                                           " left join BJACC..DATAEXTPLAIN B on A.ID = B.IDDATAEXT where A.IDMAIN = "+IDMAIN;
            DS = new DataSet();
            DA.Fill(DS, "t");
            List<BJACCRecord> Book = new List<BJACCRecord>();
            BJACCRecord rec;
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                rec = new BJACCRecord();
                rec.ID = (int)r["ID"];
                rec.IDDATA = (int)r["IDDATA"];
                rec.IDINLIST = (int)r["IDINLIST"];
                rec.IDMAIN = IDMAIN;
                rec.MNFIELD = (int)r["MNFIELD"];
                rec.MSFIELD = r["MSFIELD"].ToString();
                rec.PLAIN = r["PLAIN"].ToString();
                rec.SORT = r["SORT"].ToString();
                Book.Add(rec);
            }
            return Book;
        }

        public List<BJACCRecord> GetBookByBAR(string BAR)
        {
            DA.SelectCommand.CommandText = "select A.*,B.PLAIN from BJACC..DATAEXT A " +
                                           " left join BJACC..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                           " where A.IDMAIN = (select top 1 IDMAIN from BJACC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + BAR + "')";
            DS = new DataSet();
            DA.Fill(DS, "t");
            List<BJACCRecord> Book = new List<BJACCRecord>();
            BJACCRecord rec;
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                rec = new BJACCRecord();
                rec.ID = (int)r["ID"];
                rec.IDDATA = (int)r["IDDATA"];
                rec.IDINLIST = (int)r["IDINLIST"];
                rec.IDMAIN = (int)r["IDMAIN"]; 
                rec.MNFIELD = (int)r["MNFIELD"];
                rec.MSFIELD = r["MSFIELD"].ToString();
                rec.PLAIN = r["PLAIN"].ToString();
                rec.SORT = r["SORT"].ToString();
                Book.Add(rec);
            }
            return Book;
        }
        public bool Exists(string BAR)
        {
            DA.SelectCommand.CommandText = "select top 1 IDMAIN from BJACC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + BAR + "'";
            DS = new DataSet();
            int i = DA.Fill(DS, "t");
            if (i > 0) return true; else return false;

        }

        internal bool IsIssued(int IDDATA)
        {
            DA.SelectCommand.CommandText = "select IDMAIN from Reservation_R..ISSUED_ACC where IDDATA = "+IDDATA+" and IDSTATUS = 1";
            DS = new DataSet();
            int i = DA.Fill(DS, "t");
            if (i > 0) return true; else return false;
        }

        internal int GetIDISSUED(int IDDATA)
        {
            DA.SelectCommand.CommandText = "select ID from Reservation_R..ISSUED_ACC where IDDATA = " + IDDATA + " and IDSTATUS = 1";
            DS = new DataSet();
            int i = DA.Fill(DS, "t");
            if (i > 0) return (int)DS.Tables["t"].Rows[0]["ID"]; else return 0;
        }
    }
}
