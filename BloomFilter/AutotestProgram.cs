﻿using System;
using System.Collections;

namespace AlgorithmsDataStructures
{
    public class BloomFilter
    {
        public int filter_len;
        public BitArray bloomFilter;

        public BloomFilter(int f_len)
        {
            filter_len = f_len; // для тестов передавать 32
            bloomFilter = new BitArray(filter_len);
        }

        // хэш-функции
        public int Hash1(string str1)
        {
            const int MULTIPLIER = 17; // 17 - для тестов, в общем случае должно быть случайное число
            int code = 0;

            for (int i = 0; i < str1.Length; i++)
            {
                code = (code * MULTIPLIER + (int)str1[i]) % filter_len;
            }

            return code;
        }

        public int Hash2(string str1)
        {
            const int MULTIPLIER = 223; // 223 - для тестов, в общем случае должно быть случайное число
            int code = 0;

            for (int i = 0; i < str1.Length; i++)
            {
                code = (code * MULTIPLIER + (int)str1[i]) % filter_len;
            }

            return code;
        }

        public void Add(string str1)
        {
            bloomFilter.Set(Hash1(str1), true);
            bloomFilter.Set(Hash2(str1), true);
        }

        public bool IsValue(string str1)
        {
            Console.WriteLine("\"{0}\":\tHash1\t{1}\tHash2\t{2}", str1, Hash1(str1), Hash2(str1));
            if (bloomFilter.Get(Hash1(str1)) && bloomFilter.Get(Hash2(str1))) return true;

            return false;
        }

        public static void PrintIndexAndValues(IEnumerable myCol)
        {
            int i = 0;
            foreach (Object obj in myCol)
            {
                Console.WriteLine("    [{0}]:    {1}", i++, obj);
            }
            Console.WriteLine();
        }
    }
}