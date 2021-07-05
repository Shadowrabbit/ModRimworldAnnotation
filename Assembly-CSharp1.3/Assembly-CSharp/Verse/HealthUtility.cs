using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000279 RID: 633
	public static class HealthUtility
	{
		// Token: 0x060011AB RID: 4523 RVA: 0x000667A0 File Offset: 0x000649A0
		public static string GetGeneralConditionLabel(Pawn pawn, bool shortVersion = false)
		{
			if (pawn.health.Dead)
			{
				return "Dead".Translate();
			}
			if (!pawn.health.capacities.CanBeAwake)
			{
				return "Unconscious".Translate();
			}
			if (pawn.health.InPainShock)
			{
				return (shortVersion && "PainShockShort".CanTranslate()) ? "PainShockShort".Translate() : "PainShock".Translate();
			}
			if (pawn.Downed)
			{
				return "Incapacitated".Translate();
			}
			bool flag = false;
			for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = pawn.health.hediffSet.hediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					flag = true;
				}
			}
			if (flag)
			{
				return "Injured".Translate();
			}
			if (pawn.health.hediffSet.PainTotal > 0.3f)
			{
				return "InPain".Translate();
			}
			return "Healthy".Translate();
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x000668D0 File Offset: 0x00064AD0
		public static Pair<string, Color> GetPartConditionLabel(Pawn pawn, BodyPartRecord part)
		{
			float partHealth = pawn.health.hediffSet.GetPartHealth(part);
			float maxHealth = part.def.GetMaxHealth(pawn);
			float num = partHealth / maxHealth;
			Color second = Color.white;
			string first;
			if (partHealth <= 0f)
			{
				Hediff_MissingPart hediff_MissingPart = null;
				List<Hediff_MissingPart> missingPartsCommonAncestors = pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int i = 0; i < missingPartsCommonAncestors.Count; i++)
				{
					if (missingPartsCommonAncestors[i].Part == part)
					{
						hediff_MissingPart = missingPartsCommonAncestors[i];
						break;
					}
				}
				if (hediff_MissingPart == null)
				{
					bool fresh = false;
					if (hediff_MissingPart != null && hediff_MissingPart.IsFreshNonSolidExtremity)
					{
						fresh = true;
					}
					bool solid = part.def.IsSolid(part, pawn.health.hediffSet.hediffs);
					first = HealthUtility.GetGeneralDestroyedPartLabel(part, fresh, solid);
					second = Color.gray;
				}
				else
				{
					first = hediff_MissingPart.LabelCap;
					second = hediff_MissingPart.LabelColor;
				}
			}
			else if (num < 0.4f)
			{
				first = "SeriouslyImpaired".Translate();
				second = HealthUtility.RedColor;
			}
			else if (num < 0.7f)
			{
				first = "Impaired".Translate();
				second = HealthUtility.ImpairedColor;
			}
			else if (num < 0.999f)
			{
				first = "SlightlyImpaired".Translate();
				second = HealthUtility.SlightlyImpairedColor;
			}
			else
			{
				first = "GoodCondition".Translate();
				second = HealthUtility.GoodConditionColor;
			}
			return new Pair<string, Color>(first, second);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00066A38 File Offset: 0x00064C38
		public static string GetGeneralDestroyedPartLabel(BodyPartRecord part, bool fresh, bool solid)
		{
			if (part.parent == null)
			{
				return "SeriouslyImpaired".Translate();
			}
			if (part.depth != BodyPartDepth.Inside && !fresh)
			{
				return "MissingBodyPart".Translate();
			}
			if (solid)
			{
				return "ShatteredBodyPart".Translate();
			}
			return "DestroyedBodyPart".Translate();
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00066A9C File Offset: 0x00064C9C
		private static IEnumerable<BodyPartRecord> HittablePartsViolence(HediffSet bodyModel)
		{
			return from x in bodyModel.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside || (x.depth == BodyPartDepth.Inside && x.def.IsSolid(x, bodyModel.hediffs))
			select x;
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00066AD6 File Offset: 0x00064CD6
		public static void GiveInjuriesOperationFailureMinor(Pawn p, BodyPartRecord part)
		{
			HealthUtility.GiveRandomSurgeryInjuries(p, 20, part);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00066AE1 File Offset: 0x00064CE1
		public static void GiveInjuriesOperationFailureCatastrophic(Pawn p, BodyPartRecord part)
		{
			HealthUtility.GiveRandomSurgeryInjuries(p, 65, part);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00066AEC File Offset: 0x00064CEC
		public static void GiveInjuriesOperationFailureRidiculous(Pawn p)
		{
			HealthUtility.GiveRandomSurgeryInjuries(p, 65, null);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00066AF8 File Offset: 0x00064CF8
		public static void HealNonPermanentInjuriesAndRestoreLegs(Pawn p)
		{
			if (p.Dead)
			{
				return;
			}
			HealthUtility.tmpHediffs.Clear();
			HealthUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			for (int i = 0; i < HealthUtility.tmpHediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = HealthUtility.tmpHediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					p.health.RemoveHediff(hediff_Injury);
				}
				else
				{
					Hediff_MissingPart hediff_MissingPart = HealthUtility.tmpHediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) && (hediff_MissingPart.Part.parent == null || p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff_MissingPart.Part.parent)))
					{
						p.health.RestorePart(hediff_MissingPart.Part, null, true);
					}
				}
			}
			HealthUtility.tmpHediffs.Clear();
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00066BF8 File Offset: 0x00064DF8
		public static void HealNonPermanentInjuriesAndFreshWounds(Pawn p)
		{
			if (p.Dead)
			{
				return;
			}
			HealthUtility.tmpHediffs.Clear();
			HealthUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			foreach (Hediff hediff in HealthUtility.tmpHediffs)
			{
				if (hediff.def.everCurableByItem)
				{
					Hediff_Injury hediff_Injury;
					Hediff_MissingPart hediff_MissingPart;
					if ((hediff_Injury = (hediff as Hediff_Injury)) != null && !hediff_Injury.IsPermanent())
					{
						p.health.RemoveHediff(hediff_Injury);
					}
					else if ((hediff_MissingPart = (hediff as Hediff_MissingPart)) != null && hediff_MissingPart.IsFresh)
					{
						hediff_MissingPart.IsFresh = false;
						p.health.Notify_HediffChanged(hediff_MissingPart);
					}
				}
			}
			HealthUtility.tmpHediffs.Clear();
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00066CCC File Offset: 0x00064ECC
		public static Hediff_Injury HealRandomPermanentInjury(Pawn p)
		{
			if (p.Dead)
			{
				return null;
			}
			HealthUtility.tmpHediffs.Clear();
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				Hediff_Injury hd;
				if (hediff.def.everCurableByItem && (hd = (hediff as Hediff_Injury)) != null && hd.IsPermanent())
				{
					HealthUtility.tmpHediffs.Add(hediff);
				}
			}
			Hediff_Injury hediff_Injury = HealthUtility.tmpHediffs.RandomElementWithFallback(null) as Hediff_Injury;
			if (hediff_Injury != null)
			{
				p.health.RemoveHediff(hediff_Injury);
			}
			HealthUtility.tmpHediffs.Clear();
			return hediff_Injury;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00066D8C File Offset: 0x00064F8C
		private static void GiveRandomSurgeryInjuries(Pawn p, int totalDamage, BodyPartRecord operatedPart)
		{
			IEnumerable<BodyPartRecord> source;
			if (operatedPart == null)
			{
				source = from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where !x.def.conceptual
				select x;
			}
			else
			{
				source = from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where !x.def.conceptual
				select x into pa
				where pa == operatedPart || pa.parent == operatedPart || (operatedPart != null && operatedPart.parent == pa)
				select pa;
			}
			source = from x in source
			where HealthUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(x, p) >= 2f
			select x;
			BodyPartRecord brain = p.health.hediffSet.GetBrain();
			if (brain != null)
			{
				float maxBrainHealth = brain.def.GetMaxHealth(p);
				source = from x in source
				where x != brain || p.health.hediffSet.GetPartHealth(x) >= maxBrainHealth * 0.5f + 1f
				select x;
			}
			while (totalDamage > 0 && source.Any<BodyPartRecord>())
			{
				BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int num = Mathf.Max(3, GenMath.RoundRandom(partHealth * Rand.Range(0.5f, 1f)));
				float minHealthOfPartsWeWantToAvoidDestroying = HealthUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(bodyPartRecord, p);
				if (minHealthOfPartsWeWantToAvoidDestroying - (float)num < 1f)
				{
					num = Mathf.RoundToInt(minHealthOfPartsWeWantToAvoidDestroying - 1f);
				}
				if (bodyPartRecord == brain && partHealth - (float)num < brain.def.GetMaxHealth(p) * 0.5f)
				{
					num = Mathf.Max(Mathf.RoundToInt(partHealth - brain.def.GetMaxHealth(p) * 0.5f), 1);
				}
				if (num <= 0)
				{
					break;
				}
				DamageDef def = Rand.Element<DamageDef>(DamageDefOf.Cut, DamageDefOf.Scratch, DamageDefOf.Stab, DamageDefOf.Crush);
				DamageInfo dinfo = new DamageInfo(def, (float)num, 0f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetIgnoreArmor(true);
				p.TakeDamage(dinfo);
				totalDamage -= num;
			}
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00067010 File Offset: 0x00065210
		private static float GetMinHealthOfPartsWeWantToAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			float num = 999999f;
			while (part != null)
			{
				if (HealthUtility.ShouldRandomSurgeryInjuriesAvoidDestroying(part, pawn))
				{
					num = Mathf.Min(num, pawn.health.hediffSet.GetPartHealth(part));
				}
				part = part.parent;
			}
			return num;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00067054 File Offset: 0x00065254
		private static bool ShouldRandomSurgeryInjuriesAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			if (part == pawn.RaceProps.body.corePart)
			{
				return true;
			}
			if (part.def.tags.Any((BodyPartTagDef x) => x.vital))
			{
				return true;
			}
			for (int i = 0; i < part.parts.Count; i++)
			{
				if (HealthUtility.ShouldRandomSurgeryInjuriesAvoidDestroying(part.parts[i], pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x000670D8 File Offset: 0x000652D8
		public static void DamageUntilDowned(Pawn p, bool allowBleedingWounds = true)
		{
			if (p.health.Downed)
			{
				return;
			}
			HediffSet hediffSet = p.health.hediffSet;
			p.health.forceIncap = true;
			IEnumerable<BodyPartRecord> source = from x in HealthUtility.HittablePartsViolence(hediffSet)
			where !p.health.hediffSet.hediffs.Any((Hediff y) => y.Part == x && y.CurStage != null && y.CurStage.partEfficiencyOffset < 0f)
			select x;
			int num = 0;
			while (num < 300 && !p.Downed && source.Any<BodyPartRecord>())
			{
				num++;
				BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				int num2 = Mathf.RoundToInt(hediffSet.GetPartHealth(bodyPartRecord)) - 3;
				if (num2 >= 8)
				{
					DamageDef damageDef;
					if (bodyPartRecord.depth == BodyPartDepth.Outside)
					{
						if (!allowBleedingWounds && bodyPartRecord.def.bleedRate > 0f)
						{
							damageDef = DamageDefOf.Blunt;
						}
						else
						{
							damageDef = HealthUtility.RandomViolenceDamageType();
						}
					}
					else
					{
						damageDef = DamageDefOf.Blunt;
					}
					int num3 = Rand.RangeInclusive(Mathf.RoundToInt((float)num2 * 0.65f), num2);
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
					if (!p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num3))
					{
						DamageInfo dinfo = new DamageInfo(damageDef, (float)num3, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
						dinfo.SetAllowDamagePropagation(false);
						p.TakeDamage(dinfo);
					}
				}
			}
			if (p.Dead)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(p + " died during GiveInjuriesToForceDowned");
				for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
				{
					stringBuilder.AppendLine("   -" + p.health.hediffSet.hediffs[i].ToString());
				}
				Log.Error(stringBuilder.ToString());
			}
			p.health.forceIncap = false;
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00067300 File Offset: 0x00065500
		public static void DamageUntilDead(Pawn p)
		{
			HediffSet hediffSet = p.health.hediffSet;
			int num = 0;
			while (!p.Dead && num < 200 && HealthUtility.HittablePartsViolence(hediffSet).Any<BodyPartRecord>())
			{
				num++;
				BodyPartRecord bodyPartRecord = HealthUtility.HittablePartsViolence(hediffSet).RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
				int num2 = Rand.RangeInclusive(8, 25);
				DamageDef def;
				if (bodyPartRecord.depth == BodyPartDepth.Outside)
				{
					def = HealthUtility.RandomViolenceDamageType();
				}
				else
				{
					def = DamageDefOf.Blunt;
				}
				DamageInfo dinfo = new DamageInfo(def, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetIgnoreInstantKillProtection(true);
				p.TakeDamage(dinfo);
			}
			if (!p.Dead)
			{
				Log.Error(p + " not killed during GiveInjuriesToKill");
			}
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x000673D4 File Offset: 0x000655D4
		public static void DamageLegsUntilIncapableOfMoving(Pawn p, bool allowBleedingWounds = true)
		{
			int num = 0;
			p.health.forceIncap = true;
			Func<BodyPartRecord, bool> <>9__0;
			while (p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && num < 300)
			{
				num++;
				IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) && p.health.hediffSet.GetPartHealth(x) >= 2f));
				}
				IEnumerable<BodyPartRecord> source = notMissingParts.Where(predicate);
				if (!source.Any<BodyPartRecord>())
				{
					break;
				}
				BodyPartRecord bodyPartRecord = source.RandomElement<BodyPartRecord>();
				float maxHealth = bodyPartRecord.def.GetMaxHealth(p);
				float partHealth = p.health.hediffSet.GetPartHealth(bodyPartRecord);
				int min = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.12f), 1, (int)partHealth - 1);
				int max = Mathf.Clamp(Mathf.RoundToInt(maxHealth * 0.27f), 1, (int)partHealth - 1);
				int num2 = Rand.RangeInclusive(min, max);
				DamageDef damageDef;
				if (!allowBleedingWounds && bodyPartRecord.def.bleedRate > 0f)
				{
					damageDef = DamageDefOf.Blunt;
				}
				else
				{
					damageDef = HealthUtility.RandomViolenceDamageType();
				}
				HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
				if (p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num2))
				{
					break;
				}
				DamageInfo dinfo = new DamageInfo(damageDef, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
				dinfo.SetAllowDamagePropagation(false);
				p.TakeDamage(dinfo);
			}
			p.health.forceIncap = false;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0006757C File Offset: 0x0006577C
		public static DamageDef RandomViolenceDamageType()
		{
			switch (Rand.RangeInclusive(0, 4))
			{
			case 0:
				return DamageDefOf.Bullet;
			case 1:
				return DamageDefOf.Blunt;
			case 2:
				return DamageDefOf.Stab;
			case 3:
				return DamageDefOf.Scratch;
			case 4:
				return DamageDefOf.Cut;
			default:
				return null;
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x000675CC File Offset: 0x000657CC
		public static DamageDef RandomPermanentInjuryDamageType(bool allowFrostbite)
		{
			switch (Rand.RangeInclusive(0, 3 + (allowFrostbite ? 1 : 0)))
			{
			case 0:
				return DamageDefOf.Bullet;
			case 1:
				return DamageDefOf.Scratch;
			case 2:
				return DamageDefOf.Bite;
			case 3:
				return DamageDefOf.Stab;
			case 4:
				return DamageDefOf.Frostbite;
			default:
				throw new Exception();
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00067628 File Offset: 0x00065828
		public static HediffDef GetHediffDefFromDamage(DamageDef dam, Pawn pawn, BodyPartRecord part)
		{
			HediffDef result = dam.hediff;
			if (part.def.IsSkinCovered(part, pawn.health.hediffSet) && dam.hediffSkin != null)
			{
				result = dam.hediffSkin;
			}
			if (part.def.IsSolid(part, pawn.health.hediffSet.hediffs) && dam.hediffSolid != null)
			{
				result = dam.hediffSolid;
			}
			return result;
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00067694 File Offset: 0x00065894
		public static bool TryAnesthetize(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return false;
			}
			pawn.health.forceIncap = true;
			pawn.health.AddHediff(HediffDefOf.Anesthetic, null, null, null);
			pawn.health.forceIncap = false;
			return true;
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x000676E8 File Offset: 0x000658E8
		public static void AdjustSeverity(Pawn pawn, HediffDef hdDef, float sevOffset)
		{
			if (sevOffset == 0f)
			{
				return;
			}
			Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hdDef, false);
			if (hediff != null)
			{
				hediff.Severity += sevOffset;
				return;
			}
			if (sevOffset > 0f)
			{
				hediff = HediffMaker.MakeHediff(hdDef, pawn, null);
				hediff.Severity = sevOffset;
				pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00067754 File Offset: 0x00065954
		public static BodyPartRemovalIntent PartRemovalIntent(Pawn pawn, BodyPartRecord part)
		{
			if (pawn.health.hediffSet.hediffs.Any((Hediff d) => d.Visible && d.Part == part && d.def.isBad))
			{
				return BodyPartRemovalIntent.Amputate;
			}
			return BodyPartRemovalIntent.Harvest;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00067794 File Offset: 0x00065994
		public static int TicksUntilDeathDueToBloodLoss(Pawn pawn)
		{
			float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
			if (bleedRateTotal < 0.0001f)
			{
				return int.MaxValue;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			float num = (firstHediffOfDef != null) ? firstHediffOfDef.Severity : 0f;
			return (int)((1f - num) / bleedRateTotal * 60000f);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x000677F8 File Offset: 0x000659F8
		public static TaggedString FixWorstHealthCondition(Pawn pawn)
		{
			Hediff hediff = HealthUtility.FindLifeThreateningHediff(pawn);
			if (hediff != null)
			{
				return HealthUtility.Cure(hediff);
			}
			if (HealthUtility.TicksUntilDeathDueToBloodLoss(pawn) < 2500)
			{
				Hediff hediff2 = HealthUtility.FindMostBleedingHediff(pawn);
				if (hediff2 != null)
				{
					return HealthUtility.Cure(hediff2);
				}
			}
			if (pawn.health.hediffSet.GetBrain() != null)
			{
				Hediff_Injury hediff_Injury = HealthUtility.FindPermanentInjury(pawn, Gen.YieldSingle<BodyPartRecord>(pawn.health.hediffSet.GetBrain()));
				if (hediff_Injury != null)
				{
					return HealthUtility.Cure(hediff_Injury);
				}
			}
			float coverageAbsWithChildren = ThingDefOf.Human.race.body.GetPartsWithDef(BodyPartDefOf.Hand).First<BodyPartRecord>().coverageAbsWithChildren;
			BodyPartRecord bodyPartRecord = HealthUtility.FindBiggestMissingBodyPart(pawn, coverageAbsWithChildren);
			if (bodyPartRecord != null)
			{
				return HealthUtility.Cure(bodyPartRecord, pawn);
			}
			Hediff_Injury hediff_Injury2 = HealthUtility.FindPermanentInjury(pawn, from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.def == BodyPartDefOf.Eye
			select x);
			if (hediff_Injury2 != null)
			{
				return HealthUtility.Cure(hediff_Injury2);
			}
			Hediff hediff3 = HealthUtility.FindImmunizableHediffWhichCanKill(pawn);
			if (hediff3 != null)
			{
				return HealthUtility.Cure(hediff3);
			}
			Hediff hediff4 = HealthUtility.FindNonInjuryMiscBadHediff(pawn, true);
			if (hediff4 != null)
			{
				return HealthUtility.Cure(hediff4);
			}
			Hediff hediff5 = HealthUtility.FindNonInjuryMiscBadHediff(pawn, false);
			if (hediff5 != null)
			{
				return HealthUtility.Cure(hediff5);
			}
			if (pawn.health.hediffSet.GetBrain() != null)
			{
				Hediff_Injury hediff_Injury3 = HealthUtility.FindInjury(pawn, Gen.YieldSingle<BodyPartRecord>(pawn.health.hediffSet.GetBrain()));
				if (hediff_Injury3 != null)
				{
					return HealthUtility.Cure(hediff_Injury3);
				}
			}
			BodyPartRecord bodyPartRecord2 = HealthUtility.FindBiggestMissingBodyPart(pawn, 0f);
			if (bodyPartRecord2 != null)
			{
				return HealthUtility.Cure(bodyPartRecord2, pawn);
			}
			Hediff_Addiction hediff_Addiction = HealthUtility.FindAddiction(pawn);
			if (hediff_Addiction != null)
			{
				return HealthUtility.Cure(hediff_Addiction);
			}
			Hediff_Injury hediff_Injury4 = HealthUtility.FindPermanentInjury(pawn, null);
			if (hediff_Injury4 != null)
			{
				return HealthUtility.Cure(hediff_Injury4);
			}
			Hediff_Injury hediff_Injury5 = HealthUtility.FindInjury(pawn, null);
			if (hediff_Injury5 != null)
			{
				return HealthUtility.Cure(hediff_Injury5);
			}
			return null;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x000679CC File Offset: 0x00065BCC
		private static Hediff FindLifeThreateningHediff(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem && !hediffs[i].FullyImmune())
				{
					HediffStage curStage = hediffs[i].CurStage;
					bool flag = curStage != null && curStage.lifeThreatening;
					bool flag2 = hediffs[i].def.lethalSeverity >= 0f && hediffs[i].Severity / hediffs[i].def.lethalSeverity >= 0.8f;
					if (flag || flag2)
					{
						float num2 = (hediffs[i].Part != null) ? hediffs[i].Part.coverageAbsWithChildren : 999f;
						if (hediff == null || num2 > num)
						{
							hediff = hediffs[i];
							num = num2;
						}
					}
				}
			}
			return hediff;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00067AE4 File Offset: 0x00065CE4
		private static Hediff FindMostBleedingHediff(Pawn pawn)
		{
			float num = 0f;
			Hediff hediff = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem)
				{
					float bleedRate = hediffs[i].BleedRate;
					if (bleedRate > 0f && (bleedRate > num || hediff == null))
					{
						num = bleedRate;
						hediff = hediffs[i];
					}
				}
			}
			return hediff;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00067B68 File Offset: 0x00065D68
		private static Hediff FindImmunizableHediffWhichCanKill(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem && hediffs[i].TryGetComp<HediffComp_Immunizable>() != null && !hediffs[i].FullyImmune() && HealthUtility.CanEverKill(hediffs[i]))
				{
					float severity = hediffs[i].Severity;
					if (hediff == null || severity > num)
					{
						hediff = hediffs[i];
						num = severity;
					}
				}
			}
			return hediff;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00067C0C File Offset: 0x00065E0C
		private static Hediff FindNonInjuryMiscBadHediff(Pawn pawn, bool onlyIfCanKill)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.isBad && hediffs[i].def.everCurableByItem && !(hediffs[i] is Hediff_Injury) && !(hediffs[i] is Hediff_MissingPart) && !(hediffs[i] is Hediff_Addiction) && !(hediffs[i] is Hediff_AddedPart) && (!onlyIfCanKill || HealthUtility.CanEverKill(hediffs[i])))
				{
					float num2 = (hediffs[i].Part != null) ? hediffs[i].Part.coverageAbsWithChildren : 999f;
					if (hediff == null || num2 > num)
					{
						hediff = hediffs[i];
						num = num2;
					}
				}
			}
			return hediff;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00067D0C File Offset: 0x00065F0C
		private static BodyPartRecord FindBiggestMissingBodyPart(Pawn pawn, float minCoverage = 0f)
		{
			BodyPartRecord bodyPartRecord = null;
			foreach (Hediff_MissingPart hediff_MissingPart in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
			{
				if (hediff_MissingPart.Part.coverageAbsWithChildren >= minCoverage && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff_MissingPart.Part) && (bodyPartRecord == null || hediff_MissingPart.Part.coverageAbsWithChildren > bodyPartRecord.coverageAbsWithChildren))
				{
					bodyPartRecord = hediff_MissingPart.Part;
				}
			}
			return bodyPartRecord;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00067DA8 File Offset: 0x00065FA8
		private static Hediff_Addiction FindAddiction(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && hediff_Addiction.Visible && hediff_Addiction.def.everCurableByItem)
				{
					return hediff_Addiction;
				}
			}
			return null;
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00067E00 File Offset: 0x00066000
		private static Hediff_Injury FindPermanentInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.IsPermanent() && hediff_Injury2.def.everCurableByItem && (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x00067E84 File Offset: 0x00066084
		private static Hediff_Injury FindInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.def.everCurableByItem && (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00067F00 File Offset: 0x00066100
		public static TaggedString Cure(Hediff hediff)
		{
			Pawn pawn = hediff.pawn;
			pawn.health.RemoveHediff(hediff);
			if (hediff.def.cureAllAtOnceIfCuredByItem)
			{
				int num = 0;
				for (;;)
				{
					num++;
					if (num > 10000)
					{
						break;
					}
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediff.def, false);
					if (firstHediffOfDef == null)
					{
						goto IL_63;
					}
					pawn.health.RemoveHediff(firstHediffOfDef);
				}
				Log.Error("Too many iterations.");
			}
			IL_63:
			return "HealingCureHediff".Translate(pawn, hediff.def.label);
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00067F90 File Offset: 0x00066190
		private static TaggedString Cure(BodyPartRecord part, Pawn pawn)
		{
			pawn.health.RestorePart(part, null, true);
			return "HealingRestoreBodyPart".Translate(pawn, part.Label);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x00067FBC File Offset: 0x000661BC
		private static bool CanEverKill(Hediff hediff)
		{
			if (hediff.def.stages != null)
			{
				for (int i = 0; i < hediff.def.stages.Count; i++)
				{
					if (hediff.def.stages[i].lifeThreatening)
					{
						return true;
					}
				}
			}
			return hediff.def.lethalSeverity >= 0f;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00068020 File Offset: 0x00066220
		public static bool IsMissingSightBodyPart(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart;
				if ((hediff_MissingPart = (hediffs[i] as Hediff_MissingPart)) != null && hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.SightSource))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000DAF RID: 3503
		public static readonly Color GoodConditionColor = new Color(0.6f, 0.8f, 0.65f);

		// Token: 0x04000DB0 RID: 3504
		public static readonly Color RedColor = ColorLibrary.RedReadable;

		// Token: 0x04000DB1 RID: 3505
		public static readonly Color ImpairedColor = new Color(0.9f, 0.7f, 0f);

		// Token: 0x04000DB2 RID: 3506
		public static readonly Color SlightlyImpairedColor = new Color(0.9f, 0.9f, 0f);

		// Token: 0x04000DB3 RID: 3507
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
