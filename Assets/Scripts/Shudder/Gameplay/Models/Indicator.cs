using System.Collections.Generic;
using UnityEngine;

namespace Shudder.Gameplay.Models
{
    public class Indicator : MonoBehaviour
    {
        private List<Indicator> _box;

        public Indicator SetBox(List<Indicator> boxSelectIndicators)
        {
            _box = boxSelectIndicators;
            return this;
        }

        private void OnDestroy()
        {
            _box?.Remove(this);
        }
    }
}