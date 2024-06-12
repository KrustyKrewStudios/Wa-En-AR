using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawBeef : CookingState
{
    public override void EnterState(BeefStateManager beef)
    {
        Debug.Log("beef is raw state now");

    }
    public override void UpdateState(BeefStateManager beef)
    {

    }
}
