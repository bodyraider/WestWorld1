﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagNeighbor : MonoBehaviour {
    public List<GameObject> neighbor = new List<GameObject>();
    public float SeparateForce = 200;
    public float AlignmentForce = 500;
    public float CohesionForce = 30;
    public float BossFleeForce = 50;
    bool BossInRange = false;
    public Vector3 BossPosition;
    GameObject Boss;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //GameObject Boss = GameObject.FindGameObjectWithTag("Boss");
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TagNeighbor")   
            neighbor.Add(other.gameObject);
        if (other.tag == "Boss")
            BossInRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TagNeighbor")
            neighbor.Remove(other.gameObject);
        if (other.tag == "Boss")
            BossInRange = false;
    }

    public Vector3 Separation()
    {
        Vector3 SteeringForce = new Vector3(0, 0, 0);
        if (neighbor.Count > 0)
        {
            for (int index = 0; index < neighbor.Count; index++)
            {
                Vector3 dist = transform.position - neighbor[index].transform.position;
                SteeringForce += SeparateForce * dist.normalized / dist.magnitude;
            }
        }
        return SteeringForce;
    }

    public Vector3 BossSeparation()
    {
        GameObject Boss = GameObject.FindGameObjectWithTag("Boss");
        Vector3 SteeringForce = new Vector3(0, 0, 0);
        if (BossInRange)
        {
            BossPosition = Boss.transform.position;
            SteeringForce = transform.position - BossPosition;
            SteeringForce *= BossFleeForce;
            return SteeringForce;
        }
        return SteeringForce;

    }
    public Vector3 Alignment()
    {
        Vector3 SteeringForce = new Vector3(0, 0, 0);
        if (neighbor.Count > 0)
        {
            for (int index = 0; index < neighbor.Count; index++)
            {
                Vector3 direction =neighbor[index].GetComponentInParent<Behaviors>().direction;
                SteeringForce += direction;
            }
            SteeringForce /= neighbor.Count;
            Vector3 SelfHeading = GetComponentInParent<Behaviors>().direction;
            return (SteeringForce - SelfHeading) * AlignmentForce;
        }
        return SteeringForce;
    }
    public Vector3 Cohesion()
    {
        Vector3 SteeringForce = new Vector3(0, 0, 0);
        Vector3 CenterOfMass = new Vector3(0, 0, 0);
        if (neighbor.Count > 0)
        {
            for (int index = 0; index < neighbor.Count; index++)
            {
                CenterOfMass += neighbor[index].transform.position;
                
            }
            CenterOfMass /= neighbor.Count;
            SteeringForce = (CenterOfMass - transform.position) * CohesionForce;
            return SteeringForce;
        }
        return SteeringForce;
    }
}
