using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics;

namespace PLImg_V2
{
    public class AFCalc
    {
        public double CalcAFV(byte[] input)
       {
            try
            {

            double output = 0;
            double[] contrastArr = new double[input.Length-2];

            for (int i = 2; i < input.Length-2; i++)
            {
                contrastArr[i] = (double)(Math.Abs(input[i] - input[i - 1]) + Math.Abs(input[i] - input[i + 1])/ input.Length);
            }

            output = Measures.Variance(contrastArr);
            return output;

            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.ToString());
                return -999;
            }
        }

    }
}
