using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothFactor = 0.5f;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - player.position;
    }
    void LateUpdate()
    {
        Vector3 newPosition = offset + player.position;
        transform.position = Vector3.Slerp(transform.position, newPosition,smoothFactor);
    }
}
