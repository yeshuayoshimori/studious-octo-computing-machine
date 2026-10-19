using System;
using UnityEngine;

public class BeamGun : MonoBehaviour
{
    public BeamAmmo beamammoPrefab;
    public float shootForce = 30f;

    public float minChargeTime = 0.15f;
    public float maxChargeTime = 2f;
    public float maxChargeMultiplier = 2f;

    public Transform muzzlePoint;

    public ParticleSystem chargeFX;

    private bool _isCharging = false;
    private float _chargeStartTime = 0.0f;
    private Camera _playerCamera;

    private Color[] _beamColors = new Color[]{ //16 bytes = 4 int
        new Color(1f, 0f, 0f),
        new Color(0f, 1f, 0f),
        new Color(0f, 0f, 1f),
        new Color(1f, 1f, 1f), //combines light
    };

    private int colorIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked; //cursor point stays in the center, mouse doesnt wander around
        chargeFX.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginCharge();
        }

        if (Input.GetMouseButtonUp(0) && _isCharging)
        {
            Fire();

        }

        if (_isCharging)
        {
            UpdateChargeVisuals();
        }
    
}

    private void BeginCharge()
    {
        _isCharging = true;
        _chargeStartTime = Time.time;
        chargeFX.Play();

    }
   

    private void Fire()
    {
        _isCharging = false;
        float chargeTime = Time.time - _chargeStartTime;

        if (chargeTime < minChargeTime)
        {
            Interrupt();
            return;
        }

        float chargePercent = Mathf.Clamp01(chargeTime / maxChargeTime); //limit the limit of the charge and gets to 1 number between 0 and 1, could be (x, 0, 5) so its 0 to 5
        float finalForce = shootForce * Mathf.Lerp(1f, maxChargeMultiplier, chargePercent); //increase charge power

        chargeFX.Stop();

        SpawnBeamAmmo(finalForce);
    }

    private void SpawnBeamAmmo(float finalForce)
    {
        Color color = _beamColors[colorIndex % _beamColors.Length];
        colorIndex++;

        Vector3 spawnPos = muzzlePoint.position;

        Quaternion spawnRot = _playerCamera.transform.rotation;

        BeamAmmo ammo = Instantiate(beamammoPrefab, spawnPos, spawnRot);
        ammo.name = "BeamAmmo";

        ammo.Initialize(color);

        Rigidbody rb = ammo.GetComponent<Rigidbody>();
        Vector3 direction = _playerCamera.transform.forward;
        rb.AddForce(direction * finalForce, ForceMode.Impulse);
    }

    private void UpdateChargeVisuals()
    {
        float chargeTime = Time.time - _chargeStartTime;
        float chargePercent = Mathf.Clamp01(chargeTime / maxChargeTime);
       
        var emission = chargeFX.emission;
        emission.rateOverTime = Mathf.Lerp(5f, 50f, chargePercent);
        var main = chargeFX.main;
        main.startSpeed = Mathf.Lerp(0.5f, 3f, chargePercent);
    }

    private void Interrupt()
    {
        _isCharging = false;
        chargeFX.Stop();
    }


}
