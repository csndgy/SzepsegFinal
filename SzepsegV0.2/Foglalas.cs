using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzepsegV0._2
{
    internal class Foglalas
    {
        int foglalasID, szolgaltatasID, dolgozoID, ugyfelID;
        DateTime foglalasStart, foglalasEnd;

        public Foglalas(int foglalasID, int szolgaltatasID, int dolgozoID, int ugyfelID, DateTime foglalasStart, DateTime foglalasEnd)
        {
            this.foglalasID = foglalasID;
            this.szolgaltatasID = szolgaltatasID;
            this.dolgozoID = dolgozoID;
            this.ugyfelID = ugyfelID;
            this.foglalasStart = foglalasStart;
            this.foglalasEnd = foglalasEnd;
        }

        public int FoglalasID { get => foglalasID; set => foglalasID = value; }
        public int SzolgaltatasID { get => szolgaltatasID; set => szolgaltatasID = value; }
        public int DolgozoID { get => dolgozoID; set => dolgozoID = value; }
        public int UgyfelID { get => ugyfelID; set => ugyfelID = value; }
        public DateTime FoglalasStart { get => foglalasStart; set => foglalasStart = value; }
        public DateTime FoglalasEnd { get => foglalasEnd; set => foglalasEnd = value; }
    }

}
