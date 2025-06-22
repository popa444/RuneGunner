using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //EnemyHealth Enemy = collision.gameObject.GetComponent<EnemyHealth>();
            //if (Enemy != null)
            {
                //Enemy.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
