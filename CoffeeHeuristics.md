#Special Coffee maker
##Features
- 12 cups of coffee at a time;
- user places the filter in the filter holder
- user fills the filter with coffe grounds
- user slides the filter holder into into its receptacle
- the user pours up to 12 cups of water into the water strainer
- the user presses the Brew button
- the machine heats the water to boiling
- the pressure of the evolving steam forces the water to be sprayed over the coffee grounds and coffee drips through the filter into the pot
- the pot is kept warm for extended period of times by a warmer plate
- warmer plate only turns on if there is coffee in the pot
- if the pot is removed from the warmer plate while water is being sprayed over the grounds the flow of water is stopped so that bewed coffee does not spill on the warmer plate

##Hardware needed to monitored and controlled
- the heating boiler elements, can be turned on or off
- the heating element for the warmer place. it can be turned on or off
- the sensor for the warmer plate has three states: `warmerEmpty`, `potEmpty`, `potNotEmpty`
- a sensor for the boiler which determines whether there is water present; it has two states: `boilerEmpty` or `boilerNotEmpty`
- the _brew_ button; this is a momentari button that starts the brewing cycle. it has an indicator that lights up when the brewing cicle is over and the coffee is ready
- a pressure-relief valve that opens to reduce the pressure in the boiler. the drop in pressure stops the flow of water to the filter. it can be opened or closed


##Coffee maker API
```java

public interface CoffeeMakerAPI{
    public static CoffeeMakerAPI api = null;
    
    public int getWarmerPlateStatus();
    
    public static final int WARMER_EMPTY = 0;
    public static final int POT_EMPTY = 1;
    public static final int POT_NOT_EMPTY = 2;
    
    public int getBoilerStatus();
    
    public static final int BOILER_EMPTY = 0;
    public static final int BOILER_NOT_EMPTY = 1;
    
    public int getBrewButtonStatus();
    
    public static final int BREW_BUTTON_PUSHED = 0;
    public static final int BREW_BUTTON_NOT_PUSHED = 1;
    
    public void setBoilerState(int boilerStatus);
    
    public static final int BOILER_ON = 0;
    public static final int BOILER_OFF = 1;
    
    public void setWarmerState(int warmerState);
    
    public static final int WARMER_ON = 0;
    public static final int WARMER_OFF = 1;
    
    public void setIndicatorState(int indicatorState);
    
    public static final int INDICATOR_ON = 0;
    public static final int INDICATOR_OFF = 1;
    
    public void setReliefValveState(int reliefValveState);
    
    public static final int VALVE_OPEN = 0;
    public static final int VALVE_CLOSED = 1;
}
```

Exceptions:
1. `TooMuchWaterException`, `NoWaterException`
2. `NoFilterException`
3. `TooMuchCoffeeException`, `NoCoffeeException`
4. `NoFilterHolderException`, `EmptyHolderException`
5. `BoilerEmptyException`, `ReceptacleEmptyException`
6. ``