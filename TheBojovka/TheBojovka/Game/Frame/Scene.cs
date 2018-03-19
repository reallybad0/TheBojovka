using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
namespace TheBojovka
{
    [DelimitedRecord(";")]
    public class Scene 
    {
       
        public int ID;
        public string Description;
        public int Type;
        public int OptionCount;
        public Scene()
        {

        }
        public Scene(int iD, string description, int type, int optionCount)
        {
            ID = iD;
            Description = description;
            Type = type;
            OptionCount = optionCount;
        }
    }
}
