using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class LanguageChanger : MonoBehaviour
    {
        public string ru, en;

        private Text textComponent;

        private void Awake()
        {
            textComponent = GetComponent<Text>();
        }

        private void OnEnable()
        {
            YG2.onSwitchLang += SwitchLanguage;
            SwitchLanguage(YG2.lang);
        }
        private void OnDisable()
        {
            YG2.onSwitchLang -= SwitchLanguage;
        }

        public void SwitchLanguage(string lang)
        {
            switch (lang)
            {
                case "ru":
                    textComponent.text = ru;
                    break;
                default:
                    textComponent.text = en;
                    break;
            }
        }
    }
}