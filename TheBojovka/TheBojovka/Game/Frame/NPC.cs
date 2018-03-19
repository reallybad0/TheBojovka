using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
namespace TheBojovka
{
    [DelimitedRecord(";")]
    public class NPC
    {
        //ID scény
        public int ID;
        public int Level;
        public string Name;
        public int HP;
        public int Defense;

        public NPC()
        {
        }
        public NPC(int iD, string name, int hP, int defense,int level)
        {
            ID = iD;
            Name = name;
            HP = hP;
            Defense = defense;
            Level = level;
        }
    }
}