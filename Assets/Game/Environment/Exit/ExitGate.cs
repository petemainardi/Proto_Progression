using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  When the player enters our collider area, trigger a transition to the next scene.
 *  (... by destroying the player... XD)
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(Collider))]
public class ExitGate : MonoBehaviour
{
    // Fields =====================================================================================
    [SerializeField, Required]
    private PlayerData PlayerData;

    [SerializeField, Required]
    private HealthTracker PlayerHealth;

    [SerializeField, Required]
    private PointsAccumulator Points;
    // ============================================================================================
    // Mono =======================================================================================
    private void Awake()
    {
        if (!this.GetComponent<Collider>().isTrigger)
            Debug.LogError($"The Collider on ExitGate {this.NameAndID()} should be a trigger.");
    }

    private void Start()
    {
        this.PlayerHealth
            .OnDestroyAsObservable()
            .Subscribe(_ => this.EndRun())
            .AddTo(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == PlayerHealth.Collider)
            GameObject.Destroy(other.gameObject);
    }
    // ============================================================================================

    // ==================================================================================
    private void EndRun()
    {
        if (this.PlayerHealth.Health.Value > 0)
        {
            this.PlayerData.Points += this.Points.Points.Value;
        }

        SceneManager.LoadScene(1);
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
