
/// <summary>
/// This class keeps up with the state of the passenger.
/// It performs validations to make sure we don't have impossible state changes
/// </summary>
public class clsPassengerStateManager
{
    /// <summary>
    /// 
    /// </summary>
    public enum enState
    {
        ArriveAtNewLevel,
        Spawning,
        Hailing,
        Boarding,
        Riding,
        Deboarding,
        Leaving,
        Dying,
        Dead
    }

    /// <summary>
    /// 
    /// </summary>
    public enState state = enState.Spawning;

    public bool PlayingBoardAnimation = false;
    public bool InitBoardAnimation = false;
    public bool PlayingUnboardAnimation = false;
    public bool InitUnboardAnimation = false;
    public bool PlayingDyingAnimation = false;
    public bool InitDyingAnimation = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="InitialState"></param>
    public clsPassengerStateManager(enState InitialState)
    {
        SetState(InitialState);
    }

    /// <summary>
    /// Validates the state change against game rules.
    /// If the state change is valid, then it is peformed
    /// </summary>
    /// <param name="NextState"></param>
    public void SetState(enState NextState)
    {
        bool blnValidStateChange = false;

        switch (NextState)
        {
            case enState.ArriveAtNewLevel:
                blnValidStateChange = true;
                break;
            case enState.Spawning:
                blnValidStateChange = true;
                //reset all animation flags
                ResetAllAnimationFlags();
                break;
            case enState.Hailing:
                if (state == enState.Spawning) blnValidStateChange = true;
                break;
            case enState.Boarding:
                if (state == enState.Hailing) blnValidStateChange = true;
                break;
            case enState.Riding:
                if (state == enState.Boarding)
                {
                    blnValidStateChange = true;
                    ResetAllAnimationFlags();
                }
                break;
            case enState.Deboarding:
                if (state == enState.Riding) blnValidStateChange = true;
                break;
            case enState.Leaving:
                if (state == enState.Deboarding)
                {
                    blnValidStateChange = true;
                    ResetAllAnimationFlags();
                }
                break;
            case enState.Dying:
                blnValidStateChange = true;
                break;
            case enState.Dead:
                if (state == enState.Dying)
                {
                    blnValidStateChange = true;
                    ResetAllAnimationFlags();
                }
                break;
            default:
                blnValidStateChange = true;
                ResetAllAnimationFlags();
                break;
        }

        //if the state change passed validation, then perform it
        if (blnValidStateChange == true) state = NextState;
    }

    /// <summary>
    /// 
    /// </summary>
    private void ResetAllAnimationFlags()
    {
        PlayingBoardAnimation = false;
        InitBoardAnimation = false;
        PlayingUnboardAnimation = false;
        InitUnboardAnimation = false;
        PlayingDyingAnimation = false;
        InitDyingAnimation = false;
    }
}

