﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VertexBulletControl : MonoBehaviour
{
    public GameObject prefabBeginParticle;
    public GameObject prefabEndParticle;
    public Vector3 origin;
    public Vector3 destination;

    private Vector3 speed;
    private float gravity=9.81f;
    private float timecost = 2.0f;
    private float currentTime = 0.0f;
    private Quaternion rotation = new Quaternion(0, 0, 0, 0);
    public UnityEvent eHit;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 speedHor = (destination - origin) / timecost;
        Vector3 speedVer = gravity * timecost / 2 * Vector3.down;
        speed = speedHor - speedVer;
        GameObject begin = Instantiate(prefabBeginParticle, origin, rotation);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timecost)
        {
            GameObject end = Instantiate(prefabEndParticle, destination, rotation);
            eHit.Invoke();
            Destroy(gameObject);
        }
        transform.position = position(currentTime);
    }

    Vector3 position(float t)
    {
        return speed * t + gravity * t * t * Vector3.down / 2+origin;
    }
}