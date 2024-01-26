using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitFace.ModularSpawnSystem
{
    [System.Serializable]
    public class WaveUnit : BaseUnit
    {
        public int count;

        public int amountToSpawn;

        public override void Initialize()
        {
            count = amountToSpawn;
        }
    }
}
