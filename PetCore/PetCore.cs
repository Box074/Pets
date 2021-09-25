using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modding;

namespace PetCore
{
    class PetCore : Mod
    {
        public override void Initialize()
        {
            On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.OnEnter += SpawnObjectFromGlobalPool_OnEnter;
            On.ObjectPool.Recycle_GameObject_GameObject += ObjectPool_Recycle_GameObject_GameObject;
            On.ObjectPool.Recycle_GameObject += ObjectPool_Recycle_GameObject;
        }

        private void ObjectPool_Recycle_GameObject(On.ObjectPool.orig_Recycle_GameObject orig, UnityEngine.GameObject obj)
        {
            if (obj == null) return;
            if (obj.GetComponent<AlwaysDestroy>() != null)
            {
                UnityEngine.Object.Destroy(obj);
                return;
            }
            orig(obj);
        }

        private void ObjectPool_Recycle_GameObject_GameObject(On.ObjectPool.orig_Recycle_GameObject_GameObject orig, 
            UnityEngine.GameObject obj, UnityEngine.GameObject prefab)
        {
            if (obj == null) return;
            if (obj.GetComponent<AlwaysDestroy>() != null)
            {
                UnityEngine.Object.Destroy(obj);
                return;
            }
            orig(obj, prefab);
        }

        private void SpawnObjectFromGlobalPool_OnEnter(On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.orig_OnEnter orig, 
            HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool self)
        {
            orig(self);
            self.storeObject.Value.AddComponent<AlwaysDestroy>();
            self.storeObject.Value = self.Fsm.GameObject.GetComponent<SpawnControl>()?.TrySpawnObject(self.storeObject.Value, self)
                ?? self.storeObject.Value;
        }
    }
}
