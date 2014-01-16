using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FourSquareData;

namespace FourSquareConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var c = new conect4Sq("a","b");
            c.Authenticate();
            var data = c.VenuesToday();
            foreach (var item in data)
            {
                Console.WriteLine(item.lastHereAt);
                Console.WriteLine(item.venue.createdAt);
                Console.WriteLine(item.venue.id);
                Console.WriteLine(item.venue.name);
                
            }
        }



    }
}
