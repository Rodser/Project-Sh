using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class Setting : MonoBehaviour
    {
        private AudioSource _music;
        [field: SerializeField] public Button OnOffMusic { get; private set; }
        [field: SerializeField] public Button BackButton { get; private set; }
        
        public void Subscribe(AudioSource music, UnityAction goBackWithoutOption)
        {
            _music = music;
            
            OnOffMusic.onClick.AddListener(OnOff);
            BackButton.onClick.AddListener(goBackWithoutOption);
            
            OnOff();
        }

        private void OnOff()
        {
            if (_music.isPlaying)
                _music.Pause();
            else
                _music.Play();
        }
    }
}