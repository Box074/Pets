using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Modding;
using UnityEngine;

namespace PetCore
{
    public abstract class PetMod : Mod, ITogglableMod
    {
        public GameObject Pet { get; protected set; } = null;
        bool isEnable = false;
		public override string GetVersion()
		{
			AssemblyName name = GetType().Assembly.GetName();
			return name.Version.ToString();
		}
        public override void Initialize()
        {
            isEnable = true;
            ModHooks.HeroUpdateHook -= ModHooks_HeroUpdateHook;
            ModHooks.HeroUpdateHook += ModHooks_HeroUpdateHook;
        }
        protected void SetPet(GameObject go,Type control)
        {
            if (go == null || Pet != null) return;
            if (go.GetComponent<ConstrainPosition>() != null) UnityEngine.Object.Destroy(go.GetComponent<ConstrainPosition>());
            go.transform.parent = null;
            UnityEngine.Object.DontDestroyOnLoad(go);
            go.AddComponent<PetControl>();
            (go.AddComponent(control) as Pet)?.CallInit();
            go.SetActive(false);
            Pet = go;
        }
        protected void SetPet<T>(GameObject go) where T : Pet => SetPet(go, typeof(T));

        private void ModHooks_HeroUpdateHook()
        {
            if (isEnable)
            {
                if (Pet != null)
                {
                    if (HeroController.instance != null)
                    {
                        if (!Pet.activeSelf) Pet.SetActive(true);
                    }
                    else
                    {
                        Pet.SetActive(false);
                    }
                }
            }
        }

        public void Unload()
        {
            ModHooks.HeroUpdateHook -= ModHooks_HeroUpdateHook;
            Pet?.SetActive(false);
            isEnable = false;
        }
    }
}
