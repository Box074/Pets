using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCore;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ModCommon;

namespace GhostSlug
{
    class SlugPet : Pet
    {
        GameObject spear = null;
        GameObject warp = null;
        GameObject whiteslash = null;
        GameObject warpout = null;
        MeshRenderer renderer = null;
        bool needTele = false;
        protected override void Init()
        {
            renderer = GetComponent<MeshRenderer>();
            spear = gameObject.GetFSMActionOnState<SpawnObjectFromGlobalPool>("Attack").gameObject.Value;
            PlayMakerFSM m = gameObject.LocateMyFSM("Movement");
            warp = m.FsmVariables.FindFsmGameObject("Warp").Value;
            warpout = m.FsmVariables.FindFsmGameObject("Warp Out").Value;
            whiteslash = m.FsmVariables.FindFsmGameObject("White Flash").Value;

            Destroy(gameObject.LocateMyFSM("Warp messenger"));
            Destroy(m);

            Destroy(gameObject.LocateMyFSM("Attacking"));

            Control.RegisterAction("CHOOSE", Choose);
            Control.InvokeActionOn("CHOOSE", () => !Control.IsActionInvoking("CHOOSE"));
            Control.RegisterAction("ATTACK", Attack);
        }

        IEnumerator Choose()
        {
            yield return null;
            if (needTele)
            {
                warpout.SetActive(false);
                warp.SetActive(false);
                whiteslash.SetActive(false);

                yield return null;
                warpout.SetActive(true);
                renderer.enabled = false;
                yield return new WaitForSeconds(0.35f);
                transform.position = HeroController.instance.transform.position;
                warp.SetActive(true);
                whiteslash.SetActive(true);

                needTele = false;
                yield break;
            }
            yield return Control.InvokeWait("ATTACK");
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator Attack()
        {
            yield return null;
            GameObject target = TargetFinder.FindTarget(transform.position);
            if (target == null) yield break;
            for(float angle = 0; angle < 360; angle += 45)
            {
                GameObject go = Instantiate(spear);
                go.transform.eulerAngles = new Vector3(0, 0, angle);
                go.SetActive(true);
                go.AddComponent<AlwaysDestroy>();
                go.AddComponent<PetAttack>().AddChildren();
            }
            yield return new WaitForSeconds(0.75f);
        }

        protected override IEnumerator TeleToHero()
        {
            needTele = true;
            yield return new WaitWhile(() => needTele);
        }
    }
}
