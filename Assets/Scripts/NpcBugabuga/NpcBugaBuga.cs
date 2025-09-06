using UnityEngine;

public class NpcBugaBuga : MonoBehaviour
{
    [Header("Settings Player Check")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform checkPlayer;
    [SerializeField, Range(1f, 5f)] private float radiusCheckPlayer = 1f;
    [SerializeField] private bool isPlayer;

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (checkPlayer != null)
        {
            isPlayer = Physics2D.OverlapCircle(checkPlayer.position, radiusCheckPlayer, playerMask);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPlayer == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPlayer.position, radiusCheckPlayer);
    }
}
