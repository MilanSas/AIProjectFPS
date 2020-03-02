using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    private TargetHitboxScript target;
    public GameObject otherGameObject;

    private void Awake()
    {
        target = otherGameObject.GetComponent<TargetHitboxScript>();
    }

    void Update()
    {
        string lastScore = target.GetLastScore();
        int score = target.GetScore();
        uiText.text = lastScore + " Total: "+ score.ToString();
    }
}
