using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Target[] targetPrefabs; //6 Polymorphism = many forms || Enemies spawner, you want to make one that can set to spawn different ones

    [Header("Spawn things")]
    
    [SerializeField] private Vector3 areaSize = new Vector3(12f, 3f, 4f);
   
    [SerializeField] private float minSpacing = 1.8f;
   
    [SerializeField] private float spawnInterval = 1f;
    
    [SerializeField] private float maxTargets = 6;
    
    private List<Target> _activeTargets = new List<Target>(); //target dynamically changed
    private float _spawnTimer = 0f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _activeTargets.RemoveAll(target => target == null); //Everysingle update check ifseomthing has dissappeared if not remove null ptr from your list

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f && _activeTargets.Count < maxTargets)
        {
            if (TrySpawnRandomTarget())
            {
                _spawnTimer = spawnInterval; // start the timer again
            }
        }
    }

    private bool TrySpawnRandomTarget()
    {
        Target prefab = targetPrefabs[Random.Range(0, targetPrefabs.Length)];

        if (prefab == null) return false;

        Vector3 position = GetRandomPointInArea();
        if (IsTooCloseToOtherTargets(position)) return false;

        Target newTarget = Instantiate(prefab, position, Quaternion.identity);
        _activeTargets.Add(newTarget);

        return true;
    }



    //private Quaternion GetFacingRotation(Vector3 position)
    //{
    //    Vector3 direction = Camera.main.position;
    //}

    private bool IsTooCloseToOtherTargets(Vector3 position)
    {
        foreach (Target target in _activeTargets)
        {
            if (target != null && Vector3.Distance(target.transform.position, position) < minSpacing) return true;
        }
        return false;
    }

    private Vector3 GetRandomPointInArea()
    {
        Vector3 localOffset = new Vector3(
            Random.Range(-0.5f, 0.5f) * areaSize.x,
            Random.Range(-0.5f, 0.5f) * areaSize.y,
            Random.Range(-0.5f, 0.5f) * areaSize.z
            
          );

        return transform.position + localOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one); //shift to my position and rotation
        Gizmos.DrawWireCube(Vector3.zero, areaSize);
    }
}
