using UnityEngine;

public class NpcBugaBuga : MonoBehaviour
{
    [Header("Settings Player Check")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform checkPlayer;
    [SerializeField, Range(0.1f, 5f)] private float radiusCheckPlayer = 1f;

    private GameObject popupButton;

    private void Start()
    {
        popupButton = transform.Find("Popup-Button").gameObject;
        popupButton.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (checkPlayer == null) return;

        bool isPlayer = Physics2D.OverlapCircle(checkPlayer.position, radiusCheckPlayer, playerMask);
        OnPlayerData(isPlayer);
    }

    private void OnPlayerData(bool playerTouch)
    {
        popupButton.SetActive(playerTouch);

        if (Input.GetKeyDown(KeyCode.F) && playerTouch)
        {
            Debug.Log("Abrir panel de la UI para hablar");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPlayer == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPlayer.position, radiusCheckPlayer);
    }
}
