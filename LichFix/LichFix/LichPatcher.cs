using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Utility;
using Kingmaker.RuleSystem;
using System.Linq;

namespace LichFix
{
    class LichPatcher
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsPatcher
        {
            static bool loaded = false;
            static void Postfix()
            {
                if (loaded) return;
                loaded = true;

                patchLordBeyondTheGrave();
                patchCorruptedBlood();
                patchEyesOfTheBodak();
                patchInsightfulContemplation();

            }
        }

        static void patchLordBeyondTheGrave()
        {
            //1. patch lord beyond the grave
            if (Main.Settings.allowLichAuraAffectLivingWithBlessingOfUnlife)
            {
                ResourcesFinder.blessingOfUnlifeBuff.AddComponents(new BlueprintComponent[] { Helpers.CreateAddFact(ResourcesFinder.undeadTypeFact) });
                Main.Log("Patched: Blessing of Unlife Buff >> add undead Fact >> Lord Beyond The Grave can affect allies with blessing of unlife buff now");

                ResourcesFinder.lichBolsterUndeadAura.GetComponent<AbilityAreaEffectBuff>().CheckConditionEveryRound = true;
                Main.Log("Patched: Lord Beyond The Grave will check condition every round to apply the buff now");
            }
        }

        static void patchCorruptedBlood()
        {
            //2. patch corrupted blood + metamagic selective affect
            ResourcesFinder.corruptedBloodAbility.AvailableMetamagic = Metamagic.Selective | Metamagic.Quicken | Metamagic.Heighten | Metamagic.CompletelyNormal | Metamagic.Extend | Metamagic.Extend;
            var actionOnRandomTargetsAround = ResourcesFinder.corruptedBloodBuff.GetComponent<AddIncomingDamageTrigger>().Actions.Actions.OfType<ContextActionOnRandomTargetsAround>().First<ContextActionOnRandomTargetsAround>();
            var actionOnRandomTargetsAroundSelective = Helpers.CreateContextActionOnRandomTargetsAroundSelective(actionOnRandomTargetsAround);

            //corrupted blood explosion range
            actionOnRandomTargetsAroundSelective.Radius = new Feet(Main.Settings.corruptedBloodRange);

            Main.Log("Patched: corrupted blood buff range from 15 >> " + Main.Settings.corruptedBloodRange);

            ResourcesFinder.corruptedBloodBuff.GetComponent<AddIncomingDamageTrigger>().Actions.Actions[0] = actionOnRandomTargetsAroundSelective;
            Main.Log("Patched: corrupted blood buff will now affect by selective metamagic");

            actionOnRandomTargetsAroundSelective.Actions.Actions.OfType<ContextActionDealDamage>().First<ContextActionDealDamage>().Value.DiceType = DiceType.D2;
            actionOnRandomTargetsAroundSelective.Actions.Actions.OfType<ContextActionDealDamage>().First<ContextActionDealDamage>().Value.DiceCountValue.ValueType = ContextValueType.Rank;
            Main.Log("Patched: corrupted blood now deal 1d2 damage per caster level");

            //add save when cast corrupted blood on target
            var originalActionApplyBuffOnCast = ResourcesFinder.corruptedBloodAbility.GetComponent<AbilityEffectRunAction>().Actions.Actions.OfType<ContextActionApplyBuff>().First<ContextActionApplyBuff>();
            var actionSavingOnCast = Helpers.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(new GameAction[]{
                    Helpers.CreateContextSavedApplyBuff(ResourcesFinder.corruptedBloodBuff, originalActionApplyBuffOnCast.DurationValue, true, false, false, true)
                }));

            ResourcesFinder.corruptedBloodAbility.GetComponent<AbilityEffectRunAction>().Actions.Actions[0] = actionSavingOnCast;

            Main.Log("Patched: corrupted blood now have fortitude save");

            //when target is dead, apply corrupted blood with fortitude save
            var originalActionApplyBuff = actionOnRandomTargetsAroundSelective.Actions.Actions.OfType<ContextActionApplyBuff>().First<ContextActionApplyBuff>();

            var actionSavingOnDead = Helpers.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(new GameAction[]{
                    Helpers.CreateContextSavedApplyBuff(ResourcesFinder.corruptedBloodBuff, originalActionApplyBuff.DurationValue, true, false, false, true)
                }));

            actionOnRandomTargetsAroundSelective.Actions.Actions[0] = actionSavingOnDead;

            Main.Log("Patched: corrupted blood now apply to surround unit with fortitude save when the target is dead");

            ResourcesFinder.corruptedBloodAbility.SetDescription("You corrupt an enemy's blood, making it contagious. If the target failed a fortitude save, it is nauseated for 1 {g|Encyclopedia:Combat_Round}round{/g} per {g|Encyclopedia:Caster_Level}caster level{/g}. " +
            "Every round it can make a new {g|Encyclopedia:Saving_Throw}saving throw{/g} to remove the nauseated condition. If the saving throw is successful, the target instead becomes sickened. If the target dies while nauseated condition, " +
            "it deals {g|Encyclopedia:Dice}1d2{/g} {g|Encyclopedia:Damage}damage{/g} per caster level to all creatures in " + Main.Settings.corruptedBloodRange + " feet radius and applies Corrupted Blood to them if they failed a fortitude save.");

            ResourcesFinder.corruptedBloodBuff.SetDescription("You are nauseated. Additionally, if you die, you deal {g|Encyclopedia:Dice}1d2{/g} {g|Encyclopedia:Damage}damage{/g} per caster level to all creatures in " + Main.Settings.corruptedBloodRange + " feet radius and apply Corrupted Blood to them if they failed a fortitude save.");

        }

        static void patchEyesOfTheBodak()
        {
            //3. patch eyes of the bodak fix fx and floating numbers on negative level
            ResourcesFinder.eyeOfTheBodakBuff.GetComponent<AddAreaEffect>().m_AreaEffect = ResourcesFinder.deathGazeAura.ToReference<BlueprintAbilityAreaEffectReference>();
        }

        static void patchInsightfulContemplation()
        {
            //4. patch court poet singing fx
            ResourcesFinder.insightfulContemplationAura.Fx = ResourcesFinder.inspireGreatnessAura.Fx;
        }
    }
}
