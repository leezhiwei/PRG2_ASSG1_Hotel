using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG1_Hotel
{
    internal class DeluxeRoom:Room
    {
        public bool AdditionalBed { get; set; } = false;

        public DeluxeRoom() { }
        public DeluxeRoom(int rn, string bc, double dr, bool ia) : base(rn, bc, dr, ia)
        {
        }

        public override double CalculateCharges()
        {
            double charge = 0;
            if (AdditionalBed)
            {
                charge += 25;
            }
            charge += DailyRate;
            return charge;
        }
        public override string ToString()
        {
            return base.ToString() + $" AdditionalBed: {AdditionalBed}";
        }
    }
}
