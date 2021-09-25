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
            gameObject.layer = (int)GlobalEnums.PhysLayers.HERO_ATTACK;
            DamageHero dh = GetComponent<DamageHero>();
            if (dh != null) Destroy(dh);
        }
        void DoTouch(GameObject g)
        {
            HealthManager hm = g.GetComponent<HealthManager>();
            if (hm != null)
            {
                if (go.Contains(hm)) return;
                if (!hm.isDead)
                {
                    Modding.Logger.Log("Touch: " + hm.gameObject.name);
                    TouchEnemy(hm.gameObject, hm);
                    go.Add(hm);
                }
            }
        }
        void OnCollisionEnter2D(Collision2D c) => DoTouch(c.gameObject);
        void OnCollisionStay2D(Collision2D c) => DoTouch(c.gameObject);
        void OnTriggerEnter2D(Collider2D c) => DoTouch(c.gameObject);
        void OnTriggerStay2D(Collider2D c) => DoTouch(c.gameObject);
        public void AddChildren()
        {
            void A(GameObject root)
            {
                if (root.GetComponent(GetType()) == null) root.AddComponent(GetType());
                for (int i = 0; i < root.transform.childCount; i++) A(root.transform.GetChild(i).gameObject);
            }
            A(gameObject);
        }
        float c = 0;
        void FixedUpdate()
        {
            gameObject.layer = (int)GlobalEnums.PhysLayers.HERO_ATTACK;
            if(Time.time - c > 0.5f)
            {
                c = Time.time;
                go.Clear();
            }
        }

        protected virtual void TouchEnemy(GameObject enemy, HealthManager hm)
        {
            FSMUtility.SendEventToGameObject(enemy, "TAKE DAMAGE", false);
            hm.Hit(new HitInstance()
            {
                DamageDealt = PlayerData.instance.nailDamage,
                Multiplier = 1,
                MagnitudeMultiplier = 1,
                AttackType = AttackTypes.Nail,
                Source = gameObject,
                SpecialType = SpecialTypes.None
            });
        }
        
    }
}