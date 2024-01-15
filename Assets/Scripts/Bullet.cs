using UnityEditor.UIElements;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private string enemyTag;
    [SerializeField] private string laneEndTag;
    [SerializeField] private float lifeTimer = 5f;

    private void Update()
    {
        MoveStraight();
    }

    private void MoveStraight()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(enemyTag))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        if (collision.collider.CompareTag(laneEndTag))
        {
            Destroy(gameObject);
        }
    }
}