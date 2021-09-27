using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCore;
using UnityEngine;
using ModCommon;
using HutongGames.PlayMaker.Actions;

namespace GhostMarkoth
{
    class MarkothPet : Pet
    {
        GameObject nail = null;
        GameObject shield = null;
        GameObject whiteslash = null;
        GameObject warpout = null;
        MeshRenderer renderer = null;
        protected override void Init()
        {
            renderer = GetComponent<MeshRenderer>();

            PlayMakerFSM m = gameObject.LocateMyFSM("Movement");
            warpout = m.FsmVariables.FindFsmGameObject("Warp Out").Value;
            whiteslash = m.FsmVariables.FindFsmGameObject("White Flash").Value;

            nail = gameObject.GetFSMActionOnState<SpawnObjectFromGlobalPool>("Nail").gameObject.Value;
            shield = Instantiate(gameObject.GetFSMActionOnState<CreateObject>("Init", "Shield Attack").gameObject.Value);
            shield.AddComponent<ShieldScript>();
            shield.transform.parent = transform;

            Control.RegisterAction("ATTACK", NailAttack, () => !Control.IsActionInvoking("ATTACK"));
            Control.InvokeActionOn("ATTACK", () => true);
            

            Destroy(gameObject.LocateMyFSM("Shield Attack"));
            Destroy(gameObject.LocateMyFSM("Attacking"));
            Destroy(gameObject.LocateMyFSM("Rage Check"));
            Destroy(gameObject.LocateMyFSM("Movement"));
        }

        IEnumerator NailAttack()
        {
            transform.SetScaleX(0.4f);
            transform.SetScaleY(0.4f);
            GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));

            GameObject tar = TargetFinder.FindTarget(transform.position);
            if (tar == null) yield break;
            var g = new HutongGames.PlayMaker.FsmGameObject()
            {
                Value = tar
            };
            GameObject go = Instantiate(nail);
            var a = go.AddComponent<PetAttack>();
            a.Once = true;
            a.AddChildren();
            PlayMakerFSM control = go.LocateMyFSM("Control");
            control.FsmVariables.FindFsmFloat("X Max").Value = tar.transform.position.x + 10;
            control.FsmVariables.FindFsmFloat("Y Max").Value = tar.transform.position.y + 10;
            go.GetFSMActionOnState<GetPosition>("Init").gameObject.GameObject = g;
            go.GetFSMActionOnState<GetAngleToTarget2D>("Antic Point").target = g;
            yield return new WaitForSeconds(0.5f);
        }
        protected override IEnumerator TeleToHero()
        {
            renderer = GetComponent<MeshRenderer>();
            warpout.SetActive(false);
            whiteslash.SetActive(false);

            yield return null;
            warpout.SetActive(true);
            renderer.enabled = false;
            yield return new WaitForSeconds(0.35f);
            transform.position = HeroController.instance.transform.position;
            whiteslash.SetActive(true);
            renderer.enabled = true;
        }

    }
}
