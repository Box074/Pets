using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCore;
using UnityEngine;

namespace GhostMarkoth
{
    public class MarkothMod : PetMod
    {
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Ghost_Markoth","Warrior/Ghost Warrior Markoth")
            };
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Initialize();
            SetPet<MarkothPet>(preloadedObjects["GG_Ghost_Markoth"]["Warrior/Ghost Warrior Markoth"]);
        }
    }
}
