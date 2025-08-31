using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    [SerializeField] private int score = 0;

    
    public void GetScore()
    {
        Debug.Log($"Total del score: {score}" );
    }
}
