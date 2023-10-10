using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class EventPanel : MonoBehaviour
    {
        private AudioSource _winSFX;
        private AudioSource _loosSFX;

        [field: SerializeField] public Button NextButton { get; private set; }
        [field: SerializeField] public Button ExitButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Message  { get; private set; }

        public void Subscribe(UnityAction goLevel, Core.SoundFactory soundFactory)
        {
            NextButton.onClick.AddListener(goLevel);
            ExitButton.onClick.AddListener(OnExit);

            _winSFX = soundFactory.Create(Core.SFX.Winner);
            _loosSFX = soundFactory.Create(Core.SFX.Looser);
        }
        
        public void Notify(bool isVictory = true)
        {
             Message.text = isVictory ? "Победа" : "Проиграл";
             NextButton.GetComponentInChildren<TextMeshProUGUI>().text = isVictory ? "Вперед" : "Заново";

            if (isVictory)
                _winSFX.Play();
            else
                _loosSFX.Play();
        }

        private void OnExit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}