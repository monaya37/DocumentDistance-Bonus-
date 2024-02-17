using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;

namespace DocumentDistance
{
   
    class DocDistance
    {
        // *****************************************
        // DON'T CHANGE CLASS OR FUNCTION NAME
        // YOU CAN ADD FUNCTIONS IF YOU NEED TO
        // *****************************************
        /// <summary>
        /// Write an efficient algorithm to calculate the distance between two documents
        /// </summary>
        /// <param name="doc1FilePath">File path of 1st document</param>
        /// <param name="doc2FilePath">File path of 2nd document</param>
        /// <returns>The angle (in degree) between the 2 documents</returns>
        /// 


        


        //Semaphores
        static SemaphoreSlim dectionariesSemaphore = new SemaphoreSlim(1);

        //Dictionaries
        public static Dictionary <string, double> d1_words = new Dictionary<string, double>();
        public static Dictionary <string, double> d2_words = new Dictionary<string, double>();

        public static string doc1;
        public static string doc2;
        public static double CalculateDistance(string doc1FilePath, string doc2FilePath)
        {
            //Threads
             Thread thread1 = new Thread(new ThreadStart(doc1Function));
             Thread thread2 = new Thread(new ThreadStart(doc2Function));

            //READ FILES
            doc1 = File.ReadAllText(doc1FilePath);
            doc2 = File.ReadAllText(doc2FilePath);


            thread1.Start();
            thread2.Start();


            while (thread1.IsAlive || thread2.IsAlive) ;

            if (d1_words.ContainsKey(String.Empty))  d1_words.Remove(String.Empty);
            if (d2_words.ContainsKey(String.Empty))  d2_words.Remove(String.Empty);

            double sum1 = d1_words.Sum(x => Math.Pow(x.Value, 2));
            double sum2 = d2_words.Sum(x => Math.Pow(x.Value, 2));
            double d1Xd2 = Math.Sqrt(sum1 * sum2);



            double d1Dotd2 = 0;
            foreach (var m in d1_words)
            {
                d1Dotd2 += (d2_words[m.Key] * d1_words[m.Key]);
                //Console.WriteLine("{0} --> {1}", m.Key, m.Value);
            }

            //foreach (var m in d2_words)
            //{
            //    //d1Dotd2 += (d2_words[m.Key] * d1_words[m.Key]);
            //    Console.WriteLine("{0} --> {1}", m.Key, m.Value);
            //}

            //Console.WriteLine();
            //Console.WriteLine(d1_words.Count);
            //Console.WriteLine(d2_words.Count);

            var answer = Math.Acos(d1Dotd2 / d1Xd2) * 180 / Math.PI;
            d1_words.Clear();
            d2_words.Clear();

            return answer;
        }
        
        public static void doc1Function()
        {

            //f1Semaphore.Wait();

            string word = "";

            doc1 += '#';
            foreach (char letter in doc1)
            {
                if (Char.IsLetterOrDigit(letter))
                    word += letter;

                else
                {
                    dectionariesSemaphore.Wait();
                    {
                        if (!d1_words.ContainsKey(word.ToLower()))
                        {
                            d1_words.Add(word.ToLower(), 1);
                            if (!d2_words.ContainsKey(word.ToLower()))
                            {
                                d2_words.Add(word.ToLower(), 0);
                            }
                            
                        }
                        else
                            d1_words[word.ToLower()]++;
                    }
                    dectionariesSemaphore.Release();
                    word = "";
                }
            }


        }

        private static void doc2Function()
        {
            //f2Semaphore.Wait();
            string word = "";
            doc2 += '#';
            foreach (char letter in doc2)
            {
                if (Char.IsLetterOrDigit(letter))
                {
                    word += letter;
                }
                else
                {
                    dectionariesSemaphore.Wait();
                    {
                        //if it is in array2 then it must be in array 1
                        if (d2_words.ContainsKey(word.ToLower()))
                            d2_words[word.ToLower()]++;
                        else
                        {
                            d2_words.Add(word.ToLower(), 1);
                            if (!d1_words.ContainsKey(word.ToLower()))
                                d1_words.Add(word.ToLower(), 0);
                        }
                    }
                    dectionariesSemaphore.Release();
                    word = "";
                }
            }
           f2Semaphore.Release();
        }
    }
}
