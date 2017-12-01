using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PieFactory
{
    class Program
    {

        static ManualResetEvent hopperevent= new ManualResetEvent(false);
        //static ManualResetEvent lucyevent = new ManualResetEvent(false);
        static ManualResetEvent lucyevent = new ManualResetEvent(true);
        static CancellationTokenSource tokensource = new CancellationTokenSource();
        static Hopper fillinghopper = new Hopper();
        static Hopper flavorhopper = new Hopper();
        static Hopper toppinghopper = new Hopper();
        static Belt convbelt = new Belt();
        static object locking = new object();
        static int crustcount;

        static void LucyTask(CancellationToken token)
        {
            RobotLucy lucy = new RobotLucy(locking, fillinghopper, flavorhopper, toppinghopper, hopperevent, lucyevent, convbelt);
            while (!token.IsCancellationRequested)
            {
                lucy.DoWork();
            }
        }

        static void BeltTask(CancellationToken token)
        {
            convbelt.StartBelt(token,ref crustcount);
        }

        static void JoeTask(CancellationToken token)
        {
            RobotJoe joe = new RobotJoe(locking,fillinghopper, flavorhopper, toppinghopper,hopperevent,lucyevent);
            joe.DoWork(token);
        }

        static void Main(string[] args)
        {
            CancellationToken token = tokensource.Token;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var lucytask = Task.Factory.StartNew(() => LucyTask(token));
            var joetask = Task.Factory.StartNew(() => JoeTask(token));
            var belttas = Task.Factory.StartNew(() => BeltTask(token));
            while (crustcount<100)
            {

            }
            tokensource.Cancel();
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
            Console.ReadKey();

        }
    }
}
