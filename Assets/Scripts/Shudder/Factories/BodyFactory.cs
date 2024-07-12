using Shudder.Models;
using UnityEngine;

namespace Shudder.Factories
{
    public class BodyFactory
    {
        public BodyGrid Create(string nameBody = "Grid")
        {
            return new GameObject(nameBody).AddComponent<BodyGrid>();
        }
    }
}