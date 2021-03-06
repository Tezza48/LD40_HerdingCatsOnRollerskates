﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
public class Repulsive : MonoBehaviour
{
    public enum ECatState
    {
        Idle,
        Dragged
    }

    private static List<Repulsive> sSpawnedRepulsives;

    [Header("Settings")]
    public float repellRange = 1.0f;
    public float repellforce = 1.0f;
    public float dragForce = 4.0f;

    [Header("ComponentRefs")]
    public Rigidbody2D mRigid;
    public Collider2D mCollider;

#if UNITY_ANDROID || UNITY_IOS
    //private bool mIsFingerDragging;
    //private int mFingerIndex;

#endif
    private ECatState catState;

    public static List<Repulsive> SpawnedRepulsives
    {
        get { return sSpawnedRepulsives; } set { sSpawnedRepulsives = value; }
    }

    private void Awake()
    {
        if (sSpawnedRepulsives == null)
        {
            sSpawnedRepulsives = new List<Repulsive>();
        }
    }

    private void OnEnable()
    {
        sSpawnedRepulsives.Add(this);
    }

    private void OnDisable()
    {
        sSpawnedRepulsives.Remove(this);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 force = new Vector2();
        float distance = 0.0f;
        foreach (Repulsive item in sSpawnedRepulsives)
        {
            if (item != this)
            {
                distance = Vector2.Distance(transform.position, item.transform.position);
                if (distance < repellRange)
                {
                    force += (Vector2)(transform.position - item.transform.position);

                }
            }
        }
        mRigid.AddForce(force * repellforce, ForceMode2D.Force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, repellRange);
    }


#if UNITY_STANDALONE
    private void OnMouseDown()
    {
        
    }
    private void OnMouseDrag()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(mouseWorldPos, transform.position);
        mRigid.AddForce((mouseWorldPos - (Vector2)transform.position) * dragForce, ForceMode2D.Force);
    }
    private void OnMouseUp()
    {
        
    }
#endif

#if UNITY_ANDROID || UNITY_IOS
    void BeginTouchInput(Touch touch)
    {

    }

    void MovedTouchInput(Touch touch)
    {
        Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
        Debug.DrawLine(touchWorldPos, transform.position);
        mRigid.AddForce((touchWorldPos - (Vector2)transform.position) * dragForce, ForceMode2D.Force);
    }

    void EndTouchInput(Touch touch)
    {

    }
#endif
}