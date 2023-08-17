using Core;
using UnityEngine;

namespace Logic
{
    public class BodyFactory
    {
        public BodyGrid Create()
        {
            return new GameObject("Grid").AddComponent<BodyGrid>();
        }
    }
}