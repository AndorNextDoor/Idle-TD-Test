using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float range = 15f;
    [SerializeField] private float fireRate = 1f;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    private float fireCountdown = 0f;

    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float turnSpeed;
    private Transform target;

    [Header("Lane Information")]
    [SerializeField] private int laneIndex; // Assign the lane index for each tower in the Inspector

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f);
    }

    public void SetLaneIndex(int index)
    {
        laneIndex = index;
    }

    private void UpdateTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, enemyMask);
        if (colliders.Length > 0)
        {
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (Collider enemy in colliders)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    // Check if the enemy is in the same lane as the tower
                    if (IsEnemyInSameLane(enemy.GetComponent<Enemy>()))
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy.gameObject;
                    }
                }
            }

            if (nearestEnemy != null)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
        }
        else
        {
            target = null;
        }
    }

    private bool IsEnemyInSameLane(Enemy enemy)
    {
        if (enemy.laneIndex == laneIndex)
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (target == null)
            return;

    //    Vector3 dir = target.position - transform.position;
    //    Quaternion lookRotation = Quaternion.LookRotation(dir);
   //     Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;

     //   transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}