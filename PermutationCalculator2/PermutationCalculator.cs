using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermutationCalculator2
{
    public class PermutationCalculator
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
        public char[][] Calculate(char[] items)
        {
            int length = items.Length;
            char[][] result = new char[Factorial(length)][];

            int iteration = length - 1;
            int index = 0;
            //first item is inserted here
            result[index++] = items;
            while (iteration > 0)
            {
                //we keep count of current result
                int resultCount = index;
                for (int i = 0; i < resultCount; i++)
                {
                    int rotateLength = length - iteration;
                    var curItem = result[i];
                    if (rotateLength > 0)
                    {
                        while (rotateLength > 0)
                        {
                            RotateRight(curItem, iteration - 1);
                            //the rotated item  is new item and we copy it into new item
                            char[] newItem = new char[length];
                            curItem.CopyTo(newItem, 0);
                            result[index++] = newItem;
                            rotateLength--;
                        }
                        //This rotation causes that original item doesn't change
                        RotateRight(curItem, iteration - 1);
                    }

                }

                iteration--;
            }

            return result;
        }
        public void RotateRight(char[] items, int startIndex)
        {
            int last = items.Length - 1;
            var temp = items[last];
            int len = items.Length;
            Array.Copy(items, startIndex, items, startIndex + 1, len - (startIndex + 1));
            items[startIndex] = temp;
        }
    }
}
