using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCore;
using UnityEngine;

namespace GhostSlug
{
    public class GhostSlugMod : PetMod
    {
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Ghost_Gorb","Warrior/Ghost Warrior Slug")
            };
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            if (Init()) SetPet<SlugPet>(preloadedObjects["GG_Ghost_Gorb"]["Warrior/Ghost Warrior Slug"]);
        }
    }
}
