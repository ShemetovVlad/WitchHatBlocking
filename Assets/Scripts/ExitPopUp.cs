using UnityEngine;
using UnityEngine.UI;

public class ExitPopUp : MonoBehaviour
{
    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton;

    private void Start()
    {
        YesButton.onClick.AddListener(() =>
        {
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
        });

        NoButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        gameObject.SetActive(false);
    }
}
