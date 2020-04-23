using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class GunAgent : Agent
{
    WeaponController m_WeaponController;
    public TargetHitboxScript m_Target;
    private Vector3 m_Euler;
    Vector3 relativePosition;
    Vector3 currentPosition;
    float previousDistance;
    float projectileDistance;
    bool isShooting;
    bool shot;
    Vector3? pHit;


    // Start is called before the first frame update
    private void Start()
    {
        m_WeaponController = GetComponent<WeaponController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerWeaponsManager, WeaponController>(m_WeaponController, this, gameObject);
        m_Euler = this.transform.localEulerAngles;
        previousDistance = int.MaxValue;
        projectileDistance = int.MaxValue;
        isShooting = false;
        shot = false;
    }
    public void UpdateHit(Vector3 hit)
    {
        pHit = hit;
    }
    public override void CollectObservations()
    {
        AddVectorObs(m_Target.transform.position.x);
        AddVectorObs(m_Target.transform.position.y);
        AddVectorObs(currentPosition.x);
        AddVectorObs(currentPosition.y);
        AddVectorObs(relativePosition.x);
        AddVectorObs(relativePosition.y);
        //AddVectorObs(m_Euler.x);
        //AddVectorObs(m_Euler.y);
        AddVectorObs(this.transform.rotation.eulerAngles / 180.0f - Vector3.one);
    }

    public override void AgentAction(float[] vectorAction)
    {
        //base.AgentAction(vectorAction);
        //this.transform.Rotate(new Vector3(vectorAction[1], vectorAction[0], 0));
        //Continous Space Type
        //m_Euler.x = (m_Euler.x + Mathf.Clamp(vectorAction[1], -1, 1)) % 360;
        //m_Euler.y = (m_Euler.y - Mathf.Clamp(vectorAction[0], -1, 1)) % 360;

        //this.transform.localEulerAngles = m_Euler;
        //// m_WeaponController.HandleShootInputs(true, true, false);


        //Discrete Space Type
        int movement = Mathf.FloorToInt(vectorAction[0]);
        // Get the action index for jumping
        int shoot = Mathf.FloorToInt(vectorAction[1]);


        //Debug.Log("shoot: " + shoot);

        // Look up the index in the movement action list:
        if (movement == 1) { m_Euler.x += -0.5f; }
        if (movement == 2) { m_Euler.x += 0.5f; }
        if (movement == 3) { m_Euler.y += -0.5f; }
        if (movement == 4) { m_Euler.y += 0.5f; }
        // Look up the index in the jump action list:
        //if (shoot == 0) { /*isShooting = false; */ if (shot) { SetActionMask(new int[] { 1, 2, 3, 4 }); SetActionMask(1, 1);}}
        if (shoot == 1) {shot = true;}

        this.transform.localEulerAngles = m_Euler;

        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 50, Color.green);
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            
            Collider objectHit = hit.collider;
            currentPosition = hit.point;
            CylinderHitboxScript cylinder = objectHit.GetComponent<CylinderHitboxScript>();

            if (cylinder)
            {
                //cylinder.OnHit();
            }

            else
            {
               // m_Target.UpdateScore(11);
            }

            relativePosition = m_Target.transform.position - hit.point;
        }

        float distanceToTarget = Vector3.Distance(m_Target.transform.position,currentPosition
        );

        if(pHit != null)
        {
            projectileDistance = Vector3.Distance(m_Target.transform.position, (Vector3)pHit
        );
        }

        Debug.Log("Is Shooting: " + shot);

        if (shot)
        {
            this.m_WeaponController.HandleShootInputs(true, true, false);
        }

        else
        {
            this.m_WeaponController.HandleShootInputs(false, false, true);
        }

        //else{
        //    this.m_WeaponController.HandleShootInputs(false, false, true);
        //}
        Debug.Log(distanceToTarget);
        //rewards


        if (projectileDistance < 2)
        {
            AddReward(100.0f);
            Done();
        }

        else if(distanceToTarget >= 2 && shot == true)
        {
            AddReward(-10.0f);
            shot = false;
        }

        if (distanceToTarget < previousDistance)
        {
            AddReward(0.01f);
            previousDistance = distanceToTarget;
        }
        //Out of bounds
        if (m_Euler.y > 80 || m_Euler.y < -80)
        {
            AddReward(-30.0f);
            Done();
        }

        if (m_Euler.x > 80 || m_Euler.x < -80)
        {
            AddReward(-30.0f);
            Done();
        }
        // Time penalty
        AddReward(-0.005f);

    }
    public override void AgentReset()
    {
        m_Euler.x = (0) % 360;
        m_Euler.y = (0) % 360;
        pHit = null;
        shot = false;
        m_Target.SetPosition(new Vector3(Random.Range(-25.0f, 25.0f), 10.32f, 25));
        m_Target.ResetScore();
        var clones = GameObject.FindGameObjectsWithTag("projectile");
        projectileDistance = int.MaxValue;
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }

    public override float[] Heuristic()
    {
        float movement = 0;
        float shoot = 0;
        var action = new float[2];
        if (Input.GetKey(KeyCode.LeftArrow)){
            movement = 3;
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            movement = 4;
        }
        if (Input.GetKey(KeyCode.UpArrow)){
            movement = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)){
            movement = 2;
        }


        if (Input.GetKey(KeyCode.Space))
        {
            shoot = 1;
        }


        action[0] = movement;
        action[1] = shoot;
        return action;
    }
}
