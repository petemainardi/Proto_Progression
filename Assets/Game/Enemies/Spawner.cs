using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#pragma warning disable 0649    // Variable declared but never assigned to


// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
/**
 *  Handles instantiation of provided prefabs according to the current difficulty level.
 *  An initial burst of objects can be spawned when a new difficulty level is reached, as well as
 *  distributing spawning over the course of a difficulty duration.
 */
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
[Serializable]
public class Spawner : SerializedMonoBehaviour
{
    // Fields =====================================================================================
    public DifficultyTimer DifficultyTimer;
    private float currentDifficultyTime;
    private float spawnTimer;

    public int NumToSpawnInitially = 5;         // Spawned all at once at beginning of new difficulty
    public int NumToSpawnPerDifficulty = 5;     // Spawned over time, multiplied by difficulty level
    public Collider SpawnArea;

    [SerializeField, Space]
    private List<(int, GameObject)> spawnables = new List<(int, GameObject)>();
    private List<GameObject> canSpawn = new List<GameObject>();
    // ============================================================================================

    // Mono =======================================================================================
    // ----------------------------------------------------------------------------------
    void Awake()
    {
        if (this.SpawnArea == null)
            Debug.LogError($"{this.name}'s Spawner is missing a reference to a Collider for its spawn area.");

        if (this.DifficultyTimer == null)
            Debug.LogError($"{this.name}'s Spawner is missing a reference to a DifficultyTimer.");

        this.DifficultyTimer.Difficulty
            .Subscribe((int i) => this.IncreaseDifficulty(i))
            .AddTo(this);
    }
	// ----------------------------------------------------------------------------------
	void Update ()
	{
        this.spawnTimer += Time.deltaTime;
        if (this.spawnTimer >= this.currentDifficultyTime)
        {
            this.spawnTimer = 0;
            this.SpawnOne();
        }
	}
	// ============================================================================================

	// Events =====================================================================================
    private void IncreaseDifficulty(int difficulty)
    {
        this.canSpawn.AddRange(
            this.spawnables
            .Where(s => s.Item1 <= difficulty && !this.canSpawn.Contains(s.Item2))
            .Select(s => s.Item2)
            );
        
        this.Spawn(this.NumToSpawnInitially);

        this.spawnTimer = 0;
        this.currentDifficultyTime = this.DifficultyTimer.CurrentMaxTime
            / (this.NumToSpawnPerDifficulty * Math.Max(difficulty, 1));
    }
    // ============================================================================================

    // Spawning ===================================================================================
    private void Spawn(int n)
    {
        if (this.canSpawn.Count > 0)
        {
            List<GameObject> selected = RandomExtensions.GetEvenlyNRandom(this.canSpawn.ToList(), n, false);
            foreach (GameObject g in selected)
                this.Spawn(g);
        }
        else
            Debug.LogWarning($"{this.name}:{this.GetInstanceID()} has no spawnable prefabs for difficulty {this.DifficultyTimer.Difficulty.Value}");
    }

    private void SpawnOne()
    {
        if (this.canSpawn.Count > 0)
            this.Spawn(this.canSpawn.RandomElement());
        else
            Debug.LogWarning($"{this.name}:{this.GetInstanceID()} has no spawnable prefabs for difficulty {this.DifficultyTimer.Difficulty.Value}");
    }

    private void Spawn(GameObject g)
    {
        Bounds bounds = this.SpawnArea.bounds;
        float minDimension = Math.Min(
            bounds.extents.x,
            bounds.extents.z
            );

        Vector2 randDirection = UnityEngine.Random.insideUnitCircle * minDimension;
        Vector3 randPosition = new Vector3(randDirection.x, 0, randDirection.y);
        randPosition += bounds.center;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randPosition, out navHit, minDimension, -1);

        if (!navHit.hit)
            Debug.LogWarning($"{this.name}:{this.GetInstanceID()} missed a spawn check at {randPosition}.");
        else
        {
            GameObject.Instantiate(
                g, navHit.position, Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
        }
    }
	// ============================================================================================
	
}
// ================================================================================================
// ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
// ================================================================================================
