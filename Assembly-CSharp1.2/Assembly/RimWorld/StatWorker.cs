using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D56 RID: 7510
	public class StatWorker
	{
		// Token: 0x0600A32B RID: 41771 RVA: 0x0006C561 File Offset: 0x0006A761
		public void InitSetStat(StatDef newStat)
		{
			this.stat = newStat;
		}

		// Token: 0x0600A32C RID: 41772 RVA: 0x0006C56A File Offset: 0x0006A76A
		public float GetValue(Thing thing, bool applyPostProcess = true)
		{
			return this.GetValue(StatRequest.For(thing), true);
		}

		// Token: 0x0600A32D RID: 41773 RVA: 0x0006C579 File Offset: 0x0006A779
		public float GetValue(Thing thing, Pawn pawn, bool applyPostProcess = true)
		{
			return this.GetValue(StatRequest.For(thing, pawn), true);
		}

		// Token: 0x0600A32E RID: 41774 RVA: 0x002F6F88 File Offset: 0x002F5188
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
					Log.Error("MinifiedThing's inner thing is null.", false);
				}
			}
			float valueUnfinalized = this.GetValueUnfinalized(req, applyPostProcess);
			this.FinalizeValue(req, ref valueUnfinalized, applyPostProcess);
			return valueUnfinalized;
		}

		// Token: 0x0600A32F RID: 41775 RVA: 0x0006C589 File Offset: 0x0006A789
		public float GetValueAbstract(BuildableDef def, ThingDef stuffDef = null)
		{
			return this.GetValue(StatRequest.For(def, stuffDef, QualityCategory.Normal), true);
		}

		// Token: 0x0600A330 RID: 41776 RVA: 0x0006C59A File Offset: 0x0006A79A
		public float GetValueAbstract(AbilityDef def)
		{
			return this.GetValue(StatRequest.For(def), true);
		}

		// Token: 0x0600A331 RID: 41777 RVA: 0x002F6FEC File Offset: 0x002F51EC
		public virtual float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (!this.stat.supressDisabledError && Prefs.DevMode && this.IsDisabledFor(req.Thing))
			{
				Log.ErrorOnce(string.Format("Attempted to calculate value for disabled stat {0}; this is meant as a consistency check, either set the stat to neverDisabled or ensure this pawn cannot accidentally use this stat (thing={1})", this.stat, req.Thing.ToStringSafe<Thing>()), 75193282 + (int)this.stat.index, false);
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
				if (pawn.apparel != null)
				{
					for (int m = 0; m < pawn.apparel.WornApparel.Count; m++)
					{
						num += StatWorker.StatOffsetFromGear(pawn.apparel.WornApparel[m], this.stat);
					}
				}
				if (pawn.equipment != null && pawn.equipment.Primary != null)
				{
					num += StatWorker.StatOffsetFromGear(pawn.equipment.Primary, this.stat);
				}
				if (pawn.story != null)
				{
					for (int n = 0; n < pawn.story.traits.allTraits.Count; n++)
					{
						num *= pawn.story.traits.allTraits[n].MultiplierOfStat(this.stat);
					}
				}
				for (int num3 = 0; num3 < hediffs.Count; num3++)
				{
					HediffStage curStage2 = hediffs[num3].CurStage;
					if (curStage2 != null)
					{
						float num4 = curStage2.statFactors.GetStatFactorFromList(this.stat);
						if (Math.Abs(num4 - 1f) > 1E-45f && curStage2.statFactorEffectMultiplier != null)
						{
							num4 = StatWorker.ScaleFactor(num4, pawn.GetStatValue(curStage2.statFactorEffectMultiplier, true));
						}
						num *= num4;
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
			if (req.ForAbility && this.stat.statFactors != null)
			{
				for (int num5 = 0; num5 < this.stat.statFactors.Count; num5++)
				{
					num *= req.AbilityDef.statBases.GetStatValueFromList(this.stat.statFactors[num5], 1f);
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
					for (int num6 = 0; num6 < this.stat.statFactors.Count; num6++)
					{
						num *= req.Thing.GetStatValue(this.stat.statFactors[num6], true);
					}
				}
				if (pawn != null)
				{
					if (pawn.skills != null)
					{
						if (this.stat.skillNeedFactors != null)
						{
							for (int num7 = 0; num7 < this.stat.skillNeedFactors.Count; num7++)
							{
								num *= this.stat.skillNeedFactors[num7].ValueFor(pawn);
							}
						}
					}
					else
					{
						num *= this.stat.noSkillFactor;
					}
					if (this.stat.capacityFactors != null)
					{
						for (int num8 = 0; num8 < this.stat.capacityFactors.Count; num8++)
						{
							PawnCapacityFactor pawnCapacityFactor = this.stat.capacityFactors[num8];
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

		// Token: 0x0600A332 RID: 41778 RVA: 0x002F7590 File Offset: 0x002F5790
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
				float statFactorFromList = pawn.ageTracker.CurLifeStage.statFactors.GetStatFactorFromList(this.stat);
				if (statFactorFromList != 1f)
				{
					stringBuilder.AppendLine("StatsReport_LifeStage".Translate() + " (" + pawn.ageTracker.CurLifeStage.label + "): " + statFactorFromList.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
				}
			}
			if (req.StuffDef != null)
			{
				if (baseValueFor > 0f || this.stat.applyFactorsIfNegative)
				{
					float statFactorFromList2 = req.StuffDef.stuffProps.statFactors.GetStatFactorFromList(this.stat);
					if (statFactorFromList2 != 1f)
					{
						stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statFactorFromList2.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					}
				}
				float statOffsetFromList = req.StuffDef.stuffProps.statOffsets.GetStatOffsetFromList(this.stat);
				if (statOffsetFromList != 0f)
				{
					stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + req.StuffDef.LabelCap + "): " + statOffsetFromList.ToStringByStyle(this.stat.toStringStyle, ToStringNumberSense.Offset));
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
				for (int n = 0; n < this.stat.statFactors.Count; n++)
				{
					StatDef statDef = this.stat.statFactors[n];
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
						for (int num3 = 0; num3 < this.stat.skillNeedFactors.Count; num3++)
						{
							SkillNeed skillNeed2 = this.stat.skillNeedFactors[num3];
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
					float statOffsetFromList2 = pawn.InspirationDef.statOffsets.GetStatOffsetFromList(this.stat);
					if (statOffsetFromList2 != 0f)
					{
						stringBuilder.AppendLine("StatsReport_Inspiration".Translate(pawn.Inspiration.def.LabelCap) + ": " + this.ValueToString(statOffsetFromList2, false, ToStringNumberSense.Offset));
					}
					float statFactorFromList3 = pawn.InspirationDef.statFactors.GetStatFactorFromList(this.stat);
					if (statFactorFromList3 != 1f)
					{
						stringBuilder.AppendLine("StatsReport_Inspiration".Translate(pawn.Inspiration.def.LabelCap) + ": " + statFactorFromList3.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A333 RID: 41779 RVA: 0x002F8498 File Offset: 0x002F6698
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

		// Token: 0x0600A334 RID: 41780 RVA: 0x002F85D8 File Offset: 0x002F67D8
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

		// Token: 0x0600A335 RID: 41781 RVA: 0x002F87D4 File Offset: 0x002F69D4
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

		// Token: 0x0600A336 RID: 41782 RVA: 0x002F8848 File Offset: 0x002F6A48
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
			if ((pawn = (req.Thing as Pawn)) != null && pawn.health != null && !this.stat.showIfHediffsPresent.NullOrEmpty<HediffDef>())
			{
				for (int i = 0; i < this.stat.showIfHediffsPresent.Count; i++)
				{
					if (!pawn.health.hediffSet.HasHediff(this.stat.showIfHediffsPresent[i], false))
					{
						return false;
					}
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
				}), false);
				return false;
			}
		}

		// Token: 0x0600A337 RID: 41783 RVA: 0x002F8BE0 File Offset: 0x002F6DE0
		public virtual bool IsDisabledFor(Thing thing)
		{
			if (this.stat.neverDisabled || (this.stat.skillNeedFactors.NullOrEmpty<SkillNeed>() && this.stat.skillNeedOffsets.NullOrEmpty<SkillNeed>()))
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
			}
			return false;
		}

		// Token: 0x0600A338 RID: 41784 RVA: 0x0006C5A9 File Offset: 0x0006A7A9
		public virtual string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			return stat.ValueToString(value, numberSense, finalized);
		}

		// Token: 0x0600A339 RID: 41785 RVA: 0x002F8CE0 File Offset: 0x002F6EE0
		private static string InfoTextLineFromGear(Thing gear, StatDef stat)
		{
			float f = StatWorker.StatOffsetFromGear(gear, stat);
			return "    " + gear.LabelCap + ": " + f.ToStringByStyle(stat.finalizeEquippedStatOffset ? stat.toStringStyle : stat.ToStringStyleUnfinalized, ToStringNumberSense.Offset);
		}

		// Token: 0x0600A33A RID: 41786 RVA: 0x002F8D28 File Offset: 0x002F6F28
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

		// Token: 0x0600A33B RID: 41787 RVA: 0x0006C5B5 File Offset: 0x0006A7B5
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

		// Token: 0x0600A33C RID: 41788 RVA: 0x002F8DE8 File Offset: 0x002F6FE8
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

		// Token: 0x0600A33D RID: 41789 RVA: 0x002F8E44 File Offset: 0x002F7044
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

		// Token: 0x0600A33E RID: 41790 RVA: 0x002F8ED4 File Offset: 0x002F70D4
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

		// Token: 0x0600A33F RID: 41791 RVA: 0x002F8F40 File Offset: 0x002F7140
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

		// Token: 0x0600A340 RID: 41792 RVA: 0x0006C5CC File Offset: 0x0006A7CC
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

		// Token: 0x0600A341 RID: 41793 RVA: 0x0006C5E3 File Offset: 0x0006A7E3
		public static float ScaleFactor(float factor, float scale)
		{
			return 1f - (1f - factor) * scale;
		}

		// Token: 0x0600A342 RID: 41794 RVA: 0x002F8FCC File Offset: 0x002F71CC
		private static bool DisplayTradeStats(StatRequest req)
		{
			ThingDef thingDef;
			return (thingDef = (req.Def as ThingDef)) != null && (!req.HasThing || !EquipmentUtility.IsBiocoded(req.Thing)) && ((thingDef.category == ThingCategory.Building && thingDef.Minifiable) || TradeUtility.EverPlayerSellable(thingDef) || (thingDef.tradeability.TraderCanSell() && (thingDef.category == ThingCategory.Item || thingDef.category == ThingCategory.Pawn)));
		}

		// Token: 0x04006EB2 RID: 28338
		protected StatDef stat;
	}
}
