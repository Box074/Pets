using System.Collections;
using System.Linq;
using UnityEngine;
using HutongGames.PlayMaker;

namespace PetCore
{
    public class TargetFinder : MonoBehaviour
    {
        public FsmGameObject[] target = null;
        void OnEnable()
        {
            FindTarget();
        }
        void FixedUpdate()
        {
            if (target.Any(x => x.Value == null)) FindTarget();
        }

        public void Bind(params FsmGameObject[] v) => target = v; 

        void FindTarget()
        {
            GameObject go = null;
            float d = float.MaxValue;
            foreach(var v in FindObjectsOfType<HealthManager>())
            {
                if (v.isDead || v.hp == 0) continue;
                float d2 = Vector2.Distance(transform.position, v.transform.position);
                if (d2 < d)
                {
                    go = v.gameObject;
                    d = d2;
                }
            }
            foreach (var v in target) v.Value = go;
        }
    }
}