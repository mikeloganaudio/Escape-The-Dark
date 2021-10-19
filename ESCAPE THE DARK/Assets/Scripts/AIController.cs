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
    [SerializeField] float chaseDistance = 2.0f;
    [SerializeField] float chaseTime = 3.0f;
    private float timeSinceArrivedAtWaypoint = 0.0f;
    Mover mover;
    int currentWaypointIndex = 0;
    private float timeSinceLastSawPlayer = 0.0f;
    GameObject player;
    private bool spottedPlayer;

    private void Awake()
    {
        mover = GetComponent<Mover>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(InAttacRangeOfPlayer())
        {
            AttackBehavior();
        }
        else if(timeSinceLastSawPlayer < chaseTime && spottedPlayer)
        {
            ChaseBehavior();
        }
        else
        {
            PatrolBehavior();
        }
        SpotPlayer();
        UpdatedTimers();
    }

    private void AttackBehavior()
    {
        //Put attack behavior here.
    }

    private bool InAttacRangeOfPlayer()
    {
        return false;//Put attack range check in here.
    }

    private void SpotPlayer()
    {
        if(InChaseRangeOfPlayer())
        {
            timeSinceLastSawPlayer = 0;
            spottedPlayer = true;
        }
        else if(timeSinceLastSawPlayer > chaseTime)
        {
            print("chase timeout");
            spottedPlayer = false;
        }
    }

    private void UpdatedTimers()
    {
        timeSinceArrivedAtWaypoint += Time.deltaTime;
        timeSinceLastSawPlayer += Time.deltaTime;
    }

    private void ChaseBehavior()
    {
        print("In chase range of player");
        mover.MoveTo(player.transform.position, patrolSpeedFraction);
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

    private bool InChaseRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer < chaseDistance;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}