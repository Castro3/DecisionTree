using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class TreeMaker
    {
        #region variables
        private int counter;

        private double log2 = Math.Round(1 / Math.Log10(2),5);
        #endregion

        /// <summary>
        /// Calculate the entropy of between 2 variables
        /// </summary>
        /// <param name="k"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private double info(double k, double p)
        {
            if(k!=0 && p!=0)
            {
                double sum = k + p;
                double p1 = Math.Round((-k / sum) * Math.Log(k / sum),5);
                double p2 = Math.Round((-p / sum) * Math.Log(p / sum),5);
                return log2 * (p1 + p2);
            }
            return 0;
        }

        /// <summary>
        /// Get a new dataset by removing all data with a variable equals to var at the column index
        /// </summary>
        /// <param name="data">the current dataset</param>
        /// <param name="var">the variable</param>
        /// <param name="index">the index of the column</param>
        /// <returns>returns a new dataset</returns>
        private List<string[]> getNewData(List<string[]> data, string var, int index)
        {
            List<string> newStr;
            List<string[]> newData = new List<string[]>();
            foreach (string[] strList in data)
            {
                if (strList[index] == var)
                {
                    newStr = new List<string>();
                    string[] newStrA = new string[data[0].Length - 1];
                    for (int i = 0; i < data[0].Length; i++)
                    {
                        if (i != index)
                        {
                            newStr.Add(strList[i]);
                        }
                    }
                    newStrA = newStr.ToArray();
                    if(newStrA != null)
                    {
                        newData.Add(newStrA);
                    }
                }
            }
            return newData;
        }

        /// <summary>
        /// get a the list of variables of a column in a dataset
        /// </summary>
        /// <param name="data">the current dataset</param>
        /// <param name="col">the index of the column</param>
        /// <returns></returns>
        private List<string> getVariables(List<string[]> data, int col)
        {
            List<string> variables = new List<string>();
            for (int i = 0; i < data.Count; i++)
            {
                if(data[i] != null)
                {
                    if (!variables.Contains(data[i][col]))
                    {
                        variables.Add(data[i][col]);
                    }
                }
            }
            return variables;
        }

        /// <summary>
        /// get a new list of names by removing the name of a column already use in the algorithm
        /// </summary>
        /// <param name="name">list of names</param>
        /// <param name="col">the index of the column</param>
        /// <returns></returns>
        private List<string> getNewName(List<string> name, int col)
        {
            List<string> newName = name;
            newName.RemoveAt(col);
            return newName;
        }

        /// <summary>
        /// Checks the dataset retreived if the result contains a 100% of variables
        /// </summary>
        /// <param name="data">A dataset</param>
        /// <returns></returns>
        private bool checkData(List<string[]> data)
        {
            int nbline = data.Count;
            int nbcol = data[0].Length;
            string var = data[0][nbcol-1];
            foreach(string[] str in data)
            {
                if(str[nbcol-1] != var)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Count the number of element in a column[index] of a dataset that is equals to var 
        /// </summary>
        /// <param name="data">dataset</param>
        /// <param name="var">the variable</param>
        /// <param name="index">index of the column</param>
        /// <returns>the number of element of a column</returns>
        private int getCount(List<string[]> data, string var, int index)
        {
            int count = 0;
            foreach(string[] strL in data)
            {
                if(strL[index] == var)
                {
                    count++;
                }
            }
            return count;
        }


        /// <summary>
        /// Count the number of element in a column[index] of a dataset that is equals to var having the result cl
        /// </summary>
        /// <param name="data">dataset</param>
        /// <param name="var">the variable of the element</param>
        /// <param name="cl">the class of the element</param>
        /// <param name="index">index of the column</param>
        /// <returns>returns the number of element</returns>
        private int getCount(List<string[]> data, string var,string cl, int index)
        {
            int count = 0;
            foreach (string[] strL in data)
            {
                if (strL[index] == var && strL[strL.Length-1] == cl)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Returns the value of information of a column in a dataset
        /// </summary>
        /// <param name="data">dataset</param>
        /// <param name="vars">the list of class</param>
        /// <param name="index">the index of the column</param>
        /// <returns>returns the value of information of a column</returns>
        private double getInfo(List<string[]> data, List<string> vars, int index)
        {
            List<string> colVar = getVariables(data, index);
            List<double> calculList = new List<double>();
            List<int> count = new List<int>();
            double sum = 0;
            double x = 0;
            double y = 0;
            foreach (string cvar in colVar)
            {
                x = getCount(data, cvar, vars[0], index);
                y = getCount(data, cvar, vars[1], index);
                calculList.Add(info(x, y));
                count.Add(getCount(data, cvar, index));
                sum += getCount(data, cvar, index);
            }
            double infoSum = 0;
            for (int i = 0; i < calculList.Count; i++)
            {

                infoSum += calculList[i] * (double)count[i] / sum;
            }
            return infoSum;
        }

        /// <summary>
        /// Get the minimum value of information of the dataset
        /// </summary>
        /// <param name="data">dataset</param>
        /// <param name="name">list of column names</param>
        /// <returns>return the index of the column that has the minimum information value</returns>
        private int getMinInfo(List<string[]> data, List<string> name)
        {
            List<string> vars = getVariables(data, data[0].Length - 1);

            List<double> infoList = new List<double>();
            for (int i = 0; i < name.Count - 1; i++)
            {
                infoList.Add(getInfo(data, vars, i));
            }

            int index = 0;
            double info = infoList[0];
            for (int j = 1; j < infoList.Count; j++)
            {
                if (info > infoList[j])
                {
                    index = j;
                    info = infoList[j];
                }
            }

            return index;
        }

        /// <summary>
        /// return the node that may contains child node
        /// </summary>
        /// <param name="data">dataset</param>
        /// <param name="name">list of column names</param>
        /// <returns></returns>
        public Node getNode(List<string[]> data, List<string> name)
        {
            int index = getMinInfo(data, name);
            List<string> vars = getVariables(data, index);

            List<string[]> dataCopy = new List<string[]>(data);
            List<string> nameCopy = new List<string>(name);

            List<string> newName = new List<string>();
            List<string[]> newData = new List<string[]>();
            newName = getNewName(nameCopy, index);
            Node parent = new Node(name[index]);
            foreach(string var in vars)
            {
                newData = getNewData(dataCopy, var, index);
                if (newData.Count != 0)
                {
                    if (checkData(newData))
                    {
                        Node child = new Node(newData[0][newData[0].Length - 1]);
                        child.Choose = var;
                        parent.Children.Add(child);
                    }
                    else
                    {
                        if(newData[0].Length != 1)
                        {
                            Node child = getNode(newData, newName);
                            child.Choose = var;
                            parent.Children.Add(child);
                        }
                        else
                        {
                            string res = newData[0][newData[0].Length - 1];
                            int nbB = getCount(newData, res, newData[0].Length - 1);
                            int nbData = newData.Count;
                            double percentage = Math.Round((double)nbB*100.0 / nbData,1);
                            Node child = new Node("undecided with " + percentage + "% of " + res);
                            child.Choose = var;
                            parent.Children.Add(child);
                        }
                       
                    }
                }
            }
            return parent;
        }

        /// <summary>
        /// Show the node and all its child
        /// </summary>
        /// <param name="parent">the node to show</param>
        public void showNode(Node parent)
        {
            counter = 1;
            Console.Clear();
            Console.WriteLine(parent.Name);
            foreach (Node child in parent.Children)
            {
                showNode(child," ");
            }
        }

        /// <summary>
        /// Show the node and all its child and by adding spaces to show in the console who's child of who.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="add"></param>
        public void showNode(Node parent, string add)
        {
            Console.WriteLine(add+parent.Choose+"->"+parent.Name);
            
            foreach (Node child in parent.Children)
            {
                showNode(child,"  "+add);
                counter++;
                if (counter > 19)
                {
                    Console.WriteLine("Show more... press any key");
                    Console.ReadKey();
                    counter = 0;
                }  
            }
        }

        #region oldfunctions
        /// <summary>
        /// <depreciated/>
        /// function used to get the gain by using an initial entropy
        /// </summary>
        /// <param name="data">the dataset</param>
        /// <param name="vars">list of variables of a column</param>
        /// <param name="index">the index of the column</param>
        /// <param name="entropyInit">the initial entropy</param>
        /// <returns></returns>
        private double getGain(List<string[]> data, List<string> vars, int index, double entropyInit)
        {
            List<string> colVar = getVariables(data, index);
            List<double> calculList = new List<double>();
            List<int> count = new List<int>();
            double sum = 0;
            double x = 0;
            double y = 0;
            foreach (string cvar in colVar)
            {
                x = getCount(data, cvar, vars[0], index);
                y = getCount(data, cvar, vars[1], index);
                calculList.Add(info(x, y));
                count.Add(getCount(data, cvar, index));
                sum += getCount(data, cvar, index);
            }
            double infoSum = 0;
            for (int i = 0; i < calculList.Count; i++)
            {

                infoSum += calculList[i] * (double)count[i] / sum;
            }
            return entropyInit - infoSum;
        }

        /// <summary>
        /// <depreciated/>
        /// function used to get the maximum gain.
        /// </summary>
        /// <param name="data">the dataset</param>
        /// <param name="name"></param>
        /// <returns></returns>
        private int getMaxGain(List<string[]> data, List<string> name)
        {
            List<string> vars = getVariables(data, data[0].Length - 1);
            double x = getCount(data, vars[0], data[0].Length - 1);
            double y = getCount(data, vars[1], data[0].Length - 1);

            double entropyInit = info(x, y);

            List<double> gainList = new List<double>();
            for (int i = 0; i < name.Count - 1; i++)
            {
                gainList.Add(getGain(data, vars, i, entropyInit));
            }

            int index = 0;
            double gain = gainList[0];
            for (int j = 1; j < gainList.Count; j++)
            {
                if (gain < gainList[j])
                {
                    index = j;
                    gain = gainList[j];
                }
            }

            return index;
        }
        #endregion 

        #region test
        public void getNewDataTest()
        {
            List<string[]> data = new List<string[]>();
            string[] ex1 = { "A", "P", "N", "N", "P", "B" };
            string[] ex2 = { "P", "P", "P", "A", "A", "B" };
            string[] ex3 = { "P", "A", "A", "N", "P", "NB" };
            string[] ex4 = { "A", "N", "A", "P", "A", "B" };
            string[] ex5 = { "P", "P", "N", "N", "P", "NB" };

            data.Add(ex1);
            data.Add(ex2);
            data.Add(ex3);
            data.Add(ex4);
            data.Add(ex5);
            Console.WriteLine("Avant");
            foreach (string[] strL in data)
            {
                int i = 0;
                for (; i < strL.Length - 1; i++)
                {
                    Console.Write(strL[i] + ",");
                }
                Console.Write(strL[i++] + "\n");
            }
            Console.WriteLine("Après avoir enlevé tous les N au 4eme colonnes");
            List<string[]> newData = getNewData(data, "N", 3);
            foreach (string[] strL in newData)
            {
                int i = 0;
                for (; i < strL.Length - 1; i++)
                {
                    Console.Write(strL[i] + ",");
                }
                Console.Write(strL[i++] + "\n");
            }
        }

        public void getVariablesTest()
        {
            List<string[]> data = new List<string[]>();
            string[] ex1 = { "A", "P", "N", "N", "P", "B" };
            string[] ex2 = { "P", "P", "P", "A", "A", "B" };
            string[] ex3 = { "P", "A", "A", "N", "P", "NB" };
            string[] ex4 = { "A", "N", "A", "P", "A", "B" };
            string[] ex5 = { "P", "P", "N", "N", "P", "NB" };

            data.Add(ex1);
            data.Add(ex2);
            data.Add(ex3);
            data.Add(ex4);
            data.Add(ex5);

            List<string> vars = getVariables(data, 4);
            foreach (string var in vars)
            {
                Console.WriteLine(var);
            }
        }

        public void checkDataTest()
        {
            List<string[]> data = new List<string[]>();
            string[] ex1 = { "A", "P", "N", "A", "P", "B" };
            string[] ex2 = { "P", "P", "P", "A", "A", "B" };
            string[] ex3 = { "P", "A", "A", "N", "P", "NB" };
            string[] ex4 = { "A", "N", "A", "P", "A", "B" };
            string[] ex5 = { "P", "P", "N", "N", "P", "NB" };

            data.Add(ex1);
            data.Add(ex2);
            data.Add(ex3);
            data.Add(ex4);
            data.Add(ex5);
            if (!checkData(data))
            {
                Console.WriteLine("All class have not the same value");
            }
            Console.WriteLine("New Test");
            List<string[]> newData = getNewData(data, "N", 3);
            if (checkData(newData))
            {
                Console.WriteLine("All class have the same value");
            }
        }

        public void getMaxGainTest()
        {
            List<string[]> data = new List<string[]>();
            string[] ex1 = { "No", "3", "Yes", "Yes" };
            string[] ex2 = { "Yes", "3", "No", "No" };
            string[] ex3 = { "No", "4", "No", "Yes" };
            string[] ex4 = { "No", "3", "No", "No" };
            string[] ex5 = { "Yes", "4", "No", "Yes" };

            data.Add(ex1);
            data.Add(ex2);
            data.Add(ex3);
            data.Add(ex4);
            data.Add(ex5);

            List<string> name = new List<string>();
            name.Add("Furniture");
            name.Add("Nr rooms");
            name.Add("New Kitchen");
            name.Add("Rent");
            int index = getMaxGain(data, name);
            Console.WriteLine("Gain index:" + index);
            Console.WriteLine("Name:" + name[index]);
        }

        public void getNewNameTest()
        {
            List<string> name = new List<string>();
            name.Add("Industrial Risk");
            name.Add("Management Risk");
            name.Add("Financial Flexibility");
            name.Add("Credibility");
            name.Add("Competitiveness");
            name.Add("Operating Risk");
            name.Add("Class");


            Console.WriteLine("Before");
            foreach (string str in name)
            {
                Console.WriteLine(str);
            }
            Console.WriteLine("After credibility removed");
            List<string> newName = getNewName(name, 3);
            foreach (string str in newName)
            {
                Console.WriteLine(str);
            }
        }
        #endregion
    }

}
