using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Allow IPointsRewarders to tell this to listen to when the award points, collecting the results
 *  in this central location.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
public class PointsAccumulator : MonoBehaviour
{
    // Fields =====================================================================================
    private static List<PointsAccumulator> Accumulators = new List<PointsAccumulator>();

    [SerializeField, Sirenix.OdinInspector.Required]
    private TextMeshProUGUI Text;

    public IntReactiveProperty Points = new IntReactiveProperty();
    // ============================================================================================

    // Mono =======================================================================================
    private void Awake()
    {
        if (!Accumulators.Contains(this))
            Accumulators.Add(this);
    }
    // ============================================================================================

    // Registration ===============================================================================
    public static void RegisterWithAllAccumulators(IPointsRewarder rewarder)
    {
        Accumulators.ForEach(a => a.Register(rewarder));
    }

    public void Register(IPointsRewarder rewarder)
    {
        rewarder.Reward
            .Subscribe((int i) =>
            {
                this.Points.Value += i;
                this.Text.text = this.Points.Value.ToString();
            })
            .AddTo(this);
    }
    // ============================================================================================

}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
