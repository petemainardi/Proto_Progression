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
[RequireComponent(typeof(CharacterController))]
public class CharacterMover : MonoBehaviour
{
    // Fields =====================================================================================
    [SerializeField]
    private float moveSpeed = 6.8f;
    private Vector3 moveDir = Vector3.zero;

    [SerializeField]
    private float jumpSpeed = 0.5f;
    [SerializeField]
    private float gravity = 2f;

    private CharacterController charControl;
	// ============================================================================================

	// Mono =======================================================================================
	void Awake ()
	{
        this.charControl = this.GetComponent<CharacterController>();
	}
	// ----------------------------------------------------------------------------------
	void Start ()
	{
		
	}
	// ----------------------------------------------------------------------------------
	void FixedUpdate ()
	{
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(x, 0, z).normalized;
        Vector3 transformDir = inputDir;// this.transform.TransformDirection(inputDir);
        Vector3 flatMovement = transformDir * this.moveSpeed * Time.deltaTime;

        // TODO: If in air, maintain momentum and use to adjust flat movement

        this.moveDir = new Vector3(flatMovement.x, this.moveDir.y, flatMovement.z);
        this.moveDir.y = this.PlayerJumped
            ? this.jumpSpeed
            : this.charControl.isGrounded
            ? 0f
            : this.moveDir.y - this.gravity * Time.deltaTime;

        this.charControl.Move(this.moveDir);
	}
    // ============================================================================================

    // Helpers ====================================================================================
    private bool PlayerJumped => this.charControl.isGrounded && Input.GetButton("Jump");
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
