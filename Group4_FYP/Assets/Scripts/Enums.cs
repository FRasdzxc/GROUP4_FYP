using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroClass { Knight, Mage, Archer, Hunter, Test };
//public enum AbilityState { ready, active, cooldown };
public enum ItemType { Weapon, Armor, Consumable, Misc, Relic }; // might not be used? keep as a reference?
public enum InventorySlotType { Any, Weapon, Armor, Ability };
public enum EffectType { Health, Mana, Exp };
public enum CursorType { Arrow, Crosshair };
public enum GameState { Playing, Paused };
public enum MapType { Peaceful, Hostile, Dungeon }
public enum OrbUpgradeType { MaxHealth, HealthRegeneration, MaxMana, ManaRegeneration, ExpGainMultiplier, Defense };
public enum TooltipHintType { None, Use, Drop, UseAll, DropAll };
public enum NPCType { Normal, Merchant };
public enum BuySellType { Buy, Sell };