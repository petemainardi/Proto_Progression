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
 *  Restore the specified amount of stamina every time a new target is bounced upon.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public class RestoreStaminaOnBounce : MonoBehaviour
{
	// Fields =====================================================================================
	[SerializeField, Required] private StaminaTracker Stamina;
	[SerializeField, Range(0, 1)] private float RestorePerBounce = 0.25f;

	[SerializeField, Required] private BounceDetector Bouncer;
	[SerializeField] private bool OnlyRestoreOnNewTarget = true;
	private Collider lastBouncedOn;
	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Start ()
	{
		this.Bouncer.BounceInfo
			.Where((BounceInfo b) =>
				b.BouncedOn != null && (this.lastBouncedOn != b.BouncedOn || !this.OnlyRestoreOnNewTarget))
			.Subscribe((BounceInfo b) =>
			{
				//Debug.Log($"Bounce {b.BouncedOn.NameAndID()}:{b.Direction}");
				this.lastBouncedOn = b.BouncedOn;
				this.Stamina.RegainStamina(this.RestorePerBounce);
			})
			.AddTo(this);
	}
	// ----------------------------------------------------------------------------------
	// ============================================================================================
	
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
