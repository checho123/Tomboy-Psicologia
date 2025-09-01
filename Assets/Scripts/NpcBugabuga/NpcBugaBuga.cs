using UnityEngine;

public class NpcBugaBuga : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask; // Asigna "Player" en el Inspector

    private void Reset()
    {
        CircleCollider2D c = GetComponent<CircleCollider2D>();
        if (c != null) c.isTrigger = true;
    }

    private bool IsPlayerObj(GameObject go)
    {
        return (playerMask.value & (1 << go.layer)) != 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject root = other.attachedRigidbody ? other.attachedRigidbody.gameObject
                                                  : other.transform.root.gameObject;

        if (!IsPlayerObj(root)) return; // ignora Tilemap/Ground/etc.

        Debug.Log("encontrado con " + root.name);
        // tu lógica...
    }

}
