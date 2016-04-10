using StepperParametersCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var cols = 5;
            var rows = 5;
            var a = new StepperCalculation(cols, rows, 30, 0.2825,7.5, 900);
            Console.WriteLine("****************Stage*******************");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write("{0}\t",a.GetSteps(j,i));
                }
                Console.WriteLine();
            }
            Console.WriteLine("****************Angle*******************");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var agl = a.GetAngles(j, i);
                    //if (agl > a.RotatorHalfCircleSteps)
                    //    agl = agl - a.RotatorFullCircleSteps;
                    Console.Write("{0}\t", agl);
                }
                Console.WriteLine();
            }


            Console.WriteLine("****************Motion*******************");
            int x1=0,y1=0,x2=1,y2=1;
            Console.WriteLine("From x={0},y={1} to x={2}, y={3} steps = {4}",x1,y1,x2,y2, a.StageMovementSteps(x2, y2, x1, y1));
            Console.WriteLine("**************AngleMotion****************");
            Console.WriteLine("Half circle steps = {0}",a.RotatorHalfCircleSteps);
            Console.WriteLine("Full circle steps = {0}", a.RotatorFullCircleSteps);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write("{0}\t",a.StageRotationSteps(j,i,-1,-1));
                }
                Console.WriteLine();
            }


        }
    }
}
