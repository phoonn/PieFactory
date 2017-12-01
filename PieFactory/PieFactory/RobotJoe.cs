using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PieFactory
{
    class RobotJoe
    {
        public Hopper fillinghopper { get; private set; }
        public Hopper flavorhopper { get; private set; }
        public Hopper toppinghopper { get; private set; }
        public object locking;
        ManualResetEvent hopperevent;
        ManualResetEvent lucyevent;


        public RobotJoe(object locking ,Hopper fillinghopper,Hopper flavorhopper,Hopper toppinghopper, ManualResetEvent hopperevent, ManualResetEvent lucyevent)
        {
            this.fillinghopper = fillinghopper;
            this.flavorhopper = flavorhopper;
            this.toppinghopper = toppinghopper;
            this.locking = locking;
            this.hopperevent = hopperevent;
            this.lucyevent = lucyevent;
        }
        
        public void FillHopper(Hopper hopper)
        {
            lock (locking)
            {
                if (hopper.Body >= 1000)
                {
                    hopper.Needfilling = false;
                    return;
                }
                hopper.Body = hopper.Body + 100;
            }
            Thread.Sleep(10);
            Console.WriteLine("filled one");
        }

        public void DoWork(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                hopperevent.WaitOne();
                while (fillinghopper.Needfilling || flavorhopper.Needfilling || toppinghopper.Needfilling)
                {
                    if (fillinghopper.Needfilling)
                    {
                        FillHopper(fillinghopper);
                    }
                    if (flavorhopper.Needfilling)
                    {
                        FillHopper(flavorhopper);
                    }
                    if (toppinghopper.Needfilling)
                    {
                        FillHopper(toppinghopper);
                    }
                }
                lucyevent.Set();
            }
        }

    }
}
