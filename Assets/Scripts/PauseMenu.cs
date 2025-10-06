using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Button quiteButton;
    [SerializeField] private ExitPopUp exitPopUp;

    private void Awake()
    {
        gameInput.OnPauseAction += GameInput_OnPauseAction;
        quiteButton.onClick.AddListener(() =>
        {
            exitPopUp.gameObject.SetActive(true);
        });
            gameObject.SetActive(false);
    }

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

    private void PauseGame()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
    private void ResumeGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        exitPopUp.gameObject.SetActive(false);
    }
}
