using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG1_Hotel
{
    internal abstract class Room
    {
        public int RoomNumber { get; set; }
        public string BedConfiguration { get; set; }
        public double DailyRate { get; set; }
        public bool IsAvail { get; set; }

        public Room() { }
        public Room(int rn, string bc, double dr, bool ia)
        {
            RoomNumber = rn;
            BedConfiguration = bc;
            DailyRate = dr;
            IsAvail = ia;
        }

        public abstract double CalculateCharges();

        public override string ToString()
        {
            return $"RoomNo: {RoomNumber}, BedConfig: {BedConfiguration}, DailyRate: ${DailyRate:2F}, IsAvail: {IsAvail}";
        }
    }
}
