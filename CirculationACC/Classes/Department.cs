using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Circulation
{
    public enum BARTYPE { Book, Reader, NotExist }
    public class Department
    {
        public int ExpectedBar = 0; //0 - ожидается штрихкод книги, 1 - ожидается штрихкод читателя

        public Department() 
        {
            ExpectedBar = 0;
        }
        


        //public BookVO ScannedBook;
        //public ReaderVO ScannedReader;
        /// <summary>
        /// Возвращаемые значения:
        /// 0 - успех
        /// 1 - Штрихкод не найден ни в базе читателей ни в базе книг
        /// 2 - ожидался штрихкод читателя, а считан штрихкод издания
        /// 3 - ожидался штрихкод издания, а считан штрихкод читателя
        /// 
        /// 
        /// </summary>
        /// <param name="PortData"></param>
        public int Circulate(string PortData)
        {
            BARTYPE ScannedType;
            if ((ScannedType = BookOrReader(PortData)) == BARTYPE.NotExist)//существует ли такой штрихкод вообще либо в базе читателей либо в базе изданий
            {
                return 1;
            }
            
            if (ExpectedBar == 0)//если сейчас ожидается штрихкод книги
            {
                if (ScannedType == BARTYPE.Reader) //выяснить какой штрихкод сейчас считан: читатель или книга
                {
                    return 3;
                }
                var dbb = new DBBook();
                if (dbb.Exists(PortData)) //если есть такой штрихкод вообще в базе
                {
                    return 1;
                    //если есть в выданных то сдать
                    //если нет то на панель
                }//если нет - то ошибка
            }
            else  //если сейчас ожидается штрихкод читателя
            {
                return 1;
            }
            return 1;
        }

        private BARTYPE BookOrReader(string data) //false - книга, true - читатель
        {
            var dbg = new DBGeneral();
            
            return dbg.BookOrReader(data);
        }
    }
}
