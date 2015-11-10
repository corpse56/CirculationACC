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
    }
}
