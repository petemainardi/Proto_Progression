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
 *  Manage a percentage representing a stamina value.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public class StaminaTracker : MonoBehaviour
{
	// Fields =====================================================================================
	public FloatReactiveProperty StaminaPercentage = new FloatReactiveProperty(1f);

	[SerializeField, Range(0, 1)]
	private float StaminaRegenPerSecond = 0.05f;
	// ============================================================================================

	// Mono =======================================================================================
	private void Update()
	{
		this.StaminaPercentage.Value = 
			Math.Min(1, this.StaminaPercentage.Value + this.StaminaRegenPerSecond * Time.deltaTime);
	}
	// ============================================================================================

	// Public Interface ===========================================================================
	public void UseStamina(float percentage)
    {
		percentage = Math.Abs(percentage);
		this.StaminaPercentage.Value = Math.Max(0, this.StaminaPercentage.Value - percentage);
    }

	public void RegainStamina(float percentage)
    {
        percentage = Math.Abs(percentage);
		this.StaminaPercentage.Value = Math.Min(1, this.StaminaPercentage.Value + percentage);
    }
	// ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
