using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038D RID: 909
	public class DamageWorker_AddInjury : DamageWorker
	{
		// Token: 0x060016C1 RID: 5825 RVA: 0x000D8DDC File Offset: 0x000D6FDC
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				return base.Apply(dinfo, thing);
			}
			return this.ApplyToPawn(dinfo, pawn);
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x000D8E04 File Offset: 0x000D7004
		private DamageWorker.DamageResult ApplyToPawn(DamageInfo dinfo, Pawn pawn)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (dinfo.Amount <= 0f)
			{
				return damageResult;
			}
			if (!DebugSettings.enablePlayerDamage && pawn.Faction == Faction.OfPlayer)
			{
				return damageResult;
			}
			Map mapHeld = pawn.MapHeld;
			bool spawnedOrAnyParentSpawned = pawn.SpawnedOrAnyParentSpawned;
			if (dinfo.AllowDamagePropagation && dinfo.Amount >= (float)dinfo.Def.minDamageToFragment)
			{
				int num = Rand.RangeInclusive(2, 4);
				for (int i = 0; i < num; i++)
				{
					DamageInfo dinfo2 = dinfo;
					dinfo2.SetAmount(dinfo.Amount / (float)num);
					this.ApplyDamageToPart(dinfo2, pawn, damageResult);
				}
			}
			else
			{
				this.ApplyDamageToPart(dinfo, pawn, damageResult);
				this.ApplySmallPawnDamagePropagation(dinfo, pawn, damageResult);
			}
			if (damageResult.wounded)
			{
				DamageWorker_AddInjury.PlayWoundedVoiceSound(dinfo, pawn);
				pawn.Drawer.Notify_DamageApplied(dinfo);
				EffecterDef damageEffecter = pawn.RaceProps.FleshType.damageEffecter;
				if (damageEffecter != null)
				{
					if (pawn.health.woundedEffecter != null && pawn.health.woundedEffecter.def != damageEffecter)
					{
						pawn.health.woundedEffecter.Cleanup();
					}
					pawn.health.woundedEffecter = damageEffecter.Spawn();
					pawn.health.woundedEffecter.Trigger(pawn, dinfo.Instigator ?? pawn);
				}
				if (dinfo.Def.damageEffecter != null)
				{
					Effecter effecter = dinfo.Def.damageEffecter.Spawn();
					effecter.Trigger(pawn, pawn);
					effecter.Cleanup();
				}
			}
			if (damageResult.headshot && pawn.Spawned)
			{
				MoteMaker.ThrowText(new Vector3((float)pawn.Position.x + 1f, (float)pawn.Position.y, (float)pawn.Position.z + 1f), pawn.Map, "Headshot".Translate(), Color.white, -1f);
				if (dinfo.Instigator != null)
				{
					Pawn pawn2 = dinfo.Instigator as Pawn;
					if (pawn2 != null)
					{
						pawn2.records.Increment(RecordDefOf.Headshots);
					}
				}
			}
			if ((damageResult.deflected || damageResult.diminished) && spawnedOrAnyParentSpawned)
			{
				EffecterDef effecterDef;
				if (damageResult.deflected)
				{
					if (damageResult.deflectedByMetalArmor && dinfo.Def.canUseDeflectMetalEffect)
					{
						if (dinfo.Def == DamageDefOf.Bullet)
						{
							effecterDef = EffecterDefOf.Deflect_Metal_Bullet;
						}
						else
						{
							effecterDef = EffecterDefOf.Deflect_Metal;
						}
					}
					else if (dinfo.Def == DamageDefOf.Bullet)
					{
						effecterDef = EffecterDefOf.Deflect_General_Bullet;
					}
					else
					{
						effecterDef = EffecterDefOf.Deflect_General;
					}
				}
				else if (damageResult.diminishedByMetalArmor)
				{
					effecterDef = EffecterDefOf.DamageDiminished_Metal;
				}
				else
				{
					effecterDef = EffecterDefOf.DamageDiminished_General;
				}
				if (pawn.health.deflectionEffecter == null || pawn.health.deflectionEffecter.def != effecterDef)
				{
					if (pawn.health.deflectionEffecter != null)
					{
						pawn.health.deflectionEffecter.Cleanup();
						pawn.health.deflectionEffecter = null;
					}
					pawn.health.deflectionEffecter = effecterDef.Spawn();
				}
				pawn.health.deflectionEffecter.Trigger(pawn, dinfo.Instigator ?? pawn);
				if (damageResult.deflected)
				{
					pawn.Drawer.Notify_DamageDeflected(dinfo);
				}
			}
			if (!damageResult.deflected && spawnedOrAnyParentSpawned)
			{
				ImpactSoundUtility.PlayImpactSound(pawn, dinfo.Def.impactSoundType, mapHeld);
			}
			return damageResult;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x000D916C File Offset: 0x000D736C
		[Obsolete]
		private void CheckApplySpreadDamage(DamageInfo dinfo, Thing t)
		{
			if (dinfo.Def == DamageDefOf.Flame && !t.FlammableNow)
			{
				return;
			}
			if (Rand.Chance(0.5f))
			{
				dinfo.SetAmount((float)Mathf.CeilToInt(dinfo.Amount * Rand.Range(0.35f, 0.7f)));
				t.TakeDamage(dinfo);
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x000D91C8 File Offset: 0x000D73C8
		private void ApplySmallPawnDamagePropagation(DamageInfo dinfo, Pawn pawn, DamageWorker.DamageResult result)
		{
			if (!dinfo.AllowDamagePropagation)
			{
				return;
			}
			if (result.LastHitPart != null && dinfo.Def.harmsHealth && result.LastHitPart != pawn.RaceProps.body.corePart && result.LastHitPart.parent != null && pawn.health.hediffSet.GetPartHealth(result.LastHitPart.parent) > 0f && result.LastHitPart.parent.coverageAbs > 0f && dinfo.Amount >= 10f && pawn.HealthScale <= 0.5001f)
			{
				DamageInfo dinfo2 = dinfo;
				dinfo2.SetHitPart(result.LastHitPart.parent);
				this.ApplyDamageToPart(dinfo2, pawn, result);
			}
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x000D9294 File Offset: 0x000D7494
		private void ApplyDamageToPart(DamageInfo dinfo, Pawn pawn, DamageWorker.DamageResult result)
		{
			BodyPartRecord exactPartFromDamageInfo = this.GetExactPartFromDamageInfo(dinfo, pawn);
			if (exactPartFromDamageInfo == null)
			{
				return;
			}
			dinfo.SetHitPart(exactPartFromDamageInfo);
			float num = dinfo.Amount;
			bool flag = !dinfo.InstantPermanentInjury && !dinfo.IgnoreArmor;
			bool deflectedByMetalArmor = false;
			if (flag)
			{
				DamageDef def = dinfo.Def;
				bool diminishedByMetalArmor;
				num = ArmorUtility.GetPostArmorDamage(pawn, num, dinfo.ArmorPenetrationInt, dinfo.HitPart, ref def, out deflectedByMetalArmor, out diminishedByMetalArmor);
				dinfo.Def = def;
				if (num < dinfo.Amount)
				{
					result.diminished = true;
					result.diminishedByMetalArmor = diminishedByMetalArmor;
				}
			}
			if (dinfo.Def.ExternalViolenceFor(pawn))
			{
				num *= pawn.GetStatValue(StatDefOf.IncomingDamageFactor, true);
			}
			if (num <= 0f)
			{
				result.AddPart(pawn, dinfo.HitPart);
				result.deflected = true;
				result.deflectedByMetalArmor = deflectedByMetalArmor;
				return;
			}
			if (DamageWorker_AddInjury.IsHeadshot(dinfo, pawn))
			{
				result.headshot = true;
			}
			if (dinfo.InstantPermanentInjury && (HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart).CompPropsFor(typeof(HediffComp_GetsPermanent)) == null || dinfo.HitPart.def.permanentInjuryChanceFactor == 0f || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(dinfo.HitPart)))
			{
				return;
			}
			if (!dinfo.AllowDamagePropagation)
			{
				this.FinalizeAndAddInjury(pawn, num, dinfo, result);
				return;
			}
			this.ApplySpecialEffectsToPart(pawn, num, dinfo, result);
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x000161E7 File Offset: 0x000143E7
		protected virtual void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage = this.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn);
			this.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
			this.CheckDuplicateDamageToOuterParts(dinfo, pawn, totalDamage, result);
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x000D93F0 File Offset: 0x000D75F0
		protected float FinalizeAndAddInjury(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			if (pawn.health.hediffSet.PartIsMissing(dinfo.HitPart))
			{
				return 0f;
			}
			HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart);
			Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
			hediff_Injury.Part = dinfo.HitPart;
			hediff_Injury.source = dinfo.Weapon;
			hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
			hediff_Injury.sourceHediffDef = dinfo.WeaponLinkedHediff;
			hediff_Injury.Severity = totalDamage;
			if (dinfo.InstantPermanentInjury)
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff_Injury.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null)
				{
					hediffComp_GetsPermanent.IsPermanent = true;
				}
				else
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to create instant permanent injury on Hediff without a GetsPermanent comp: ",
						hediffDefFromDamage,
						" on ",
						pawn
					}), false);
				}
			}
			return this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, result);
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x000D94CC File Offset: 0x000D76CC
		protected float FinalizeAndAddInjury(Pawn pawn, Hediff_Injury injury, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			HediffComp_GetsPermanent hediffComp_GetsPermanent = injury.TryGetComp<HediffComp_GetsPermanent>();
			if (hediffComp_GetsPermanent != null)
			{
				hediffComp_GetsPermanent.PreFinalizeInjury();
			}
			float partHealth = pawn.health.hediffSet.GetPartHealth(injury.Part);
			if (pawn.IsColonist && !dinfo.IgnoreInstantKillProtection && dinfo.Def.ExternalViolenceFor(pawn) && !Rand.Chance(Find.Storyteller.difficultyValues.allowInstantKillChance))
			{
				float num = (injury.def.lethalSeverity > 0f) ? (injury.def.lethalSeverity * 1.1f) : 1f;
				float min = 1f;
				float max = Mathf.Min(injury.Severity, partHealth);
				int num2 = 0;
				while (num2 < 7 && pawn.health.WouldDieAfterAddingHediff(injury))
				{
					float num3 = Mathf.Clamp(partHealth - num, min, max);
					if (DebugViewSettings.logCauseOfDeath)
					{
						Log.Message(string.Format("CauseOfDeath: attempt to prevent death for {0} on {1} attempt:{2} severity:{3}->{4} part health:{5}", new object[]
						{
							pawn.Name,
							injury.Part.Label,
							num2 + 1,
							injury.Severity,
							num3,
							partHealth
						}), false);
					}
					injury.Severity = num3;
					num *= 2f;
					min = 0f;
					num2++;
				}
			}
			pawn.health.AddHediff(injury, null, new DamageInfo?(dinfo), result);
			float num4 = Mathf.Min(injury.Severity, partHealth);
			result.totalDamageDealt += num4;
			result.wounded = true;
			result.AddPart(pawn, injury.Part);
			result.AddHediff(injury);
			return num4;
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x000D9680 File Offset: 0x000D7880
		private void CheckDuplicateDamageToOuterParts(DamageInfo dinfo, Pawn pawn, float totalDamage, DamageWorker.DamageResult result)
		{
			if (!dinfo.AllowDamagePropagation)
			{
				return;
			}
			if (dinfo.Def.harmAllLayersUntilOutside && dinfo.HitPart.depth == BodyPartDepth.Inside)
			{
				BodyPartRecord parent = dinfo.HitPart.parent;
				do
				{
					if (pawn.health.hediffSet.GetPartHealth(parent) != 0f && parent.coverageAbs > 0f)
					{
						Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, parent), pawn, null);
						hediff_Injury.Part = parent;
						hediff_Injury.source = dinfo.Weapon;
						hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
						hediff_Injury.Severity = totalDamage;
						if (hediff_Injury.Severity <= 0f)
						{
							hediff_Injury.Severity = 1f;
						}
						this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, result);
					}
					if (parent.depth == BodyPartDepth.Outside)
					{
						break;
					}
					parent = parent.parent;
				}
				while (parent != null);
			}
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x0001620B File Offset: 0x0001440B
		private static bool IsHeadshot(DamageInfo dinfo, Pawn pawn)
		{
			return !dinfo.InstantPermanentInjury && dinfo.HitPart.groups.Contains(BodyPartGroupDefOf.FullHead) && dinfo.Def == DamageDefOf.Bullet;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x000D976C File Offset: 0x000D796C
		private BodyPartRecord GetExactPartFromDamageInfo(DamageInfo dinfo, Pawn pawn)
		{
			if (dinfo.HitPart == null)
			{
				BodyPartRecord bodyPartRecord = this.ChooseHitPart(dinfo, pawn);
				if (bodyPartRecord == null)
				{
					Log.Warning("ChooseHitPart returned null (any part).", false);
				}
				return bodyPartRecord;
			}
			if (!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x == dinfo.HitPart))
			{
				return null;
			}
			return dinfo.HitPart;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00016240 File Offset: 0x00014440
		protected virtual BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, dinfo.Depth, null);
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x000D97E4 File Offset: 0x000D79E4
		private static void PlayWoundedVoiceSound(DamageInfo dinfo, Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			if (dinfo.InstantPermanentInjury)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (dinfo.Def.ExternalViolenceFor(pawn))
			{
				LifeStageUtility.PlayNearestLifestageSound(pawn, (LifeStageAge ls) => ls.soundWounded, 1f);
			}
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x000D9848 File Offset: 0x000D7A48
		protected float ReduceDamageToPreserveOutsideParts(float postArmorDamage, DamageInfo dinfo, Pawn pawn)
		{
			if (!DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(dinfo.HitPart))
			{
				return postArmorDamage;
			}
			float partHealth = pawn.health.hediffSet.GetPartHealth(dinfo.HitPart);
			if (postArmorDamage < partHealth)
			{
				return postArmorDamage;
			}
			float maxHealth = dinfo.HitPart.def.GetMaxHealth(pawn);
			float f = (postArmorDamage - partHealth) / maxHealth;
			if (Rand.Chance(this.def.overkillPctToDestroyPart.InverseLerpThroughRange(f)))
			{
				return postArmorDamage;
			}
			return postArmorDamage = partHealth - 1f;
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00016268 File Offset: 0x00014468
		public static bool ShouldReduceDamageToPreservePart(BodyPartRecord bodyPart)
		{
			return bodyPart.depth == BodyPartDepth.Outside && !bodyPart.IsCorePart;
		}
	}
}
