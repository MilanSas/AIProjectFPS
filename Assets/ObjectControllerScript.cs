using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControllerScript : MonoBehaviour
{
    public TargetHitboxScript target;
    public PlayerCharacterController player;
    public PlayerWeaponsManager playerWeapon;
    public Vector3 playerPosition = new Vector3(0, 1.5f, 0);
    public Vector3 targetPosition = new Vector3(0, 15, 37.50f);
    private WeaponController activeWeapon;
    private float _nextRelease = 0.0f;
    private float _nextCharge = 0.0f;
    public float chargePercentage = 0.4f;
    private float _chargeTime;
    private float _shotDelay = 1.0f;
    private bool _isCharging = false;
    private bool _hasFired;
    public float xAxis = 0;
    public float yAxis = -37.5f; 
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(activeWeapon.maxChargeDuration);   
        _nextCharge = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        player.setXLookAxis(xAxis);
        player.setYLookAxis(yAxis);
        if (targetPosition != null)
        {
            target.SetPosition(targetPosition);
        }
        if (playerPosition != null)
        {
            player.SetPosition(playerPosition);
        }
        activeWeapon = playerWeapon.GetActiveWeapon();
        _chargeTime = activeWeapon.maxChargeDuration * chargePercentage;

        Debug.Log("Time " + Time.time + " NextCharge " + _nextCharge);
        if (!_isCharging && Time.time > _nextCharge)
        {
            _hasFired = activeWeapon.HandleShootInputs(false, true, false);
            _nextRelease = Time.time + _chargeTime;
            _isCharging = true;
        }

        else if (_isCharging && !_hasFired && Time.time > _nextRelease)
        {
            _hasFired = activeWeapon.HandleShootInputs(false, false, true);
            _nextRelease = Time.time + _chargeTime;
            _nextCharge = Time.time + _shotDelay;
            _isCharging = false;
        }
        
    }
}
