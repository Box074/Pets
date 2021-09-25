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
            StartCoroutine(AutoFinder());
        }
        void FixedUpdate()
        {
            if (target.Any(x => x.Value == null)) FindTarget();
        }
        IEnumerator AutoFinder()
        {
            while (true)
            {
                yield return new WaitForSeconds(3);
                FindTarget();
            }
        }
        public void Bind(params FsmGameObject[] v) => target = v;

        void FindTarget()
        {
            GameObject go = FindTarget(transform.position);
            foreach (var v in target) v.Value = go;
        }
        public static GameObject FindTarget(Vector2 pos)
        {
            GameObject go = null;
            float d = float.MaxValue;
            foreach (var v in FindObjectsOfType<HealthManager>())
            {
                if (v.isDead || v.hp == 0 || v.IsInvincible || v.GetComponent<Rigidbody2D>()?.isKinematic == true) continue;
                float d2 = Vector2.Distance(pos, v.transform.position);
                if (d2 < d)
                {
                    go = v.gameObject;
                    d = d2;
                }
            }
            return go;
        }
    }
}