using System.Collections;
using UnityEngine;
using ModCommon;
using HutongGames.PlayMaker.Actions;

namespace PetCore
{
    public class Pet : SpawnControl
    {
        public PetControl Control => gameObject.GetControl();
        public Rigidbody2D Rigidbody => gameObject.GetRigidbody2D();
        public virtual float MaxDistance => 10;
        public virtual float MinSpeed => 0.1f;
        public virtual float MaxSpeed => 2;
        public virtual bool OrigFaceRight => true;
        protected virtual void Init()
        {

        }
        
        internal void CallInit() => Init();
        protected virtual void Start()
        {
            Rigidbody.gravityScale = 0;

            Control.RegisterAction("MOVE TO LEFT", MoveToLeft);
            Control.RegisterAction("MOVE TO RIGHT", MoveToRight);

            Control.RegisterAction("FACE LEFT", FaceLeft);
            Control.RegisterAction("FACE RIGHT", FaceRight);

            Control.RegisterAction("TELE TO HERO", TeleToHero);
            Control.InvokeActionOn("TELE TO HERO", () => {
                return Vector2.Distance(transform.position, HeroController.instance.transform.position) > MaxDistance
                 });
        }
        protected virtual IEnumerator TeleToHero()
        {
            yield return null;
        }
        protected virtual IEnumerator FaceLeft()
        {
            transform.localScale.SetX(OrigFaceRight ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x));
            yield return null;
        }
        protected virtual IEnumerator FaceRight()
        {
            transform.localScale.SetX(OrigFaceRight ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x));
            yield return null;
        }
        protected virtual IEnumerator MoveToLeft()
        {
            Control.InvokeAction("FACE LEFT");
            Rigidbody.velocity.SetX(-Random.Range(MinSpeed, MaxSpeed));
            yield return null;
        }
        protected virtual IEnumerator MoveToRight()
        {
            Control.InvokeAction("FACE RIGHT");
            Rigidbody.velocity.SetX(Random.Range(MinSpeed, MaxSpeed));
            yield return null;
        }
        protected bool RestTest() => Control.Sleeptime > 4;
        protected bool IdleTest() => Control.Sleeptime == 0;
    }
}