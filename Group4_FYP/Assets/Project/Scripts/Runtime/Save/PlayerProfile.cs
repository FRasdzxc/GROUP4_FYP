using System;
using UnityEngine;

namespace PathOfHero.Save
{
    [Serializable]
    public class PlayerProfile
    {
        public string name;
        public int level;
        public int coins;
        public PlayerInventoryItem[] inventory;

        public string ToJson()
            => JsonUtility.ToJson(this);
        public void Load(string json)
            => JsonUtility.FromJsonOverwrite(json, this);
        public void Load(PlayerProfile profile)
        {
            name = profile.name;
            level = profile.level;
            coins = profile.coins;
            inventory = profile.inventory;
        }
    }
}
