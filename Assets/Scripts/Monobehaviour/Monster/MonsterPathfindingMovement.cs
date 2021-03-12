using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPathfindingMovement : MonoBehaviour
{
    private const float SPEED = 5f;

    private MonsterMain monsterMain;
    private List<Vector3> pathVectorList;
    private int currentPathIndex;
    private Vector3 lastmoveVector;

    void Awake()
    {
        monsterMain = GetComponent<MonsterMain>();
    }

    void Update()
    {
        HandleMovement();
    }

    void FixedUpdate()
    {
        monsterMain.MonsterRigidbody2D.velocity = monsterMain.moveVector * SPEED;
    }

    private void HandleMovement()
    {
        PrintPathfindingPath();
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            float reachedTargetDistance = .5f;
            if (Vector3.Distance(GetPosition(), targetPosition) > reachedTargetDistance)
            {
                monsterMain.moveVector = (targetPosition - GetPosition()).normalized;
                lastmoveVector = monsterMain.moveVector;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                }
            }
        }
        else
        {
        }
    }

    public void StopMoving()
    {
        pathVectorList = null;
        monsterMain.moveVector = Vector3.zero;
    }

    public List<Vector3> GetPathVectorList()
    {
        return pathVectorList;
    }

    private void PrintPathfindingPath()
    {
        if (pathVectorList != null)
        {
            for (int i = 0; i < pathVectorList.Count - 1; i++)
            {
                Debug.DrawLine(pathVectorList[i], pathVectorList[i + 1]);
            }
        }
    }

    public void MoveTo(Vector3 targetPosition)
    {
        SetTargetPosition(targetPosition);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;

        pathVectorList = Pathfinding.Instance.FindPath(transform.position, targetPosition);
        if (pathVectorList == null) StopMoving();

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetLastmoveVector()
    {
        return lastmoveVector;
    }

}
