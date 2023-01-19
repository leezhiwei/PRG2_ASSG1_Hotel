using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG1_Hotel
{
    internal class Stay
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public List<Room> roomList { get; set; } = new List<Room>();

        public Stay() { }
        public Stay(DateTime cid, DateTime cod)
        {
            CheckInDate = cid;
            CheckOutDate = cod;
        }

        public void AddRoom(Room room)
        {
            roomList.Add(room);
        }

        public double CalculateTotal()
        {
            return 0;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
