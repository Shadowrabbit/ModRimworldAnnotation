using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E1 RID: 737
	public static class PawnCapacityUtility
	{
		// Token: 0x060013E9 RID: 5097 RVA: 0x00071078 File Offset: 0x0006F278
		public static bool BodyCanEverDoCapacity(BodyDef bodyDef, PawnCapacityDef capacity)
		{
			return capacity.Worker.CanHaveCapacity(bodyDef);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00071088 File Offset: 0x0006F288
		public static float CalculateCapacityLevel(HediffSet diffSet, PawnCapacityDef capacity, List<PawnCapacityUtility.CapacityImpactor> impactors = null, bool forTradePrice = false)
		{
			if (capacity.zeroIfCannotBeAwake && !diffSet.pawn.health.capacities.CanBeAwake)
			{
				if (impactors != null)
				{
					impactors.Add(new PawnCapacityUtility.CapacityImpactorCapacity
					{
						capacity = PawnCapacityDefOf.Consciousness
					});
				}
				return 0f;
			}
			float num = capacity.Worker.CalculateCapacityLevel(diffSet, impactors);
			if (num > 0f)
			{
				float num2 = 99999f;
				float num3 = 1f;
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff hediff = diffSet.hediffs[i];
					if (!forTradePrice || hediff.def.priceImpact)
					{
						List<PawnCapacityModifier> capMods = hediff.CapMods;
						if (capMods != null)
						{
							for (int j = 0; j < capMods.Count; j++)
							{
								PawnCapacityModifier pawnCapacityModifier = capMods[j];
								if (pawnCapacityModifier.capacity == capacity)
								{
									num += pawnCapacityModifier.offset;
									float num4 = pawnCapacityModifier.postFactor;
									if (hediff.CurStage != null && hediff.CurStage.capacityFactorEffectMultiplier != null)
									{
										num4 = StatWorker.ScaleFactor(num4, hediff.pawn.GetStatValue(hediff.CurStage.capacityFactorEffectMultiplier, true));
									}
									num3 *= num4;
									float num5 = pawnCapacityModifier.EvaluateSetMax(diffSet.pawn);
									if (num5 < num2)
									{
										num2 = num5;
									}
									if (impactors != null)
									{
										impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
										{
											hediff = hediff
										});
									}
								}
							}
						}
					}
				}
				num *= num3;
				num = Mathf.Min(num, num2);
			}
			num = Mathf.Max(num, capacity.minValue);
			return GenMath.RoundedHundredth(num);
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00071218 File Offset: 0x0006F418
		public static float CalculatePartEfficiency(HediffSet diffSet, BodyPartRecord part, bool ignoreAddedParts = false, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartRecord rec;
			Func<Hediff_AddedPart, bool> <>9__0;
			for (rec = part.parent; rec != null; rec = rec.parent)
			{
				if (diffSet.HasDirectlyAddedPartFor(rec))
				{
					IEnumerable<Hediff_AddedPart> hediffs = diffSet.GetHediffs<Hediff_AddedPart>();
					Func<Hediff_AddedPart, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Hediff_AddedPart x) => x.Part == rec));
					}
					Hediff_AddedPart hediff_AddedPart = hediffs.Where(predicate).First<Hediff_AddedPart>();
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
						{
							hediff = hediff_AddedPart
						});
					}
					return hediff_AddedPart.def.addedPartProps.partEfficiency;
				}
			}
			if (part.parent != null && diffSet.PartIsMissing(part.parent))
			{
				return 0f;
			}
			float num = 1f;
			if (!ignoreAddedParts)
			{
				for (int i = 0; i < diffSet.hediffs.Count; i++)
				{
					Hediff_AddedPart hediff_AddedPart2 = diffSet.hediffs[i] as Hediff_AddedPart;
					if (hediff_AddedPart2 != null && hediff_AddedPart2.Part == part)
					{
						num *= hediff_AddedPart2.def.addedPartProps.partEfficiency;
						if (hediff_AddedPart2.def.addedPartProps.partEfficiency != 1f && impactors != null)
						{
							impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
							{
								hediff = hediff_AddedPart2
							});
						}
					}
				}
			}
			float b = -1f;
			float num2 = 0f;
			bool flag = false;
			for (int j = 0; j < diffSet.hediffs.Count; j++)
			{
				if (diffSet.hediffs[j].Part == part && diffSet.hediffs[j].CurStage != null)
				{
					HediffStage curStage = diffSet.hediffs[j].CurStage;
					num2 += curStage.partEfficiencyOffset;
					flag |= curStage.partIgnoreMissingHP;
					if (curStage.partEfficiencyOffset != 0f && curStage.becomeVisible && impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorHediff
						{
							hediff = diffSet.hediffs[j]
						});
					}
				}
			}
			if (!flag)
			{
				float num3 = diffSet.GetPartHealth(part) / part.def.GetMaxHealth(diffSet.pawn);
				if (num3 != 1f)
				{
					if (DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(part))
					{
						num3 = Mathf.InverseLerp(0.1f, 1f, num3);
					}
					if (impactors != null)
					{
						impactors.Add(new PawnCapacityUtility.CapacityImpactorBodyPartHealth
						{
							bodyPart = part
						});
					}
					num *= num3;
				}
			}
			num += num2;
			if (num > 0.0001f)
			{
				num = Mathf.Max(num, b);
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00071498 File Offset: 0x0006F698
		public static float CalculateImmediatePartEfficiencyAndRecord(HediffSet diffSet, BodyPartRecord part, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			if (diffSet.AncestorHasDirectlyAddedParts(part))
			{
				return 1f;
			}
			return PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, impactors);
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x000714B4 File Offset: 0x0006F6B4
		public static float CalculateNaturalPartsAverageEfficiency(HediffSet diffSet, BodyPartGroupDef bodyPartGroup)
		{
			float num = 0f;
			int num2 = 0;
			foreach (BodyPartRecord part in from x in diffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.groups.Contains(bodyPartGroup)
			select x)
			{
				if (!diffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
				{
					num += PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, null);
				}
				num2++;
			}
			if (num2 == 0 || num < 0f)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00071558 File Offset: 0x0006F758
		public static float CalculateTagEfficiency(HediffSet diffSet, BodyPartTagDef tag, float maximum = 3.4028235E+38f, FloatRange lerp = default(FloatRange), List<PawnCapacityUtility.CapacityImpactor> impactors = null, float bestPartEfficiencySpecialWeight = -1f)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			float num3 = 0f;
			List<PawnCapacityUtility.CapacityImpactor> list = null;
			foreach (BodyPartRecord part in body.GetPartsWithTag(tag))
			{
				float num4 = PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, list);
				if (impactors != null && num4 != 1f && list == null)
				{
					list = new List<PawnCapacityUtility.CapacityImpactor>();
					PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, list);
				}
				num += num4;
				num3 = Mathf.Max(num3, num4);
				num2++;
			}
			if (num2 == 0)
			{
				return 1f;
			}
			float num5;
			if (bestPartEfficiencySpecialWeight >= 0f && num2 >= 2)
			{
				num5 = num3 * bestPartEfficiencySpecialWeight + (num - num3) / (float)(num2 - 1) * (1f - bestPartEfficiencySpecialWeight);
			}
			else
			{
				num5 = num / (float)num2;
			}
			float num6 = num5;
			if (lerp != default(FloatRange))
			{
				num6 = lerp.LerpThroughRange(num6);
			}
			num6 = Mathf.Min(num6, maximum);
			if (impactors != null && list != null && (maximum != 1f || num5 <= 1f || num6 == 1f))
			{
				impactors.AddRange(list);
			}
			return num6;
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00071690 File Offset: 0x0006F890
		public static float CalculateLimbEfficiency(HediffSet diffSet, BodyPartTagDef limbCoreTag, BodyPartTagDef limbSegmentTag, BodyPartTagDef limbDigitTag, float appendageWeight, out float functionalPercentage, List<PawnCapacityUtility.CapacityImpactor> impactors)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			Func<BodyPartRecord, float> <>9__0;
			foreach (BodyPartRecord bodyPartRecord in body.GetPartsWithTag(limbCoreTag))
			{
				float num4 = PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, bodyPartRecord, impactors);
				foreach (BodyPartRecord part in bodyPartRecord.GetConnectedParts(limbSegmentTag))
				{
					num4 *= PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, part, impactors);
				}
				if (bodyPartRecord.HasChildParts(limbDigitTag))
				{
					float a = num4;
					float num5 = num4;
					IEnumerable<BodyPartRecord> childParts = bodyPartRecord.GetChildParts(limbDigitTag);
					Func<BodyPartRecord, float> selector;
					if ((selector = <>9__0) == null)
					{
						selector = (<>9__0 = ((BodyPartRecord digitPart) => PawnCapacityUtility.CalculateImmediatePartEfficiencyAndRecord(diffSet, digitPart, impactors)));
					}
					num4 = Mathf.Lerp(a, num5 * childParts.Average(selector), appendageWeight);
				}
				num += num4;
				num2++;
				if (num4 > 0f)
				{
					num3++;
				}
			}
			if (num2 == 0)
			{
				functionalPercentage = 0f;
				return 0f;
			}
			functionalPercentage = (float)num3 / (float)num2;
			return num / (float)num2;
		}

		// Token: 0x02001A05 RID: 6661
		public abstract class CapacityImpactor
		{
			// Token: 0x17001984 RID: 6532
			// (get) Token: 0x06009B27 RID: 39719 RVA: 0x000126F5 File Offset: 0x000108F5
			public virtual bool IsDirect
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06009B28 RID: 39720
			public abstract string Readable(Pawn pawn);
		}

		// Token: 0x02001A06 RID: 6662
		public class CapacityImpactorBodyPartHealth : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x06009B2A RID: 39722 RVA: 0x003659F8 File Offset: 0x00363BF8
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1} / {2}", this.bodyPart.LabelCap, pawn.health.hediffSet.GetPartHealth(this.bodyPart), this.bodyPart.def.GetMaxHealth(pawn));
			}

			// Token: 0x040063C1 RID: 25537
			public BodyPartRecord bodyPart;
		}

		// Token: 0x02001A07 RID: 6663
		public class CapacityImpactorCapacity : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x17001985 RID: 6533
			// (get) Token: 0x06009B2C RID: 39724 RVA: 0x0001276E File Offset: 0x0001096E
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06009B2D RID: 39725 RVA: 0x00365A54 File Offset: 0x00363C54
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", this.capacity.GetLabelFor(pawn).CapitalizeFirst(), (pawn.health.capacities.GetLevel(this.capacity) * 100f).ToString("F0"));
			}

			// Token: 0x040063C2 RID: 25538
			public PawnCapacityDef capacity;
		}

		// Token: 0x02001A08 RID: 6664
		public class CapacityImpactorHediff : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x06009B2F RID: 39727 RVA: 0x00365AA5 File Offset: 0x00363CA5
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}", this.hediff.LabelCap);
			}

			// Token: 0x040063C3 RID: 25539
			public Hediff hediff;
		}

		// Token: 0x02001A09 RID: 6665
		public class CapacityImpactorPain : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x17001986 RID: 6534
			// (get) Token: 0x06009B31 RID: 39729 RVA: 0x0001276E File Offset: 0x0001096E
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06009B32 RID: 39730 RVA: 0x00365ABC File Offset: 0x00363CBC
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", "Pain".Translate(), (pawn.health.hediffSet.PainTotal * 100f).ToString("F0"));
			}
		}
	}
}
