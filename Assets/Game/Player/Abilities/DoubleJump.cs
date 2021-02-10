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
 *  Allows for an extra jump to influence a CharacterMover's velocity.
 *  In order to do so, the CharacterMover must already be in the air, not still be moving upward,
 *  and not have already used this ability before landing.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(CharacterMover))]
public class DoubleJump : MonoBehaviour
{
    // Fields =====================================================================================
    private CharacterMover mover;

    public float JumpSpeed = 0.5f;
    private bool jumpInput;
    private bool hasDoubleJumped;

	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Awake ()
	{
        this.mover = this.GetComponent<CharacterMover>();
	}
    // ----------------------------------------------------------------------------------
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            this.jumpInput = true;
        else if (Input.GetButtonUp("Jump"))
            this.jumpInput = false;
    }
    // ----------------------------------------------------------------------------------
    void FixedUpdate ()
	{
        if (this.mover.IsGrounded)
            this.hasDoubleJumped = false;

		if (this.jumpInput && this.CanDoubleJump)
        {
            this.hasDoubleJumped = true;
            this.mover.AddForce(Vector3.up * this.JumpSpeed);
        }
    }
    // ----------------------------------------------------------------------------------
    // ============================================================================================

    private bool CanDoubleJump =>
        !this.mover.IsGrounded
        && this.mover.Movement.y <= 0
        && !this.hasDoubleJumped
        ;
	
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
