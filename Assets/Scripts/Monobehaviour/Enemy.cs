using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

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
        SetTargetPosition(Vector3.zero);
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
                    pathVectorList = null;
                    moveVector = Vector3.zero;
                }
            }
        }
        else
        {
            moveVector = Vector3.zero;
        }

        // Move
        rb.MovePosition(rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }


}
