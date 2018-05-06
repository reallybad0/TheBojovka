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
        public int Value;
        public int Type;
        /*
        TYPES:
        0 - edible
        1 - exchangable 
             
        */
        public Item() { }

        public Item(int iD, string name, string description, int value, int type)
        {
            ID = iD;
            Name = name;
            Description = description;
            Value = value;
            Type = type;
        }

        public override string ToString()
        {
            return Name;
            //return base.ToString();
        }
    }
}
