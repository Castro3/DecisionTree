using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class Node
    {
        private List<Node> children;
        private string name;
        private string choose;

        internal List<Node> Children
        {
            get
            {
                return children;
            }

            set
            {
                children = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Choose
        {
            get
            {
                return choose;
            }

            set
            {
                choose = value;
            }
        }

        public Node(string name)
        {
            this.name = name;
            children = new List<Node>();
        }

    }
}
