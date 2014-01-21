using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FourSquareData;

namespace FourSquareConsole
{
    class Program
    {
        static void WriteToLog(string val)
        {
            Console.WriteLine(val);
            File.AppendAllText("a.txt", val + Environment.NewLine);
        }
        
        static void Main(string[] args)
        {

            var c = new conect4Sq(conect4Sq.FakeClientId, conect4Sq.FakeClientSecret);
            c.Authenticate2();
            

            var data = c.CheckinsYesterday();
            foreach (var item in data)
            {
                var dt = conect4Sq.FromUnixTime(long.Parse( item.createdAt)).ToLocalTime();

                WriteToLog(dt.ToLongDateString() + " " + dt.ToLongTimeString());
                WriteToLog(item.venue.name);
                if (item.comments != null && item.comments.count > 0)
                {
                    foreach (var com in item.comments.items)
                    {
                        WriteToLog(com.ToString());            
                    }
                            
                }
                
                
                
            }
            Process.Start("a.txt");
            Console.WriteLine("A");
        }



    }
}
