using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circulation
{
    public class BookVO
    {
        public BookVO() { }
        public BookVO(int IDMAIN)
        {
            DBBook dbb = new DBBook();
            this.BookRecord = dbb.GetBookByIDMAIN(IDMAIN);
        }
        public BookVO(string BAR)
        {
            DBBook dbb = new DBBook();
            this.BookRecord = dbb.GetBookByBAR(BAR);
        }

        public List<BJACCRecord> BookRecord;


    }

}
