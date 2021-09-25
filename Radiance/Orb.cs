using System.Collections;
using UnityEngine;
using PetCore;

namespace Radiance
{
    public class Orb : PetAttack
    {
        protected override void TouchEnemy(GameObject enemy, HealthManager hm)
        {
            hm.Hit(new HitInstance()
            {
                Source = gameObject,
                AttackType = AttackTypes.Spell,
                IgnoreInvulnerable = false,
                MagnitudeMultiplier = 1,
                Multiplier = 1,
                DamageDealt = 30,
                SpecialType = SpecialTypes.None
            });
            StartCoroutine(Destroy());
        }

        IEnumerator Destroy()
        {
            yield return null;
            FSMUtility.SendEventToGameObject(gameObject, "DESTROY");
        }
    }
}