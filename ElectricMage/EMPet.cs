using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PetCore;
using ModCommon;
using HutongGames.PlayMaker.Actions;

namespace ElectricMage
{
    class EMPet : Pet
    {
        GameObject zap = null;
        GameObject flash = null;
        GameObject wflash = null;
        tk2dSpriteAnimator anim = null;
        protected override void Init()
        {
            PlayMakerFSM control = gameObject.LocateMyFSM("Electric Mage");
            zap = gameObject.GetFSMActionOnState<SpawnObjectFromGlobalPool>("Gen").gameObject.Value;
            anim = GetComponent<tk2dSpriteAnimator>();
            wflash = control.FsmVariables.FindFsmGameObject("White Flash").Value;
            flash = control.FsmVariables.FindFsmGameObject("Appear Flash").Value;
            Destroy(control);

            GetComponent<Rigidbody2D>().isKinematic = true;

            Control.RegisterAction("ATTACK", Fire);
            Control.RegisterAction("AC", AttackControl, () => !Control.IsActionInvoking("AC"));
            Control.InvokeActionOn("AC", () => true);
        }
        protected override IEnumerator TeleToHero()
        {
            transform.SetScaleX(0.5f);
            transform.SetScaleY(0.5f);
            yield return null;
            flash.SetActive(false);
            wflash.SetActive(false);
            yield return null;
            flash.SetActive(true);
            wflash.SetActive(true);
            transform.position = HeroController.instance.transform.position;
            anim.Play("Teleport In");
			yield return new WaitForSeconds(0.25f);
			anim.Play("Idle");
        }
        IEnumerator AttackControl()
        {
            yield return Control.InvokeWait("ATTACK");
            anim.Play("Idle");
            yield return new WaitForSeconds(0.75f);
        }
        IEnumerator Fire()
        {
            yield return null;
            GameObject go = TargetFinder.FindTarget(transform.position);
            if (go == null) yield break;
            anim.Play("Cast");
            yield return new WaitForSeconds(0.5f);
            for(int i = 0; i < 4; i++)
            {
				if(go == null) break;
                GameObject z = Instantiate(zap);
                z.AddComponent<AlwaysDestroy>();
                z.transform.position = go.transform.position;
                var d = z.AddComponent<PetAttack>();
				d.Once = true;
				d.AddChildren();
                yield return new WaitForSeconds(1.2f);
            }
            anim.Play("Cast End");
            yield return new WaitForSeconds(0.75f);
        }
    }
}
