using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamColor
{
    Green,
    Yellow,
    Red,
    Blue
    // Add more team colors as needed
}

public class TeamIdentifier : MonoBehaviour
{
    public TeamColor teamColor;
    public bool isFlag; // distinguish flags from billions
}

