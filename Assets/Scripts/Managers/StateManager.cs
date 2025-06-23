using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    public int lastScore;
    public List<int> highScores = new List<int>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SubmitScore(int score)
    {
        lastScore = score;
        highScores.Add(score);
        highScores.Sort((a, b) => b.CompareTo(a)); // Descending
        if (highScores.Count > 5)
            highScores.RemoveAt(highScores.Count - 1);
    }
}
