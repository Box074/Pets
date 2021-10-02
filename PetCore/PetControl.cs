using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PetCore
{
    public class PetControl : MonoBehaviour
    {
        Dictionary<string, Action> actions = new Dictionary<string, Action>();
        List<(string, Func<bool>)> events = new List<(string, Func<bool>)>();
        public int InvokeCount => invokeCount;
        int invokeCount = 0;
        public void RegisterAction(string name, Func<IEnumerator> c,params Func<bool>[] test)
        {
            actions[name] = new Action()
            {
                c = c,
                test = test
            };
        }

        public int GetMaxInvoke(string name)
        {
            if(actions.TryGetValue(name,out var v))
            {
                return v.maxInvoke;
            }
            return 0;
        }
        public void SetMaxInvoke(string name, int max)
        {
            if (actions.TryGetValue(name, out var v))
            {
                v.maxInvoke = max;
            }
        }
        public void InvokeOnUpdate(string name)
        {
            InvokeActionOn(name, _ATrue);
        }
        public void UnsafeInvoke(string name)
        {
            if (actions.TryGetValue(name, out var v))
            {
                StartCoroutine(_invoke(v));
            }
        }
        private bool _ATrue() => true;
        public void NamedAction(string orig,string n)
        {
            if(actions.TryGetValue(orig,out var v))
            {
                actions[n] = v;
            }
        }
        public void StopAction(string name)
        {
            if(actions.TryGetValue(name,out var v))
            {
                foreach(var v2 in v.instances)
                {
                    v2.Stop();
                    StopCoroutine(v2.coroutine);
                }
                v.instances.Clear();
            }
        }
        public void StopAction(ActionInstance instance)
        {
            StopCoroutine(instance.coroutine);
            instance.Stop();
        }
        public void InvokeAction(string name)
        {
            if(actions.TryGetValue(name,out var v))
            {
                if (v.invokeCount >= v.maxInvoke) return;
                if (v.test != null && v.test.Length>0)
                {
                    if (!v.test.All(x => x())) return;
                }
                StartCoroutine(_invoke(v));
            }
        }
        public bool IsActionInvoking(string name)
        {
            if(actions.TryGetValue(name,out var v))
            {
                return v.invokeCount > 0;
            }
            return false;
        }
        public int ActionInvokeCount(string name)
        {
            if (actions.TryGetValue(name, out var v))
            {
                return v.invokeCount;
            }
            return 0;
        }
        public IEnumerator InvokeWait(string name)
        {
            if (actions.TryGetValue(name, out var v))
            {
                if (v.invokeCount >= v.maxInvoke) yield break;
                if (v.test != null && v.test.Length > 0)
                {
                    if (!v.test.All(x => x())) yield break;
                }
                yield return StartCoroutine(_invoke(v));
            }
        }
        public IEnumerator WaitAction(string name)
        {
            if (actions.TryGetValue(name, out var v))
            {
                while (v.invokeCount > 0) yield return null;
            }
        }
        public IEnumerator WaitAction(ActionInstance instance)
        {
            while (!instance.isEnd) yield return null;
        }
        IEnumerator _invoke(Action action)
        {
            action.invokeCount++;
            invokeCount++;
            ActionInstance instance = null;
            try
            {
                Coroutine coroutine = StartCoroutine(action.c());
                instance = new ActionInstance(action, coroutine);
                action.instances.Add(instance);
                yield return coroutine;
            }
            finally
            {
                instance?.Stop();
                action.invokeCount--;
                invokeCount--;
            }
        }
        public void InvokeActionOn(string name, Func<bool> test)
        {
            if (test != null)
            {
                events.Add((name, test));
            }
        }

        public float Sleeptime { get; private set; } = 0;
        void FixedUpdate()
        {
            if (PlayerData.instance.atBench)
            {
                Sleeptime += Time.fixedDeltaTime;
            }
            else
            {
                Sleeptime = 0;
            }
        }
        void Awake()
        {
            foreach (var v in GetComponentsInChildren<Transform>()) v.gameObject.layer = (int)GlobalEnums.PhysLayers.ENEMY_DETECTOR;
            gameObject.layer = (int)GlobalEnums.PhysLayers.ENEMY_DETECTOR;
            foreach (var v in GetComponentsInChildren<DamageHero>()) Destroy(v);
            Destroy(GetComponent<HealthManager>());
        }

        void Update()
        {
            if (HeroController.instance == null)
            {
                gameObject.SetActive(false);
                return;
            }
           
            foreach(var v in events)
            {
                try
                {
                    if (v.Item2())
                    {
                        InvokeAction(v.Item1);
                    }
                }
                catch (Exception e)
                {
                    Modding.Logger.LogError(e);
                }
            }
        }

        void OnEnable()
        {
            gameObject.transform.position = HeroController.instance.transform.position;
        }
        void OnDisable()
        {
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }
            DontDestroyOnLoad(gameObject);
            foreach (var v in actions)
            {
                foreach (var v2 in v.Value.instances) v2.Stop();
                v.Value.instances.Clear();
                v.Value.invokeCount = 0;
            }
            invokeCount = 0;
        }

        internal class Action
        {
            public string name = "";
            public Func<IEnumerator> c = null;
            public Func<bool>[] test = null;
            public int invokeCount = 0;
            public int maxInvoke = int.MaxValue;
            public int weight = 1;
            public List<ActionInstance> instances = new List<ActionInstance>();
        }
        public class ActionInstance
        {
            internal ActionInstance(Action action,Coroutine coroutine)
            {
                this.action = action;
                this.coroutine = coroutine;
                guid = Guid.NewGuid().ToString();
            }
            internal Action action = null;
            public bool isEnd = false;
            public Coroutine coroutine = null;
            public readonly string guid = "";
            public override bool Equals(object obj)
            {
                if (obj == this) return true;
                if(obj is ActionInstance a)
                {
                    if (a.guid == guid) return true;
                    else return false;
                }
                else
                {
                    return false;
                }
            }
            public void Stop()
            {
                action.instances.Remove(this);
                isEnd = true;
            }
            public override string ToString()
            {
                return $"{action.name} - {guid}";
            }
            public override int GetHashCode()
            {
                return guid.GetHashCode();
            }
        }
    }
}
