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
        GameObject OrbPrefab = null;
        GameObject beam = null;
        GameObject beamg = null;
        bool needTele = false;
        protected override void Init()
        {
            Destroy(gameObject.LocateMyFSM("Control"));
            Destroy(gameObject.LocateMyFSM("Phase Control"));
            Destroy(gameObject.LocateMyFSM("Attack Choices"));
            Destroy(gameObject.GetComponent<HealthManager>());
            ac = gameObject.LocateMyFSM("Attack Commands");

            OrbPrefab = gameObject.GetFSMActionOnState<SpawnObjectFromGlobalPool>("Spawn Fireball").gameObject.Value;
            beamg = ac.FsmVariables.FindFsmGameObject("Eye Beam Glow").Value;

            Control.RegisterAction("ORB FIRE", OrbFire);
            Control.RegisterAction("EYE BEAM", EyeBeam);
            Control.RegisterAction("CHOOSE ATTACK", ChooseAttack, () => !Control.IsActionInvoking("CHOOSE ATTACK"));
            Control.InvokeActionOn("CHOOSE ATTACK", () => true);
            

        }
        void OnEnable()
        {
            ac.enabled = true;
            beamg?.SetActive(false);
            transform.SetScaleX(0.3f);
            transform.SetScaleY(0.3f);
        }

        IEnumerator ChooseAttack()
        { 
            yield return null;
            if (needTele)
            {
                PlayMakerFSM tele = gameObject.LocateMyFSM("Teleport");
                tele.FsmVariables.FindFsmVector3("Destination").Value =
                    HeroController.instance.transform.position;
                FSMUtility.SendEventToGameObject(gameObject, "TELEPORT");
                yield return null;
                yield return new WaitWhile(() => tele.ActiveStateName != "Idle");
                needTele = false;
            }
            switch (Random.Range(0, 6))
            {
                case 0:
                case 1:
                case 2:
                    yield return Control.InvokeWait("EYE BEAM");
                    break;
                case 3:
                case 4:
                default:
                    yield return OrbFire();
                    break;
            }
            yield return new WaitForSeconds(0.75f);
        }
        IEnumerator EyeBeam()
        {

            yield return null;
            beam = ac.FsmVariables.FindFsmGameObject("Ascend Beam").Value;
            beamg.SetActive(true);
            beam.SetActive(true);
            beam.GetOrAddComponent<PetAttack>().AddChildren();
            GameObject go = TargetFinder.FindTarget(transform.position);
            if (go == null)
            {
                beamg.SetActive(false);
                yield break;
            }
            float angle = Mathf.Atan2(go.transform.position.y - transform.position.y,
                go.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            beam.transform.eulerAngles = new Vector3(0, 0, angle);
            FSMUtility.SendEventToGameObject(beam, "ANTIC");
            yield return new WaitForSeconds(0.5f);
            FSMUtility.SendEventToGameObject(beam, "FIRE");
            yield return new WaitForSeconds(0.3f);
            FSMUtility.SendEventToGameObject(beam, "END");
            yield return new WaitForSeconds(0.35f);
            beamg.SetActive(false);
        }
        IEnumerator OrbFire()
        {
            HutongGames.PlayMaker.FsmGameObject target = new HutongGames.PlayMaker.FsmGameObject();
            target.Value = TargetFinder.FindTarget(transform.position);
            if (target.Value == null) yield break;
            GameObject go = Instantiate(OrbPrefab);
            go.transform.position = transform.position;
            TargetFinder tf = go.AddComponent<TargetFinder>();
            
            
            go.GetFSMActionOnState<ChaseObjectV2>("Chase Hero").target = target;
            go.GetFSMActionOnState<ChaseObjectV2>("Chase Hero 2").target = target;
            tf.Bind(
                target
                );
            go.AddComponent<Orb>().AddChildren();
            go.AddComponent<AlwaysDestroy>();

            FSMUtility.SendEventToGameObject(go, "FIRE");
            go.GetComponent<Rigidbody2D>().isKinematic = false;
            yield break;
        }

        protected override IEnumerator TeleToHero()
        {
            needTele = true;
            yield return new WaitWhile(() => needTele);
        }
    }
}