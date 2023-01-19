using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG1_Hotel
{
    internal class Room
    {
        public int RoomNumber { get; set; }
        public string BedConfiguration { get; set; }
        public double DailyRate { get; set; }
        public bool isAvail { get; set; }

        public Room() { }
        public Room(int rn, string bc, double dr, bool ia)
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
