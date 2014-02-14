using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    static class A
    {
        public static bool IsTrue(this bool value) { return true == value; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool aa = false;

            if (aa.IsTrue())
            {

            }

            var obj = new { Id = 1, Name = "sadfasdf" };
            //obj.Id = 4;
            Console.WriteLine(obj.Id);



            IShape[] shapes = new IShape[] { new Square(), new Rectangle() };
            foreach (var shape in shapes)
            {
                shape.Draw();
            }


            var a = new MyClass(10);
            foreach (var i in a.CountFrom(5))
            {
                Console.WriteLine(i);
            }

            Console.ReadLine();
        }
    }


    class MyClass
    {
        int limit = 0;
        public MyClass(int limit) { this.limit = limit; }

        public IEnumerable<int> CountFrom(int start)
        {
            for (int i = start; i <= limit; i++)
            {
                yield return i;
            }

            //yield return -1;
            //yield return -2;
            //yield return -3;
        }
    }

    interface IShape
    {
        void Draw();
    }

    class Rectangle : IShape
    {
        public void Draw()
        {
            Console.WriteLine("Rectangle");
        }
    }

    class Square : Rectangle
    {
        public void Draw()
        {
            Console.WriteLine("Square");
        }
    }

    class Entry<K, V>
    {
        private readonly K key;
        private readonly V value;

        public Entry(K k, V v)
        {
            key = k;
            value = v;
        }
    }
}
