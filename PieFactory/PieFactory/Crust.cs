using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieFactory
{
    class Crust
    {
        public int Filling { get; set; }
        private int flavor;
        private int topping;

        public int Flavor
        {
            get { return flavor; }
            set
            {
                if (Filling == 250)
                {
                    this.flavor = value;
                }
                else flavor = 0;
            }
        }

        public int Topping
        {
            get { return topping; }
            set
            {
                if (flavor == 10)
                {
                    this.topping = value;
                }
                else topping = 100;
            }
        }
        
    }
}
