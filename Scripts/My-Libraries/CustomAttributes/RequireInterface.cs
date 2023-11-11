using System;
using UnityEngine;

namespace PaleLuna.Attributes
{
    public class RequireInterface : PropertyAttribute
    {
        public readonly Type RequireType;
        public bool allowSceneObject;

        public RequireInterface(Type requireType, bool allowSceneObject = true)
        {
            RequireType = requireType;
            this.allowSceneObject = allowSceneObject;
        }
    }
}