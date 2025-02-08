using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Wave Data")]
public class WaveData : ScriptableObject
{
    public EnemySpawnData[] enemiesToSpawn;
}
