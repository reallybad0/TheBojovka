using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
namespace TheBojovka
{
    [DelimitedRecord(";")]
    public class Item
    {
        public int ID;
        public string Name;
        public string Description;
        public bool Equipped;
        public Item() { }
        Item(int iD, string name, string description,bool equipped)
        {
            ID = iD;
            Name = name;
            Description = description;
            Equipped = equipped;
        }
        public override string ToString()
        {
            return Name;
            //return base.ToString();
        }
    }
}
