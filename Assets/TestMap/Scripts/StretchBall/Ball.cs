using System;
using UnityEngine;
using Random = GameProject.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] float speed = 2;
    Rigidbody2D rig;
    Vector2 dir;                            // 移动方向
    public Action<Camp> onResult;
    public bool Active { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        // 随机速度100 - 600
        //speed = Random.GetValue(100, 600);

        // 随机方向
        float x = Random.GetValue(.1f, .2f);
        x = Random.GetValue(0, 2) == 0 ? -x : x;
        float y = Random.GetValue(0, 2) == 0 ? -1 : 1;
        dir = new Vector2(x, y).normalized;
        //Debug.Log($"speed:{speed} - dir:{dir}");
    }

    private void FixedUpdate()
    {
        if (!Active) return;
        // 每帧朝着方向移动
        rig.MovePosition(rig.position + dir * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Active) return;
        if (collision.CompareTag("Player1Camp"))
        {
            onResult?.Invoke(Camp.Player2);
        }else if (collision.CompareTag("Player2Camp"))
        {
            onResult?.Invoke(Camp.Player1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Active) return;
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            rig.velocity = Vector2.zero;
            var p = collision.GetContact(0);
            dir = Vector2.Reflect(dir, p.normal).normalized;
            speed *= 1.05f;
        }
    }
}
