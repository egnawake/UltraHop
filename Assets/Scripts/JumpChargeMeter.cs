using UnityEngine;
using TMPro;

public class JumpChargeMeter : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        text.text = $"{playerMovement.JumpChargePower:f2}s";
    }
}
