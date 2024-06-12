using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeefStateManager : MonoBehaviour
{
    CookingState currentState;
    RawBeef RawState = new RawBeef();
    RareBeef RareState = new RareBeef();
    MediumRareBeef MediumRareState = new MediumRareBeef();
    MediumWellBeef MediumWellBeef = new MediumWellBeef();
    WellBeef WellState = new WellBeef();
    BurntBeef BurntBeef = new BurntBeef();

    // Start is called before the first frame update
    void Start()
    {
        // starting state for state machine
        currentState = RawState;

        currentState.EnterState(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        
    }

    public void SwitchState(CookingState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
