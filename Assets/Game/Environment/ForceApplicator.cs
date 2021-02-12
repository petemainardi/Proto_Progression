using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
[RequireComponent(typeof(Collider))]
public class ForceApplicator : MonoBehaviour
{
    // Fields =====================================================================================
	public LayerMask Mask;

    public Vector3 Force;

    //public bool ApplyToCharacterMover = true;
    //private Action<Collision> applicator;
    // ============================================================================================

    // Mono =======================================================================================
    private void Awake()
    {
        //this.applicator = this.ApplyToCharacterMover
        //    ? (Action<Collision>)this.ApplyForceToCharacterMover
        //    : (Action<Collision>)this.ApplyForceToRigidbody;
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((this.Mask & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.rigidbody != null)
                collision.rigidbody.AddForce(this.Force);
        }
    }
    // ============================================================================================

    // ==================================================================================
    private void ApplyForceToRigidbody(Collision collision)
    {
    }
    private void ApplyForceToCharacterMover(Collision collision)
    {
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
