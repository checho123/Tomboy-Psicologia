using UnityEngine;

public class FruitPickup : MonoBehaviour
{
    [Header("Fruit Type")]
    [SerializeField] private FruitType type;
    public string triggerName = "Collision";
    public float destroyAfter = 0.6f;

    private Animator anim;
    

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"El Player tocó la fruta {type}");
            anim.SetTrigger(triggerName);
            Destroy(gameObject, destroyAfter);
        }
    }
}
