using UnityEngine;

public class MovingTarget : Target //Inheritance+
{
    [Header("Movement")]
    [Tooltip("How far it slides (iin Unity units")]
    [SerializeField] private float slideDistance = 2f;

    [Tooltip("How far it slides")]
    [SerializeField] private float slideSpeed = 2f;

    private Vector3 _startPosition;
    private float _timeOffset;

    protected override void Awake()
    {
        base.Awake();

        _startPosition = transform.position;
        _timeOffset = Random.Range(0f, 10f);  
    }
    protected override void Move()
    {
        float offset = Mathf.Sin((Time.time + _timeOffset) * slideSpeed) *slideDistance; // y= sin(x*3+a) * 2  x, offset, amplitud

        transform.position = _startPosition + transform.right * offset; //right of the cube
    }

}
