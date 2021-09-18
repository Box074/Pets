using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCore;
using UnityEngine;

namespace Radiance
{
    public class RadianceMod : PetMod
    {
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>()
            {
                ("GG_Radiance","Boss Control/Absolute Radiance")
            };
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Initialize();
            SetPet<RadiancePet>(preloadedObjects["GG_Radiance"]["Boss Control/Absolute Radiance"]);
            
        }
    }
}
