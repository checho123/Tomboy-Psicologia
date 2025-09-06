using UnityEngine;

public class NpcBugaBuga : MonoBehaviour
{
    [Header("Settings Player Check")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform checkPlayer;
    [SerializeField, Range(0.1f, 5f)] private float radiusCheckPlayer = 1f;

    private GameObject popupButton;
    private bool isPlayerNearby;

    private void Start()
    {
        popupButton = transform.Find("Popup-Button").gameObject;
        popupButton.SetActive(false);
    }

    private void Update()
    {
        if (checkPlayer == null) return;


        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Abrir panel de la UI para hablar");
            //TODO:Abrir panel UI para interactuar
        }
    }

    private void FixedUpdate()
    {
        isPlayerNearby = Physics2D.OverlapCircle(checkPlayer.position, radiusCheckPlayer, playerMask);
        popupButton.SetActive(isPlayerNearby);
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPlayer == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPlayer.position, radiusCheckPlayer);
    }
}
