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
 *  Limit the air control value of the attached CharacterMover component based on the currently
 *  available stamina.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(ECM_Controller))]
public class AirControlLimiter : MonoBehaviour
{
	// Fields =====================================================================================
	[SerializeField, Required] private StaminaTracker Stamina;

	private ECM_Controller Mover;
	[SerializeField, MinMaxSlider(0, 1)] private Vector2 controlRange;
	public float MinControl => this.controlRange.x;
	public float MaxControl => this.controlRange.y;
	// ============================================================================================

	// Mono =======================================================================================
	// ----------------------------------------------------------------------------------
	void Awake ()
	{
		this.Mover = this.GetComponent<ECM_Controller>();
	}
	// ----------------------------------------------------------------------------------
	void Start ()
	{
        this.Stamina.StaminaPercentage
            .Subscribe((float s) => this.Mover.airControl = this.NormalizeToRange(s))
			.AddTo(this);
	}
	// ----------------------------------------------------------------------------------
	// ============================================================================================

	// ==================================================================================
	public float NormalizeToRange(float stamina)
    {
		return Mathf.Clamp01(stamina) * (this.MaxControl - this.MinControl) + this.MinControl;
    }
	// ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
