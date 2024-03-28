using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Events : UnityEvent
{
    public static UnityEvent<TeamColor, bool> billionDeath = new UnityEvent<TeamColor, bool>();
}
