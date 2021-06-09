using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156C RID: 5484
	public class Trait : IExposable
	{
		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x060076FB RID: 30459 RVA: 0x000504FB File Offset: 0x0004E6FB
		public int Degree
		{
			get
			{
				return this.degree;
			}
		}

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x060076FC RID: 30460 RVA: 0x00050503 File Offset: 0x0004E703
		public TraitDegreeData CurrentData
		{
			get
			{
				return this.def.DataAtDegree(this.degree);
			}
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x060076FD RID: 30461 RVA: 0x00050516 File Offset: 0x0004E716
		public string Label
		{
			get
			{
				return this.CurrentData.GetLabelFor(this.pawn);
			}
		}

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x060076FE RID: 30462 RVA: 0x00050529 File Offset: 0x0004E729
		public string LabelCap
		{
			get
			{
				return this.CurrentData.GetLabelCapFor(this.pawn);
			}
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x060076FF RID: 30463 RVA: 0x0005053C File Offset: 0x0004E73C
		public bool ScenForced
		{
			get
			{
				return this.scenForced;
			}
		}

		// Token: 0x06007700 RID: 30464 RVA: 0x00006B8B File Offset: 0x00004D8B
		public Trait()
		{
		}

		// Token: 0x06007701 RID: 30465 RVA: 0x00050544 File Offset: 0x0004E744
		public Trait(TraitDef def, int degree = 0, bool forced = false)
		{
			this.def = def;
			this.degree = degree;
			this.scenForced = forced;
		}

		// Token: 0x06007702 RID: 30466 RVA: 0x00243064 File Offset: 0x00241264
		public void ExposeData()
		{
			Scribe_Defs.Look<TraitDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
			Scribe_Values.Look<bool>(ref this.scenForced, "scenForced", false, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == null)
			{
				this.def = DefDatabase<TraitDef>.GetRandom();
				this.degree = PawnGenerator.RandomTraitDegree(this.def);
			}
		}

		// Token: 0x06007703 RID: 30467 RVA: 0x002430D4 File Offset: 0x002412D4
		public float OffsetOfStat(StatDef stat)
		{
			float num = 0f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statOffsets != null)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					if (currentData.statOffsets[i].stat == stat)
					{
						num += currentData.statOffsets[i].value;
					}
				}
			}
			return num;
		}

		// Token: 0x06007704 RID: 30468 RVA: 0x00243138 File Offset: 0x00241338
		public float MultiplierOfStat(StatDef stat)
		{
			float num = 1f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statFactors != null)
			{
				for (int i = 0; i < currentData.statFactors.Count; i++)
				{
					if (currentData.statFactors[i].stat == stat)
					{
						num *= currentData.statFactors[i].value;
					}
				}
			}
			return num;
		}

		// Token: 0x06007705 RID: 30469 RVA: 0x0024319C File Offset: 0x0024139C
		public string TipString(Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TraitDegreeData currentData = this.CurrentData;
			stringBuilder.Append(currentData.description.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true).Resolve());
			bool flag = this.CurrentData.skillGains.Count > 0;
			bool flag2 = this.GetPermaThoughts().Any<ThoughtDef>();
			bool flag3 = currentData.statOffsets != null;
			bool flag4 = currentData.statFactors != null;
			if (flag || flag2 || flag3 || flag4)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
			}
			if (flag)
			{
				foreach (KeyValuePair<SkillDef, int> keyValuePair in this.CurrentData.skillGains)
				{
					if (keyValuePair.Value != 0)
					{
						string value = "    " + keyValuePair.Key.skillLabel.CapitalizeFirst() + ":   " + keyValuePair.Value.ToString("+##;-##");
						stringBuilder.AppendLine(value);
					}
				}
			}
			if (flag2)
			{
				foreach (ThoughtDef thoughtDef in this.GetPermaThoughts())
				{
					stringBuilder.AppendLine("    " + "PermanentMoodEffect".Translate() + " " + thoughtDef.stages[0].baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset));
				}
			}
			if (flag3)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					StatModifier statModifier = currentData.statOffsets[i];
					string valueToStringAsOffset = statModifier.ValueToStringAsOffset;
					string value2 = "    " + statModifier.stat.LabelCap + " " + valueToStringAsOffset;
					stringBuilder.AppendLine(value2);
				}
			}
			if (flag4)
			{
				for (int j = 0; j < currentData.statFactors.Count; j++)
				{
					StatModifier statModifier2 = currentData.statFactors[j];
					string toStringAsFactor = statModifier2.ToStringAsFactor;
					string value3 = "    " + statModifier2.stat.LabelCap + " " + toStringAsFactor;
					stringBuilder.AppendLine(value3);
				}
			}
			if (currentData.hungerRateFactor != 1f)
			{
				string t = currentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor);
				string value4 = "    " + "HungerRate".Translate() + " " + t;
				stringBuilder.AppendLine(value4);
			}
			if (ModsConfig.RoyaltyActive)
			{
				List<MeditationFocusDef> allowedMeditationFocusTypes = this.CurrentData.allowedMeditationFocusTypes;
				if (!allowedMeditationFocusTypes.NullOrEmpty<MeditationFocusDef>())
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("EnablesMeditationFocusType".Translate() + ":\n" + (from f in allowedMeditationFocusTypes
					select f.LabelCap.RawText).ToLineList("  - ", false));
				}
			}
			if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '\n')
			{
				if (stringBuilder.Length > 1 && stringBuilder[stringBuilder.Length - 2] == '\r')
				{
					stringBuilder.Remove(stringBuilder.Length - 2, 2);
				}
				else
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007706 RID: 30470 RVA: 0x0024355C File Offset: 0x0024175C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Trait(",
				this.def.ToString(),
				"-",
				this.degree,
				")"
			});
		}

		// Token: 0x06007707 RID: 30471 RVA: 0x00050561 File Offset: 0x0004E761
		private IEnumerable<ThoughtDef> GetPermaThoughts()
		{
			TraitDegreeData degree = this.CurrentData;
			List<ThoughtDef> allThoughts = DefDatabase<ThoughtDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < allThoughts.Count; i = num + 1)
			{
				if (allThoughts[i].IsSituational && allThoughts[i].Worker is ThoughtWorker_AlwaysActive && allThoughts[i].requiredTraits != null && allThoughts[i].requiredTraits.Contains(this.def) && (!allThoughts[i].RequiresSpecificTraitsDegree || allThoughts[i].requiredTraitsDegree == degree.degree))
				{
					yield return allThoughts[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06007708 RID: 30472 RVA: 0x00050571 File Offset: 0x0004E771
		private bool AllowsWorkType(WorkTypeDef workDef)
		{
			return (this.def.disabledWorkTags & workDef.workTags) == WorkTags.None;
		}

		// Token: 0x06007709 RID: 30473 RVA: 0x00050588 File Offset: 0x0004E788
		public void Notify_MentalStateEndedOn(Pawn pawn, bool causedByMood)
		{
			if (causedByMood)
			{
				this.Notify_MentalStateEndedOn(pawn);
			}
		}

		// Token: 0x0600770A RID: 30474 RVA: 0x002435A8 File Offset: 0x002417A8
		public void Notify_MentalStateEndedOn(Pawn pawn)
		{
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.mentalBreakInspirationGainSet.NullOrEmpty<InspirationDef>() || Rand.Value > currentData.mentalBreakInspirationGainChance)
			{
				return;
			}
			pawn.mindState.inspirationHandler.TryStartInspiration_NewTemp(currentData.mentalBreakInspirationGainSet.RandomElement<InspirationDef>(), currentData.mentalBreakInspirationGainReasonText);
		}

		// Token: 0x0600770B RID: 30475 RVA: 0x00050594 File Offset: 0x0004E794
		public IEnumerable<WorkTypeDef> GetDisabledWorkTypes()
		{
			int num;
			for (int i = 0; i < this.def.disabledWorkTypes.Count; i = num + 1)
			{
				yield return this.def.disabledWorkTypes[i];
				num = i;
			}
			List<WorkTypeDef> workTypeDefList = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < workTypeDefList.Count; i = num + 1)
			{
				WorkTypeDef workTypeDef = workTypeDefList[i];
				if (!this.AllowsWorkType(workTypeDef))
				{
					yield return workTypeDef;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04004E6B RID: 20075
		public Pawn pawn;

		// Token: 0x04004E6C RID: 20076
		public TraitDef def;

		// Token: 0x04004E6D RID: 20077
		private int degree;

		// Token: 0x04004E6E RID: 20078
		private bool scenForced;
	}
}
