using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ModCommon;

namespace PetCore
{
    public static class Helper
    {
        public static PetControl GetControl(this GameObject go) => go.GetOrAddComponent<PetControl>();
        public static Rigidbody2D GetRigidbody2D(this GameObject go) => go.GetComponent<Rigidbody2D>();
    }
}
