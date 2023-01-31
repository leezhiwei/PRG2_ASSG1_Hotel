using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_ASSG1_Hotel
{
    internal class Food
    {
        public string FoodName { get; set; }
        public double FoodPrice { get; set; }

        public Food() { }
        public Food(string fn, double fp)
        {
            FoodName = fn;
            FoodPrice = fp;
        }

        public override string ToString()
        {
            return $"Food selected: {FoodName} Price: {FoodPrice}";
        }
    }
}
