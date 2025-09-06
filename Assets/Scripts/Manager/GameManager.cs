using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    [Header("Credist")]
    [SerializeField, Range(0, 1000)] private int credits = 300;
    public int Credits { get { return credits; } }
}