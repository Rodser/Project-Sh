using System;
using UnityEngine;

namespace Rodser.Model
{
    public class Ground : MonoBehaviour
    {
        public bool IsFree { get; internal set; }
        public int Number { get; internal set; }
        public bool Raised { get; private set; }

        internal void Raise(bool raised)
        {
            Raised = raised;
        }

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