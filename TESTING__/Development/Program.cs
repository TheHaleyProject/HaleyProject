using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Development
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initiated");
            string strinput = "Hello";
            int intInput = 1;
            bool boolInput = false;
            double dblInput = 20.4;
           
            Check(strinput);
            Check(intInput);
            Check(boolInput);
            Check(dblInput);

        }

        public static void Check(object value)
        {
            Type objtype = value.GetType();
            
            if ( objtype == typeof(string)) { Console.WriteLine($@"Input is a {objtype.FullName}: string"); }
            if (objtype == typeof(int)) { Console.WriteLine($@"Input is a {objtype.FullName}: int"); }
            if (objtype == typeof(double)) { Console.WriteLine($@"Input is a {objtype.FullName}: double "); }
            if (objtype == typeof(bool)) { Console.WriteLine($@"Input is a {objtype.FullName}: bool"); }
        }

        public static void InputToString(object value)
        {
           
        }
    }
}
