using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPattern : ScriptableObject
{
    public abstract void Fire(StateController controller);
}
