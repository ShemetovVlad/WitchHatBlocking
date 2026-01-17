using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        StartCoroutine(WaitAndReloadData());
    }

    private System.Collections.IEnumerator WaitAndReloadData()
    {
        yield return new WaitForSeconds(0.1f);

        if (LocalSaveManager.Instance != null)
        {
            LocalSaveManager.Instance.ReloadDataForGameScene();
        }
    }
}