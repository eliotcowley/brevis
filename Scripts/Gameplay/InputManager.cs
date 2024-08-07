using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static string RabbitPrimary
    {
        get { return PersistentData.P1Character == Character.Rabbit ? Constants.Input_P1Primary : Constants.Input_P2Primary; }
    }
        
    public static string RabbitSecondary
    {
        get { return PersistentData.P1Character == Character.Rabbit ? Constants.Input_P1Secondary : Constants.Input_P2Secondary; }
    }

    public static string RabbitSpecial
    {
        get { return PersistentData.P1Character == Character.Rabbit ? Constants.Input_P1Special : Constants.Input_P2Special; }
    }

    public static string RabbitRock
    {
        get { return PersistentData.P1Character == Character.Rabbit ? Constants.Input_P1Rock : Constants.Input_P2Rock; }
    }

    public static string RabbitPaper
    {
        get { return PersistentData.P1Character == Character.Rabbit ? Constants.Input_P1Paper : Constants.Input_P2Paper; }
    }

    public static string RabbitScissors
    {
        get { return PersistentData.P1Character == Character.Rabbit ? Constants.Input_P1Scissors : Constants.Input_P2Scissors; }
    }

    public static string DogPrimary
    {
        get { return PersistentData.P1Character == Character.Dog ? Constants.Input_P1Primary : Constants.Input_P2Primary; }
    }

    public static string DogSecondary
    {
        get { return PersistentData.P1Character == Character.Dog ? Constants.Input_P1Secondary : Constants.Input_P2Secondary; }
    }

    public static string DogSpecial
    {
        get { return PersistentData.P1Character == Character.Dog ? Constants.Input_P1Special : Constants.Input_P2Special; }
    }

    public static string DogRock
    {
        get { return PersistentData.P1Character == Character.Dog ? Constants.Input_P1Rock : Constants.Input_P2Rock; }
    }

    public static string DogPaper
    {
        get { return PersistentData.P1Character == Character.Dog ? Constants.Input_P1Paper : Constants.Input_P2Paper; }
    }

    public static string DogScissors
    {
        get { return PersistentData.P1Character == Character.Dog ? Constants.Input_P1Scissors : Constants.Input_P2Scissors; }
    }
}
