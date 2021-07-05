using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001515 RID: 5397
	public abstract class Verb_MeleeAttack : Verb
	{
		// Token: 0x0600807F RID: 32895 RVA: 0x002D84A0 File Offset: 0x002D66A0
		protected override bool TryCastShot()
		{
			Pawn casterPawn = this.CasterPawn;
			if (!casterPawn.Spawned)
			{
				return false;
			}
			if (casterPawn.stances.FullBodyBusy)
			{
				return false;
			}
			Thing thing = this.currentTarget.Thing;
			if (!this.CanHitTarget(thing))
			{
				Log.Warning(string.Concat(new object[]
				{
					casterPawn,
					" meleed ",
					thing,
					" from out of melee position."
				}));
			}
			casterPawn.rotationTracker.Face(thing.DrawPos);
			if (!this.IsTargetImmobile(this.currentTarget) && casterPawn.skills != null)
			{
				casterPawn.skills.Learn(SkillDefOf.Melee, 200f * this.verbProps.AdjustedFullCycleTime(this, casterPawn), false);
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && !pawn.Dead && (casterPawn.MentalStateDef != MentalStateDefOf.SocialFighting || pawn.MentalStateDef != MentalStateDefOf.SocialFighting) && (casterPawn.story == null || !casterPawn.story.traits.DisableHostilityFrom(pawn)))
			{
				pawn.mindState.meleeThreat = casterPawn;
				pawn.mindState.lastMeleeThreatHarmTick = Find.TickManager.TicksGame;
			}
			Map map = thing.Map;
			Vector3 drawPos = thing.DrawPos;
			SoundDef soundDef;
			bool result;
			if (Rand.Chance(this.GetNonMissChance(thing)))
			{
				if (!Rand.Chance(this.GetDodgeChance(thing)))
				{
					if (thing.def.category == ThingCategory.Building)
					{
						soundDef = this.SoundHitBuilding();
					}
					else
					{
						soundDef = this.SoundHitPawn();
					}
					if (this.verbProps.impactMote != null)
					{
						MoteMaker.MakeStaticMote(drawPos, map, this.verbProps.impactMote, 1f);
					}
					if (this.verbProps.impactFleck != null)
					{
						FleckMaker.Static(drawPos, map, this.verbProps.impactFleck, 1f);
					}
					BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesHit, true);
					result = true;
					DamageWorker.DamageResult damageResult = this.ApplyMeleeDamageToTarget(this.currentTarget);
					if (damageResult.stunned && damageResult.parts.NullOrEmpty<BodyPartRecord>())
					{
						Find.BattleLog.RemoveEntry(battleLogEntry_MeleeCombat);
					}
					else
					{
						damageResult.AssociateWithLog(battleLogEntry_MeleeCombat);
						if (damageResult.deflected)
						{
							battleLogEntry_MeleeCombat.RuleDef = this.maneuver.combatLogRulesDeflect;
							battleLogEntry_MeleeCombat.alwaysShowInCompact = false;
						}
					}
				}
				else
				{
					result = false;
					soundDef = this.SoundDodge(thing);
					MoteMaker.ThrowText(drawPos, map, "TextMote_Dodge".Translate(), 1.9f);
					this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesDodge, false);
				}
			}
			else
			{
				result = false;
				soundDef = this.SoundMiss();
				this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesMiss, false);
			}
			soundDef.PlayOneShot(new TargetInfo(thing.Position, map, false));
			if (casterPawn.Spawned)
			{
				casterPawn.Drawer.Notify_MeleeAttackOn(thing);
			}
			if (pawn != null && !pawn.Dead && pawn.Spawned)
			{
				pawn.stances.StaggerFor(95);
			}
			if (casterPawn.Spawned)
			{
				casterPawn.rotationTracker.FaceCell(thing.Position);
			}
			if (casterPawn.caller != null)
			{
				casterPawn.caller.Notify_DidMeleeAttack();
			}
			return result;
		}

		// Token: 0x06008080 RID: 32896 RVA: 0x002D87FC File Offset: 0x002D69FC
		public BattleLogEntry_MeleeCombat CreateCombatLog(Func<ManeuverDef, RulePackDef> rulePackGetter, bool alwaysShow)
		{
			if (this.maneuver == null)
			{
				return null;
			}
			if (this.tool == null)
			{
				return null;
			}
			BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackGetter(this.maneuver), alwaysShow, this.CasterPawn, this.currentTarget.Thing, base.ImplementOwnerType, this.tool.labelUsedInLogging ? this.tool.label : "", (base.EquipmentSource == null) ? null : base.EquipmentSource.def, (base.HediffCompSource == null) ? null : base.HediffCompSource.Def, this.maneuver.logEntryDef);
			Find.BattleLog.Add(battleLogEntry_MeleeCombat);
			return battleLogEntry_MeleeCombat;
		}

		// Token: 0x06008081 RID: 32897 RVA: 0x002D88AC File Offset: 0x002D6AAC
		private float GetNonMissChance(LocalTargetInfo target)
		{
			if (this.surpriseAttack)
			{
				return 1f;
			}
			if (this.IsTargetImmobile(target))
			{
				return 1f;
			}
			float num = this.CasterPawn.GetStatValue(StatDefOf.MeleeHitChance, true);
			if (ModsConfig.IdeologyActive && target.HasThing)
			{
				if (DarknessCombatUtility.IsOutdoorsAndLit(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeHitChanceOutdoorsLitOffset, true);
				}
				else if (DarknessCombatUtility.IsOutdoorsAndDark(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeHitChanceOutdoorsDarkOffset, true);
				}
				else if (DarknessCombatUtility.IsIndoorsAndDark(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeHitChanceIndoorsDarkOffset, true);
				}
				else if (DarknessCombatUtility.IsIndoorsAndLit(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeHitChanceIndoorsLitOffset, true);
				}
			}
			return num;
		}

		// Token: 0x06008082 RID: 32898 RVA: 0x002D8990 File Offset: 0x002D6B90
		private float GetDodgeChance(LocalTargetInfo target)
		{
			if (this.surpriseAttack)
			{
				return 0f;
			}
			if (this.IsTargetImmobile(target))
			{
				return 0f;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
			if (stance_Busy != null && stance_Busy.verb != null && !stance_Busy.verb.verbProps.IsMeleeAttack)
			{
				return 0f;
			}
			float num = pawn.GetStatValue(StatDefOf.MeleeDodgeChance, true);
			if (ModsConfig.IdeologyActive && target.HasThing)
			{
				if (DarknessCombatUtility.IsOutdoorsAndLit(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeDodgeChanceOutdoorsLitOffset, true);
				}
				else if (DarknessCombatUtility.IsOutdoorsAndDark(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeDodgeChanceOutdoorsDarkOffset, true);
				}
				else if (DarknessCombatUtility.IsIndoorsAndDark(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeDodgeChanceIndoorsDarkOffset, true);
				}
				else if (DarknessCombatUtility.IsIndoorsAndLit(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeDodgeChanceIndoorsLitOffset, true);
				}
			}
			return num;
		}

		// Token: 0x06008083 RID: 32899 RVA: 0x002D8AB8 File Offset: 0x002D6CB8
		private bool IsTargetImmobile(LocalTargetInfo target)
		{
			Thing thing = target.Thing;
			Pawn pawn = thing as Pawn;
			return thing.def.category != ThingCategory.Pawn || pawn.Downed || pawn.GetPosture() > PawnPosture.Standing;
		}

		// Token: 0x06008084 RID: 32900
		protected abstract DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target);

		// Token: 0x06008085 RID: 32901 RVA: 0x002D8AF4 File Offset: 0x002D6CF4
		private SoundDef SoundHitPawn()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.def.meleeHitSound.NullOrUndefined())
			{
				return base.EquipmentSource.def.meleeHitSound;
			}
			if (this.tool != null && !this.tool.soundMeleeHit.NullOrUndefined())
			{
				return this.tool.soundMeleeHit;
			}
			if (base.EquipmentSource != null && base.EquipmentSource.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (this.CasterPawn != null && !this.CasterPawn.def.race.soundMeleeHitPawn.NullOrUndefined())
			{
				return this.CasterPawn.def.race.soundMeleeHitPawn;
			}
			return SoundDefOf.Pawn_Melee_Punch_HitPawn;
		}

		// Token: 0x06008086 RID: 32902 RVA: 0x002D8C2C File Offset: 0x002D6E2C
		private SoundDef SoundHitBuilding()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.def.meleeHitSound.NullOrUndefined())
			{
				return base.EquipmentSource.def.meleeHitSound;
			}
			if (this.tool != null && !this.tool.soundMeleeHit.NullOrUndefined())
			{
				return this.tool.soundMeleeHit;
			}
			if (base.EquipmentSource != null && base.EquipmentSource.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (this.CasterPawn != null && !this.CasterPawn.def.race.soundMeleeHitBuilding.NullOrUndefined())
			{
				return this.CasterPawn.def.race.soundMeleeHitBuilding;
			}
			return SoundDefOf.Pawn_Melee_Punch_HitBuilding;
		}

		// Token: 0x06008087 RID: 32903 RVA: 0x002D8D64 File Offset: 0x002D6F64
		private SoundDef SoundMiss()
		{
			if (this.CasterPawn != null)
			{
				if (this.tool != null && !this.tool.soundMeleeMiss.NullOrUndefined())
				{
					return this.tool.soundMeleeMiss;
				}
				if (!this.CasterPawn.def.race.soundMeleeMiss.NullOrUndefined())
				{
					return this.CasterPawn.def.race.soundMeleeMiss;
				}
			}
			return SoundDefOf.Pawn_Melee_Punch_Miss;
		}

		// Token: 0x06008088 RID: 32904 RVA: 0x002D8DD6 File Offset: 0x002D6FD6
		private SoundDef SoundDodge(Thing target)
		{
			if (target.def.race != null && target.def.race.soundMeleeDodge != null)
			{
				return target.def.race.soundMeleeDodge;
			}
			return this.SoundMiss();
		}

		// Token: 0x04004FFE RID: 20478
		private const int TargetCooldown = 50;
	}
}
