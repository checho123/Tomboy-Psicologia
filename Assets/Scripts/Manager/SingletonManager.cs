using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    // Propiedad estática pública (PascalCase)
    public static T Instance { get; private set; }

    // Único control para DontDestroyOnLoad (PascalCase por ser público y estático)
    public static bool EnablePersistence = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;

            if (EnablePersistence)
            {
                DontDestroyOnLoad(gameObject);
            }
            return;
        }

        // Ya existe uno: destruye el duplicado
        Destroy(gameObject);
    }

}
