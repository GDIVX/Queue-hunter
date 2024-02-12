
namespace Tzipory.Helpers.Consts
{
    public static class Constant
    {
        public const float ISOMETRIC_SCALE = 1.455f;
        
        public static class EffectSequenceIds
        {
            public const int DEATH = 1;
            public const int ATTACK = 2;
            public const int CRIT_ATTACK = 3;
            public const int MOVE = 4;
            public const int GET_HIT = 5;
            public const int GET_CRIT_HIT = 6;
        }
        
        public static class DataId
        {
            public const int SHAMAN_DATA_ID = 0;
            public const int ENEMY_DATA_ID = 1;
            public const int STAT_DATA_ID = 2;
            public const int ABILITY_DATA_ID = 3;
            public const int STATUS_EFFECT_DATA_ID = 4;
            public const int PARTY_DATA_ID = 5;
            public const int PLAYER_DATA_ID = 6;
            public const int MAP_DATA_ID = 7;
            public const int NODE_DATA_ID = 8;
            public const int ITEM_DATA_ID = 9;
            public const int CAMP_DATA_ID = 10;
            public const int CAMP_BUILDING_DATA_ID = 11;
            public const int CAMP_FACILITY_DATA_ID = 12;
            public const int INVENTORY_DATA_ID = 13;
        }

        public enum Materials
        {
            BronzeIngot = 1,
            Bones = 2,
            Honey = 3,
            Beewax = 4,
            Fur = 5,
            Linen = 6,
            Amber = 100,
            Amethyst = 101,
            Copper = 102,
            Emerald = 103,
            Garnet = 104,
            Gold = 105,
            LapisLazuli = 107,
            Obsidian = 108,
            Tin = 109,
            LeatherScraps = 201,
            Hide = 202,
            CedarWood = 300,
            Clay = 400,
            Limestone=401,
            Myrrh = 501,
            Frankincense = 502,
            Garlic = 503,
            Jute = 504,
            Flax = 505,
            OliveOil = 600
        }


        public static class ShamanId
        {
            public const int  TOOR_ID = 0;
            public const int  NADIA_ID = 1;
            public const int  JAVAN_ID = 2;
            public const int  GUNI_ID = 3;
            public const int  NANA_ID = 4;
        }
        
        public static class EnemyId
        {
            public const int  MUDU_ID = 0;
            public const int  GALLANITE_ID = 1;
        }
        
        public static class CampBuildingFacilityId
        {
            public const int WORKSHOP_ITEMS_FACILITY = 0;
        }

        public enum StatsId
        {
            Health,
            AttackDamage,
            AttackRate,
            AttackRange,
            TargetingRange,
            MovementSpeed,
            CritDamage,
            CritChance,
            InvincibleTime,
            AbilityCooldown,
            AbilityCastTime,
            ProjectileSpeed,
            ProjectilePenetration,
            AoeRadius,
            AoeDuration,
            ChainRadius,
            ChainDuration,
            ChainAmount,
            Armor,
            Regeneration,
            CoreAttackDamage,
            Modifier,
            Duration,
            Interval
        }
        
        public enum StatHolderType
        {
            Entity,
            Ability,
        }
        
        public enum AbilityType
        {
            AoeAbility,
            ProjectileAbility,
            ChainAbility,
        }
    }

    
}