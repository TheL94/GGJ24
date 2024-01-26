using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplitFace.ModularSpawnSystem
{
    public interface IPool<T>
    {
        public T Sample { get; }

        public T Get();
        public T Reclaim();
    }
}
