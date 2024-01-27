using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace SplitFace.ModularSpawnSystem
{
    public class WaveSpawner : MonoBehaviour
    {
        #region INSPECTOR FIELDS

        public List<Wave> waves = new List<Wave>();
        public List<Transform> spawnPoints = new List<Transform>();
        public List<GameObject> fodderUnits = new List<GameObject>();

        public LayerMask unitLayer;
        public float spawnRadiusCheck = 2f;

        public int maxSlots;
        public int maxFodderSlots;

        public float spawnDelay;

        public bool enableFodderUnits;
        public bool waitForEmptyWave;

        #endregion

        #region PROPERTIES

        public int TotalUnitsLeft
        {
            get
            {
                int totalUnits = 0;

                foreach (Wave wave in waves)
                {
                    totalUnits += currentWave.unitsLeft;
                }

                return totalUnits;
            }
        }

        public int CurrentWaveIndex { get => waves.IndexOf(currentWave); }
        public int WavesCount { get => waves.Count; }

        #endregion

        private int usedSlots = 0;
        private int usedFodderSlots = 0;

        private List<SpawnedUnit> activeUnits = new List<SpawnedUnit>();
        private List<SpawnedUnit> activeFodderUnits = new List<SpawnedUnit>();

        private Wave currentWave;

        /// <summary>
        /// Starts the spawner
        /// </summary>
        public void StartSpawner()
        {
            for (int i = 0; i < waves.Count; i++)
            {
                waves[i] = new Wave(waves[i]);
            }

            currentWave = waves[0];
            currentWave.Initialize();

            StartAsyncSpawn();
        }

        /// <summary>
        /// Resets the WaveSpawner to the first wave
        /// </summary>
        public void ResetSpawner()
        {
            CleanUnitList();

            currentWave = waves[0];
            currentWave.Initialize();

            StartAsyncSpawn();
        }

        /// <summary>
        /// Stops the WaveSpawner
        /// </summary>
        public void StopSpawner()
        {
            StopAllCoroutines();
            CleanUnitList();

            currentWave = waves[0];
        }

        /// <summary>
        /// Switches to the next wave, if it's available
        /// </summary>
        public void SwitchWave()
        {
            SwitchWave(waves.IndexOf(currentWave) + 1);
        }

        public void SwitchWave(int index)
        {
            if (index < waves.Count && waves[index] != null)
            {
                currentWave = waves[index];
                currentWave.Initialize();

                StartAsyncSpawn();
            }
            else
            {
                StopSpawner();
            }
        }

        /// <summary>
        /// Removes inactive and null units from the lists of active units
        /// </summary>
        private void CleanUnitList()
        {
            activeUnits = new List<SpawnedUnit>();
            activeFodderUnits = new List<SpawnedUnit>();

            //if (activeUnits.Count > 0)
            //{
            //    foreach (SpawnedUnit unit in activeUnits)
            //    {
            //        Destroy(unit.gameObject);
            //    }

            //    activeUnits = new List<SpawnedUnit>();
            //}

            //if (activeFodderUnits.Count > 0)
            //{
            //    foreach (SpawnedUnit unit in activeFodderUnits)
            //    {
            //        Destroy(unit.gameObject);
            //    }

            //    activeFodderUnits = new List<SpawnedUnit>();
            //}
        }

        /// <summary>
        /// Checks if there's enough space to spawn the unit in the given location
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <returns></returns>
        private bool SpawnIsLegal(Transform targetTransform)
        {
            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, spawnRadiusCheck, unitLayer);

            foreach (Collider collider in colliders)
            {
                GameObject go = collider.gameObject;

                if ((unitLayer.value & (1 << go.layer)) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        #region UNIT CALLBACKS

        private void OnUnitDeath(SpawnedUnit spawnedUnit)
        {
            activeUnits.Remove(spawnedUnit);

            usedSlots -= spawnedUnit.unitData.slotsOccupied;

            if ((!waitForEmptyWave && !currentWave.HasPrefabAvailable(maxSlots - usedSlots) && !(currentWave.timeBeforeNextWave > 0))
                || (waitForEmptyWave && currentWave.isWaveEmpty && usedSlots == 0) && !(currentWave.timeBeforeNextWave > 0))
                SwitchWave();
        }

        private void OnFodderUnitDeath(SpawnedUnit spawnedUnit)
        {
            activeFodderUnits.Remove(spawnedUnit);

            usedFodderSlots -= spawnedUnit.unitData.slotsOccupied;
        }

        private void OnUnitEnabled(SpawnedUnit spawnedUnit)
        {
            activeUnits.Add(spawnedUnit);

            usedSlots += spawnedUnit.unitData.slotsOccupied;
        }

        private void OnFodderUnitEnabled(SpawnedUnit spawnedUnit)
        {
            activeFodderUnits.Add(spawnedUnit);

            usedFodderSlots += spawnedUnit.unitData.slotsOccupied;
        }

        #endregion

        #region ASYNC SPAWN

        private void StartAsyncSpawn()
        {
            StopAllCoroutines();
            StartCoroutine(SpawnUnits());
            StartCoroutine(SpawnFodderUnits());
        }

        private IEnumerator SpawnUnits()
        {
            while (!currentWave.isWaveEmpty)
            {
                #region WAVE ENEMIES

                if (usedSlots < maxSlots)
                {
                    int slotsToFill = maxSlots - usedSlots;

                    List<Transform> legalSpawns = new List<Transform>();

                    foreach (Transform spawnPoint in spawnPoints)
                    {
                        if (SpawnIsLegal(spawnPoint))
                        {
                            legalSpawns.Add(spawnPoint);
                        }
                    }

                    while (currentWave.HasPrefabAvailable(slotsToFill) && legalSpawns.Count > 0)
                    {
                        Transform spawnPosition = null;

                        //if (currentWave.CheckCustomSpawn && SpawnIsLegal(currentWave.GetCustomSpawn))
                        //{
                        //    spawnPosition = currentWave.GetCustomSpawn;
                        //}
                        if (legalSpawns.Any())
                        {
                            spawnPosition = legalSpawns[UnityEngine.Random.Range(0, legalSpawns.Count)];
                        }

                        if (spawnPosition != null)
                        {
                            (int, GameObject) objectData = currentWave.GetPrefab(slotsToFill);

                            SpawnedUnit newUnit = Instantiate(objectData.Item2, spawnPosition.position, spawnPosition.rotation).
                                AddComponent<SpawnedUnit>();

                            newUnit.unitData.slotsOccupied = objectData.Item1;
                            usedSlots += objectData.Item1;

                            newUnit.onDeathEvent = OnUnitDeath;
                            newUnit.onEnableEvent = OnUnitEnabled;

                            activeUnits.Add(newUnit);

                            //if (!currentWave.CheckCustomSpawn)
                            //{
                            //    legalSpawns.Remove(spawnPosition);
                            //}

                            slotsToFill -= objectData.Item1;
                        }
                        else
                        {
                            slotsToFill -= currentWave.AvailablePrefab.slotsOccupied;
                        }
                    }
                }

                #endregion

                yield return new WaitForSeconds(spawnDelay);
            }

            if (currentWave.timeBeforeNextWave > 0)
            {
                yield return new WaitForSecondsRealtime(currentWave.timeBeforeNextWave);
                SwitchWave();
            }
        }

        IEnumerator SpawnFodderUnits()
        {
            while (enableFodderUnits)
            {
                #region FODDER UNITS

                int slotsToFill = maxFodderSlots - usedFodderSlots;

                for (int i = 0; i < slotsToFill; i++)
                {
                    GameObject objectToIstantiate = fodderUnits[UnityEngine.Random.Range(0, fodderUnits.Count)];
                    Transform spawnPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];

                    if (SpawnIsLegal(spawnPosition))
                    {
                        SpawnedUnit newFodderUnity = Instantiate(objectToIstantiate, spawnPosition.position, spawnPosition.rotation).
                            AddComponent<SpawnedUnit>();

                        newFodderUnity.unitData.slotsOccupied = 1;

                        usedFodderSlots++;

                        newFodderUnity.onDeathEvent = OnFodderUnitDeath;
                        newFodderUnity.onEnableEvent = OnFodderUnitEnabled;

                        activeFodderUnits.Add(newFodderUnity);
                    }
                }

                #endregion

                yield return new WaitForSeconds(spawnDelay);
            }
        }

        #endregion
    }
}
