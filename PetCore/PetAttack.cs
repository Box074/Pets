using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PetCore
{
    public class PetAttack : MonoBehaviour
    {
        List<HealthManager> go = new List<HealthManager>();
        void Awake()
        {
            DamageHero dh = GetComponent<DamageHero>();
            if (dh != null) Destroy(dh);
        }
        float c = 0;
        void FixedUpdate()
        {
            Collider2D collider = GetComponent<Collider2D>();
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, collider.bounds.size, 0, Vector2.zero);
            foreach(var v in hits)
            {
                HealthManager hm = v.collider.GetComponent<HealthManager>();
                if (hm != null)
                {
                    if (go.Contains(hm)) continue;
                    if (!hm.isDead)
                    {
                        TouchEnemy(hm.gameObject, hm);
                        go.Add(hm);
                    }
                }
            }

            if(Time.time - c > 0.5f)
            {
                c = Time.time;
                go.Clear();
            }
        }

        protected virtual void TouchEnemy(GameObject enemy, HealthManager hm)
        {

        }
        
    }
}