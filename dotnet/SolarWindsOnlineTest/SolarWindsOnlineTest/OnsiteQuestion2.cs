using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarWindsOnlineTest
{
    public interface IInterface1
    {
        void Fun();
    }

    public interface IInterface2 : IInterface1
    {
        void Fun();
    }

    public class Class1 : IInterface2
    {
        void IInterface1.Fun()
        {
            Console.WriteLine("IInterface1.Fun()");
        }

        void IInterface2.Fun()
        {
            Console.WriteLine("IInterface2.Fun()");
        }
    }
}
