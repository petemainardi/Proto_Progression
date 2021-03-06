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
    public Collider LastBouncedOn { get; private set; }

    private bool canBounce;
    [SerializeField]
    private float bounceHeight = 0.5f; // TODO: replace this when magnitude is baked into BounceInfo.Direction
    
    public ReactiveProperty<BounceInfo> BounceInfo = new ReactiveProperty<BounceInfo>();
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
    private void OnTriggerEnter(Collider other)
    {
        if (this.canBounce)
            this.Bounce(other);
        //else
        //    Debug.Log($"Collided with {other.NameAndID()} but can't bounce.");
    }
    private void OnTriggerStay(Collider other)
    {
        if (!this.trackedColliders.Contains(other) && this.canBounce)
            this.Bounce(other);
        //else
        //    Debug.Log($"Colliding with {other.NameAndID()} but can't bounce.");
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"Removed {other.NameAndID()}");
        this.trackedColliders.Remove(other);
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

    private void Bounce(Collider collider)
    {
        //Debug.Log($"Bouncing on {collider.NameAndID()}.");

        this.BounceInfo.Value = new BounceInfo(Vector3.up * this.bounceHeight, collider); // TODO: Calculate collision normal for actual direction
        // TODO: probably should also bake bounce force into the vector based on the colliding object's properties instead of using bounceHeight
        this.trackedColliders.Add(collider);
        this.LastBouncedOn = collider;

        this.BounceInfo.Value = default;    // Not ideal, but only emits when value completely changes so...
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================