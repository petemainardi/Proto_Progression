using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Encapsulate matadata about the occurance of a bounce.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public struct BounceInfo
{
    public Vector3 Direction { get; private set; }
    public Collider BouncedOn { get; private set; }

    public BounceInfo(Vector3 direction, Collider bouncedOn)
    {
        this.Direction = direction;
        this.BouncedOn = bouncedOn;
    }
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Tracks collisions to determine whether/when a bounce occurs.
 *  
 *  NOTE: The purpose of this class is to decide WHEN a bounce happens (according to a supplied
 *  condition that allows the possibility for a bounce). What that condition is, and how to respond
 *  to a bounce event, are not the concerns of this class. A bounce event is signalled by changing
 *  the BounceInfo value, so subscribe to it to respond.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(Collider))]
public class BounceDetector : MonoBehaviour
{
    // Fields =====================================================================================
    private List<Collider> trackedColliders = new List<Collider>();

    private bool canBounce;
    [SerializeField]
    private readonly float bounceHeight = 0.5f; // TODO: replace this when magnitude is baked into BounceInfo.Direction
    
    public readonly ReactiveProperty<BounceInfo> BounceInfo = new ReactiveProperty<BounceInfo>();
    // ============================================================================================

    // Mono =======================================================================================
    // Startup --------------------------------------------------------------------------
    private void Awake()
    {
        if (this.gameObject.layer != LayerMask.NameToLayer("Bouncing"))
            Debug.LogWarning($"{this.NameAndID()} has a BounceDetector, it should be on the Bouncing layer.");
    }
    // ----------------------------------------------------------------------------------
    // Collision ------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        if (this.canBounce)
            this.Bounce(collision);
        //else
        //    Debug.Log($"Collided with {collision.collider.NameAndID()} but can't bounce.");
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!this.trackedColliders.Contains(collision.collider) && this.canBounce)
            this.Bounce(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log($"Removed {collision.collider.NameAndID()}");
        this.trackedColliders.Remove(collision.collider);
    }
    // ----------------------------------------------------------------------------------
    // ============================================================================================

    // ==================================================================================
    public void SetBounceCondition(IObservable<bool> condition)
    {
        condition
            .Subscribe((bool b) => this.canBounce = b)
            .AddTo(this);
        // TODO: need to protect against or handle multiple subscriptions interfering with each other
    }

    private void Bounce(Collision collision)
    {
        //Debug.Log($"Bouncing on {collision.collider.NameAndID()}.");

        this.BounceInfo.Value = new BounceInfo(Vector3.up * this.bounceHeight, collision.collider); // TODO: Calculate collision normal for actual direction
        // TODO: probably should also bake bounce force into the vector based on the colliding object's properties instead of using bounceHeight
        this.trackedColliders.Add(collision.collider);

        this.BounceInfo.Value = default;    // Not ideal, but only emits when value completely changes so...
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================