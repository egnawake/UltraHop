using UnityEngine;
using TMPro;

public class GoalDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text progressText;

    public void SetInfo(string text)
    {
        infoText.text = text;
    }

    public void SetProgress(string text)
    {
        progressText.text = text;
    }
}
