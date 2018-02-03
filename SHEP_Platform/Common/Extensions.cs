using System;

namespace SHEP_Platform.Common
{
    public static class Extensions
    {
        public static double GetNextDouble(this Random random,double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}