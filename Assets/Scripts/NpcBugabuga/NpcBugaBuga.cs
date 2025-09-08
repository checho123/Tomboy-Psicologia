using UnityEngine;

public class NpcBugaBuga : MonoBehaviour
{
    [Header("Settings Player Check")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform checkPlayer;
    [SerializeField, Range(0.1f, 5f)] private float radiusCheckPlayer = 1f;

    [Header("Dialogue Start")]
    [SerializeField] private int startSpeakerId = 0;

    private GameObject popupButton;
    private bool isPlayerNearby;

    [Header("Player Controller")]
    [SerializeField] private PlayerController player;

    private void Start()
    {
        popupButton = transform.Find("Popup-Button").gameObject;
        popupButton.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (checkPlayer == null) return;


        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            PanelUIController.Instance.OpenDialogue(true, startSpeakerId);
        }

        if(player != null)
        {
            player.enabled = !PanelUIController.Instance.openPanelDialogue;
        }

    }

    private void FixedUpdate()
    {
        isPlayerNearby = Physics2D.OverlapCircle(checkPlayer.position, radiusCheckPlayer, playerMask);
        popupButton.SetActive(isPlayerNearby && !PanelUIController.Instance.openPanelDialogue);
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPlayer == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPlayer.position, radiusCheckPlayer);
    }
}
