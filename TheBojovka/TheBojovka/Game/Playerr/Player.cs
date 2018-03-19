using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
namespace TheBojovka
{
    [DelimitedRecord(";")]
    public class Player
    {
        public string Name;
        public int hp;
        public int Level;
        public int FrameID;
        //public List<int> itemsID;
      //  public List<Item> items;

        public Player() { }

        public Player(string name, int hp, int level, int frameID)
        {
            Name = name;
            this.hp = hp;
            Level = level;
            FrameID = frameID;
            
        }
    }
}
