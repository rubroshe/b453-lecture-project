using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Events : UnityEvent
{
    // event for billion death
    public static UnityEvent<TeamColor, bool> billionDeath = new UnityEvent<TeamColor, bool>();

    // event for rank change, passing the new rank and team color
    public static UnityEvent<int, TeamColor> rankChange = new UnityEvent<int, TeamColor>();
}
