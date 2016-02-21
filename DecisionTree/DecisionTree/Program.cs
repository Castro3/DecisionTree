using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class Program
    {

        

        static void Main(string[] args)
        {

            TreeMaker myTree = new TreeMaker();
            bool fileCharged = true;
            //myTree.getNewDataTest();
            //myTree.getVariablesTest();
            //myTree.getNewNameTest();
            //myTree.checkDataTest();
            //myTree.getMaxGainTest();
            Console.WriteLine("Welcome to decision Tree maker\nPlease type the filename with its extension");
            fileReader file = new fileReader();
            string filename = Console.ReadLine();
            try
            {
                file.getDataSet(filename);
                
            }
            catch(Exception e)
            {
                fileCharged = false;
                Console.WriteLine("Your file is not valid");
            }
            
            /*
            List<string> name = new List<string>();
            name.Add("Industrial Risk");
            name.Add("Management Risk");
            name.Add("Financial Flexibility");
            name.Add("Credibility");
            name.Add("Competitiveness");
            name.Add("Operating Risk");
            name.Add("Class");
            
            List<string[]> dataList = new List<string[]>();
            string[] ex1 = { "No", "3", "Yes", "Yes" };
            string[] ex2 = { "Yes", "3", "No", "No" };
            string[] ex3 = { "No", "4", "No", "Yes" };
            string[] ex4 = { "No", "3", "No", "No" };
            string[] ex5 = { "Yes", "4", "No", "Yes" };

            dataList.Add(ex1);
            dataList.Add(ex2);
            dataList.Add(ex3);
            dataList.Add(ex4);
            dataList.Add(ex5);

            
            List<string> nameList = new List<string>();
            nameList.Add("Furniture");
            nameList.Add("Nr rooms");
            nameList.Add("New Kitchen");
            nameList.Add("Rent");
            */
            if(fileCharged)
            {
                try
                {
                    List<string[]> data = file.getData;
                    List<string> name = file.getNameSet();
                    Node root = myTree.getNode(data, name);
                    myTree.showNode(root);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Data input has not finished loading");
                }
            }
            
            Console.ReadKey();

        }

    }
}
