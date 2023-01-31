using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG1_Hotel
{
    internal class RoomService
    {
        public List<Food> FoodList = new List<Food>();
        public RoomService() { }
        public void AddFood(Food f)
        {
            FoodList.Add(f);
        }
        public double CalculateFoodCharges()
        {
            double total = 0;
            foreach (Food f in FoodList)
            {
                total += f.FoodPrice;
            }
            return total;
        }
        public override string ToString()
        {
            string returnstring = "";
            foreach (Food f in FoodList)
            {
                returnstring += f.ToString();
            }
            return returnstring;
        }
    }
}
