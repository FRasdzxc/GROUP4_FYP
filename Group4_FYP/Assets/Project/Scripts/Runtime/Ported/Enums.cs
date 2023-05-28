public enum HeroClass { Knight, Mage, Archer, Hunter, Test };
//public enum AbilityState { ready, active, cooldown };
public enum ItemType { Weapon, Armor, Consumable, Misc, Runic, Gem, Fragment };
public enum InventorySlotType { Any, Weapon, Armor, Ability };
public enum InventoryMode { Normal, Apply, Revert, Throw, Preview };
public enum EffectType { Health, Mana, Exp };
// public enum CursorType { Arrow, Crosshair };
public enum GameState { Playing, Paused };
public enum MapType { Peaceful, Hostile, Dungeon }
public enum MapDifficulty { Peaceful, Easy, Medium, Hard, Extreme, Varies }
public enum DungeonType { Regular, Wave, Boss, Special }
public enum OrbUpgradeType { MaxHealth, HealthRegeneration, MaxMana, ManaRegeneration, ExpGainMultiplier, Defense, AbilityDamage };
public enum TooltipHintType { None, Use, Drop, UseAll, DropAll, Transfer, TransferAll };
public enum NPCType { Normal, Merchant, Blacksmith };
public enum BuySellType { Buy, Sell };
public enum EventRequestType { None, ShowBuyPanel, ShowSellPanel, ShowWeaponUpgradePanel, ShowHeroPanel, GoToTown };
public enum MapSelectionType { Map, Dungeon };
public enum DirectionType { Mob, Object, ReturnPortal };
public enum PlayerType { TopDown, Side };
public enum PanelState { None, Showing, Shown, Hiding, Hidden };