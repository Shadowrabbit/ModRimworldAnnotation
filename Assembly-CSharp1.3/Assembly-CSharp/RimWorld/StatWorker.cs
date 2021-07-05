using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F1 RID: 5361
	public class StatWorker
	{
		// Token: 0x06007FB6 RID: 32694 RVA: 0x002D17DB File Offset: 0x002CF9DB
		public void InitSetStat(StatDef newStat)
		{
			this.stat = newStat;
		}

		// Token: 0x06007FB7 RID: 32695 RVA: 0x002D17E4 File Offset: 0x002CF9E4
		public float GetValue(Thing thing, bool applyPostProcess = true)
		{
			return this.GetValue(StatRequest.For(thing), true);
		}

		// Token: 0x06007FB8 RID: 32696 RVA: 0x002D17F3 File Offset: 0x002CF9F3
		public float GetValue(Thing thing, Pawn pawn, bool applyPostProcess = true)
		{
			return this.GetValue(StatRequest.For(thing, pawn), true);
		}

		// Token: 0x06007FB9 RID: 32697 RVA: 0x002D1804 File Offset: 0x002CFA04
		public float GetValue(StatRequest req, bool applyPostProcess = true)
		{
			if (this.stat.minifiedThingInherits)
			{
				MinifiedThing minifiedThing = req.Thing as MinifiedThing;
				if (minifiedThing != null)
				{
					if (minifiedThing.InnerThing != null)
					{
						return minifiedThing.InnerThing.GetStatValue(this.stat, applyPostProcess);
					}
					Log.Error("MinifiedThing's inner thing is null.");
				}
			}
			float valueUnfinalized = this.GetValueUnfinalized(req, applyPostProcess);
			this.FinalizeValue(req, ref valueUnfinalized, applyPostProcess);
			return valueUnfinalized;
		}

		// Token: 0x06007FBA RID: 32698 RVA: 0x002D1867 File Offset: 0x002CFA67
		public float GetValueAbstract(BuildableDef def, ThingDef stuffDef = null)
		{
			return this.GetValue(StatRequest.For(def, stuffDef, QualityCategory.Normal), true);
		}

		// Token: 0x06007FBB RID: 32699 RVA: 0x002D1878 File Offset: 0x002CFA78
		public float GetValueAbstract(AbilityDef def, Pawn forPawn = null)
		{
			return this.GetValue(StatRequest.For(def, forPawn), true);
		}

		// Token: 0x06007FBC RID: 32700 RVA: 0x002D1888 File Offset: 0x002CFA88
		public virtual float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (!this.stat.supressDisabledError && Prefs.DevMode && this.IsDisabledFor(req.Thing))
			{
				Log.ErrorOnce(string.Format("Attempted to calculate value for disabled stat {0}; this is meant as a consistency check, either set the stat to neverDisabled or ensure this pawn cannot accidentally use this stat (thing={1})", this.stat, req.Thing.ToStringSafe<Thing>()), 75193282 + (int)this.stat.index);
			}
			float num = this.GetBaseValueFor(req);
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedOffsets != null)
					{
						for (int i = 0; i < this.stat.skillNeedOffsets.Count; i++)
						{
							num += this.stat.skillNeedOffsets[i].ValueFor(pawn);
						}
					}
				}
				else
				{
					num += this.stat.noSkillOffset;
				}
				if (this.stat.capacityOffsets != null)
				{
					for (int j = 0; j < this.stat.capacityOffsets.Count; j++)
					{
						PawnCapacityOffset pawnCapacityOffset = this.stat.capacityOffsets[j];
						num += pawnCapacityOffset.GetOffset(pawn.health.capacities.GetLevel(pawnCapacityOffset.capacity));
					}
				}
				if (pawn.story != null)
				{
					for (int k = 0; k < pawn.story.traits.allTraits.Count; k++)
					{
						num += pawn.story.traits.allTraits[k].OffsetOfStat(this.stat);
					}
				}
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int l = 0; l < hediffs.Count; l++)
				{
					HediffStage curStage = hediffs[l].CurStage;
					if (curStage != null)
					{
						float num2 = curStage.statOffsets.GetStatOffsetFromList(this.stat);
						if (num2 != 0f && curStage.statOffsetEffectMultiplier != null)
						{
							num2 *= pawn.GetStatValue(curStage.statOffsetEffectMultiplier, true);
						}
						num += num2;
					}
				}
				if (pawn.Ideo != null)
				{
					List<Precept> preceptsListForReading = pawn.Ideo.PreceptsListForReading;
					for (int m = 0; m < preceptsListForReading.Count; m++)
					{
						if (preceptsListForReading[m].def.statOffsets != null)
						{
							float statOffsetFromList = preceptsListForReading[m].def.statOffsets.GetStatOffsetFromList(this.stat);
							num += statOffsetFromList;
						}
					}
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (role != null && role.def.roleEffects != null)
					{
						using (List<RoleEffect>.Enumerator enumerator = role.def.roleEffects.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								RoleEffect_PawnStatOffset roleEffect_PawnStatOffset;
								if ((roleEffect_PawnStatOffset = (enumerator.Current as RoleEffect_PawnStatOffset)) != null && roleEffect_PawnStatOffset.statDef == this.stat)
								{
									num += roleEffect_PawnStatOffset.modifier;
								}
							}
						}
					}
				}
				if (pawn.apparel != null)
				{
					for (int n = 0; n < pawn.apparel.WornApparel.Count; n++)
					{
						num += StatWorker.StatOffsetFromGear(pawn.apparel.WornApparel[n], this.stat);
					}
				}
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					num += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.stat);
				}
				if (pawn.story != null)
				{
					for (int num3 = 0; num3 < pawn.story.traits.allTraits.Count; num3++)
					{
						num *= pawn.story.traits.allTraits[num3].MultiplierOfStat(this.stat);
					}
				}
				for (int num4 = 0; num4 < hediffs.Count; num4++)
				{
					HediffStage curStage2 = hediffs[num4].CurStage;
					if (curStage2 != null)
					{
						float num5 = curStage2.statFactors.GetStatFactorFromList(this.stat);
						if (Math.Abs(num5 - 1f) > 1E-45f && curStage2.statFactorEffectMultiplier != null)
						{
							num5 = StatWorker.ScaleFactor(num5, pawn.GetStatValue(curStage2.statFactorEffectMultiplier, true));
						}
						num *= num5;
					}
				}
				if (pawn.Ideo != null)
				{
					List<Precept> preceptsListForReading2 = pawn.Ideo.PreceptsListForReading;
					for (int num6 = 0; num6 < preceptsListForReading2.Count; num6++)
					{
						if (preceptsListForReading2[num6].def.statFactors != null)
						{
							float statFactorFromList = preceptsListForReading2[num6].def.statFactors.GetStatFactorFromList(this.stat);
							num *= statFactorFromList;
						}
					}
					Precept_Role role2 = pawn.Ideo.GetRole(pawn);
					if (role2 != null && role2.def.roleEffects != null)
					{
						using (List<RoleEffect>.Enumerator enumerator = role2.def.roleEffects.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								RoleEffect_PawnStatFactor roleEffect_PawnStatFactor;
								if ((roleEffect_PawnStatFactor = (enumerator.Current as RoleEffect_PawnStatFactor)) != null && roleEffect_PawnStatFactor.statDef == this.stat)
								{
									num *= roleEffect_PawnStatFactor.modifier;
								}
							}
						}
					}
				}
				num *= pawn.ageTracker.CurLifeStage.statFactors.GetStatFactorFromList(this.stat);
			}
			if (req.StuffDef != null)
			{
				if (num > 0f || this.stat.applyFactorsIfNegative)
				{
					num *= req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
				}
				num += req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
			}
			if (req.ForAbility)
			{
				if (this.stat.statFactors != null)
				{
					for (int num7 = 0; num7 < this.stat.statFactors.Count; num7++)
					{
						num *= req.AbilityDef.statBases.GetStatValueFromList(this.stat.statFactors[num7], 1f);
					}
				}
				Pawn pawn2 = req.Pawn;
				if (pawn2 != null && pawn2.Ideo != null)
				{
					List<Precept> preceptsListForReading3 = pawn2.Ideo.PreceptsListForReading;
					for (int num8 = 0; num8 < preceptsListForReading3.Count; num8++)
					{
						if (preceptsListForReading3[num8].def.statFactors != null)
						{
							float statFactorFromList2 = preceptsListForReading3[num8].def.statFactors.GetStatFactorFromList(this.stat);
							num *= statFactorFromList2;
						}
						if (preceptsListForReading3[num8].def.abilityStatFactors != null)
						{
							foreach (AbilityStatModifiers abilityStatModifiers in preceptsListForReading3[num8].def.abilityStatFactors)
							{
								if (abilityStatModifiers.ability == req.AbilityDef)
								{
									float statFactorFromList3 = abilityStatModifiers.modifiers.GetStatFactorFromList(this.stat);
									num *= statFactorFromList3;
								}
							}
						}
					}
				}
			}
			if (req.HasThing)
			{
				CompAffectedByFacilities compAffectedByFacilities = req.Thing.TryGetComp<CompAffectedByFacilities>();
				if (compAffectedByFacilities != null)
				{
					num += compAffectedByFacilities.GetStatOffset(this.stat);
				}
				if (this.stat.statFactors != null)
				{
					for (int num9 = 0; num9 < this.stat.statFactors.Count; num9++)
					{
						num *= req.Thing.GetStatValue(this.stat.statFactors[num9], true);
					}
				}
				if (pawn != null)
				{
					if (pawn.skills != null)
					{
						if (this.stat.skillNeedFactors != null)
						{
							for (int num10 = 0; num10 < this.stat.skillNeedFactors.Count; num10++)
							{
								num *= this.stat.skillNeedFactors[num10].ValueFor(pawn);
							}
						}
					}
					else
					{
						num *= this.stat.noSkillFactor;
					}
					if (this.stat.capacityFactors != null)
					{
						for (int num11 = 0; num11 < this.stat.capacityFactors.Count; num11++)
						{
							PawnCapacityFactor pawnCapacityFactor = this.stat.capacityFactors[num11];
							float factor = pawnCapacityFactor.GetFactor(pawn.health.capacities.GetLevel(pawnCapacityFactor.capacity));
							num = Mathf.Lerp(num, num * factor, pawnCapacityFactor.weight);
						}
					}
					if (pawn.Inspired)
					{
						num += pawn.InspirationDef.statOffsets.GetStatOffsetFromList(this.stat);
						num *= pawn.InspirationDef.statFactors.GetStatFactorFromList(this.stat);
					}
				}
			}
			return num;
		}

		// Token: 0x06007FBD RID: 32701 RVA: 0x002D2114 File Offset: 0x002D0314
		public virtual string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			float baseValueFor = this.GetBaseValueFor(req);
			if (baseValueFor != 0f || this.stat.showZeroBaseValue)
			{
				stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + this.stat.ValueToString(baseValueFor, numberSense, true));
			}
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedOffsets != null)
					{
						stringBuilder.AppendLine("StatsReport_Skills".Translate());
						for (int i = 0; i < this.stat.skillNeedOffsets.Count; i++)
						{
							SkillNeed skillNeed = this.stat.skillNeedOffsets[i];
							int level = pawn.skills.GetSkill(skillNeed.skill).Level;
							float val = skillNeed.ValueFor(pawn);
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"    " + skillNeed.skill.LabelCap + " (",
								level,
								"): ",
								val.ToStringSign(),
								this.ValueToString(val, false, ToStringNumberSense.Absolute)
							}));
						}
					}
				}
				else if (this.stat.noSkillOffset != 0f)
				{
					stringBuilder.AppendLine("StatsReport_Skills".Translate());
					stringBuilder.AppendLine("    " + "default".Translate().CapitalizeFirst() + " : " + this.stat.noSkillOffset.ToStringSign() + this.ValueToString(this.stat.noSkillOffset, false, ToStringNumberSense.Absolute));
				}
				if (this.stat.capacityOffsets != null)
				{
					stringBuilder.AppendLine("StatsReport_Health".CanTranslate() ? "StatsReport_Health".Translate() : "StatsReport_HealthFactors".Translate());
					foreach (PawnCapacityOffset pawnCapacityOffset in from hfa in this.stat.capacityOffsets
					orderby hfa.capacity.listOrder
					select hfa)
					{
						string text = pawnCapacityOffset.capacity.GetLabelFor(pawn).CapitalizeFirst();
						float level2 = pawn.health.capacities.GetLevel(pawnCapacityOffset.capacity);
						float offset = pawnCapacityOffset.GetOffset(pawn.health.capacities.GetLevel(pawnCapacityOffset.capacity));
						string text2 = this.ValueToString(offset, false, ToStringNumberSense.Absolute);
						string text3 = Mathf.Min(level2, pawnCapacityOffset.max).ToStringPercent() + ", " + "HealthOffsetScale".Translate(pawnCapacityOffset.scale.ToString() + "x");
						if (pawnCapacityOffset.max < 999f)
						{
							text3 += ", " + "HealthFactorMaxImpact".Translate(pawnCapacityOffset.max.ToStringPercent());
						}
						stringBuilder.AppendLine(string.Concat(new string[]
						{
							"    ",
							text,
							": ",
							offset.ToStringSign(),
							text2,
							" (",
							text3,
							")"
						}));
					}
				}
				if (pawn.RaceProps.intelligence >= Intelligence.ToolUser)
				{
					if (pawn.story != null && pawn.story.traits != null)
					{
						List<Trait> list = (from tr in pawn.story.traits.allTraits
						where tr.CurrentData.statOffsets != null && tr.CurrentData.statOffsets.Any((StatModifier se) => se.stat == this.stat)
						select tr).ToList<Trait>();
						List<Trait> list2 = (from tr in pawn.story.traits.allTraits
						where tr.CurrentData.statFactors != null && tr.CurrentData.statFactors.Any((StatModifier se) => se.stat == this.stat)
						select tr).ToList<Trait>();
						if (list.Count > 0 || list2.Count > 0)
						{
							stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate());
							for (int j = 0; j < list.Count; j++)
							{
								Trait trait = list[j];
								string valueToStringAsOffset = trait.CurrentData.statOffsets.First((StatModifier se) => se.stat == this.stat).ValueToStringAsOffset;
								stringBuilder.AppendLine("    " + trait.LabelCap + ": " + valueToStringAsOffset);
							}
							for (int k = 0; k < list2.Count; k++)
							{
								Trait trait2 = list2[k];
								string toStringAsFactor = trait2.CurrentData.statFactors.First((StatModifier se) => se.stat == this.stat).ToStringAsFactor;
								stringBuilder.AppendLine("    " + trait2.LabelCap + ": " + toStringAsFactor);
							}
						}
					}
					if (StatWorker.RelevantGear(pawn, this.stat).Any<Thing>())
					{
						stringBuilder.AppendLine("StatsReport_RelevantGear".Translate());
						if (pawn.apparel != null)
						{
							for (int l = 0; l < pawn.apparel.WornApparel.Count; l++)
							{
								Apparel apparel = pawn.apparel.WornApparel[l];
								if (StatWorker.GearAffectsStat(apparel.def, this.stat))
								{
									stringBuilder.AppendLine(StatWorker.InfoTextLineFromGear(apparel, this.stat));
								}
							}
						}
						if (pawn.equipment != null && pawn.equipment.Primary != null && (StatWorker.GearAffectsStat(pawn.equipment.Primary.def, this.stat) || StatWorker.GearHasCompsThatAffectStat(pawn.equipment.Primary, this.stat)))
						{
							stringBuilder.AppendLine(StatWorker.InfoTextLineFromGear(pawn.equipment.Primary, this.stat));
						}
					}
				}
				bool flag = false;
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int m = 0; m < hediffs.Count; m++)
				{
					HediffStage curStage = hediffs[m].CurStage;
					if (curStage != null)
					{
						float num = curStage.statOffsets.GetStatOffsetFromList(this.stat);
						if (num != 0f)
						{
							float val2 = num;
							if (curStage.statOffsetEffectMultiplier != null)
							{
								num *= pawn.GetStatValue(curStage.statOffsetEffectMultiplier, true);
							}
							if (!flag)
							{
								stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate());
								flag = true;
							}
							stringBuilder.Append("    " + hediffs[m].LabelBaseCap + ": " + this.ValueToString(num, false, ToStringNumberSense.Offset));
							if (curStage.statOffsetEffectMultiplier != null)
							{
								stringBuilder.Append(string.Concat(new string[]
								{
									" (",
									this.ValueToString(val2, false, ToStringNumberSense.Offset),
									" x ",
									this.ValueToString(pawn.GetStatValue(curStage.statOffsetEffectMultiplier, true), true, curStage.statOffsetEffectMultiplier.toStringNumberSense),
									" "
								}) + curStage.statOffsetEffectMultiplier.LabelCap + ")");
							}
							stringBuilder.AppendLine();
						}
						float num2 = curStage.statFactors.GetStatFactorFromList(this.stat);
						if (Math.Abs(num2 - 1f) > 1E-45f)
						{
							float val3 = num2;
							if (curStage.statFactorEffectMultiplier != null)
							{
								num2 = StatWorker.ScaleFactor(num2, pawn.GetStatValue(curStage.statFactorEffectMultiplier, true));
							}
							if (!flag)
							{
								stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate());
								flag = true;
							}
							stringBuilder.Append("    " + hediffs[m].LabelBaseCap + ": " + this.ValueToString(num2, false, ToStringNumberSense.Factor));
							if (curStage.statFactorEffectMultiplier != null)
							{
								stringBuilder.Append(string.Concat(new string[]
								{
									" (",
									this.ValueToString(val3, false, ToStringNumberSense.Factor),
									" x ",
									this.ValueToString(pawn.GetStatValue(curStage.statFactorEffectMultiplier, true), false, ToStringNumberSense.Absolute),
									" "
								}) + curStage.statFactorEffectMultiplier.LabelCap + ")");
							}
							stringBuilder.AppendLine();
						}
					}
				}
				if (pawn.Ideo != null)
				{
					List<Precept> preceptsListForReading = pawn.Ideo.PreceptsListForReading;
					for (int n = 0; n < preceptsListForReading.Count; n++)
					{
						float statOffsetFromList = preceptsListForReading[n].def.statOffsets.GetStatOffsetFromList(this.stat);
						if (statOffsetFromList != 0f)
						{
							stringBuilder.AppendLine("StatsReport_Ideoligion".Translate() + ": " + this.ValueToString(statOffsetFromList, false, ToStringNumberSense.Offset));
						}
						float statFactorFromList = preceptsListForReading[n].def.statFactors.GetStatFactorFromList(this.stat);
						if (statFactorFromList != 1f)
						{
							stringBuilder.AppendLine("StatsReport_Ideoligion".Translate() + ": " + this.ValueToString(statFactorFromList, false, ToStringNumberSense.Factor));
						}
					}
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (role != null && role.def.roleEffects != null)
					{
						foreach (RoleEffect roleEffect in role.def.roleEffects)
						{
							RoleEffect_PawnStatOffset roleEffect_PawnStatOffset;
							RoleEffect_PawnStatFactor roleEffect_PawnStatFactor;
							if ((roleEffect_PawnStatOffset = (roleEffect as RoleEffect_PawnStatOffset)) != null)
							{
								if (roleEffect_PawnStatOffset.statDef == this.stat)
								{
									stringBuilder.AppendLine(role.LabelCap + ": " + this.ValueToString(roleEffect_PawnStatOffset.modifier, false, ToStringNumberSense.Offset));
								}
							}
							else if ((roleEffect_PawnStatFactor = (roleEffect as RoleEffect_PawnStatFactor)) != null && roleEffect_PawnStatFactor.statDef == this.stat)
							{
								stringBuilder.AppendLine(role.LabelCap + ": " + this.ValueToString(roleEffect_PawnStatFactor.modifier, false, ToStringNumberSense.Factor));
							}
						}
					}
				}
				float statFactorFromList2 = pawn.ageTracker.CurLifeStage.statFactors.GetStatFactorFromList(this.stat);
				if (statFactorFromList2 != 1f)
				{
					stringBuilder.AppendLine("StatsReport_LifeStage".Translate() + " (" + pawn.ageTracker.CurLifeStage.label + "): " + statFactorFromList2.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
				}
			}
			if (req.StuffDef != null)
			{
				if (baseValueFor > 0f || this.stat.applyFactorsIfNegative)
				{
					float statFactorFromList3 = req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
					if (statFactorFromList3 != 1f)
					{
						stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statFactorFromList3.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					}
				}
				float statOffsetFromList2 = req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
				if (statOffsetFromList2 != 0f)
				{
					stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statOffsetFromList2.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset));
				}
			}
			CompAffectedByFacilities compAffectedByFacilities = req.Thing.TryGetComp<CompAffectedByFacilities>();
			if (compAffectedByFacilities != null)
			{
				compAffectedByFacilities.GetStatsExplanation(this.stat, stringBuilder);
			}
			if (this.stat.statFactors != null)
			{
				stringBuilder.AppendLine("StatsReport_OtherStats".Translate());
				for (int num3 = 0; num3 < this.stat.statFactors.Count; num3++)
				{
					StatDef statDef = this.stat.statFactors[num3];
					stringBuilder.AppendLine("    " + statDef.LabelCap + ": x" + statDef.Worker.GetValue(req, true).ToStringPercent());
				}
			}
			if (pawn != null)
			{
				if (pawn.skills != null)
				{
					if (this.stat.skillNeedFactors != null)
					{
						stringBuilder.AppendLine("StatsReport_Skills".Translate());
						for (int num4 = 0; num4 < this.stat.skillNeedFactors.Count; num4++)
						{
							SkillNeed skillNeed2 = this.stat.skillNeedFactors[num4];
							int level3 = pawn.skills.GetSkill(skillNeed2.skill).Level;
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"    " + skillNeed2.skill.LabelCap + " (",
								level3,
								"): x",
								skillNeed2.ValueFor(pawn).ToStringPercent()
							}));
						}
					}
				}
				else if (this.stat.noSkillFactor != 1f)
				{
					stringBuilder.AppendLine("StatsReport_Skills".Translate());
					stringBuilder.AppendLine("    " + "default".Translate().CapitalizeFirst() + " : x" + this.stat.noSkillFactor.ToStringPercent());
				}
				if (this.stat.capacityFactors != null)
				{
					stringBuilder.AppendLine("StatsReport_Health".CanTranslate() ? "StatsReport_Health".Translate() : "StatsReport_HealthFactors".Translate());
					if (this.stat.capacityFactors != null)
					{
						foreach (PawnCapacityFactor pawnCapacityFactor in from hfa in this.stat.capacityFactors
						orderby hfa.capacity.listOrder
						select hfa)
						{
							string text4 = pawnCapacityFactor.capacity.GetLabelFor(pawn).CapitalizeFirst();
							string text5 = pawnCapacityFactor.GetFactor(pawn.health.capacities.GetLevel(pawnCapacityFactor.capacity)).ToStringPercent();
							string text6 = "HealthFactorPercentImpact".Translate(pawnCapacityFactor.weight.ToStringPercent());
							if (pawnCapacityFactor.max < 999f)
							{
								text6 += ", " + "HealthFactorMaxImpact".Translate(pawnCapacityFactor.max.ToStringPercent());
							}
							if (pawnCapacityFactor.allowedDefect != 0f)
							{
								text6 += ", " + "HealthFactorAllowedDefect".Translate((1f - pawnCapacityFactor.allowedDefect).ToStringPercent());
							}
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"    ",
								text4,
								": x",
								text5,
								" (",
								text6,
								")"
							}));
						}
					}
				}
				if (pawn.Inspired)
				{
					float statOffsetFromList3 = pawn.InspirationDef.statOffsets.GetStatOffsetFromList(this.stat);
					if (statOffsetFromList3 != 0f)
					{
						stringBuilder.AppendLine("StatsReport_Inspiration".Translate(pawn.Inspiration.def.LabelCap) + ": " + this.ValueToString(statOffsetFromList3, false, ToStringNumberSense.Offset));
					}
					float statFactorFromList4 = pawn.InspirationDef.statFactors.GetStatFactorFromList(this.stat);
					if (statFactorFromList4 != 1f)
					{
						stringBuilder.AppendLine("StatsReport_Inspiration".Translate(pawn.Inspiration.def.LabelCap) + ": " + statFactorFromList4.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007FBE RID: 32702 RVA: 0x002D3200 File Offset: 0x002D1400
		public virtual void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
		{
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					this.stat.parts[i].TransformValue(req, ref val);
				}
			}
			if (applyPostProcess && this.stat.postProcessCurve != null)
			{
				val = this.stat.postProcessCurve.Evaluate(val);
			}
			if (applyPostProcess && this.stat.postProcessStatFactors != null)
			{
				for (int j = 0; j < this.stat.postProcessStatFactors.Count; j++)
				{
					val *= req.Thing.GetStatValue(this.stat.postProcessStatFactors[j], true);
				}
			}
			if (Find.Scenario != null)
			{
				val *= Find.Scenario.GetStatFactor(this.stat);
			}
			if (Mathf.Abs(val) > this.stat.roundToFiveOver)
			{
				val = Mathf.Round(val / 5f) * 5f;
			}
			if (this.stat.roundValue)
			{
				val = (float)Mathf.RoundToInt(val);
			}
			if (applyPostProcess)
			{
				val = Mathf.Clamp(val, this.stat.minValue, this.stat.maxValue);
			}
		}

		// Token: 0x06007FBF RID: 32703 RVA: 0x002D3340 File Offset: 0x002D1540
		public virtual string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					string text = this.stat.parts[i].ExplanationPart(req);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text);
					}
				}
			}
			if (this.stat.postProcessCurve != null)
			{
				float value = this.GetValue(req, false);
				float num = this.stat.postProcessCurve.Evaluate(value);
				if (!Mathf.Approximately(value, num))
				{
					string t = this.ValueToString(value, false, ToStringNumberSense.Absolute);
					string t2 = this.stat.ValueToString(num, numberSense, true);
					stringBuilder.AppendLine("StatsReport_PostProcessed".Translate() + ": " + t + " => " + t2);
				}
			}
			if (this.stat.postProcessStatFactors != null)
			{
				stringBuilder.AppendLine("StatsReport_OtherStats".Translate());
				for (int j = 0; j < this.stat.postProcessStatFactors.Count; j++)
				{
					StatDef statDef = this.stat.postProcessStatFactors[j];
					stringBuilder.AppendLine(string.Format("    {0}: x{1}", statDef.LabelCap, statDef.Worker.GetValue(req, true).ToStringPercent()));
				}
			}
			float statFactor = Find.Scenario.GetStatFactor(this.stat);
			if (statFactor != 1f)
			{
				stringBuilder.AppendLine("StatsReport_ScenarioFactor".Translate() + ": " + statFactor.ToStringPercent());
			}
			stringBuilder.Append("StatsReport_FinalValue".Translate() + ": " + this.stat.ValueToString(finalVal, this.stat.toStringNumberSense, true));
			return stringBuilder.ToString();
		}

		// Token: 0x06007FC0 RID: 32704 RVA: 0x002D353C File Offset: 0x002D173C
		public string GetExplanationFull(StatRequest req, ToStringNumberSense numberSense, float value)
		{
			if (this.IsDisabledFor(req.Thing))
			{
				return "StatsReport_PermanentlyDisabled".Translate();
			}
			string text = this.stat.Worker.GetExplanationUnfinalized(req, numberSense).TrimEndNewlines();
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			return text + this.stat.Worker.GetExplanationFinalizePart(req, numberSense, value);
		}

		// Token: 0x06007FC1 RID: 32705 RVA: 0x002D35B0 File Offset: 0x002D17B0
		public virtual bool ShouldShowFor(StatRequest req)
		{
			if (this.stat.alwaysHide)
			{
				return false;
			}
			Def def = req.Def;
			if (!this.stat.showIfUndefined && !req.StatBases.StatListContains(this.stat))
			{
				return false;
			}
			if (!this.stat.CanShowWithLoadedMods())
			{
				return false;
			}
			Pawn pawn;
			if ((pawn = (req.Thing as Pawn)) != null)
			{
				if (pawn.health != null && !this.stat.showIfHediffsPresent.NullOrEmpty<HediffDef>())
				{
					for (int i = 0; i < this.stat.showIfHediffsPresent.Count; i++)
					{
						if (!pawn.health.hediffSet.HasHediff(this.stat.showIfHediffsPresent[i], false))
						{
							return false;
						}
					}
				}
				if (this.stat.showOnSlavesOnly && !pawn.IsSlave)
				{
					return false;
				}
			}
			if (this.stat == StatDefOf.MaxHitPoints && req.HasThing)
			{
				return false;
			}
			if (!this.stat.showOnUntradeables && !StatWorker.DisplayTradeStats(req))
			{
				return false;
			}
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				if (thingDef.category == ThingCategory.Pawn)
				{
					if (!this.stat.showOnPawns)
					{
						return false;
					}
					if (!this.stat.showOnHumanlikes && thingDef.race.Humanlike)
					{
						return false;
					}
					if (!this.stat.showOnNonWildManHumanlikes && thingDef.race.Humanlike)
					{
						Pawn pawn2 = req.Thing as Pawn;
						if (pawn2 == null || !pawn2.IsWildMan())
						{
							return false;
						}
					}
					if (!this.stat.showOnAnimals && thingDef.race.Animal)
					{
						return false;
					}
					if (!this.stat.showOnMechanoids && thingDef.race.IsMechanoid)
					{
						return false;
					}
				}
				if (!this.stat.showOnUnhaulables && !thingDef.EverHaulable && !thingDef.Minifiable)
				{
					return false;
				}
			}
			if (this.stat.category == StatCategoryDefOf.BasicsPawn || this.stat.category == StatCategoryDefOf.BasicsPawnImportant || this.stat.category == StatCategoryDefOf.PawnCombat)
			{
				return thingDef != null && thingDef.category == ThingCategory.Pawn;
			}
			if (this.stat.category == StatCategoryDefOf.PawnMisc || this.stat.category == StatCategoryDefOf.PawnSocial || this.stat.category == StatCategoryDefOf.PawnWork)
			{
				return thingDef != null && thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike;
			}
			if (this.stat.category == StatCategoryDefOf.Building)
			{
				if (thingDef == null)
				{
					return false;
				}
				if (this.stat == StatDefOf.DoorOpenSpeed)
				{
					return thingDef.IsDoor;
				}
				return (this.stat.showOnNonWorkTables || thingDef.IsWorkTable) && thingDef.category == ThingCategory.Building;
			}
			else
			{
				if (this.stat.category == StatCategoryDefOf.Apparel)
				{
					return thingDef != null && (thingDef.IsApparel || thingDef.category == ThingCategory.Pawn);
				}
				if (this.stat.category == StatCategoryDefOf.Weapon)
				{
					return thingDef != null && (thingDef.IsMeleeWeapon || thingDef.IsRangedWeapon);
				}
				if (this.stat.category == StatCategoryDefOf.BasicsNonPawn || this.stat.category == StatCategoryDefOf.BasicsNonPawnImportant)
				{
					return (thingDef == null || thingDef.category != ThingCategory.Pawn) && !req.ForAbility;
				}
				if (this.stat.category == StatCategoryDefOf.Terrain)
				{
					return def is TerrainDef;
				}
				if (req.ForAbility)
				{
					return this.stat.category == StatCategoryDefOf.Ability;
				}
				if (this.stat.category.displayAllByDefault)
				{
					return true;
				}
				Log.Error(string.Concat(new object[]
				{
					"Unhandled case: ",
					this.stat,
					", ",
					def
				}));
				return false;
			}
		}

		// Token: 0x06007FC2 RID: 32706 RVA: 0x002D397C File Offset: 0x002D1B7C
		public virtual bool IsDisabledFor(Thing thing)
		{
			if (this.stat.neverDisabled)
			{
				return false;
			}
			if (this.stat.skillNeedFactors.NullOrEmpty<SkillNeed>() && this.stat.skillNeedOffsets.NullOrEmpty<SkillNeed>() && this.stat.disableIfSkillDisabled == null)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.story != null)
			{
				if (this.stat.skillNeedFactors != null)
				{
					for (int i = 0; i < this.stat.skillNeedFactors.Count; i++)
					{
						if (pawn.skills.GetSkill(this.stat.skillNeedFactors[i].skill).TotallyDisabled)
						{
							return true;
						}
					}
				}
				if (this.stat.skillNeedOffsets != null)
				{
					for (int j = 0; j < this.stat.skillNeedOffsets.Count; j++)
					{
						if (pawn.skills.GetSkill(this.stat.skillNeedOffsets[j].skill).TotallyDisabled)
						{
							return true;
						}
					}
				}
				if (this.stat.disableIfSkillDisabled != null && pawn.skills.GetSkill(this.stat.disableIfSkillDisabled).TotallyDisabled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007FC3 RID: 32707 RVA: 0x002D3AB4 File Offset: 0x002D1CB4
		public virtual string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			return stat.ValueToString(value, numberSense, finalized);
		}

		// Token: 0x06007FC4 RID: 32708 RVA: 0x002D3AC0 File Offset: 0x002D1CC0
		private static string InfoTextLineFromGear(Thing gear, StatDef stat)
		{
			float f = StatWorker.StatOffsetFromGear(gear, stat);
			return "    " + gear.LabelCap + ": " + f.ToStringByStyle(stat.finalizeEquippedStatOffset ? stat.toStringStyle : stat.ToStringStyleUnfinalized, ToStringNumberSense.Offset);
		}

		// Token: 0x06007FC5 RID: 32709 RVA: 0x002D3B08 File Offset: 0x002D1D08
		public static float StatOffsetFromGear(Thing gear, StatDef stat)
		{
			float num = gear.def.equippedStatOffsets.GetStatOffsetFromList(stat);
			CompBladelinkWeapon compBladelinkWeapon = gear.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon != null)
			{
				List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
				for (int i = 0; i < traitsListForReading.Count; i++)
				{
					num += traitsListForReading[i].equippedStatOffsets.GetStatOffsetFromList(stat);
				}
			}
			if (Math.Abs(num) > 1E-45f && !stat.parts.NullOrEmpty<StatPart>())
			{
				foreach (StatPart statPart in stat.parts)
				{
					statPart.TransformValue(StatRequest.For(gear), ref num);
				}
			}
			return num;
		}

		// Token: 0x06007FC6 RID: 32710 RVA: 0x002D3BC8 File Offset: 0x002D1DC8
		private static IEnumerable<Thing> RelevantGear(Pawn pawn, StatDef stat)
		{
			if (pawn.apparel != null)
			{
				foreach (Apparel apparel in pawn.apparel.WornApparel)
				{
					if (StatWorker.GearAffectsStat(apparel.def, stat))
					{
						yield return apparel;
					}
				}
				List<Apparel>.Enumerator enumerator = default(List<Apparel>.Enumerator);
			}
			if (pawn.equipment != null)
			{
				foreach (ThingWithComps thingWithComps in pawn.equipment.AllEquipmentListForReading)
				{
					if (StatWorker.GearAffectsStat(thingWithComps.def, stat) || StatWorker.GearHasCompsThatAffectStat(thingWithComps, stat))
					{
						yield return thingWithComps;
					}
				}
				List<ThingWithComps>.Enumerator enumerator2 = default(List<ThingWithComps>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06007FC7 RID: 32711 RVA: 0x002D3BE0 File Offset: 0x002D1DE0
		private static bool GearAffectsStat(ThingDef gearDef, StatDef stat)
		{
			if (gearDef.equippedStatOffsets != null)
			{
				for (int i = 0; i < gearDef.equippedStatOffsets.Count; i++)
				{
					if (gearDef.equippedStatOffsets[i].stat == stat && gearDef.equippedStatOffsets[i].value != 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007FC8 RID: 32712 RVA: 0x002D3C3C File Offset: 0x002D1E3C
		private static bool GearHasCompsThatAffectStat(Thing gear, StatDef stat)
		{
			CompBladelinkWeapon compBladelinkWeapon = gear.TryGetComp<CompBladelinkWeapon>();
			if (compBladelinkWeapon == null)
			{
				return false;
			}
			List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
			for (int i = 0; i < traitsListForReading.Count; i++)
			{
				if (!traitsListForReading[i].equippedStatOffsets.NullOrEmpty<StatModifier>())
				{
					for (int j = 0; j < traitsListForReading[i].equippedStatOffsets.Count; j++)
					{
						StatModifier statModifier = traitsListForReading[i].equippedStatOffsets[j];
						if (statModifier.stat == stat && statModifier.value != 0f)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007FC9 RID: 32713 RVA: 0x002D3CCC File Offset: 0x002D1ECC
		protected float GetBaseValueFor(StatRequest request)
		{
			float result = this.stat.defaultBaseValue;
			if (request.StatBases != null)
			{
				for (int i = 0; i < request.StatBases.Count; i++)
				{
					if (request.StatBases[i].stat == this.stat)
					{
						result = request.StatBases[i].value;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06007FCA RID: 32714 RVA: 0x002D3D38 File Offset: 0x002D1F38
		public virtual string ValueToString(float val, bool finalized, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			if (!finalized)
			{
				string text = val.ToStringByStyle(this.stat.ToStringStyleUnfinalized, numberSense);
				if (numberSense != ToStringNumberSense.Factor && !this.stat.formatStringUnfinalized.NullOrEmpty())
				{
					text = string.Format(this.stat.formatStringUnfinalized, text);
				}
				return text;
			}
			string text2 = val.ToStringByStyle(this.stat.toStringStyle, numberSense);
			if (numberSense != ToStringNumberSense.Factor && !this.stat.formatString.NullOrEmpty())
			{
				text2 = string.Format(this.stat.formatString, text2);
			}
			return text2;
		}

		// Token: 0x06007FCB RID: 32715 RVA: 0x002D3DC1 File Offset: 0x002D1FC1
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			Pawn pawn = statRequest.Thing as Pawn;
			if (pawn != null)
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				int num3;
				for (int i = 0; i < hediffs.Count; i = num3 + 1)
				{
					HediffStage curStage = hediffs[i].CurStage;
					if (curStage != null)
					{
						float num = curStage.statOffsets.GetStatOffsetFromList(this.stat);
						if (num != 0f && curStage.statOffsetEffectMultiplier != null)
						{
							num *= pawn.GetStatValue(curStage.statOffsetEffectMultiplier, true);
						}
						float num2 = curStage.statFactors.GetStatFactorFromList(this.stat);
						if (Math.Abs(num2 - 1f) > 1E-45f && curStage.statFactorEffectMultiplier != null)
						{
							num2 = StatWorker.ScaleFactor(num2, pawn.GetStatValue(curStage.statFactorEffectMultiplier, true));
						}
						if (Mathf.Abs(num) > 0f || Math.Abs(num2 - 1f) > 1E-45f)
						{
							yield return new Dialog_InfoCard.Hyperlink(hediffs[i].def, -1);
						}
					}
					num3 = i;
				}
				foreach (Thing thing in StatWorker.RelevantGear(pawn, this.stat))
				{
					yield return new Dialog_InfoCard.Hyperlink(thing, -1);
				}
				IEnumerator<Thing> enumerator = null;
				if (this.stat.parts != null)
				{
					foreach (StatPart statPart in this.stat.parts)
					{
						foreach (Dialog_InfoCard.Hyperlink hyperlink in statPart.GetInfoCardHyperlinks(statRequest))
						{
							yield return hyperlink;
						}
						IEnumerator<Dialog_InfoCard.Hyperlink> enumerator3 = null;
					}
					List<StatPart>.Enumerator enumerator2 = default(List<StatPart>.Enumerator);
				}
				hediffs = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06007FCC RID: 32716 RVA: 0x002D3DD8 File Offset: 0x002D1FD8
		public static float ScaleFactor(float factor, float scale)
		{
			return 1f - (1f - factor) * scale;
		}

		// Token: 0x06007FCD RID: 32717 RVA: 0x002D3DEC File Offset: 0x002D1FEC
		private static bool DisplayTradeStats(StatRequest req)
		{
			ThingDef thingDef;
			return (thingDef = (req.Def as ThingDef)) != null && (!req.HasThing || !CompBiocodable.IsBiocoded(req.Thing)) && ((thingDef.category == ThingCategory.Building && thingDef.Minifiable) || TradeUtility.EverPlayerSellable(thingDef) || (thingDef.tradeability.TraderCanSell() && (thingDef.category == ThingCategory.Item || thingDef.category == ThingCategory.Pawn)));
		}

		// Token: 0x04004FA6 RID: 20390
		protected StatDef stat;
	}
}
