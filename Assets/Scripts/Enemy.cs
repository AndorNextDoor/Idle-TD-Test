using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    private Transform target;
    private int waypointIndex = 0;
    private int laneIndex = 0;

    private void Start()
    {

    }

    public void SetLane(int lane)
    {
        laneIndex = lane;
        target = Path.lanes[laneIndex].transform.GetChild(waypointIndex);
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if(waypointIndex >= Path.lanes[laneIndex].transform.childCount - 2)
        {
            GameManager.Instance.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        waypointIndex++;
        target = Path.lanes[laneIndex].transform.GetChild(waypointIndex);
    }
    
    private void OnDestroy()
    {
        GameManager.Instance.OnEmemyDestroyed();
    }

}
