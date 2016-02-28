public interface ICoffeeMakerAPI
{
    WarmerPlateStatus GetWarmerPlateStatus();
    BoilerStatus GetBoilerStatus();
    BrewButtonStatus GetBrewButtonStatus();
    void SetBoilerState(BoilerState boilerState);
    void SetWarmerState(WarmerPlateState warmerState);
    void SetIndicatorState(IndicatorState indicatorState);
    void SetReliefValveState(ReliefValveState reliefValveState);
}

public interface ICoffeeMakerAPIController
{
    WarmerPlateStatus WarmerPlateStatus { get; set; }
    BrewButtonStatus BrewButtonStatus { get; set; }
    BoilerStatus BoilerStatus { get; set; }
    WarmerPlateState WarmerPlateState { get; set; }
    BoilerState BoilerState { get; set; }
    IndicatorState IndicatorState { get; set; }
    ReliefValveState ReliefValveState { get; set; }
}

public enum WarmerPlateStatus
{
    WARMER_EMPTY = 0,
    POT_EMPTY = 1,
    POT_NOT_EMPTY = 2
}

public enum BoilerStatus
{
    BOILER_EMPTY = 0,
    BOILER_NOT_EMPTY = 1
}

public enum BrewButtonStatus
{
    BREW_BUTTON_PUSHED = 0,
    BREW_BUTTON_NOT_PUSHED = 1
}

public enum BoilerState
{
    BOILER_ON = 0,
    BOILER_OFF = 1
}

public enum WarmerPlateState
{
    WARMER_ON = 0,
    WARMER_OFF = 1
}

public enum IndicatorState
{
    INDICATOR_ON = 0,
    INDICATOR_OFF = 1
}

public enum ReliefValveState
{
    VALVE_OPEN = 0,
    VALVE_CLOSED = 1
}

public class CoffeeMakerAPI
    : ICoffeeMakerAPI,
        ICoffeeMakerAPIController
{
    public static ICoffeeMakerAPI instance = new CoffeeMakerAPI
    {
        BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_NOT_PUSHED,
        BoilerState = BoilerState.BOILER_OFF
    };

    public WarmerPlateStatus GetWarmerPlateStatus()
    {
        return WarmerPlateStatus;
    }

    public BoilerStatus GetBoilerStatus()
    {
        return BoilerStatus;
    }

    public BrewButtonStatus GetBrewButtonStatus()
    {
        //Resets the button status after first query
        var buttonState = BrewButtonStatus;
        BrewButtonStatus = BrewButtonStatus.BREW_BUTTON_NOT_PUSHED;

        return buttonState;
    }

    public void SetBoilerState(BoilerState boilerState)
    {
        BoilerState = boilerState;
    }

    public void SetWarmerState(WarmerPlateState warmerState)
    {
        WarmerPlateState = warmerState;
    }

    public void SetIndicatorState(IndicatorState indicatorState)
    {
        IndicatorState = indicatorState;
        if (IndicatorState == IndicatorState.INDICATOR_OFF &&
            WarmerPlateStatus != WarmerPlateStatus.WARMER_EMPTY)
        {
            WarmerPlateStatus = WarmerPlateStatus.POT_NOT_EMPTY;
        }
    }

    public void SetReliefValveState(ReliefValveState reliefValveState)
    {
        ReliefValveState = reliefValveState;
    }

    public WarmerPlateStatus WarmerPlateStatus { get; set; }
    public BrewButtonStatus BrewButtonStatus { get; set; }
    public BoilerStatus BoilerStatus { get; set; }
    public WarmerPlateState WarmerPlateState { get; set; }
    public BoilerState BoilerState { get; set; }
    public IndicatorState IndicatorState { get; set; }
    public ReliefValveState ReliefValveState { get; set; }
}