using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG1_Hotel
{
    internal class Guest
    {
        public string Name { get; set; }
        public string PassportNum { get; set; }
        public Stay HotelStay { get; set; }
        public Membership Member { get; set; }
        public bool IsCheckedin { get; set; }

        public Guest() { }
        public Guest(string n, string pn, Stay hs, Membership m)
        {
            Name = n;
            PassportNum = pn;
            HotelStay = hs;
            Member = m;
        }

        public override string ToString()
        {
            return $"Name: {Name} Passport Number: {PassportNum} Stay: {HotelStay} Member: {Member} Check in status: {IsCheckedin}";
        }
    }
}
