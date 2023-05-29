using UnityEngine;

public class Block : MonoBehaviour
{
    Rigidbody2D rig;

    public float Speed { get; set; } = 0;
    public bool Active { get; set; } = true;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        tag = "Block";
    }

    private void FixedUpdate()
    {
        if (!Active) return;
        rig.MovePosition(rig.position + new Vector2(0, -Speed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bottom"))
        {
            Destroy(gameObject);
        }
    }
}
