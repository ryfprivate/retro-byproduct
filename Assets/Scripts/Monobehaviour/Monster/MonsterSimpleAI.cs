using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MonsterSimpleAI : MonoBehaviour
{
    private enum State
    {
        Stationary,
        Roaming,
        ChaseTarget
    }

    private MonsterPathfindingMovement pathfindingMovement;

    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private int roamRadius = 0;

    private State state;
    private bool spawned = false;

    IEnumerator RoamTimer()
    {
        yield return new WaitForSeconds(5f);
        roamPosition = GetRoamingPosition();
        StartCoroutine(RoamTimer());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // roamPosition = GetRoamingPosition();
        // SetTargetPosition(roamPosition);
    }

    void Awake()
    {
        pathfindingMovement = GetComponent<MonsterPathfindingMovement>();
        state = State.Roaming;
    }

    void Update()
    {
        if (!spawned) return;

        switch (state)
        {
            default:
            case State.Roaming:
                pathfindingMovement.MoveTo(roamPosition);

                float reachedPositionDistance = 1f;
                if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
                {
                    StopCoroutine(RoamTimer());
                    // Find new roaming position
                    roamPosition = GetRoamingPosition();
                }

                FindTarget();
                break;
            case State.ChaseTarget:
                if (PlayerController.Instance == null) break;
                Vector3 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
                pathfindingMovement.MoveTo(transform.position + direction);

                float stopChaseDistance = 5f;
                if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) > stopChaseDistance)
                {
                    // Too far, stop chasing
                    state = State.Stationary;
                }
                break;
            case State.Stationary:
                FindTarget();
                break;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        Vector3 position = startingPosition + UtilsClass.GetRandomDir() * Random.Range(1f, roamRadius);
        int width = Pathfinding.Instance.GetGrid().GetWidth();
        int height = Pathfinding.Instance.GetGrid().GetHeight();
        while (position.x < 0 || position.x > width || position.y < 0 || position.y > height)
        {
            position = startingPosition + UtilsClass.GetRandomDir() * Random.Range(1f, 10f);
        }

        return position;
    }

    private void FindTarget()
    {
        if (PlayerController.Instance == null) return;

        float targetRange = 5f;
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }

    private void Attack()
    {

    }

    public void SetRoamRadius(int radius)
    {
        roamRadius = radius;
    }

    public void SetStartingPosition(Vector3 pos)
    {
        startingPosition = pos;
    }

    public void StartRoaming()
    {
        roamPosition = GetRoamingPosition();
        StartCoroutine(RoamTimer());
        Debug.Log(roamPosition);
        spawned = true;
    }
}
