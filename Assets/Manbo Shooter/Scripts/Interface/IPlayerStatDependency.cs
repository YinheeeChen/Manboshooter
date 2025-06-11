using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStatDependency
{
    void UpdateStats(PlayerStatManager playerStatManager);
}
