using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            double[] arr = { 82.0,2.20, 1, 5.350, 77, 640, 70, 240, 9 };


            float[] mas = new float[] { 1.1f, 31.23f, 1.4f, 55.25f, 6f, 6f, 8f, 2f, 5f, 9f, 0f,1f };
            mas = Heap(mas);

            foreach (float a in mas)
            {
                Console.Write(a + ", ");
            }

            int num1 = 12, num2 = 23;
            Console.WriteLine("\n" + num1 + " " + num2);
            num2 ^= num1;
            num1 ^= num2;
            num2 ^= num1;
            Console.WriteLine("\n" + num1 + " " + num2);

            Console.ReadKey();

        }

        /// <summary>
        /// Universal bubble sort changes the postion of numbers or changing an unordered sequence into an ordered sequence.
        /// </summary>
        public static T[] Bubble<T>(T[] arr)
        {
            dynamic array = arr, temp;
            if (arr.GetType() == typeof(Int32))
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    for (int j = 0; j < arr.Length - 1; j++)
                    {
                        if (array[j] > array[j + 1])
                        {
                            array[j + 1] ^= array[j];
                            array[j]     ^= array[j + 1];
                            array[j + 1] ^= array[j];

                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    for (int j = 0; j < arr.Length - 1; j++)
                    {
                        if (array[j] > array[j + 1])
                        {
                            temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
            }

            return arr;
        }

        /// <summary>
        /// Universal merge sort
        /// </summary>
        public static T[] Merge<T>(T[] massive)
        {
            if (massive.Length == 1)
                return massive;
            int mid_point = massive.Length / 2;
            return MergeSort<T>(Merge<T>(massive.Take(mid_point).ToArray()), Merge<T>(massive.Skip(mid_point).ToArray()));
        }
        private static T[] MergeSort<T>(T[] mass1, T[] mass2)
        {
            int a = 0, b = 0;
            T[] merged = new T[mass1.Length + mass2.Length];

            dynamic arr1 = mass1, arr2 = mass2;

            for (int i = 0; i < mass1.Length + mass2.Length; i++)
            {
                if (b < mass2.Length && a < mass1.Length)
                {

                    if (arr1[a] > arr2[b] && b < mass2.Length)
                        merged[i] = mass2[b++];
                    else
                        merged[i] = mass1[a++];
                }
                else
                    if (b < mass2.Length)
                        merged[i] = mass2[b++];
                else
                    merged[i] = mass1[a++];
            }
            return merged;
        }


        /// <summary>
        /// Universal pyramid sort
        /// </summary>
        public static T[] Heap<T>(T[] input)
        {
            //Build-Max-Heap
            int heapSize = input.Length;
            for (int p = (heapSize - 1) / 2; p >= 0; p--)
                MaxHeapify<T>(input, heapSize, p);

            for (int i = input.Length - 1; i > 0; i--)
            {
                //Swap
                dynamic temp = input[i];
                input[i] = input[0];
                input[0] = temp;

                heapSize--;
                MaxHeapify<T>(input, heapSize, 0);
            }
            return input;
        }
        private static void MaxHeapify<T>(T[] inpt, int heapSize, int index)
        {
            dynamic input = inpt;

            int left = (index + 1) * 2 - 1;
            int right = (index + 1) * 2;
            int largest = 0;

            if (left < heapSize && input[left] > input[index])
                largest = left;
            else
                largest = index;

            if (right < heapSize && input[right] > input[largest])
                largest = right;

            if (largest != index)
            {
                dynamic temp = input[index];
                input[index] = input[largest];
                input[largest] = temp;

                MaxHeapify(input, heapSize, largest);
            }
        }
    }
}
