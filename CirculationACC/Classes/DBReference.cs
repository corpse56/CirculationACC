using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Circulation
{
    class DBReference:DB
    {
        public DBReference()
        { }
        internal DataTable GetAllIssuedBook()
        {
            DA.SelectCommand.CommandText = "select 1,C.PLAIN tit,D.PLAIN avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when B.LiveEmail is null and B.RegistrationEmail is null and B.WorkEmail is null then 'false' else 'true' end) email, E.PLAIN shifr" +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJACC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT EE on A.IDMAIN = EE.IDMAIN and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join BJACC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS = 1 ";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];

        }



        internal object GetAllOverdueBook()
        {
            DA.SelectCommand.CommandText = "select distinct 1,C.PLAIN tit,D.PLAIN avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when (B.LiveEmail is null or B.LiveEmail = '')  and (B.RegistrationEmail is null or B.RegistrationEmail = '') and (B.WorkEmail is null or B.WorkEmail = '') then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent, E.PLAIN shifr " +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJACC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT EE on A.IDMAIN = EE.IDMAIN and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS EM on EM.IDISSUED_ACC = A.IDREADER and EM.IDACTION = 4" + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                " left join BJACC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS = 1 and A.DATE_RETURN < getdate()";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetReaderHistory(ReaderVO reader)
        {
            DA.SelectCommand.CommandText = "select 1 ID,C.PLAIN tit,D.PLAIN avt," +
                " INV.SORT inv,A.DATE_ISSUE,ret.DATEACTION DATE_RETURN" +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJACC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS ret on ret.IDISSUED_ACC = A.ID and ret.IDACTION = 2 " +
                " where A.IDSTATUS = 2 and A.IDREADER = "+reader.ID+"order by DATE_ISSUE desc";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetAllBooks()
        {
            DA.SelectCommand.CommandText = "select 1 ID,C.PLAIN tit,D.PLAIN avt," +
                " INV.SORT inv" +
                " from BJACC..MAIN A" +
                " left join BJACC..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on A.ID = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$w'"+
                " where INV.SORT is not null";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetBookNegotiability()
        {
            DA.SelectCommand.CommandText = "with F1 as  "+
                                           " ( "+
                                           " select B.IDMAIN,COUNT(B.IDMAIN) cnt "+
                                           " from Reservation_R..ISSUED_ACC_ACTIONS A "+
                                           " left join Reservation_R..ISSUED_ACC B on B.ID = A.IDISSUED_ACC "+
                                           " where A.IDACTION = 2 "+
                                           " group by B.IDMAIN "+
                                           " ) "+
                                           " select 1 ID,C.PLAIN tit,D.PLAIN avt, "+
                                           " INV.SORT inv,A.cnt "+
                                           "  from F1 A "+
                                           " left join BJACC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' "+
                                           "  left join BJACC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a' "+
                                           " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID "+
                                           "  left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
                                           "  left join BJACC..DATAEXT INV on A.IDMAIN = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$w'"+
                                           " order by cnt desc";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetBooksWithRemovedResponsibility()
        {
            DA.SelectCommand.CommandText = "select 1,C.PLAIN tit,D.PLAIN avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT inv,A.DATE_ISSUE,AA.DATEACTION " +
                
                " from Reservation_R..ISSUED_ACC A" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS AA on A.ID = AA.IDISSUED_ACC " +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJACC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where AA.IDACTION = 5";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];

        }

        internal object GetViolators()
        {
            DA.SelectCommand.CommandText = "select distinct 1,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " (case when (B.LiveEmail is null or B.LiveEmail = '')  and (B.RegistrationEmail is null or B.RegistrationEmail = '') and (B.WorkEmail is null or B.WorkEmail = '') then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent " +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS EM on EM.IDISSUED_ACC = A.IDREADER and EM.IDACTION = 4" + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                " where A.IDSTATUS = 1 and A.DATE_RETURN < getdate()";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }
    }
}
