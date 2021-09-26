using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCore;
using UnityEngine;

namespace ElectricMage
{
    public class ElectricMageMod : PetMod
    {
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Room_Colosseum_Gold","Colosseum Manager/Waves/Wave 25/Electric Mage New")
            };
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Initialize();
            SetPet<EMPet>(preloadedObjects["Room_Colosseum_Gold"]["Colosseum Manager/Waves/Wave 25/Electric Mage New"]);
        }
    }
}
