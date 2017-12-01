using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PieFactory
{
    class Belt
    {
        Queue<Crust> crustbelt;
        public ManualResetEvent stopbelt;
        public AutoResetEvent crustready;

        public Belt()
        {
            crustbelt = new Queue<Crust>(5);
            for (int i = 0; i < 5; i++)
            {
                crustbelt.Enqueue(new Crust());
            }
            this.stopbelt = new ManualResetEvent(true);
            this.crustready = new AutoResetEvent(true);
        }

        public void StartBelt(CancellationToken token, ref int crustcount)
        {
            while (!token.IsCancellationRequested)
            {
                stopbelt.WaitOne();
                Thread.Sleep(50);
                Crust crustdequeue = crustbelt.Dequeue();
                crustcount++;
                Console.WriteLine(crustdequeue.Filling + "gr filling , " + crustdequeue.Flavor + "gr flavor, " + crustdequeue.Topping + "gr topping.");
                crustbelt.Enqueue(new Crust());
                crustready.Set();
            }
        }
        
        public Crust GetCurrentCrust()
        {
            return crustbelt.Peek();
        }
    }
}
