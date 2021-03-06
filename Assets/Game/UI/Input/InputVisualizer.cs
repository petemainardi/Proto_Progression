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
 *  Enable and disable a UI element based on whether the specified Keycode is active.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(Button))]
public class InputVisualizer : MonoBehaviour
{
    // Fields =====================================================================================
    [SerializeField] private List<KeyCode> keys = new List<KeyCode>();

    private Button Button;
    // ============================================================================================

    // Mono =======================================================================================
    // ----------------------------------------------------------------------------------
    private void Awake()
    {
		this.Button = this.GetComponent<Button>();
    }
    // ----------------------------------------------------------------------------------
    void Update ()
	{
        this.Button.interactable = keys.Any(k => Input.GetKey(k));
	}
    // ----------------------------------------------------------------------------------
    // ============================================================================================	
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
