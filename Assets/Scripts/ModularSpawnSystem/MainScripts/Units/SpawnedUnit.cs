using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SplitFace.ModularSpawnSystem
{
    public class SpawnedUnit : MonoBehaviour
    {
        //TODO: Find a way to use UnitData

        public UnitData unitData;

        public Action<SpawnedUnit> onDeathEvent;
        public Action<SpawnedUnit> onEnableEvent;

        private void OnDestroy()
        {
            onDeathEvent?.Invoke(this);
        }

        private void OnEnable()
        {
            onEnableEvent?.Invoke(this);
        }
    }
}
