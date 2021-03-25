using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 offset;

    void Update()
    {
        PlayerManager player = PlayerManager.Instance;
        // If there is a player, then follow it with the camera
        if (player) transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, -10f);
    }
}
