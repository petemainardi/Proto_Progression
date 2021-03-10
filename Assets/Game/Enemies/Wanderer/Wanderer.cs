using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Basic NavMeshAgent-based enemy type that wanders the NavMesh until the Player-tagged gameobject
 *  enters the defined aggro area, while in which they are pursued by this enemy.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(NavMeshAgent))]
public class Wanderer : MonoBehaviour
{
    // Fields =====================================================================================
    private NavMeshAgent agent;

    [SerializeField]
    private Collider aggroArea;
    private Transform aggroTarget;

    [SerializeField]
    private float wanderRadius;
    private Vector3 wanderTarget;
    // ============================================================================================

    // Mono =======================================================================================
    // ----------------------------------------------------------------------------------
    void Awake()
    {
        this.agent = this.GetComponent<NavMeshAgent>();

        if (this.aggroArea != null && !this.aggroArea.isTrigger)
            Debug.LogWarning($"Aggro collider for {this.name}:{this.GetInstanceID()} should be a trigger.");
    }
    // ----------------------------------------------------------------------------------
    void Start()
    {
        if (this.aggroArea != null)
        {
            this.aggroArea
                .OnTriggerEnterAsObservable()
                .Subscribe((Collider other) => this.Aggro(other))
                .AddTo(this);

            this.aggroArea
                .OnTriggerExitAsObservable()
                .Subscribe((Collider other) => this.Release(other))
                .AddTo(this);
        }

        this.wanderTarget = this.Wander();
    }
    // ----------------------------------------------------------------------------------
    void Update()
    {
        Vector3 target = this.aggroTarget != null
            ? this.aggroTarget.position
            : target = this.CheckWander();

        this.agent.SetDestination(target);
    }
    // ----------------------------------------------------------------------------------
    // ============================================================================================

    // Movement AI ================================================================================
    // Mark the other collider to be pursued if it belongs to the player ----------------
    private bool Aggro(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log($"{other.name} aggroed {this.name}");
            this.aggroTarget = other.transform;
            return true;
        }
        return false;
    }
    // ----------------------------------------------------------------------------------
    // Stop pursuit of the collider if it is currently our quarry -----------------------
    private bool Release(Collider other)
    {
        if (other.transform == this.aggroTarget)
        {
            this.aggroTarget = null;
            return true;
        }
        return false;
    }
    // ----------------------------------------------------------------------------------
    // Check whether we have arrived at our intended destination ------------------------
    private Vector3 CheckWander()
    {
        if (Vector3.SqrMagnitude(this.transform.position - this.wanderTarget) < 1)
            this.wanderTarget = this.Wander(); // TODO: could add a randomized slight pause before continuing

        return this.wanderTarget;
    }
    // ----------------------------------------------------------------------------------
    // Choose a new nearby random place on the navmesh ----------------------------------
    private Vector3 Wander()
    {
        Vector3 randDirection = this.agent.transform.position
            + UnityEngine.Random.insideUnitSphere * this.wanderRadius;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, this.wanderRadius, -1);
        //Debug.Log(navHit.position);
        return navHit.position;
    }
    // ----------------------------------------------------------------------------------
    // ============================================================================================
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
