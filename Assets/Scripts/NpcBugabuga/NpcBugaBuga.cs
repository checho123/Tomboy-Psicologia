using UnityEngine;

public class NpcBugaBuga : MonoBehaviour
{
    [Header("Settings Player Check")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform checkPlayer;
    [SerializeField, Range(0.1f, 5f)] private float radiusCheckPlayer = 1f;

    [SerializeField] private bool isPlayer;
    private bool lastIsPlayer;

    private void FixedUpdate()
    {
        if (checkPlayer == null) return;

        bool now = Physics2D.OverlapCircle(checkPlayer.position, radiusCheckPlayer, playerMask);
        if (now != lastIsPlayer)
        {
            isPlayer = now;
            lastIsPlayer = now;
            OnPlayerData();
        }
    }

    private void OnPlayerData()
    {
        if (isPlayer)
        {
            Debug.Log("Entró el player");
            return;
        }


    }

    private void OnDrawGizmosSelected()
    {
        if (checkPlayer == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPlayer.position, radiusCheckPlayer);
    }
}
