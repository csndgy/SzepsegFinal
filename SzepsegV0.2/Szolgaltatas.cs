using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzepsegV0._2
{
    internal class Szolgaltatas
    {
        int szolgaltatasID, szolgaltatasAr;
        string szolgaltatasKategoria;
        DateTime szolgaltatasIdotartalma;

        public Szolgaltatas(int szolgaltatasID, int szolgaltatasAr, string szolgaltatasKategoria, DateTime szolgaltatasIdotartalma)
        {
            this.szolgaltatasID = szolgaltatasID;
            this.szolgaltatasAr = szolgaltatasAr;
            this.szolgaltatasKategoria = szolgaltatasKategoria;
            this.szolgaltatasIdotartalma = szolgaltatasIdotartalma;
        }

        public int SzolgaltatasID { get => szolgaltatasID; set => szolgaltatasID = value; }
        public int SzolgaltatasAr { get => szolgaltatasAr; set => szolgaltatasAr = value; }
        public string SzolgaltatasKategoria { get => szolgaltatasKategoria; set => szolgaltatasKategoria = value; }
        public DateTime SzolgaltatasIdotartalma { get => szolgaltatasIdotartalma; set => szolgaltatasIdotartalma = value; }
    }
}

