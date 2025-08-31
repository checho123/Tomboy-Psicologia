using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{
    // Propiedad est�tica p�blica (PascalCase)
    public static T Instance { get; private set; }

    // �nico control para DontDestroyOnLoad (PascalCase por ser p�blico y est�tico)
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
