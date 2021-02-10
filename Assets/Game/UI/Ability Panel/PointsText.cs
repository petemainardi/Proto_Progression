using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  TEMP: Update the text of the player's point count to reflect the PlayerData state.
 *  TODO: Do this in a less ugly way...
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(TextMeshProUGUI))]
public class PointsText : MonoBehaviour
{
    [Sirenix.OdinInspector.Required]
    public PlayerData PlayerData;

    private TextMeshProUGUI TextMesh;

    private void Awake()
    {
        this.TextMesh = this.GetComponent<TextMeshProUGUI>();

        this.PlayerData
            .ObserveEveryValueChanged((PlayerData p) => p.Points)
            .Subscribe((int p) => this.ReplaceText(p))
            .AddTo(this);
    }


    public void ReplaceText(int points)
    {
        var text = this.TextMesh.text.Split(' ');
        this.TextMesh.text = $"{text[0]} {text[1]} {points}";
    }

    public void Reset()
    {
        this.PlayerData.Points = 0;
        this.PlayerData.HasDoubleJump = false;
    }

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
