using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GhostMarkoth
{
    class ShieldScript : MonoBehaviour
    {
        void Awake()
        {
            gameObject.AddComponent<PetCore.PetAttack>().AddChildren();
        }
        void Update()
        {
            gameObject.layer = (int)GlobalEnums.PhysLayers.HERO_ATTACK;

        }
        void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.gameObject.layer == (int)GlobalEnums.PhysLayers.ENEMY_ATTACK)
            {
                FSMUtility.SendEventToGameObject(collider.gameObject, "ORBIT SHIELD");
            }
        }
    }
}
