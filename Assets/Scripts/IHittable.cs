using UnityEngine;

public interface IHittable
{
    void OnBeamAmmoHit(Color beamAmmoColor, Vector3 hitPoint); //polymorphism so u can leave empty
}