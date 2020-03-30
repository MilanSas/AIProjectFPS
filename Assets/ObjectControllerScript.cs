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
    public Vector2 windDirection;
    public float windIntensity;
    public float chargePercentage = 0.4f;
    public float xAxis = 0;
    public float yAxis = -37.5f;
    private WeaponController activeWeapon;
    private float _nextRelease = 0.0f;
    private float _nextCharge = 1.0f;
    private float _chargeTime;
    private float _shotDelay = 1.0f;
    private bool _isCharging = false;
    private bool _hasFired;
    private int _score;
    [SerializeField]
    private bool _isFire;
 
    // Start is called before the first frame update
    void Start()
    {
        _nextCharge = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        SetLookAxis();
        SetPositions();
        if (_isFire)
        {
            activeWeapon = playerWeapon.GetActiveWeapon();
            _chargeTime = activeWeapon.maxChargeDuration * chargePercentage;

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
                _isFire = false;

            }

        }
    }
    void SetLookAxis()
    {
        player.setXLookAxis(xAxis);
        player.setYLookAxis(yAxis);
    }

    void SetPositions()
    {
        if (targetPosition != null)
        {
            target.SetPosition(targetPosition);
        }
        if (playerPosition != null)
        {
            player.SetPosition(playerPosition);
        }
    }

    void Fire()
    {
        _isFire = true;
    }

    void LoadData(Vector3 pPosition, Vector3 tPosition, Vector2 wDirection, float wIntensity, float cPercentage, float xLook, float yLook)
    {
        this.playerPosition = pPosition;
        this.targetPosition = tPosition;
        this.windDirection = wDirection;
        this.windIntensity = wIntensity;
        this.chargePercentage = cPercentage;
        this.xAxis = xLook;
        this.yAxis = yLook;
    }

    void CalculateScore()
    {
        _score = target.GetScore();
        target.ResetScore();
    }

    void SaveResults()
    {
        var results = new { this.playerPosition, this.targetPosition, this.windDirection, this.windIntensity, this.chargePercentage, this.xAxis, this.yAxis, this._score };
    }
}
