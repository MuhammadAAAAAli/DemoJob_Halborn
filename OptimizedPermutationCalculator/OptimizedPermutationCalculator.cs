using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizedPermutationCalculator
{
    public class OptimizedPermutationCalculator
    {
        int Factorial(int cnt)
        {
            int sum = 1;
            while (cnt > 0)
            {
                sum *= cnt;
                cnt--;
            }
            return sum;
        }
        public char[] Calculate(char[] items)
        {
            int length = items.Length;
            char[] result = new char[Factorial(length) * length];

            int iteration = length - 1;
            int index = 1;
            //first item is inserted here
            Array.Copy(items, result, length);
            while (iteration > 0)
            {
                //we keep count of current result
                int resultCount = index;
                for (int i = 0; i < resultCount; i++)
                {
                    int rotateLength = length - iteration;
                    if (rotateLength > 0)
                    {
                        int startIndex = i * length + (iteration - 1);
                        int lastIndex = (i + 1) * length - 1;
                        while (rotateLength > 0)
                        {
                            RotateRight(result, startIndex, lastIndex);
                            //the rotated item  is new item and we copy it into new item
                            Array.Copy(result, i * length, result, index * length, length);
                            index++;
                            rotateLength--;
                        }
                        //This rotation causes that original item doesn't change
                        RotateRight(result, startIndex, lastIndex);
                    }
                }

                iteration--;
            }

            return result;
        }
        public void RotateRight(char[] items, int startIndex, int lastIndex)
        {
            var temp = items[lastIndex];
            int len = lastIndex + 1;
            Array.Copy(items, startIndex, items, startIndex + 1, len - (startIndex + 1));
            items[startIndex] = temp;

        }
    }
}
