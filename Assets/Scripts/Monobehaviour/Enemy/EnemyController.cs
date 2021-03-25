using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;

public class EnemyController : MonoBehaviour
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
                if (PlayerManager.Instance == null) break;
                Vector3 direction = (PlayerManager.Instance.transform.position - transform.position).normalized;
                pathfindingMovement.MoveTo(transform.position + direction);

                float stopChaseDistance = 5f;
                if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) > stopChaseDistance)
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
        if (PlayerManager.Instance == null) return;

        float targetRange = 5f;
        if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }

    private void Attack() {

    }
}
