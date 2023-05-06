using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightCamera : MonoBehaviour
{
    [SerializeField]private Transform player;
    [SerializeField]private float bias;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = player.position - player.forward * 10.0f + Vector3.up * 5.0f;
        transform.position = transform.position * bias + offset * (1.0f - bias);
        transform.LookAt(player.position + player.forward * 30.0f);
        
    }
}
