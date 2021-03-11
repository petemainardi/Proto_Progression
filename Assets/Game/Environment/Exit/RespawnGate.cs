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
 *  This class does things...
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[RequireComponent(typeof(Collider))]
public class RespawnGate : MonoBehaviour
{
    // Fields =====================================================================================
    [SerializeField, Required]
    private PlayerData PlayerData;

    [SerializeField, Required]
    private HealthTracker PlayerHealth;


    // ============================================================================================
    // Mono =======================================================================================
    private void Awake()
    {
        if (!this.GetComponent<Collider>().isTrigger)
            Debug.LogError($"The Collider on RespawnGate {this.NameAndID()} should be a trigger.");
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
