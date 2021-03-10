using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    Vector3 startingPosition;
    Vector3 roamPosition;

    int currentPathIndex;
    List<Vector3> pathVectorList;

    public float moveSpeed = 5f;
    Vector2 moveVector;
    bool isMoving;

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(transform.position, targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        SetTargetPosition(roamPosition);
    }

    void Update()
    {
        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 0.8f)
            {
                moveVector = (targetPosition - transform.position).normalized;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    // pathVectorList = null;
                    // moveVector = Vector3.zero;
                    roamPosition = GetRoamingPosition();
                    SetTargetPosition(roamPosition);
                }
            }
        }
        else
        {
            roamPosition = GetRoamingPosition();
            SetTargetPosition(roamPosition);
        }

        // Move
        rb.MovePosition(rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }

    Vector3 GetRoamingPosition() {
        return startingPosition + UtilsClass.GetRandomDir() * Random.Range(1f, 7f);
    }

    void FindTarget() {
        // float targetRange = 5f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        roamPosition = GetRoamingPosition();
        SetTargetPosition(roamPosition);
    }
}
