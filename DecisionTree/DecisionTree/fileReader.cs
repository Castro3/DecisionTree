using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class fileReader
    {
        private int lineCount;
        private List<string[]> data;


        public List<string[]> getData
        {
            get
            {
                return data;
            }
        }

        public void getDataSet(string filename)
        {
            data = new List<String[]>();
            lineCount = 0;
            using (var reader = File.OpenText(filename))
            {
                while (reader.ReadLine() != null)
                {
                    lineCount++;
                }
            }

            string[] AllLines = new string[lineCount]; //only allocate memory here
            using (StreamReader sr = File.OpenText(filename))
            {
                int x = 0;
                while (!sr.EndOfStream)
                {
                    AllLines[x] = sr.ReadLine();
                    x += 1;
                }
            } //CLOSE THE FILE because we are now DONE with it.
            Parallel.For(0, AllLines.Length, x =>
            {
                data.Add(AllLines[x].Split(','));
            });


        }

        public List<string> getNameSet()
        {
            Console.Clear();
            Console.WriteLine("Please insert the name of each column");
            List<string> nameSet = new List<string>();
            for(int i = 0; i< data[0].Length; i++)
            {
                Console.WriteLine("Column n°"+i);
                string name = Console.ReadLine();
                nameSet.Add(name);
            }
            return nameSet;
        }
    }
}
