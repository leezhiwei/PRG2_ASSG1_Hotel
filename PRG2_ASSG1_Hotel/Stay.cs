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
        public RoomService RoomServiceForStay { get; set; }  = new RoomService();
        public List<Room> RoomList { get; set; } = new List<Room>();

        public Stay() { }
        public Stay(DateTime cid, DateTime cod)
        {
            CheckInDate = cid;
            CheckOutDate = cod;
        }

        public void AddRoom(Room room)
        {
            RoomList.Add(room);
        }

        public double CalculateTotal()
        {
            double total = 0;
            if (RoomList.Count() < 0)
            {
                return 0;
            }
            foreach (Room room in RoomList)
            {
                total += room.CalculateCharges();
            }
            return total * ((CheckOutDate- CheckInDate).TotalDays);
        }
        public double CalculateTotalWithRoomService()
        {
            double total = 0;
            if (RoomList.Count() < 0)
            {
                return 0;
            }
            foreach (Room room in RoomList)
            {
                total += room.CalculateCharges();
            }
            return total * ((CheckOutDate - CheckInDate).TotalDays) + RoomServiceForStay.CalculateFoodCharges();
        }
        public override string ToString()
        {
            string returnstring = "";
            returnstring += $"CheckInDate: {CheckInDate.ToString()}, CheckOutDate: {CheckOutDate.ToString()}";
            foreach (Room room in RoomList)
            {
                returnstring += room.ToString();
            }
            return returnstring;
        }
    }
}
