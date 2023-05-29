using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    Transform target;
    Vector3 targetPos;

    public static CameraFollow Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, .1f);
    }

    public void SetTarget(Transform _target) => target = _target;
}
