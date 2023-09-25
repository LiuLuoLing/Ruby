using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulle : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        //方向乘以力度
        rigidbody2d.AddForce(direction * force);
    }
    private void Update()
    {
        if (transform.position.magnitude > 50)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyMove enemyMove = collision.gameObject.GetComponent<EnemyMove>();
        if (enemyMove != null)
        {
            enemyMove.Fix();
        }
        Destroy(gameObject);
    }
}
