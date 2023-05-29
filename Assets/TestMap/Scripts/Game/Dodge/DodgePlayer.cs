using System;
using UnityEngine;

public class DodgePlayer : MonoBehaviour
{
    [SerializeField] Camp camp;
    [SerializeField] KeyCode leftKey;
    [SerializeField] KeyCode rightKey;
    [SerializeField] Vector2 left;
    [SerializeField] Vector2 right;

    Rigidbody2D rig;
    public Action<Camp> onDeath;
    public bool Active { get; set; } = false;
    public float LeftX => left.x;
    public float RightX => right.x;
    Vector2 targetPos;
    bool reach = true;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!Active) return;
        if (Input.GetKeyDown(leftKey))
        {
            targetPos = left;
            reach = false;
        }
        if (Input.GetKeyDown(rightKey))
        {
            targetPos = right;
            reach = false;
        }
    }

    private void FixedUpdate()
    {
        if (!Active) return;
        if (reach) return;
        if (Vector2.Distance(rig.position, targetPos) < 0.001f)
        {
            reach = true;
        }
        else
        {
            rig.MovePosition(Vector2.Lerp(rig.position, targetPos, .7f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            if (Active)
            {
                onDeath?.Invoke(camp);
                collision.GetComponent<Block>().Active = false;
            }
        }
    }
}
