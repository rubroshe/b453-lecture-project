using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillionHealthManager : MonoBehaviour
{
    private void OnEnable()
    {
        Events.rankChange.AddListener(OnRankChange);
    }

    private void OnDisable()
    {
        Events.rankChange.RemoveListener(OnRankChange);
    }

    private void OnRankChange(int rank, TeamColor teamColor)
    {
        Billions.UpdateHealthModifier(rank, teamColor);
    }
}
