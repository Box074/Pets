using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCore;
using UnityEngine;

namespace MantisHeavyFlyer
{
    public class MantisHeavyFlyer : PetMod
    {
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Deepnest_43","Mantis Heavy Flyer")
            };
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            if (Init()) SetPet<MHFPet>(preloadedObjects["Deepnest_43"]["Mantis Heavy Flyer"]);
        }
    }
}
