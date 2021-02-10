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
 *  TEMP: Reset state of PlayerData to default.
 *  TODO: Amend this to work with actual data.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public class ResetButton : MonoBehaviour
{
    [Sirenix.OdinInspector.Required]
    public PlayerData PlayerData;

    public void Reset()
    {
        this.PlayerData.Points = 0;
        this.PlayerData.HasDoubleJump = false;
        this.PlayerData.HasSprint = false;
    }
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================