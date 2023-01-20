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

        public double EarnPoints(double spent)
        {
            Points += Convert.ToInt32(spent / 10);
            return Points;
        }

        public bool RedeemPoints(int redpoint)
        {
            if (Points < 0)
            {
                return false;
            }
            else if (redpoint > Points)
            {
                return false;
            }
            else
            {
                Points -= redpoint;
                return true;
            }
        }

        public override string ToString()
        {
            return $"Status: {Status}, Points: {Points}";
        }
    }
}
