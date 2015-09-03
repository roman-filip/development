using System;

namespace SolarWindsOnlineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // On Site question 1            
            // What value will by in the X variable once following statement is executed.
            var myClass = new MyClass();
            //myClass.Struct.X += 5;
            // The solution is that code above cannot be compiled because Struct is value type and is defined as property

            var a = new Class1();
            ((IInterface1)a).Fun();
            ((IInterface2)a).Fun();
        }
    }
}
