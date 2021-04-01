using System;
using System.Collections.Generic;
using System.Linq;

namespace Client
{
    public class Calculator
    {
        public long Count
        {
            get { return summaryCount; }
        }

        private long summaryValue;
        private long summaryCount;
        private SortedDictionary<int, ulong> valuesCount;

        public Calculator()
        {
            summaryCount = 0;
            summaryValue = 0;
            valuesCount = new SortedDictionary<int, ulong>();
        }

        public void Add(int value)
        {
            summaryValue += value;
            summaryCount++;


            if (valuesCount.ContainsKey(value))
                valuesCount[value]++;
            else
                valuesCount.Add(value, 1);
        }

        public double CalcAverage()
        {
            return (double)summaryValue / summaryCount;
        }

        public List<int> CalcMode()
        {
            var maxCount = valuesCount.Values.Max();
            var mode = valuesCount.Where(e => e.Value == maxCount).Select(e => e.Key).ToList();

            return mode;
        }

        public int CalcMedian()
        {
            double relativeFrequency = 0;
            foreach (var key in valuesCount.Keys)
            {
                relativeFrequency += ((double)valuesCount[key] / summaryCount);
                if (relativeFrequency > 0.5)
                    return key;
            }
            return 0;
        }

        public double SquaredDeviation()
        {
            var maxValue = valuesCount.Keys.Max();
            var minValue = valuesCount.Keys.Min();
            return (maxValue - minValue) / (2 * Math.Sqrt(3));
        }
    }
}
