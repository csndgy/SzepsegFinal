using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzepsegV0._2
{
    internal class Dolgozok
    {
        int dolgozoID, szolgaltatasID;
        string dolgozoFirstName, dolgozoLastName,
            dolgozoTel, dolgozoEmail;
        bool statusz;

        public Dolgozok(int dolgozoID, int szolgaltatasID, string dolgozoFirstName, string dolgozoLastName, string dolgozoTel, string dolgozoEmail, bool statusz)
        {
            this.dolgozoID = dolgozoID;
            this.szolgaltatasID = szolgaltatasID;
            this.dolgozoFirstName = dolgozoFirstName;
            this.dolgozoLastName = dolgozoLastName;
            this.dolgozoTel = dolgozoTel;
            this.dolgozoEmail = dolgozoEmail;
            this.statusz = statusz;
        }

        public int DolgozoID { get => dolgozoID; set => dolgozoID = value; }
        public int SzolgaltatasID { get => szolgaltatasID; set => szolgaltatasID = value; }
        public string DolgozoFirstName { get => dolgozoFirstName; set => dolgozoFirstName = value; }
        public string DolgozoLastName { get => dolgozoLastName; set => dolgozoLastName = value; }
        public string DolgozoTel { get => dolgozoTel; set => dolgozoTel = value; }
        public string DolgozoEmail { get => dolgozoEmail; set => dolgozoEmail = value; }
        public bool Statusz { get => statusz; set => statusz = value; }
    }
}
