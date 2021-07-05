using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EA5 RID: 3749
	public class Trait : IExposable
	{
		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x0600581C RID: 22556 RVA: 0x001DED3A File Offset: 0x001DCF3A
		public int Degree
		{
			get
			{
				return this.degree;
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x0600581D RID: 22557 RVA: 0x001DED42 File Offset: 0x001DCF42
		public TraitDegreeData CurrentData
		{
			get
			{
				return this.def.DataAtDegree(this.degree);
			}
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x0600581E RID: 22558 RVA: 0x001DED55 File Offset: 0x001DCF55
		public string Label
		{
			get
			{
				return this.CurrentData.GetLabelFor(this.pawn);
			}
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x0600581F RID: 22559 RVA: 0x001DED68 File Offset: 0x001DCF68
		public string LabelCap
		{
			get
			{
				return this.CurrentData.GetLabelCapFor(this.pawn);
			}
		}

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06005820 RID: 22560 RVA: 0x001DED7B File Offset: 0x001DCF7B
		public bool ScenForced
		{
			get
			{
				return this.scenForced;
			}
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x000033AC File Offset: 0x000015AC
		public Trait()
		{
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x001DED83 File Offset: 0x001DCF83
		public Trait(TraitDef def, int degree = 0, bool forced = false)
		{
			this.def = def;
			this.degree = degree;
			this.scenForced = forced;
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x001DEDA0 File Offset: 0x001DCFA0
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

		// Token: 0x06005824 RID: 22564 RVA: 0x001DEE10 File Offset: 0x001DD010
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

		// Token: 0x06005825 RID: 22565 RVA: 0x001DEE74 File Offset: 0x001DD074
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

		// Token: 0x06005826 RID: 22566 RVA: 0x001DEED8 File Offset: 0x001DD0D8
		public string TipString(Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TraitDegreeData currentData = this.CurrentData;
			stringBuilder.AppendLine(currentData.description.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true).Resolve());
			bool flag = this.CurrentData.skillGains.Count > 0;
			bool flag2 = this.GetPermaThoughts().Any<ThoughtDef>();
			bool flag3 = currentData.statOffsets != null;
			bool flag4 = currentData.statFactors != null;
			if (flag || flag2 || flag3 || flag4)
			{
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
					stringBuilder.AppendLine("EnablesMeditationFocusType".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + (from f in allowedMeditationFocusTypes
					select f.LabelCap.Resolve()).ToLineList("  - ", false));
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				List<IssueDef> affectedIssues = this.CurrentData.GetAffectedIssues(this.def);
				if (affectedIssues.Count != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("OverridesSomePrecepts".Translate().Colorize(ColoredText.TipSectionTitleColor) + ": " + (from x in affectedIssues
					select x.label).ToCommaList(false, false).CapitalizeFirst());
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

		// Token: 0x06005827 RID: 22567 RVA: 0x001DF30C File Offset: 0x001DD50C
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

		// Token: 0x06005828 RID: 22568 RVA: 0x001DF358 File Offset: 0x001DD558
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

		// Token: 0x06005829 RID: 22569 RVA: 0x001DF368 File Offset: 0x001DD568
		private bool AllowsWorkType(WorkTypeDef workDef)
		{
			return (this.def.disabledWorkTags & workDef.workTags) == WorkTags.None;
		}

		// Token: 0x0600582A RID: 22570 RVA: 0x001DF37F File Offset: 0x001DD57F
		public void Notify_MentalStateEndedOn(Pawn pawn, bool causedByMood)
		{
			if (causedByMood)
			{
				this.Notify_MentalStateEndedOn(pawn);
			}
		}

		// Token: 0x0600582B RID: 22571 RVA: 0x001DF38C File Offset: 0x001DD58C
		public void Notify_MentalStateEndedOn(Pawn pawn)
		{
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.mentalBreakInspirationGainSet.NullOrEmpty<InspirationDef>() || Rand.Value > currentData.mentalBreakInspirationGainChance)
			{
				return;
			}
			pawn.mindState.inspirationHandler.TryStartInspiration(currentData.mentalBreakInspirationGainSet.RandomElement<InspirationDef>(), currentData.mentalBreakInspirationGainReasonText, true);
		}

		// Token: 0x0600582C RID: 22572 RVA: 0x001DF3DE File Offset: 0x001DD5DE
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

		// Token: 0x040033DF RID: 13279
		public Pawn pawn;

		// Token: 0x040033E0 RID: 13280
		public TraitDef def;

		// Token: 0x040033E1 RID: 13281
		private int degree;

		// Token: 0x040033E2 RID: 13282
		private bool scenForced;
	}
}
