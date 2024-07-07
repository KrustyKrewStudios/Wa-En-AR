using UnityEngine;

public class SirloinBeef : BeefBase
{
    public enum BeefType { Sirloin }
    //private BeefType beefType = BeefType.Sirloin;

    protected override void Start()
    {
        base.Start();
        // Additional initialization specific to Sirloin beef
    }

    protected override void Update()
    {
        base.Update();
        // Additional update logic specific to Sirloin beef
    }

    protected override void AdvanceCookingState()
    {
        base.AdvanceCookingState();
        // Additional logic specific to Sirloin beef
    }

    public override BeefState GetCurrentState()
    {
        return currentState;
    }
}
