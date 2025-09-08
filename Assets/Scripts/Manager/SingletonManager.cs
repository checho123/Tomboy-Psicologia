using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    public static bool EnablePersistence = true;


    protected virtual void Awake()
    {
        if (Instance != null && Instance != (T)(object)this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        if (EnablePersistence) DontDestroyOnLoad(gameObject);
    }
}
