using Core;
using UnityEngine;

namespace Logic
{
    public class BodyFactory
    {
        public BodyGrid Create(string nameBody = "Grid")
        {
            return new GameObject(nameBody).AddComponent<BodyGrid>();
        }
    }
}