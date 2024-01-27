using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitFace.ModularSpawnSystem
{
    public abstract class BaseUnit
    {
        public GameObject prefab;

        public string unitName;
        public int slotsOccupied;

        public abstract void Initialize();
    }
}

