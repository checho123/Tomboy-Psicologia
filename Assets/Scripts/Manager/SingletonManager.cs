using UnityEngine;

public class SingletonManager<T> : MonoBehaviour
{
    public static T instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = (T)(object)this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }
}
