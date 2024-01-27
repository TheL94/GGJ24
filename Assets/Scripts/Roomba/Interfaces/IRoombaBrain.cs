using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoombaBrain
{
    bool PlayerDetected { get; set; }

    StateMachineBehaviour CurrentState { get; set; }
}
