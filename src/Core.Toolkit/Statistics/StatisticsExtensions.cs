using System;
using System.Collections.Generic;
using System.Linq;
using Core.Linq;

namespace Core
{
    public static class StatisticsExtensions
    {
        public static double CalculateStdDev(this IEnumerable<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //Compute the Average      
                double avg = values.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }

        public static IEnumerable<double> FilterAbnormalValues(this IEnumerable<double> values, double standardValue, double upperLimit, double lowLimit)
        {
            foreach (double value in values)
            {
                var isAbnormal = value.CheckIsAbnormalValue(standardValue, upperLimit, lowLimit);
                if (isAbnormal) continue;
                yield return value;
            }
        }

        public static int GetAbnormalValuesCount(this IEnumerable<double> values, double standardValue, double upperLimit, double lowLimit)
        {
            int count = 0;
            foreach (double value in values)
            {
                var isAbnormal = value.CheckIsAbnormalValue(standardValue, upperLimit, lowLimit);
                if (isAbnormal) count ++;
            }
            return count;
        }

        public static bool CheckIsAbnormalValue(this double value, double standardValue, double upperLimit, double lowLimit)
        {
            if (value > standardValue + upperLimit) return true;
            if (value < standardValue + lowLimit) return true;
            if (double.IsNaN(value)) return true;
            if (double.IsInfinity(value)) return true;
            return false;
        }

        public static bool CheckIsLimitedValue(this double value, double upperLimit, double lowLimit)
        {
            if (value > upperLimit) return true;
            if (value < lowLimit) return true;
            if (double.IsNaN(value)) return true;
            if (double.IsInfinity(value)) return true;
            return false;
        }

        public static IEnumerable<double> CheckIfEmpty(this IEnumerable<double> values, double defaultValue)
        {
            var doubles = values as double[] ?? values.ToArray();
            return !doubles.Any() ? defaultValue.ToEnumerable() : doubles;
        }
    }
}