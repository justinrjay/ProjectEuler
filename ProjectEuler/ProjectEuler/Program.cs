using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ProjectEuler
{
    class Program
    {
        public struct NameScores
        {
            public string name;
            public int score;
            public long tscore;
        }
        static void Main(string[] args)
        {
            //Console.WriteLine(Esieve(600851475143));
            //Console.WriteLine(Esieve2());
            //Console.WriteLine(LargestProduct());
            //Console.WriteLine(LargestProduct2());
            //PowerSum();
            //Console.WriteLine(Lattice(10));
            //Console.WriteLine(FindFactorial(10));
            //Console.WriteLine(SumStringDigits(FindFactorialString(1000)));
            List<NameScores> scorelist = new List<NameScores>();
            MatchCollection mc = ReadFile("p022_names.txt", "\"*,*\"(?<names>\\w+)");
            foreach (Match newmatch in mc)
            {
                NameScores nc = new NameScores();
                nc.name = newmatch.Groups["names"].Value;
                nc.score = CalculateNameScore(newmatch.Groups["names"].Value);
                scorelist.Add(nc);
            }
            scorelist.Sort((x, y) => x.name.CompareTo(y.name));
            for (int i = 0; i < scorelist.Count; i++)
            {
                NameScores totaled = new NameScores();
                totaled.name = scorelist[i].name;
                totaled.score = scorelist[i].score;
                totaled.tscore = (i + 1) * scorelist[i].score;
                scorelist[i] = totaled;
            }
            long totalscore = 0;
            foreach (NameScores nc in scorelist)
            {
                totalscore += nc.tscore;
            }
            Console.WriteLine(totalscore);
        }

        static int Esieve(long number)
        {
            long sqrrt = (long)Math.Round(Math.Sqrt(number));
            List<int> numlist = new List<int>();
            int x = 2;
            while (x < sqrrt)
            {
                numlist.Add(x);
                int y = 0;
                while (numlist.Contains(x) && (y < numlist.Count-1))
                {
                    if (x % numlist[y] == 0)
                    {
                        numlist.Remove(x);
                    }
                    else
                    {
                        y++;
                    }
                }
                if (number % x != 0)
                {
                    numlist.Remove(x);
                }
                x++;
            }
            return numlist[numlist.Count - 1];
        }

        static int Esieve2()
        {
            List<int> numlist = new List<int>();
            numlist.Add(2);
            int x = 3;
            while (numlist.Count != 10001)
            {
                numlist.Add(x);
                int y = 0;
                while (numlist.Contains(x) && (numlist[y] <= Math.Sqrt(x)))
                {
                    if (x % numlist[y] == 0)
                    {
                        numlist.Remove(x);
                    }
                    else
                    {
                        y++;
                    }
                }
                x += 2;
            }
            return numlist[numlist.Count - 1];
        }

        static double LargestProduct()
        {
            string numstring = "7316717653133062491922511967442657474235534919493496983520312774506326239578318016984801869478851843858615607891129494954595017379583319528532088055111254069874715852386305071569329096329522744304355766896648950445244523161731856403098711121722383113622298934233803081353362766142828064444866452387493035890729629049156044077239071381051585930796086670172427121883998797908792274921901699720888093776657273330010533678812202354218097512545405947522435258490771167055601360483958644670632441572215539753697817977846174064955149290862569321978468622482839722413756570560574902614079729686524145351004748216637048440319989000889524345065854122758866688116427171479924442928230863465674813919123162824586178664583591245665294765456828489128831426076900422421902267105562632111110937054421750694165896040807198403850962455444362981230987879927244284909188845801561660979191338754992005240636899125607176060588611646710940507754100225698315520005593572972571636269561882670428252483600823257530420752963450";
            int x = 0;
            
            List<string> splitstrings = new List<string>();
            while (x < numstring.Length - 14)
            {
                int y = x + 1;
                if (!numstring.Substring(y, 13).Contains('0'))
                {
                    splitstrings.Add(numstring.Substring(y, 13));
                }
                x++;
            }
            x = 0;
            while (x < splitstrings.Count)
            {
                int y = 0;
                List<char> charlist = new List<char>();
                while (y < splitstrings[x].Length)
                {
                    charlist.Add(splitstrings[x][y]);
                    y++;
                }
                charlist.Sort();
                splitstrings[x] = "";
                foreach (char c in charlist)
                {
                    splitstrings[x] += c;
                }
                x++;
            }
            splitstrings.Sort();
            double returnval = 1;
            foreach (char c in splitstrings[splitstrings.Count - 1])
            {
                returnval *= char.GetNumericValue(c);
            }
            return returnval;
        }

        static double LargestProduct2()
        {
            string numstring = "7316717653133062491922511967442657474235534919493496983520312774506326239578318016984801869478851843858615607891129494954595017379583319528532088055111254069874715852386305071569329096329522744304355766896648950445244523161731856403098711121722383113622298934233803081353362766142828064444866452387493035890729629049156044077239071381051585930796086670172427121883998797908792274921901699720888093776657273330010533678812202354218097512545405947522435258490771167055601360483958644670632441572215539753697817977846174064955149290862569321978468622482839722413756570560574902614079729686524145351004748216637048440319989000889524345065854122758866688116427171479924442928230863465674813919123162824586178664583591245665294765456828489128831426076900422421902267105562632111110937054421750694165896040807198403850962455444362981230987879927244284909188845801561660979191338754992005240636899125607176060588611646710940507754100225698315520005593572972571636269561882670428252483600823257530420752963450";
            int x = 0;

            List<char[]> splitstrings = new List<char[]>();
            while (x < numstring.Length - 14)
            {
                int y = x + 1;
                if (!numstring.Substring(y, 13).Contains('0'))
                {
                    char[] charsplit = new char[13];
                    int z = 0;
                    while (z < numstring.Substring(y, 13).Length)
                    {
                        charsplit[z] = numstring[z];
                        z++;
                    }
                    Array.Sort(charsplit);
                    splitstrings.Add(charsplit);
                }
                x++;
            }
            x = 0;
            splitstrings.Sort();
            double returnval = 1;
            foreach (char c in splitstrings[splitstrings.Count - 1])
            {
                returnval *= char.GetNumericValue(c);
            }
            return returnval;
        }

        static void PowerSum()
        {
            long x = 0;
            double previousval = 0;
            double powsumtest = 1;
            string binarystring = "";
            while (x < 15)
            {
                binarystring += "1";

                //long newnum = (long)Math.Pow(2, x);
                //string newnumstring = newnum.ToString();
                //double powsum = 0;
                //Console.WriteLine(newnum);
                //foreach (char c in newnumstring)
                //{
                //    powsum += (double)char.GetNumericValue(c);
                //}
                ////Console.WriteLine(powsum.ToString() + " " + newnumstring + " " + (powsum - previousval).ToString());
                //if (x % 4 == 0)
                //{
                //    //Console.WriteLine(newnum.ToString("x"));
                        
                //}

                //previousval = powsum;
                x++;
            }
            long test = 0;
            x = 0;
            while (x < binarystring.Length)
            {
                test = (long)Math.Pow(2, x);
                x++;
            }
            //Console.WriteLine(test);
            //Console.WriteLine(Math.Pow(2, 30));
        }

        static long Lattice(int grid)
        {
            int topfact = grid + 1;
            int bottomfact = grid / 2;
            long numpaths = 1;
            while (topfact <= grid * 2)
            {
                numpaths *= topfact % 2 == 0 ? 2 : topfact;
                topfact++;
            }
            while (bottomfact > 1)
            {
                numpaths /= bottomfact;
                bottomfact--;
            }
            return numpaths;
        }

        static string FindFactorial(int factorial)
        {
            int x = factorial - 1;
            string factstring = factorial.ToString();
            string resultstring = "";
            while (x >= 2)
            {
                int singledigit = 1;
                int remainder = 0;
                for (int i = 0; i < x.ToString().Length; i++)
                {
                    
                    for (int j = 0; j < factstring.ToString().Length; j++)
                    {
                        singledigit = (int)Char.GetNumericValue(factstring[j]) * x + remainder;
                        
                        singledigit = singledigit % 10;
                    }
                    
                }
                if (remainder > 0) { resultstring = remainder.ToString() + resultstring; }
                x--;
            }
            return resultstring;
        }

        static string MultiplyString(string val1, string val2)
        {
            if (val2.Length > val1.Length)
            {
                string temp = val1;
                val1 = val2;
                val2 = temp;
            }
            string value = "0";
            int trailingzero = 0;
            int remainder = 0;
            for (int i = val2.Length - 1; i >= 0; i--)
            {
                int singledigit = 1;
                string newval = "";
                for (int j = val1.Length - 1; j >= 0; j--)
                {
                    singledigit = (int)Char.GetNumericValue(val2[i]) * (int)Char.GetNumericValue(val1[j]) + remainder;
                    remainder = singledigit / 10;
                    singledigit = singledigit % 10;
                    newval = singledigit.ToString() + newval;
                }
                int k = trailingzero;
                while (k > 0)
                {
                    newval += "0";
                    k--;
                }
                trailingzero++;
                newval = remainder > 0 ? remainder.ToString() + newval : newval;
                remainder = 0;
                value = AddNumberString(newval, value);
            }
            return value;
        }

        static string AddNumberString(string val1, string val2)
        {
            int remainder = 0;
            if (val2.Length > val1.Length)
            {
                string temp = val1;
                val1 = val2;
                val2 = temp;
            }
            int x = val1.Length - 1;
            int y = val2.Length - 1;
            string disp = "";
            while (x >= 0)
            {
                int singledigit = remainder;
                if (y >= 0)
                {
                    singledigit += (int)Char.GetNumericValue(val1[x]) + (int)Char.GetNumericValue(val2[y]);
                    remainder = singledigit / 10;
                    singledigit = singledigit % 10;
                }
                else
                {
                    singledigit += (int)Char.GetNumericValue(val1[x]);
                    remainder = singledigit / 10;
                    singledigit = singledigit % 10;
                }
                disp = singledigit.ToString() + disp;
                x--;
                y--;
            }
            disp = remainder > 0 ? remainder + disp : disp;
            return disp;
        }

        static string FindFactorialString(int startval)
        {
            string multstring = startval.ToString();
            while (startval > 2)
            {
                int y = startval - 1;
                multstring = MultiplyString(multstring, y.ToString());
                startval--;
            }
            return multstring;
        }

        static int SumStringDigits(string digitstring)
        {
            int sum = 0;
            foreach (char c in digitstring)
            {
                sum += (int)Char.GetNumericValue(c);
            }
            return sum;
        }

        static MatchCollection ReadFile(string filename, string regexcondition)
        {
            string readdata = File.ReadAllText(filename);
            Regex newregex = new Regex(regexcondition);
            return newregex.Matches(readdata);
        }

        static int CalculateNameScore(string name)
        {
            name = name.ToLower();
            int sum = 0;
            for (int i = 0; i < name.Length; i++)
            {
                sum += name[i] == 'a' ? 1 : name[i] == 'b' ? 2 : name[i] == 'c' ? 3 : name[i] == 'd' ? 4 : name[i] == 'e' ? 5 : name[i] == 'f' ? 6 : name[i] == 'g' ? 7 : name[i] == 'h' ? 8 : name[i] == 'i' ? 9 : name[i] == 'j' ? 10 : name[i] == 'k' ? 11 : name[i] == 'l' ? 12 : name[i] == 'm' ? 13 : name[i] == 'n' ? 14 : name[i] == 'o' ? 15 : name[i] == 'p' ? 16 : name[i] == 'q' ? 17 : name[i] == 'r' ? 18 : name[i] == 's' ? 19 : name[i] == 't' ? 20 : name[i] == 'u' ? 21 : name[i] == 'v' ? 22 : name[i] == 'w' ? 23 : name[i] == 'x' ? 24 : name[i] == 'y' ? 25 : 26; 
            }
            return sum;
        }
    }

    
    
}
