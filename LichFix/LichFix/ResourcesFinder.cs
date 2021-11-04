using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace LichFix
{
    class ResourcesFinder
    {
        public static BlueprintBuff blessingOfUnlifeBuff
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("e4e9f9169c9b28e40aa2c9d10c369254");
            }
        }

        public static BlueprintUnitFact undeadTypeFact
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintUnitFact>("734a29b693e9ec346ba2951b27987e33");
            }
        }

        public static BlueprintBuff corruptedBloodBuff
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("1419d2e2eee432849b0a596e82b9e0a2");
            }
        }

        public static BlueprintAbility corruptedBloodAbility
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbility>("bbbcaa880ac0fa0479ce3ee8ac937d50");
            }
        }

        public static BlueprintAbilityAreaEffect lichBolsterUndeadAura
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbilityAreaEffect>("47d52975b5b1b8c4699fdd43c6d797f0");
            }
        }

        public static BlueprintBuff eyeOfTheBodakBuff
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintBuff>("d404c44b919667347877e7580e1e7498");
            }
        }

        public static BlueprintAbilityAreaEffect deathGazeAura
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbilityAreaEffect>("d6c0ab2f2828dc0479867fe173984016");
            }
        }

        public static BlueprintAbilityAreaEffect insightfulContemplationAura
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbilityAreaEffect>("cad5dd5157db3304c80399472bb48bdf");
            }
        }

        public static BlueprintAbilityAreaEffect inspireGreatnessAura
        {
            get
            {
                return ResourcesLibrary.TryGetBlueprint<BlueprintAbilityAreaEffect>("23ddd38738bd1d84595f3cdbb8512873");
            }
        }
    }
}
