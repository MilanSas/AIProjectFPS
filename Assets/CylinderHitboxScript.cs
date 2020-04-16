using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderHitboxScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit()
    {
        TargetHitboxScript target = gameObject.GetComponentInParent(typeof(TargetHitboxScript)) as TargetHitboxScript;
        int score = int.Parse(this.gameObject.tag);
        target.UpdateScore(score);
    }

}
