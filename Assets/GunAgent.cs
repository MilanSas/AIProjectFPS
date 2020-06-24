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
    int ChargeAmount = 0;
    float totalScore = 0;
    float previousDistance;
    float projectileDistance;
    bool isCharging;
    bool shot;
    RaycastHit hit;
    List<ProjectileStandard> projectiles;
    Vector3? pHit;

    // Start is called before the first frame update
    private void Start()
    {
        m_WeaponController = GetComponent<WeaponController>();
        m_WeaponController.owner = gameObject;
        m_WeaponController.sourcePrefab = m_WeaponController.gameObject;
        DebugUtility.HandleErrorIfNullGetComponent<PlayerWeaponsManager, WeaponController>(m_WeaponController, this, gameObject);
        m_Euler = this.transform.localEulerAngles;
        previousDistance = int.MaxValue;
        ChargeAmount = 0;
        totalScore = 0;
        isCharging = false;
        shot = false;
        projectiles = new List<ProjectileStandard>();
    }
    public void UpdateHit(Vector3 hit2)
    {
        pHit = hit2;
    }

    public override void CollectObservations()
    {
        AddVectorObs(m_Target.transform.position.x);
        AddVectorObs(m_Target.transform.position.y);
        AddVectorObs(currentPosition.x);
        AddVectorObs(currentPosition.y);
        AddVectorObs(relativePosition.x);
        AddVectorObs(relativePosition.y);
        AddVectorObs(ChargeAmount);
        AddVectorObs(this.transform.rotation.eulerAngles / 180.0f - Vector3.one);
    }
    void FixedUpdate()
    {
        Debug.Log("Total Score: " + totalScore);
        if (!shot)
        {
            RequestDecision();
        }


        this.transform.localEulerAngles = m_Euler;
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

        float distanceToTarget = Vector3.Distance(m_Target.transform.position, currentPosition);
        Debug.Log("Distance to target" + distanceToTarget);
        Debug.Log("Previous Distance" + previousDistance);
        Debug.Log("Projectile Distance" + projectileDistance);

        //rewards


        if (projectileDistance < 2 && pHit != null)
        {
            AddReward(100.0f);
            totalScore += 100;
            Done();
           
        }

        else if (projectileDistance >= 2 && pHit != null)
        {
            AddReward(-projectileDistance);
            totalScore -= projectileDistance;
            Done();
        }

        if (distanceToTarget < previousDistance)
        {
            Debug.Log("Closer");
            totalScore += 0.1f;
            AddReward(0.1f);
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


    public override void AgentAction(float[] vectorAction)
    {
        //Discrete Space Type
        int movement = Mathf.FloorToInt(vectorAction[0]);
        // Get the action index for jumping
        int shoot = Mathf.FloorToInt(vectorAction[1]);

        if (isCharging)
        {
            ChargeAmount += 1;
        }
        // Look up the index in the movement action list:
        if (movement == 1) { m_Euler.x += -0.5f; }
        if (movement == 2) { m_Euler.x += 0.5f; }
        if (movement == 3) { m_Euler.y += -0.5f; }
        if (movement == 4) { m_Euler.y += 0.5f; }
        // Look up the index in the jump action list:
        

        if (shoot == 1 && !shot)
        {
            this.m_WeaponController.HandleShootInputs(true, true, false);
            isCharging = true;
            ChargeAmount += 1;
        }

        else if (shoot == 0 && isCharging)
        {
            Debug.Log("shot");
            this.m_WeaponController.HandleShootInputs(false, false, true);
            ChargeAmount = 0;
            totalScore += 2.0f;
            AddReward(2.0f);
            shot = true;
            isCharging = false;
        }

        else if (shoot == 0 && !isCharging)
        {
            this.m_WeaponController.HandleShootInputs(false, false, true);
        }

    }
    public override void AgentReset()
    {
        totalScore = 0;
        m_Euler.x = (0) % 360;
        m_Euler.y = (0) % 360;
        pHit = null;
        shot = false;
        isCharging = false;
        ChargeAmount = 0;
        previousDistance = int.MaxValue;
        m_Target.SetPosition(new Vector3(Random.Range(-25.0f, 25.0f), 10.32f, 25));
        m_Target.ResetScore();
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
