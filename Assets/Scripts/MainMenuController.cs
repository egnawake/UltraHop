using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string startScene;

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void ContinueGame()
    {
        return;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
