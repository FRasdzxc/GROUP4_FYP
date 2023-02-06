using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Orb Upgrade List", menuName = "Game/Orb Upgrade List")]
public class OrbUpgradeList : ScriptableObject
{
    [Serializable]
    public struct OrbUpgradeEntry
    {
        public string orbUpgradeName;
        public OrbUpgradeType type;
        [TextArea(2, 2)] public string description;
        public float value;
    }

    public OrbUpgradeEntry[] orbUpgrades;
}
