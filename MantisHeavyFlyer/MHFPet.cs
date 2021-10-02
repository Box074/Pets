using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetCore;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using ModCommon;
using ModCommon.Util;

namespace MantisHeavyFlyer
{
    class MHFPet : Pet
    {
        tk2dSpriteAnimator anim = null;
        GameObject shot = null;
        protected override void Init()
        {
            anim = gameObject.GetComponent<tk2dSpriteAnimator>();
            shot = gameObject.GetFSMActionOnState<SpawnObjectFromGlobalPool>("Shot").gameObject.Value;
            PlayMakerFSM control = gameObject.LocateMyFSM("Heavy Flyer");
            Destroy(control);
        }

        IEnumerator Attack()
        {
            GameObject tar = TargetFinder.FindTarget(transform.position, 20);
            if (tar == null)
            {
                anim.Play("Fly");
                yield return new WaitForSeconds(0.5f);
                yield break;
            }
            if (tar.transform.position.x < transform.position.x)
            {
                yield return Control.InvokeWait("FACE LEFT");
            }
            else
            {
                yield return Control.InvokeWait("FACE RIGHT");
            }
            yield return new WaitForSeconds(0.5f);
            yield return anim.PlayAnimWait("Shoot Antic");
            GameObject go = Instantiate(shot);
            go.transform.position = transform.position;
            go.AddComponent<PetAttack>().AddChildren();
            float angle = Mathf.Atan2(tar.transform.position.y - transform.position.y,
                tar.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

        }
        protected override IEnumerator FaceLeft()
        {
            anim.Play("TurnToFly");
            yield return base.FaceLeft();
        }
        protected override IEnumerator FaceRight()
        {
            anim.Play("TurnToFly");
            yield return base.FaceRight();
        }
    }
}
