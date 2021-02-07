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
 *  This class does things...
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public class BounceDetector : MonoBehaviour
{
    // Fields =====================================================================================
    private List<Collider> trackedColliders = new List<Collider>();

    private bool canBounce;
    [SerializeField]
    private float bounceHeight = 0.5f; // TODO: replace this when magnitude is baked into BounceDirection

    public Vector3ReactiveProperty BounceDirection = new Vector3ReactiveProperty();
    // ============================================================================================

    // Mono =======================================================================================



    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Collided with {collision.collider.name}:{collision.collider.GetInstanceID()}.");

        if (this.canBounce)
            this.Bounce(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!this.trackedColliders.Contains(collision.collider) && this.canBounce)
            this.Bounce(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        this.trackedColliders.Remove(collision.collider);
    }
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
        Debug.Log($"Bouncing on {collision.collider.name}:{collision.collider.GetInstanceID()}");

        this.BounceDirection.Value = Vector3.up * this.bounceHeight; // TODO: Calculate collision normal for actual direction
        // TODO: probably should also bake bounce force into the vector based on the colliding object's properties
        this.trackedColliders.Add(collision.collider);

        this.BounceDirection.Value = Vector3.zero;
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
