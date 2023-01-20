using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG1_Hotel
{
    internal class StandardRoom:Room
    {
        public bool RequireWifi { get; set; }
        public bool RequireBreakfast { get; set; }

        public StandardRoom() { }
        public StandardRoom(int rn, string bc, double dr, bool ia, bool rw, bool rb) : base(rn, bc, dr, ia)
        {
            RequireWifi = rw;
            RequireBreakfast = rb;
        }

        public override double CalculateCharges()
        {
            double perdaycharge = 0;
            if (RequireWifi)
            {
                perdaycharge += 10;
            }
            if (RequireBreakfast)
            {
                perdaycharge += 20;
            }
            perdaycharge += DailyRate;
            return perdaycharge;

        }

        public override string ToString()
        {
            return base.ToString() + $" RequireWifi: {RequireWifi}, RequireBreakFast: {RequireBreakfast}";
        }
    }
}
