using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        int levelProgress = SaveLoad.LevelProgress;

        level1Button.onClick.AddListener(LoadLevel1);

        if (levelProgress >= 2)
        {
            level2Button.onClick.AddListener(LoadLevel2);
            level2Button.interactable = true;
        }

        if (levelProgress >= 3)
        {
            level3Button.onClick.AddListener(LoadLevel3);
            level3Button.interactable = true;
        }

        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void LoadLevel1()
    {
        SceneManager.LoadScene($"Level_1");
    }

    private void LoadLevel2()
    {
        SceneManager.LoadScene($"Level_2");
    }

    private void LoadLevel3()
    {
        SceneManager.LoadScene($"Level_2");
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
