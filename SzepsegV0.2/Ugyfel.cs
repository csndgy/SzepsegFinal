using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzepsegV0._2
{
    internal class Ugyfel
    {
        int ugyfelID, ugyfelPontok;
        string ugyfelFirstName,
            ugyfelLastName, ugyfelTel, ugyfelEmail;

        public Ugyfel(int ugyfelID, int ugyfelPontok, string ugyfelFirstName, string ugyfelLastName, string ugyfelTel, string ugyfelEmail)
        {
            this.ugyfelID = ugyfelID;
            this.ugyfelPontok = ugyfelPontok;
            this.ugyfelFirstName = ugyfelFirstName;
            this.ugyfelLastName = ugyfelLastName;
            this.ugyfelTel = ugyfelTel;
            this.ugyfelEmail = ugyfelEmail;
        }

        public int UgyfelID { get => ugyfelID; set => ugyfelID = value; }
        public int UgyfelPontok { get => ugyfelPontok; set => ugyfelPontok = value; }
        public string UgyfelFirstName { get => ugyfelFirstName; set => ugyfelFirstName = value; }
        public string UgyfelLastName { get => ugyfelLastName; set => ugyfelLastName = value; }
        public string UgyfelTel { get => ugyfelTel; set => ugyfelTel = value; }
        public string UgyfelEmail { get => ugyfelEmail; set => ugyfelEmail = value; }
    }
}
