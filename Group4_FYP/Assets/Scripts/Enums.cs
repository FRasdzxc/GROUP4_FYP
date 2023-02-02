using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroClass { Knight, Mage, Archer, Hunter, Test };
//public enum AbilityState { ready, active, cooldown };
public enum ItemType { Weapon, Armor, Consumable, Misc };
public enum InventorySlotType { Any, Weapon, Armor, Ability };
public enum EffectType { Health, Mana, Exp };
public enum CursorType { Arrow, Crosshair };
public enum GameState { Playing, Paused };