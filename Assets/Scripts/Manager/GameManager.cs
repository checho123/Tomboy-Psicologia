using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    [Header("Credist")]
    [SerializeField, Range(0, 1000)] private int credits = 300;

    public int Credits
    {
        get => credits;
        set
        {
            credits = Mathf.Clamp(value, 0, 1000);
        }
    }

    public void AddCredits(int add)
    {
        credits += add;

        if (PanelUIController.Instance != null)
        {
            PanelUIController.Instance.CreditsModify();
        }
    }
}