using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Target : MonoBehaviour, IHittable //create subclass to instantiate Target Interfaces
{
    [Header("Target Stats")]
    [Tooltip("How many paintballs it takes for the target to break.")] //expose  settings to designers N explain
    [SerializeField] private int maxHits = 1;

    [Tooltip("Points pplayer gets from breaking the target.")]
    [SerializeField] private int points = 10;

    [Header("Hit Flash")]
    [Tooltip("How long the target flashes when hit.")]
    [SerializeField] private float flashDuration = 0.1f;


    //Encapsulation - Internal State
    private int _hitsRemaining;
    private Renderer _targetRenderer;
    private Color _baseColor = Color.white;
    private float flashTimer = 0f;

    //3.Encapsulation - Properties
    public int Points => points;
    public int HItsRemaining => _hitsRemaining;
    public bool IsDefeated { get; private set; } //getter public and setter private

    // 3. Encapsulation - Protecteed functions and variables - Private for all and public for children
    protected Color BaseColor
    {
        get { return _baseColor; }
        set
        {
            _baseColor = value;
        }
    }

    protected virtual void Awake() //Child overrides the function
    {
        _hitsRemaining = Mathf.Max(1, maxHits);

        _targetRenderer = GetComponent<Renderer>(); //TODO: Check if we do it in child

        _baseColor = _targetRenderer.material.color; // extract color wwe can use it later
    }

    protected virtual void Update()
    {
        if (IsDefeated)return; //RET RUN
        
            Move();  // 4. Polymorphism, || I dont care just move, each child will have their "flair"
            UpdateFlash();
    }

    
    private void UpdateFlash()
    {
        if (flashTimer <= 0f) return;

        flashTimer -= Time.deltaTime;
        if (flashTimer <= 0f)
        {
            SetRenderColor(_baseColor);
        }
    }

    private void SetRenderColor(Color baseColor)
    {
        _targetRenderer.material.color = baseColor;
    }

    protected abstract void Move();

    public void OnBeamAmmoHit(Color beamAmmoColor, Vector3 hitPoint) //process of being hit by ammobeam
    {
        if (IsDefeated) return;

        _hitsRemaining--;

        OnHit(beamAmmoColor, hitPoint);

        if (_hitsRemaining <= 0)
        {
            IsDefeated = true;
            OnDefeated();
        }
    }

    protected virtual void OnHit(Color beamAmmoColor, Vector3 hitPoint) //brocess of hit
    {
        Flash(beamAmmoColor);
    }

    protected void Flash(Color beamAmmoColor)
    {
        if (flashDuration <= 0f) return;
        flashTimer = flashDuration;
        SetRenderColor(beamAmmoColor);
    }

    protected virtual void OnDefeated()
    {
       // TODO: Add points to the Score Manager.
       Destroy(gameObject);
    }



}
