using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SimpleMonsterAI : MonoBehaviour
{
    private enum State
    {
        Stationary,
        Roaming,
        ChaseTarget,
        Return
    }

    private MonsterMain MonsterMain;
    private MonsterPathfindingMovement pathfindingMovement;

    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private int roamRadius = 0;

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
        MonsterMain = GetComponent<MonsterMain>();
        pathfindingMovement = GetComponent<MonsterPathfindingMovement>();
    }

    void Start() {
        state = State.Roaming;
        SetRoamRadius(3);
        SetStartingPosition(transform.position);
        StartRoaming();
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
                if (PlayerController.Instance == null) break;

                Vector3 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
                Attack(direction);
                pathfindingMovement.MoveTo(transform.position + direction);

                if (Vector3.Distance(startingPosition, PlayerController.Instance.transform.position) > roamRadius + 1.5f)
                {
                    // Too far, stop chasing
                    state = State.Return;
                }
                break;
            case State.Stationary:
                FindTarget();
                break;
            case State.Return:
                pathfindingMovement.MoveTo(startingPosition);
                // Reached distance
                float distance = .5f;
                if (Vector3.Distance(startingPosition, transform.position) > distance)
                {
                    // Too far, stop chasing
                    state = State.Roaming;
                }
                FindTarget();
                return;
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

        // Searches for target from center
        if (Vector3.Distance(startingPosition, PlayerController.Instance.transform.position) < roamRadius + 1.5f)
        {
            state = State.ChaseTarget;
        }
    }

    private void Attack(Vector3 direction)
    {
        if (!MonsterMain.canAttack) return;

        float attackRange = 1f;
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            MonsterMain.animator.SetTrigger("Attack");
            MonsterMain.aimVector = direction;
            float angle = Vector3.SignedAngle(new Vector2(0, -1), MonsterMain.aimVector, Vector3.forward);
            MonsterMain.aimTransform.rotation = Quaternion.Euler(0, 0, angle);

            GameObject punch = Instantiate(MonsterMain.punchPrefab, MonsterMain.firePoint.position, MonsterMain.firePoint.rotation);
            punch.GetComponent<Punch>().SetParent(gameObject);
            Physics2D.IgnoreCollision(punch.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            MonsterMain.canAttack = false;
            StartCoroutine(MonsterMain.Reload());
        }
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
    }
}
