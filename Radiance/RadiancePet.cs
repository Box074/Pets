using System.Collections;
using UnityEngine;
using PetCore;
using ModCommon;
using ModCommon.Util;
using HutongGames.PlayMaker.Actions;

namespace Radiance
{
    public class RadiancePet : Pet
    {
        PlayMakerFSM ac = null;
        bool attackEnd = true;
        protected override void Init()
        {
            Destroy(gameObject.LocateMyFSM("Control"));
            Destroy(gameObject.LocateMyFSM("Phase Control"));
            Destroy(gameObject.LocateMyFSM("Attack Choices"));
            ac = gameObject.LocateMyFSM("Attack Commands");

            ac.InsertMethod("Orb End", 0, EndAttack);

            Control.RegisterAction("ORB FIRE", OrbFire, AttackEnd);
        }
        
        public bool AttackEnd() => attackEnd;
        void AttackBegin() => attackEnd = false;
        void EndAttack() => attackEnd = true;

        IEnumerator OrbFire()
        {
            yield return null;
            ac.FsmVariables.FindFsmFloat("Orb Max X").Value = transform.position.x + 5;
            ac.FsmVariables.FindFsmFloat("Orb Min X").Value = transform.position.x - 5;
            ac.FsmVariables.FindFsmFloat("Orb Max Y").Value = transform.position.y + 5;
            ac.FsmVariables.FindFsmFloat("Orb Min Y").Value = transform.position.y - 5;
            FSMUtility.SendEventToGameObject(gameObject, "ORBS");
        }

        public override GameObject TrySpawnObject(GameObject go, SpawnObjectFromGlobalPool state)
        {
            if(state.State.Name == "Spawn Fireball")
            {
                TargetFinder tf = go.AddComponent<TargetFinder>();
                tf.Bind(
                    go.GetFSMActionOnState<ChaseObjectV2>("Chase Hero").target,
                    go.GetFSMActionOnState<ChaseObjectV2>("Chase Hero 2").target
                    );
                go.AddComponent<Orb>();
            }
            else
            {
                return go;
            }
            return go;
        }
    }
}