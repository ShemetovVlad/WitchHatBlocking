using UnityEngine;
using UnityEngine.UI;

public class ExitPopUp : MonoBehaviour
{
    //[SerializeField] private GameInput gameInput;
    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton;

    private void Awake()
    {
        //gameInput.OnPauseAction += GameInput_OnPauseAction;
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

    /*
    private void GameInput_OnPauseAction(object sender, System.EventArgs e)
    {
        bool isPaused = Time.timeScale == 0f;
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    */
}
