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

    [Header("Panel Dialogue")]
    [SerializeField] private GameObject panelDialogue;
    public bool openPanelDialogue;

    private DialogueManager manager;

    private void Start()
    {
        CreditsModify();
        manager = transform.Find("Panel-Dialogue").GetComponent<DialogueManager>();
    }

    public void CreditsModify()
    {
        creditsCount = GameManager.Instance.Credits;
        creditsText.text = $"Creditos: {creditsCount}";
    }

    public void OpenDialogue(bool active, int index)
    {
        openPanelDialogue = active;
        panelDialogue.SetActive(openPanelDialogue);

        manager.StartDialogueById(index);
    }

}