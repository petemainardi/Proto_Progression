using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Controllers;
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
public class ECM_Controller : BaseCharacterController
{
	// Fields =====================================================================================

	[SerializeField]
	private BounceDetector Bouncer;
	public bool CanBounce => this.movement.velocity.y < 0;
    private Vector3 bounceForce;

	public bool CanJump => this._canJump
		&& (this.isGrounded || this._midAirJumpCount < this.maxMidAirJumps);// || this.maxMidAirJumps == 0);

	// TEMP
	[Sirenix.OdinInspector.Required]
	public PlayerData PlayerData;
	public float SprintMultiplier = 1.5f;

	// ============================================================================================

	// Mono =======================================================================================
	#region Mono
	public override void Awake ()
	{
		base.Awake();

		if (this.Bouncer == null)
			Debug.LogError($"{this.NameAndID()}'s CharacterMover is missing a reference to a BounceDetector.");


	}
	// ----------------------------------------------------------------------------------
	void Start ()
	{
		this.Bouncer.SetBounceCondition(this.ObserveEveryValueChanged((ECM_Controller c) => c.CanBounce));
		this.Bouncer.BounceInfo
			.Where((BounceInfo b) => b.BouncedOn != null)
			.Subscribe((BounceInfo b) => this.Bounce(b.Direction))
			.AddTo(this);

		this.maxMidAirJumps = this.PlayerData.HasDoubleJump ? 1 : 0;
	}
    // ----------------------------------------------------------------------------------
    #endregion
    // ============================================================================================

    // ECM ========================================================================================
    protected override void Move()
    {
        base.Move();
		if (this.bounceForce.sqrMagnitude > 0)
        {
			if (Input.GetButton("Jump"))
            {
				this._updateJumpTimer = true;
				this.bounceForce.y += this.jumpImpulse;
            }
			this.movement.ApplyImpulse(this.bounceForce);
			this.bounceForce = Vector3.zero;
        }
    }
    // ============================================================================================

    private void Bounce(Vector3 direction)
    {
		//Debug.Log($"{this.NameAndID()} bouncing {direction}");
		this.bounceForce = direction;
    }

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
