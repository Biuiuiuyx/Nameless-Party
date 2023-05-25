using UnityEngine;

public class StretchPlayer : MonoBehaviour
{
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] float speed = 2f;
    Rigidbody2D rig;

    public bool Active { get; set; } = false;
    float dir { get; set; } = 0;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;
        if (Input.GetKey(left))
        {
            dir = -1;
        }else if (Input.GetKey(right))
        {
            dir = 1;
        }
        else
        {
            dir = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!Active) return;
        if (dir != 0)
        {
            rig.MovePosition(rig.position + new Vector2(dir * speed * Time.fixedDeltaTime, 0));
        }
    }
}
