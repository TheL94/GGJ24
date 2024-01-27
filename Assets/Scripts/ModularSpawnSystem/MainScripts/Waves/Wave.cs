using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SplitFace.ModularSpawnSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Wave", menuName = "WaveSpawner/New wave")]
    public class Wave : ScriptableObject
    {
        public bool isWaveEmpty { get { return unitsLeft == 0 && !isInfinite; } }
        public WaveUnit AvailablePrefab { get { return availablePrefab; } }

        public string waveName = "New Wave";
        public bool shuffleOnSpawn = true;
        public float timeBeforeNextWave = 0;
        public bool isInfinite = false;

        public List<WaveUnit> unitsToSpawn = new List<WaveUnit>();

        public int unitsLeft = 0;

        private WaveUnit availablePrefab;

        public Wave(Wave wave)
        {
            waveName = wave.waveName;
            shuffleOnSpawn = wave.shuffleOnSpawn;
            timeBeforeNextWave = wave.timeBeforeNextWave;
            isInfinite = wave.isInfinite;

            unitsToSpawn = wave.unitsToSpawn;
        }

        /// <summary>
        /// Initializes the wave, resetting the count of enemies to spawn
        /// </summary>
        public void Initialize()
        {
            foreach (WaveUnit enemy in unitsToSpawn)
            {
                enemy.Initialize();
            }

            unitsLeft = unitsToSpawn.Sum(x => x.count);
        }

        /// <summary>
        /// Returns the Prefab to spawn along with the slots occupied
        /// </summary>
        /// <param name="slotsToFill"></param>
        /// <returns></returns>
        public (int, GameObject) GetPrefab(int slotsToFill)
        {
            if (!HasPrefabAvailable(slotsToFill))
                return (0, null);

            if (availablePrefab.slotsOccupied <= slotsToFill && availablePrefab.count > 0)
            {
                if (!isInfinite)
                    availablePrefab.count--;

                if (shuffleOnSpawn)
                    unitsToSpawn.Shuffle();

                unitsLeft--;
                return (availablePrefab.slotsOccupied, availablePrefab.prefab);
            }

            return (0, null);
        }

        /// <summary>
        /// Checks if there's a prefab available to fill the amount of slots given
        /// </summary>
        /// <param name="slotsToFill"></param>
        /// <returns></returns>
        public bool HasPrefabAvailable(int slotsToFill)
        {
            foreach (WaveUnit unitData in unitsToSpawn)
            {
                if (unitData.slotsOccupied <= slotsToFill && unitData.count > 0)
                {
                    availablePrefab = unitData;

                    return true;
                }
            }

            return false;
        }
    }
}
