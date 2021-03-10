using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using Sirenix.OdinInspector;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Timer that updates a fillable Image and text to reflect the amount of time spent on the current
 *  difficulty level.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public class DifficultyTimer : MonoBehaviour
{
    // Fields =====================================================================================
    public Image timerFill;
    public TextMeshProUGUI timerText;

    public AnimationCurve timePerCycle;
    private float currentCycleTime;
    private float timer;
    private List<Func<bool>> timerConditions = new List<Func<bool>>();

    public float CurrentTime => this.timer;
    public float CurrentMaxTime => this.currentCycleTime;
    public float CurrentTimePercent => this.timer / this.currentCycleTime;


    [DisplayAsString, LabelText("Current Difficulty")]
    public IntReactiveProperty Difficulty = new IntReactiveProperty(-1);
    // ============================================================================================

    // Mono =======================================================================================
    // ----------------------------------------------------------------------------------
    void Awake ()
    {
        if (this.timerFill == null)
            Debug.LogError($"{this.name}'s DifficultyTimer is missing a reference to a fillable Image.");
        if (this.timerText == null)
            Debug.LogError($"{this.name}'s DifficultyTimer is missing a reference to a TextMeshPro Text.");
    }
    // ----------------------------------------------------------------------------------
    private void Start()
    {
        this.UpdateTimer();
    }
    // ----------------------------------------------------------------------------------
    void Update ()
	{
        if (this.timerConditions.Count == 0 || this.timerConditions.Any(c => c()))
        {
            this.timer += Time.deltaTime;
            this.UpdateTimer();
        }
    }
    // ----------------------------------------------------------------------------------
    // ============================================================================================

    // Timer ======================================================================================
    // ----------------------------------------------------------------------------------
    private void UpdateTimer()
    {
        if (this.timer >= this.currentCycleTime)
        {
            int next = this.Difficulty.Value + 1;   // Update internal state before triggering reactions

            this.timer = 0f;
            this.timerText.text = next.ToString();
            this.currentCycleTime = this.timePerCycle.Evaluate(next);

            this.Difficulty.Value++;
        }

        this.timerFill.fillAmount = this.timer / this.currentCycleTime;
    }
    // ----------------------------------------------------------------------------------
    public void AddTimerCondition(Func<bool> condition)
    {
        this.timerConditions.Add(condition);
    }
    // ----------------------------------------------------------------------------------
    // ============================================================================================
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
