using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareBeef : CookingState
{
    public override void EnterState(BeefStateManager beef)
    {
        Debug.Log("beef is rare now");

    }
    public override void UpdateState(BeefStateManager beef)
    {

    }
}
