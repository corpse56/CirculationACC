using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Circulation
{
    class DBGeneral:DB
    {
//=====================================================================LOGIN==============================================================================
        public int EmpID;
        public string UserName;

        public bool Login(string name,string pass)
        {
            DA.SelectCommand.CommandText = "select * from BJACC..USERS where lower(LOGIN) = '" +name.ToLower()+"' and lower(PASSWORD) = '"+pass.ToLower()+"'";
            DS = new DataSet();
            int i = DA.Fill(DS, "login");
            if (i == 0) return false;
            EmpID = (int)DS.Tables["login"].Rows[0]["ID"];
            UserName = DS.Tables["login"].Rows[0]["NAME"].ToString();
            return true;
        }
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^LOGIN^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        internal BARTYPE BookOrReader(string data)
        {
            DA.SelectCommand.CommandText = "select top 1 IDMAIN from BJACC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + data + "'";
            DS = new DataSet();
            int i = DA.Fill(DS, "t");
            if (i > 0) return BARTYPE.Book;

            DA.SelectCommand.CommandText = "select top 1 NumberReader from Readers..Main where BarCode = '" + data.Substring(1)+"'";
            DS = new DataSet();
            try
            {
                i = DA.Fill(DS, "t");
            }
            catch
            {
                i = 0;
            }
            if (i > 0) return BARTYPE.Reader;

            DA.SelectCommand.CommandText = "select top 1 NumberReader from Readers..Main where NumberSC = '" + data.Substring(0,data.IndexOf(" "))+
                                                                                       "' and SerialSC = '" + data.Substring(data.IndexOf(" ")+1)+"'";
            DS = new DataSet();
            i = DA.Fill(DS, "t");
            if (i > 0) return BARTYPE.Reader;

            return BARTYPE.NotExist;
            //R.Tables["t"].Rows[0]["NumberSC"].ToString().Trim().Replace("\0", "") + R.Tables["t"].Rows[0]["SerialSC"].ToString().Trim().Replace("\0", "");
        }


    }
}
