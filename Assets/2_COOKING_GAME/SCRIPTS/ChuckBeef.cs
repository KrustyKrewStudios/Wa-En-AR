using UnityEngine;

public class ChuckBeef : BeefBase
{
    public enum BeefType { Chuck }
    //private BeefType beefType = BeefType.Karubi;

    protected override void Start()
    {
        base.Start();
        // Additional initialization specific to Karubi beef
    }

    protected override void Update()
    {
        base.Update();
        // Additional update logic specific to Karubi beef
    }

    protected override void AdvanceCookingState()
    {
        base.AdvanceCookingState();
        // Additional logic specific to Karubi beef
    }

    public override BeefState GetCurrentState()
    {
        return currentState;
    }
}
