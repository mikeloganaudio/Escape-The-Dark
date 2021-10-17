using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointTolerance = 1.0f;
    [SerializeField] float waypointDwellTime = 0.0f;
    [SerializeField] float patrolSpeedFraction = 0.2f;
    private float timeSinceArrivedAtWaypoint = 0.0f;
    Mover mover;
    int currentWaypointIndex = 0;

    private void Awake()
    {
        mover = GetComponent<Mover>();
    }

    // Update is called once per frame
    void Update()
    {
        PatrolBehavior();
        UpdatedTimers();
    }

    private void UpdatedTimers()
    {
        timeSinceArrivedAtWaypoint += Time.deltaTime;
    }

    private void PatrolBehavior()
    {
        Vector3 nextPosition = transform.position;

        if (patrolPath != null)
        {
            if (AtWaypoint())
            {
                timeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
        }

        if (timeSinceArrivedAtWaypoint > waypointDwellTime)
        {
            mover.MoveTo(nextPosition, patrolSpeedFraction);
        }
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }
}
