using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PieFactory
{
    class RobotLucy
    {
        private object locking;

        private int grfilling = 250;
        private int grflavor = 10;
        private int grtopping = 100;
        public Hopper fillinghopper { get; private set; }
        public Hopper flavorhopper { get; private set; }
        public Hopper toppinghopper { get; private set; }
        private ManualResetEvent hopperevent;
        private ManualResetEvent lucyevent;
        private Belt convbelt;
        

        public RobotLucy(object locking, Hopper fillinghopper, Hopper flavorhopper, Hopper toppinghopper, ManualResetEvent hopperevent,ManualResetEvent lucyevent,Belt convbelt)
        {
            this.locking = locking;
            this.fillinghopper = fillinghopper;
            this.flavorhopper = flavorhopper;
            this.toppinghopper = toppinghopper;
            this.hopperevent = hopperevent;
            this.lucyevent = lucyevent;
            this.convbelt = convbelt;
        }

        public void DoWork()
        {
            if (fillinghopper.Body < grfilling)
            {
                fillinghopper.Needfilling = true;
            }
            if (flavorhopper.Body < grflavor)
            {
                flavorhopper.Needfilling = true;
            }
            if (toppinghopper.Body < grtopping)
            {
                toppinghopper.Needfilling = true;
            }
            if (fillinghopper.Needfilling || flavorhopper.Needfilling || toppinghopper.Needfilling)
            {
                convbelt.stopbelt.Reset();
                hopperevent.Set();
                lucyevent.Reset();
                lucyevent.WaitOne();
            }
            hopperevent.Reset();
            convbelt.stopbelt.Set();

            fillinghopper.Needfilling = false;
            flavorhopper.Needfilling = false;
            toppinghopper.Needfilling = false;

            convbelt.crustready.WaitOne();

            Crust crust = convbelt.GetCurrentCrust();
            lock (locking)
            {
                //filling
                fillinghopper.Body -= grfilling;
                crust.Filling += grfilling;
                //flavor
                flavorhopper.Body -= grflavor;
                crust.Flavor += grflavor;
                //topping
                toppinghopper.Body -= grtopping;
                crust.Topping += grtopping;
            }
        }
    }
}
