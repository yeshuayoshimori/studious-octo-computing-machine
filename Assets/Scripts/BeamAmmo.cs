using UnityEngine;

public class BeamAmmo : MonoBehaviour
{
    public float splatSizeMin = 0.3f;
    public float splatSizeMax = 0.6f;

    public float splatLifeTime = 30f;


    public float beamAmmoLifeTime = 5f;

    public Color beamAmmoColor = Color.orange;

    private bool _hasHit = false;


    private void Start()
    {
        Destroy(gameObject, beamAmmoLifeTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_hasHit) return;
        _hasHit = true;

        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        CreateSplat(hitPoint, hitNormal, collision.transform);

        IHittable hittable = collision.collider.GetComponent<IHittable>(); // 6 Polymorphism
        if (hittable != null)
        {
            hittable.OnBeamAmmoHit(beamAmmoColor, hitPoint);
        }

        Destroy(gameObject);  //Dissipate beam
    }

    public void Initialize(Color color) //Here to play with BeamAmmo
    {
        beamAmmoColor = color;
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        mat.color = beamAmmoColor;
    }


    private void CreateSplat(Vector3 position, Vector3 normal, Transform parent)
    {
        GameObject splat = GameObject.CreatePrimitive(PrimitiveType.Quad);
        splat.name = "BeamSplat";
        splat.transform.position = position + normal * 0.001f; //Stays slightly above the wall colition
        splat.transform.rotation = Quaternion.LookRotation(-normal); //Neg normal so we can face the quad, how it works in unity
        splat.transform.Rotate(0, 0, Random.Range(0f, 360f)); // F makes it inclusive
      
        //splat.transform.parent = parent;
  
        float size = Random.Range(splatSizeMin, splatSizeMax);
        splat.transform.localScale = new Vector3(size, size, 1f);

        splat.transform.SetParent(parent);

        Destroy(splat.GetComponent<Collider>());

        Renderer rend = splat.GetComponent<Renderer>();
        Material mat = rend.material;
        mat.color = beamAmmoColor;

        Destroy(splat, splatLifeTime);
    }
}