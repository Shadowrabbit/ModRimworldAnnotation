using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003A4 RID: 932
	public static class HealthUtility
	{
		// Token: 0x06001710 RID: 5904 RVA: 0x000DAA9C File Offset: 0x000D8C9C
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

		// Token: 0x06001711 RID: 5905 RVA: 0x000DABCC File Offset: 0x000D8DCC
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

		// Token: 0x06001712 RID: 5906 RVA: 0x000DAD34 File Offset: 0x000D8F34
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

		// Token: 0x06001713 RID: 5907 RVA: 0x000DAD98 File Offset: 0x000D8F98
		private static IEnumerable<BodyPartRecord> HittablePartsViolence(HediffSet bodyModel)
		{
			return from x in bodyModel.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside || (x.depth == BodyPartDepth.Inside && x.def.IsSolid(x, bodyModel.hediffs))
			select x;
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0001640C File Offset: 0x0001460C
		public static void GiveInjuriesOperationFailureMinor(Pawn p, BodyPartRecord part)
		{
			HealthUtility.GiveRandomSurgeryInjuries(p, 20, part);
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x00016417 File Offset: 0x00014617
		public static void GiveInjuriesOperationFailureCatastrophic(Pawn p, BodyPartRecord part)
		{
			HealthUtility.GiveRandomSurgeryInjuries(p, 65, part);
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x00016422 File Offset: 0x00014622
		public static void GiveInjuriesOperationFailureRidiculous(Pawn p)
		{
			HealthUtility.GiveRandomSurgeryInjuries(p, 65, null);
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x000DADD4 File Offset: 0x000D8FD4
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

		// Token: 0x06001718 RID: 5912 RVA: 0x000DAED4 File Offset: 0x000D90D4
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
				DamageInfo dinfo = new DamageInfo(def, (float)num, 0f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				dinfo.SetIgnoreArmor(true);
				p.TakeDamage(dinfo);
				totalDamage -= num;
			}
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x000DB158 File Offset: 0x000D9358
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

		// Token: 0x0600171A RID: 5914 RVA: 0x000DB19C File Offset: 0x000D939C
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

		// Token: 0x0600171B RID: 5915 RVA: 0x000DB220 File Offset: 0x000D9420
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
						DamageInfo dinfo = new DamageInfo(damageDef, (float)num3, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
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
				Log.Error(stringBuilder.ToString(), false);
			}
			p.health.forceIncap = false;
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x000DB448 File Offset: 0x000D9648
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
				DamageInfo dinfo = new DamageInfo(def, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				dinfo.SetIgnoreInstantKillProtection(true);
				p.TakeDamage(dinfo);
			}
			if (!p.Dead)
			{
				Log.Error(p + " not killed during GiveInjuriesToKill", false);
			}
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x000DB51C File Offset: 0x000D971C
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
				DamageInfo dinfo = new DamageInfo(damageDef, (float)num2, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
				dinfo.SetAllowDamagePropagation(false);
				p.TakeDamage(dinfo);
			}
			p.health.forceIncap = false;
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x000DB6C0 File Offset: 0x000D98C0
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

		// Token: 0x0600171F RID: 5919 RVA: 0x000DB710 File Offset: 0x000D9910
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

		// Token: 0x06001720 RID: 5920 RVA: 0x000DB77C File Offset: 0x000D997C
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

		// Token: 0x06001721 RID: 5921 RVA: 0x000DB7D0 File Offset: 0x000D99D0
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

		// Token: 0x06001722 RID: 5922 RVA: 0x000DB83C File Offset: 0x000D9A3C
		public static BodyPartRemovalIntent PartRemovalIntent(Pawn pawn, BodyPartRecord part)
		{
			if (pawn.health.hediffSet.hediffs.Any((Hediff d) => d.Visible && d.Part == part && d.def.isBad))
			{
				return BodyPartRemovalIntent.Amputate;
			}
			return BodyPartRemovalIntent.Harvest;
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x000DB87C File Offset: 0x000D9A7C
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

		// Token: 0x06001724 RID: 5924 RVA: 0x000DB8E0 File Offset: 0x000D9AE0
		public static void CureHediff(Hediff hediff)
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
						return;
					}
					pawn.health.RemoveHediff(firstHediffOfDef);
				}
				Log.Error("Too many iterations.", false);
				return;
			}
		}

		// Token: 0x040011EA RID: 4586
		public static readonly Color GoodConditionColor = new Color(0.6f, 0.8f, 0.65f);

		// Token: 0x040011EB RID: 4587
		public static readonly Color RedColor = ColoredText.RedReadable;

		// Token: 0x040011EC RID: 4588
		[Obsolete]
		public static readonly Color DarkRedColor = ColoredText.RedReadable;

		// Token: 0x040011ED RID: 4589
		public static readonly Color ImpairedColor = new Color(0.9f, 0.7f, 0f);

		// Token: 0x040011EE RID: 4590
		public static readonly Color SlightlyImpairedColor = new Color(0.9f, 0.9f, 0f);

		// Token: 0x040011EF RID: 4591
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
