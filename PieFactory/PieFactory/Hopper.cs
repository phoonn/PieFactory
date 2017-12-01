using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PieFactory
{
    class Hopper
    {
        private int body { get; set; }
        private object locker;

        private bool needfilling { get; set; }

        public Hopper()
        {
            body = 1500;
            needfilling = false;
            locker = new object();
        }

        public int Body
        {
            get { return body;}
            set { body = value; }
        }
        
        public bool Needfilling
        {
            get { lock (locker) return needfilling; }
            set { lock (locker) needfilling = value; }
        }
    }
}
