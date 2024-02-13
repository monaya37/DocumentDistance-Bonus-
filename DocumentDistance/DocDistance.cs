﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;

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
        public static double CalculateDistance(string doc1FilePath, string doc2FilePath)
        {
 
            //READ FILES
            string d1 = File.ReadAllText(doc1FilePath);
            string d2 = File.ReadAllText(doc2FilePath);

            //Dictionaries
            Dictionary <string, int> d1_words = new Dictionary<string, int>();
            Dictionary <string, int> d2_words = new Dictionary<string, int>();

            //(The privilage of being a CS Student)
            //REGULAR EXPRESSIONS AND MATCHES
            string s = "[A-Za-z0-9]+";
            Regex re = new Regex(s);
            var m1 = Regex.Matches(d1, s);
            var m2 = Regex.Matches(d2, s);

            Console.WriteLine("THE LIST");
            foreach(Match m in m1)
            {
               
                if (!d1_words.ContainsKey(m.Value.ToLower()))
                    d1_words.Add(m.Value.ToLower(), 1);
                else
                    d1_words[m.Value.ToLower()]++;
            }
            

            foreach (Match m in m2)
            {
                if (!d2_words.ContainsKey(m.Value.ToLower()))
                    d2_words.Add(m.Value.ToLower(), 1);
                else
                    d2_words[m.Value.ToLower()]++;
            }

            var sum1 = d1_words.Sum(x => x.Value^2);
            var sum2 = d1_words.Sum(x => x.Value^2);
            var d1Xd2 = Math.Sqrt(sum1 * sum2);

            foreach (KeyValuePair<string, int> m in d1_words)
            {
                Console.WriteLine("{0} {1}", m.Key, m.Value);
            }
            Console.WriteLine("END OF THE LIST\n");

           // Console.Write("\nD1:\n");
            //Console.Write(d1);
            //Console.Write("\nD2:\n");
            //Console.Write(d2);
            // char[] seperators = { ' ', '\t', '\n', ',' };

            return 0;
        }
    }
}
