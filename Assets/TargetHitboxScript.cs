using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitboxScript : MonoBehaviour
{
    private int totalScore = 0;
    private string lastScore = "";
    private int[] scoreArray = { 10, 8, 6, 4, 2, -2, -4, -6, -8, -10, -20 };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int scorePosition)
    {
        int score = scoreArray[scorePosition -1];

        if (score > 0)
        {
            lastScore = "+" + score.ToString();
        }
        else
        {
            lastScore = score.ToString();
        }
        totalScore += score;
        Debug.Log(totalScore);
    }

    public int GetScore()
    {
        return totalScore;
    }

    public int ResetScore()
    {
        totalScore = 0;
        return totalScore;
    } 
    public string GetLastScore()
    {
        return lastScore;
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
