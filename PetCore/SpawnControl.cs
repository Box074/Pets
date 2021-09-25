using System.Collections;
using UnityEngine;
using HutongGames.PlayMaker.Actions;

namespace PetCore
{
    public class SpawnControl : MonoBehaviour
    {
        public virtual GameObject TrySpawnObject(GameObject go, SpawnObjectFromGlobalPool state)
        {
            Debug.Log("A SPAW " + go.name);
            return go;
        }
    }
}