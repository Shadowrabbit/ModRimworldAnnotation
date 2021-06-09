using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000428 RID: 1064
	public static class PawnCapacityUtility
	{
		// Token: 0x060019DD RID: 6621 RVA: 0x000181D1 File Offset: 0x000163D1
		public static bool BodyCanEverDoCapacity(BodyDef bodyDef, PawnCapacityDef capacity)
		{
			return capacity.Worker.CanHaveCapacity(bodyDef);
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x000E3CFC File Offset: 0x000E1EFC
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

		// Token: 0x060019DF RID: 6623 RVA: 0x000E3E8C File Offset: 0x000E208C
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

		// Token: 0x060019E0 RID: 6624 RVA: 0x000181DF File Offset: 0x000163DF
		public static float CalculateImmediatePartEfficiencyAndRecord(HediffSet diffSet, BodyPartRecord part, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			if (diffSet.AncestorHasDirectlyAddedParts(part))
			{
				return 1f;
			}
			return PawnCapacityUtility.CalculatePartEfficiency(diffSet, part, false, impactors);
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x000E410C File Offset: 0x000E230C
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

		// Token: 0x060019E2 RID: 6626 RVA: 0x000E41B0 File Offset: 0x000E23B0
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

		// Token: 0x060019E3 RID: 6627 RVA: 0x000E42E8 File Offset: 0x000E24E8
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

		// Token: 0x02000429 RID: 1065
		public abstract class CapacityImpactor
		{
			// Token: 0x170004C7 RID: 1223
			// (get) Token: 0x060019E4 RID: 6628 RVA: 0x0000A2A7 File Offset: 0x000084A7
			public virtual bool IsDirect
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060019E5 RID: 6629
			public abstract string Readable(Pawn pawn);
		}

		// Token: 0x0200042A RID: 1066
		public class CapacityImpactorBodyPartHealth : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x060019E7 RID: 6631 RVA: 0x000E4454 File Offset: 0x000E2654
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1} / {2}", this.bodyPart.LabelCap, pawn.health.hediffSet.GetPartHealth(this.bodyPart), this.bodyPart.def.GetMaxHealth(pawn));
			}

			// Token: 0x04001345 RID: 4933
			public BodyPartRecord bodyPart;
		}

		// Token: 0x0200042B RID: 1067
		public class CapacityImpactorCapacity : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x170004C8 RID: 1224
			// (get) Token: 0x060019E9 RID: 6633 RVA: 0x0000A2E4 File Offset: 0x000084E4
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060019EA RID: 6634 RVA: 0x000E44A8 File Offset: 0x000E26A8
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", this.capacity.GetLabelFor(pawn).CapitalizeFirst(), (pawn.health.capacities.GetLevel(this.capacity) * 100f).ToString("F0"));
			}

			// Token: 0x04001346 RID: 4934
			public PawnCapacityDef capacity;
		}

		// Token: 0x0200042C RID: 1068
		public class CapacityImpactorHediff : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x060019EC RID: 6636 RVA: 0x00018201 File Offset: 0x00016401
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}", this.hediff.LabelCap);
			}

			// Token: 0x04001347 RID: 4935
			public Hediff hediff;
		}

		// Token: 0x0200042D RID: 1069
		public class CapacityImpactorPain : PawnCapacityUtility.CapacityImpactor
		{
			// Token: 0x170004C9 RID: 1225
			// (get) Token: 0x060019EE RID: 6638 RVA: 0x0000A2E4 File Offset: 0x000084E4
			public override bool IsDirect
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060019EF RID: 6639 RVA: 0x000E44FC File Offset: 0x000E26FC
			public override string Readable(Pawn pawn)
			{
				return string.Format("{0}: {1}%", "Pain".Translate(), (pawn.health.hediffSet.PainTotal * 100f).ToString("F0"));
			}
		}
	}
}
