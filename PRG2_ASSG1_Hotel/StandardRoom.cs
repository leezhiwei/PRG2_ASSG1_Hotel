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
        public StandardRoom(int rn, string bc, double dr, bool ia) : base(rn, bc, dr, ia)
        {
            RoomNumber = rn;
            BedConfiguration = bc;
            DailyRate = dr;
            isAvail = ia;
        }

        public double CalculateCharges()
        {
            return 0;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
