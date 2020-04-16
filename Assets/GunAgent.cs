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


    // Start is called before the first frame update
    private void Start()
    {
        m_WeaponController = GetComponent<WeaponController>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerWeaponsManager, WeaponController>(m_WeaponController, this, gameObject);
        m_Euler = this.transform.localEulerAngles;
        previousDistance = int.MaxValue;
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

        m_Euler.x = (m_Euler.x + Mathf.Clamp(vectorAction[1], -1, 1)) % 360;
        m_Euler.y = (m_Euler.y - Mathf.Clamp(vectorAction[0], -1, 1)) % 360;

        this.transform.localEulerAngles = m_Euler;
        // m_WeaponController.HandleShootInputs(true, true, false);

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {

            Collider objectHit = hit.collider;
            currentPosition = hit.point;
            CylinderHitboxScript cylinder = objectHit.GetComponent<CylinderHitboxScript>();

            if (cylinder)
            {
                cylinder.OnHit();
            }

            else
            {
                m_Target.UpdateScore(11);
            }

            relativePosition = m_Target.transform.position - hit.point;
        }

        float distanceToTarget = Vector3.Distance(m_Target.transform.position,currentPosition
        );

        Debug.Log(distanceToTarget);
        //rewards
        if (distanceToTarget < 1)
        {
            AddReward(10.0f);
            Done();
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
        AddReward(-0.05f);

    }

    public override void AgentReset()
    {
        m_Euler.x = (0) % 360;
        m_Euler.y = (0) % 360;
        m_Target.SetPosition(new Vector3(Random.Range(-25.0f, 25.0f), 10.32f, 25));
        m_Target.ResetScore();
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        //charge time
        //action[2] = m_InputHandler.GetLookInputsVertical();
        return action;
    }
}
