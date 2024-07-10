using Shudder.Gameplay.Models;
using UnityEngine;

namespace Shudder.Gameplay.Factories
{
    public class BodyFactory
    {
        public BodyGrid Create(string nameBody = "Grid")
        {
            return new GameObject(nameBody).AddComponent<BodyGrid>();
        }
    }
}