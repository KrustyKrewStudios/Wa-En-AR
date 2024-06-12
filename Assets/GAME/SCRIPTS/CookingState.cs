using UnityEngine;

public abstract class CookingState
{
    public abstract void EnterState(BeefStateManager beef);
    public abstract void UpdateState(BeefStateManager beef);


}
