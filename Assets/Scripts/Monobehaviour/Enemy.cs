using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using CodeMonkey.Utils;

public class Enemy : MonoBehaviour
{
    private enum State {
        Stationary,
        Roaming,
        ChaseTarget
    }

    public Rigidbody2D rb;
    public Animator animator;

    private Vector3 startingPosition;
    private Vector3 roamPosition;

    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    private float moveSpeed = 5f;
    private Vector2 moveVector;
    private bool isMoving;

    private State state;

    void OnCollisionEnter2D(Collision2D col)
    {
        // roamPosition = GetRoamingPosition();
        // SetTargetPosition(roamPosition);
    }

    void Start()
    {
        state = State.Roaming;
        startingPosition = transform.position;
        SetNewDestination();
    }

    void Update()
    {
        animator.SetFloat("Horizontal", moveVector.x);
        animator.SetFloat("Vertical", moveVector.y);
        animator.SetFloat("Speed", moveVector.sqrMagnitude);
    }

    void FixedUpdate()
    {
        FindTarget();
        HandleMovement();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        pathVectorList = Pathfinding.Instance.FindPath(transform.position, targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    private void HandleMovement()
    {
        // If there is a path set for the character
        if (pathVectorList != null)
        {
            // Gets the first tile position in the path
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            // If it hasn't reached the tile, set the character's direction to the position
            if (Vector3.Distance(transform.position, targetPosition) > 0.5f)
            {
                moveVector = (targetPosition - transform.position).normalized;
            }
            // If it has reached the tile, increment to the next tile position in the path
            else
            {
                currentPathIndex++;
                // If it has reached the end destination, set a new position
                if (currentPathIndex >= pathVectorList.Count)
                {
                    SetNewDestination();
                }
            }
        }
        // If there is no path set for the character
        else
        {
            SetNewDestination();
        }

        // Drawing enemy path
        // GameController.Instance.pathTilemap.SetTile(Vector3Int.FloorToInt(transform.position), GameController.Instance.pathTile);
        // Move
        rb.MovePosition(rb.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }

    private void SetNewDestination() {
        switch (state) {
            case State.Roaming:
                // Debug.Log("roaming");
                roamPosition = GetRoamingPosition();
                SetTargetPosition(roamPosition);
                break;
            case State.ChaseTarget:
                pathVectorList = null;
                Debug.Log("chasing target");

                break;
            default:
                pathVectorList = null;
                moveVector = Vector2.zero;
                break;
        }
    }

    private Vector3 GetRoamingPosition() {
        return startingPosition + UtilsClass.GetRandomDir() * Random.Range(1f, 7f);
    }

    private void FindTarget() {
        if (Player.Instance == null) return;

        float targetRange = 5f;
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) < targetRange) {
            state = State.Roaming;
        } else {
            state = State.Roaming;
        }
    }
}
