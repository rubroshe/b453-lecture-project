using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
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
        Bullet.UpdateDamageModifier(rank, teamColor);
        BillionBaseBullet.UpdateDamageModifier(rank, teamColor);
    }

}
