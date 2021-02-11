using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Articulate an object for display purposes.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public class DisplayArticulator : MonoBehaviour
{
    // Fields =====================================================================================
    public Vector3 RotationDir;
    public AnimationCurve Rotation;
    private Vector3 StartRotation;

    public Vector3 TranslationDir;
    public AnimationCurve Translation;
    private Vector3 StartPos;

    [SerializeField, ReadOnly]
    private float rotationTimer, translationTimer;

    private float endRotationTime, endTranslationTime;
    // ============================================================================================

    // Mono =======================================================================================
    private void Start()
    {
        this.StartPos = this.transform.position;
        this.StartRotation = this.transform.localRotation.eulerAngles;

        this.endRotationTime = this.Rotation.keys.Last().time;
        this.endTranslationTime = this.Translation.keys.Last().time;
    }
    private void FixedUpdate()
    {
        this.rotationTimer += Time.deltaTime;
        this.rotationTimer %= this.endRotationTime;
        this.transform.localRotation = Quaternion.Euler(this.StartRotation
            + this.RotationDir * this.Rotation.Evaluate(this.rotationTimer));

        this.translationTimer += Time.deltaTime;
        this.translationTimer %= this.endTranslationTime;
        this.transform.position = this.StartPos
            + this.TranslationDir * this.Translation.Evaluate(this.translationTimer);

    }
    // ============================================================================================

    // ==================================================================================
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
