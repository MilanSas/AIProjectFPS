using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitboxScript : MonoBehaviour
{
    private int totalScore = 0;
    private string lastScore = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score)
    {
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

    public string GetLastScore()
    {
        return lastScore;
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
