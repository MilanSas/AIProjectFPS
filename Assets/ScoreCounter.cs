using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    private PlayerCharacterController player;
    public GameObject otherGameObject;
    private float previousDistance = float.MaxValue;

    private void Awake()
    {
        player = otherGameObject.GetComponent<PlayerCharacterController>();
    }

    void Update()
    {
        var mousePos = Input.mousePosition;
        var targetWorldPosition = Camera.main.WorldToScreenPoint(player.target.transform.position);
        float distance = (Input.mousePosition -Camera.main.WorldToScreenPoint(player.target.transform.position)).magnitude;
        Vector3 relativePosition = mousePos - targetWorldPosition;
        float relativeY = relativePosition.y;
        float relativeX = relativePosition.x;


        if (distance < 60)
        {
            uiText.text += "< 60";
        }
        else
        {
            uiText.text = "Mousepos " + mousePos;
            uiText.text += "targetpos " + targetWorldPosition;
            uiText.text += "distance MT " + distance;
            uiText.text += "Relative X " + relativeX;
            uiText.text += "Relatvie Y " + relativeY;

        }

     


    }
}
