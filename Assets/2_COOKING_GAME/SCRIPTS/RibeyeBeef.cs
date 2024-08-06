/*
 * Author: Curtis Low
 * Date: 06/08/2024
 * Description:  * This class defines behavior specific to ribeye beef.
 * It extends the BeefBase class, inheriting common functionalities for handling 
 * beef cooking states and interactions with the grill.
 */
using UnityEngine;

public class RibeyeBeef : BeefBase
{
    public enum BeefType { Ribeye }

    // Override Start method from BeefBase
    protected override void Start()
    {
        base.Start();
    }

    // Override Update method from BeefBase
    protected override void Update()
    {
        base.Update();
    }

    // Override Update AdvanceCookingState from BeefBase
    protected override void AdvanceCookingState()
    {
        base.AdvanceCookingState();
    }

    // Override GetCurrentState method from BeefBase
    public override BeefState GetCurrentState()
    {
        return currentState;
    }
}
