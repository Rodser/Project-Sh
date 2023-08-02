using System;
using UnityEngine;

namespace Rodser.Model
{
    public class Ground : MonoBehaviour
    {
        public bool IsFree { get; internal set; }
        public int Number { get; internal set; }

        internal void Remove()
        {
            throw new NotImplementedException();
        }

        internal void SetData(Vector3 position, int number)
        {
            throw new NotImplementedException();
        }
    }
}