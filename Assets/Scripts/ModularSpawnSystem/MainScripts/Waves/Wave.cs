using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SplitFace.ModularSpawnSystem
{
    [System.Serializable]
    //[CreateAssetMenu(fileName = "New Wave", menuName = "WaveSpawner/New wave")]
    public class Wave /*: ScriptableObject*/
    {
        public bool isWaveEmpty { get { return unitsLeft == 0; } }
        public WaveUnit AvailablePrefab { get { return availablePrefab; } }

        public string waveName = "New Wave";
        public bool shuffleOnSpawn = true;

        public List<WaveUnit> unitsToSpawn = new List<WaveUnit>();

        public int unitsLeft = 0;

        private WaveUnit availablePrefab;

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
                availablePrefab.count--;

                if (shuffleOnSpawn)
                    unitsToSpawn.Shuffle();

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
            foreach (WaveUnit enemyData in unitsToSpawn)
            {
                if (enemyData.slotsOccupied <= slotsToFill && enemyData.count > 0)
                {
                    availablePrefab = enemyData;

                    return true;
                }
            }

            return false;
        }
    }
}
