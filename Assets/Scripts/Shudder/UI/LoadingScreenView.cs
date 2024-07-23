using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shudder.UI
{
    public class LoadingScreenView : MonoBehaviour, IProgress<float>
    {
        [SerializeField] private Slider _progressSlider;
        
        public void Report(float value)
        {
            _progressSlider.SetValueWithoutNotify(value);
            // _progressSlider.value = value;
            Debug.Log($"Slider value: {value}");
        }
    }
}