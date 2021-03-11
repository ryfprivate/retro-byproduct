using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Stationary,
        Roaming,
        ChaseTarget
    }

    private EnemyPathfindingMovement pathfindingMovement;

    private Vector3 startingPosition;
    private Vector3 roamPosition;

    private State state;

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
        pathfindingMovement = GetComponent<EnemyPathfindingMovement>();
        state = State.Roaming;
    }

    void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        StartCoroutine(RoamTimer());
    }

    void Update()
    {
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
                if (Player.Instance == null) break;
                pathfindingMovement.MoveTo(Player.Instance.transform.position);

                float stopChaseDistance = 5f;
                if (Vector3.Distance(transform.position, Player.Instance.transform.position) > stopChaseDistance)
                {
                    // Too far, stop chasing
                    state = State.Stationary;
                }
                break;
            case State.Stationary:
                Debug.Log("stationary");
                break;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        Vector3 position = startingPosition + UtilsClass.GetRandomDir() * Random.Range(1f, 10f);
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
        if (Player.Instance == null) return;

        float targetRange = 5f;
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }
}
