using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControllerScript : MonoBehaviour
{
    public TargetHitboxScript target;
    public PlayerCharacterController player;

    public float xAxis = 0;
    public float yAxis = 90; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player.setXLookAxis(xAxis);
        player.setYLookAxis(yAxis);
    }
}
