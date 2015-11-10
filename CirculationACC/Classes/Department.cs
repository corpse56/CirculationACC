using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circulation
{
    class Department
    {

        public Department() { }



        public BookVO ScannedBook;
        public ReaderVO ScannedReader;
        public void Circulate(string PortData)
        {
            //выяснить какой штрихкод сейчас ожидается: читатель или книга
            var dbb = new DBBook();
            if (dbb.Exists(PortData)) //если есть такой штрихкод вообще в базе
            {
                //если есть в выданных то сдать
                //если нет то на панель
            }//если нет - то ошибка


        }
    }
}
