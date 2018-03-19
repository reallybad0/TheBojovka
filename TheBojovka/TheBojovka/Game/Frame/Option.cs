using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
namespace TheBojovka
{
    [DelimitedRecord(";")]
    
    public class Option
    {
        public int ID;
        public int Scene_ID;
        public string Description;
        public int FollowingScene;

        public Option()
        {

        }
        public Option(int iD, int scene_ID, string description, int followingScene)
        {
            ID = iD;
            Scene_ID = scene_ID;
            Description = description;
            FollowingScene = followingScene;
        }
    }
}
