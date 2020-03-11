using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControllerScript : MonoBehaviour
{
    public TargetHitboxScript target;
    public PlayerCharacterController player;
    public PlayerWeaponsManager playerWeapon;
    public Vector3 playerPosition;
    public Vector3 targetPosition;
    private WeaponController activeWeapon;
    private float _nextRelease = 0.0f;
    private float _nextCharge = 0.0f;
    public float chargePercentage = 0.5f;
    private float _chargeTime;
    private float _shotDelay = 1.0f;
    private bool _isCharging = false;
    private bool _hasFired;
    public float xAxis = 5;
    public float yAxis = -45; 
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
