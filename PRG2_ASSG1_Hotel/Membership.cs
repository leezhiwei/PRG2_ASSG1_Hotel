using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG1_Hotel
{
    internal class Membership
    {
        public string Status { get; set; }
        public int Points { get; set; }

        public Membership() { }
        public Membership(string s, int p)
        {
            Status = s;
            Points = p;
        }

        public double EarnPoints()
        {
            return 0;
        }

        public bool RedeemPoints()
        {
            return false;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
