using TMPro;
using UnityEngine;

public class PanelUIController : SingletonManager<PanelUIController>
{
    PanelUIController()
    {
        EnablePersistence = false;
    }

    [Header("Credist Text")]
    [SerializeField] private TMP_Text creditsText;
    private int creditsCount = 0;

    private void Start()
    {
        creditsCount = GameManager.Instance.Credits;
        CreditsModify();
    }

    public void CreditsModify()
    {
        creditsText.text = $"Creditos: {creditsCount}";
    }

}