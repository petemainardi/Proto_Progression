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
[RequireComponent(typeof(CharacterMover))]
public class MovementLimiter : MonoBehaviour
{
	// Fields =====================================================================================
	[SerializeField, Required] private StaminaTracker Stamina;

	private CharacterMover Mover;
	private float OriginalMoveSpeed;
	[SerializeField, Range(0, 1)] private float GroundedSpeedPercent = 0.2f;
	[SerializeField, Range(0, 1)] private float AirborneSpeedPercent = 1f;

	[SerializeField, Range(0, 1)] private float StaminaPerJump = 0.25f;
	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Awake ()
	{
		this.Mover = this.GetComponent<CharacterMover>();
		this.OriginalMoveSpeed = this.Mover.MoveSpeed;
	}
	// ----------------------------------------------------------------------------------
	void Start ()
	{
//#if UNITY_EDITOR
//		this.Mover.ObserveEveryValueChanged((CharacterMover cm) => cm.IsGrounded)
//			.Subscribe((bool b) => Debug.Log($"Grounded {b}"))
//			.AddTo(this);
//		this.Mover.ObserveEveryValueChanged((CharacterMover cm) => cm.PlayerJumped)
//			.Subscribe((bool b) => Debug.Log($"Jumped {b}"))
//			.AddTo(this);
//#endif

		this.Mover.ObserveEveryValueChanged((CharacterMover cm) => cm.IsGrounded)
			.Subscribe((bool b) =>
            {
				if (b)
					this.Mover.MoveSpeed = this.OriginalMoveSpeed * this.GroundedSpeedPercent;
            })
			.AddTo(this);

		this.Mover.ObserveEveryValueChanged((CharacterMover cm) => cm.PlayerJumped)
			.Subscribe((bool b) =>
			{
				if (b && this.Mover.IsGrounded)
					this.Stamina.UseStamina(this.StaminaPerJump);

				float jumpSpeed = this.Stamina.StaminaPercentage.Value > this.StaminaPerJump
				? this.AirborneSpeedPercent
				: this.GroundedSpeedPercent;

				this.Mover.MoveSpeed = this.OriginalMoveSpeed * jumpSpeed;
			})
			.AddTo(this);
	}
    // ----------------------------------------------------------------------------------
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
