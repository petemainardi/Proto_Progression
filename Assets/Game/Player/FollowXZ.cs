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
 *  Follow a target transform's movement in the XZ plane while maintaining the existing offset from
 *  that transform.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public class FollowXZ : MonoBehaviour
{
    // Fields =====================================================================================
    public Transform target;
    private Vector3 offset;

    public float followSpeed = 0.2f;
	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Start ()
	{
        if (this.target == null)
            Debug.LogError($"FollowXZ component on {this.name}:{this.GetInstanceID()} is missing a target.");

        this.offset = this.target.position - this.transform.position;
	}
	// ----------------------------------------------------------------------------------
	void Update ()
	{
        if (target != null)
        {
            Vector3 targetPos = this.target.position - this.offset;
            targetPos.y = this.transform.position.y;

            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, this.followSpeed);
        }
    }
	// ----------------------------------------------------------------------------------
	// ============================================================================================
	
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
