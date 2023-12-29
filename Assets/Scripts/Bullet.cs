using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 70;
    private Transform target;
    private float lifeTimer = 2f;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
        if (target == null) return;

        Vector3 dir = target.position - transform.position;

        if(dir.magnitude <= speed * Time.deltaTime)
        {
            Destroy(target.gameObject);
            Destroy(gameObject);
            return;
        }

        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }


}
