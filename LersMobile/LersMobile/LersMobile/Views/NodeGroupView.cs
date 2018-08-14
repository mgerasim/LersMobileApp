using System;
using System.Collections.Generic;
using System.Text;

namespace LersMobile.Views
{
    public class NodeGroupView
    {
        public NodeGroupView(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; protected set; }
        public string Name { get; protected set; }
    }
}
