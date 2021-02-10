using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  TEMP: Simulate purchasing an ability by specifically allowing the purchase of DoubleJump or
 *  Sprint.
 *  TODO: Replace this with an actual abstraction.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(Button))]
public class AbilityButton : MonoBehaviour
{
    // Fields =====================================================================================
    private Button button;

    [Sirenix.OdinInspector.Required]
    public PlayerData PlayerData;
    private bool IsForSprint;

    [SerializeField]
    private int Cost = 100;

    public GameObject CostImage;
    // ============================================================================================

    // Mono =======================================================================================
    private void Awake()
    {
        this.IsForSprint = this.name.ToLower().Contains("sprint");
        this.button = this.GetComponent<Button>();

        if ((this.IsForSprint ? this.PlayerData.HasSprint : this.PlayerData.HasDoubleJump))
            this.MarkAsPurchased();
        else if (PlayerData.Points < this.Cost)
            this.button.interactable = false;
    }
    // ============================================================================================

    // Events =====================================================================================
    public void PurchaseAbility()
    {
        this.PlayerData.Points -= this.Cost;
        if (this.IsForSprint)
            this.PlayerData.HasSprint = true;
        else
            this.PlayerData.HasDoubleJump = true;
        this.MarkAsPurchased();
    }

    private void MarkAsPurchased()
    {
        this.button.interactable = false;
        if (this.CostImage != null)
            this.CostImage.SetActive(false);
    }
	// ============================================================================================
	
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
