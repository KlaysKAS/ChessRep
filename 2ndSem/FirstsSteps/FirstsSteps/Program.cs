using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstsSteps
{
    class Program
    {
        static void Swap(ref string x, ref string y)
        {
            var t = x;
            x = y;
            y = t;
        }

        //метод возвращающий индекс опорного элемента
        static int Partition(ref string[] array, int minIndex, int maxIndex)
        {
            var pivot = minIndex - 1;
            for (var i = minIndex; i < maxIndex; i++)
            {
                if (String.Compare(array[i], array[maxIndex]) < 0)
                {
                    pivot++;
                    Swap(ref array[pivot], ref array[i]);
                }
            }

            pivot++;
            Swap(ref array[pivot], ref array[maxIndex]);
            return pivot;
        }

        //быстрая сортировка
        static string[] QuickSort(ref string[] array, int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
            {
                return array;
            }

            var pivotIndex = Partition(ref array, minIndex, maxIndex);
            QuickSort(ref array, minIndex, pivotIndex - 1);
            QuickSort(ref array, pivotIndex + 1, maxIndex);

            return array;
        }

        static string[] QuickSort(ref string[] array)
        {
            return QuickSort(ref array, 0, array.Length - 1);
        }

        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.GetEncoding(65001);
            Console.OutputEncoding = Encoding.GetEncoding(65001);
            int n = int.Parse(Console.ReadLine());
            string[] mas = new string[n];
            for (int i = 0; i < n; i++)
            {
                mas[i] = Console.ReadLine();
            }
            QuickSort(ref mas, 0, n - 1);
            Console.Write(mas[0]);
        }
    }
}
