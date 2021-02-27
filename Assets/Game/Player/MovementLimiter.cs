using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Limit the movement speed of an attached CharacterMover component based on whether the character
 *  is grounded or jumping, as well as how much stamina is currently available.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(ECM_Controller))]
public class MovementLimiter : MonoBehaviour
{
	// Fields =====================================================================================
	[SerializeField, Required] private StaminaTracker Stamina;

	private ECM_Controller Mover;
	private float OriginalMoveSpeed;
	[SerializeField, Range(0, 1)] private float GroundedSpeedPercent = 0.2f;
	[SerializeField, Range(0, 1)] private float AirborneSpeedPercent = 1f;

	[SerializeField, Required] private BounceDetector Bouncer;
	private Collider lastBouncedOn;


	[SerializeField, Range(0, 1)] private float StaminaPerJump = 0.25f;
	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Awake ()
	{
		this.Mover = this.GetComponent<ECM_Controller>();
		this.OriginalMoveSpeed = this.Mover.speed;
	}
	// ----------------------------------------------------------------------------------
	void Start ()
	{
		// Limit grounded movement speed
		this.Mover.ObserveEveryValueChanged((ECM_Controller cm) => cm.isGrounded)
			.Where((bool b) => b)
			.Subscribe((bool b) =>
				this.Mover.speed = this.OriginalMoveSpeed * this.GroundedSpeedPercent
				)
			.AddTo(this);

		// On jump, use stamina and airborne speed
		this.Mover.ObserveEveryValueChanged((ECM_Controller cm) => cm.jump)
			.Where((bool b) => b && this.Mover.CanJump)
			.Subscribe((bool b) =>
			{
				this.Stamina.UseStamina(this.StaminaPerJump);
				this.Mover.speed = this.OriginalMoveSpeed * this.AirborneSpeedPercent;
			})
			.AddTo(this);

		// Restore jump stamina when bouncing on a new target
		// BUG: this is getting doubled...
		this.Bouncer.BounceInfo
			.Where((BounceInfo b) => this.lastBouncedOn != b.BouncedOn)
			.Subscribe((BounceInfo b) =>
			{
				Debug.Log("Bounce reaction");
				this.lastBouncedOn = b.BouncedOn;
				this.Stamina.RegainStamina(this.StaminaPerJump);
			})
			.AddTo(this);
	}
    // ----------------------------------------------------------------------------------
    // ============================================================================================
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
