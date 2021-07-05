using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFF RID: 3839
	public class RitualAttachableOutcomeEffectDef : Def
	{
		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x06005BAD RID: 23469 RVA: 0x001FB11C File Offset: 0x001F931C
		public RitualAttachableOutcomeEffectWorker Worker
		{
			get
			{
				if (this.workerInstance == null)
				{
					this.workerInstance = (RitualAttachableOutcomeEffectWorker)Activator.CreateInstance(this.workerClass);
					this.workerInstance.def = this;
				}
				return this.workerInstance;
			}
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x06005BAE RID: 23470 RVA: 0x001FB14E File Offset: 0x001F934E
		public bool AppliesToAllOutcomes
		{
			get
			{
				return !this.onlyBestOutcome && !this.onlyPositiveOutcomes && !this.onlyNegativeOutcomes && !this.onlyWorstOutcome;
			}
		}

		// Token: 0x06005BAF RID: 23471 RVA: 0x001FB174 File Offset: 0x001F9374
		public bool AppliesToSeveralOutcomes(Precept_Ritual ritual)
		{
			bool flag = false;
			foreach (OutcomeChance outcomeChance in ritual.outcomeEffect.def.outcomeChances)
			{
				if (this.AppliesToOutcome(ritual.outcomeEffect.def, outcomeChance))
				{
					if (flag)
					{
						return true;
					}
					flag = true;
				}
			}
			return false;
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06005BB0 RID: 23472 RVA: 0x001FB1EC File Offset: 0x001F93EC
		private string RequiredMemeList
		{
			get
			{
				if (this.requiredMemeList == null && !this.requiredMemeAny.NullOrEmpty<MemeDef>())
				{
					this.requiredMemeList = (from m in this.requiredMemeAny
					select m.label.ResolveTags()).ToCommaList(false, false);
				}
				return this.requiredMemeList;
			}
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x001FB24C File Offset: 0x001F944C
		public AcceptanceReport CanAttachToRitual(Precept_Ritual ritual)
		{
			if (!this.requiredMemeAny.NullOrEmpty<MemeDef>() && !ritual.ideo.memes.SharesElementWith(this.requiredMemeAny))
			{
				return "RitualAttachedRewardRequiredMemeAny".Translate() + ": " + this.RequiredMemeList.CapitalizeFirst();
			}
			if ((!this.allowedRituals.NullOrEmpty<RitualPatternDef>() && !this.allowedRituals.Contains(ritual.sourcePattern)) || (!this.disallowedRituals.NullOrEmpty<RitualPatternDef>() && this.disallowedRituals.Contains(ritual.sourcePattern)))
			{
				return "RitualAttachedRewardCantUseWithRitual".Translate(this.LabelCap, ritual.LabelCap);
			}
			if (this.requiredFaction != null && Find.FactionManager.FirstFactionOfDef(this.requiredFaction) == null)
			{
				return "RitualAttachedRewardRequiredFaction".Translate(this.requiredFaction.LabelCap);
			}
			return true;
		}

		// Token: 0x06005BB2 RID: 23474 RVA: 0x001FB350 File Offset: 0x001F9550
		public bool AppliesToOutcome(RitualOutcomeEffectDef effectDef, OutcomeChance outcomeChance)
		{
			return ModLister.CheckIdeology("Attachable ritual reward") && (!this.onlyPositiveOutcomes || outcomeChance.Positive) && (!this.onlyNegativeOutcomes || !outcomeChance.Positive) && (!this.onlyBestOutcome || outcomeChance == effectDef.BestOutcome) && (!this.onlyWorstOutcome || outcomeChance == effectDef.WorstOutcome);
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x001FB3B8 File Offset: 0x001F95B8
		public string DescriptionForRitualValidated(Precept_Ritual ritual)
		{
			if (!this.AppliesToAllOutcomes)
			{
				return "RitualAttachedOutcomesInfo".Translate(ritual.shortDescOverride ?? ritual.def.label, this.AppliesToOutcomesListString(ritual.outcomeEffect.def)) + ", " + this.effectDesc;
			}
			return this.description.CapitalizeFirst();
		}

		// Token: 0x06005BB4 RID: 23476 RVA: 0x001FB430 File Offset: 0x001F9630
		public string DescriptionForRitualValidated(Precept_Ritual ritual, Map map)
		{
			string text = this.DescriptionForRitualValidated(ritual);
			AcceptanceReport report = this.Worker.CanApplyNow(ritual, map);
			if (!report)
			{
				text += " (" + "RitualAttachedOutcomeCantApply".Translate() + ": " + report.Reason + ")";
				text = text.Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}

		// Token: 0x06005BB5 RID: 23477 RVA: 0x001FB4A8 File Offset: 0x001F96A8
		public string TooltipForRitual(Precept_Ritual ritual)
		{
			string text = this.DescriptionForRitualValidated(ritual).ResolveTags();
			if (!this.requiredMemeAny.NullOrEmpty<MemeDef>())
			{
				text = text + "\n\n" + ("UnlockedByMeme".Translate().Resolve() + ":\n").Colorize(ColoredText.TipSectionTitleColor);
				foreach (MemeDef memeDef in this.requiredMemeAny)
				{
					text = text + "  - " + memeDef.LabelCap.Resolve() + "\n";
				}
			}
			return text;
		}

		// Token: 0x06005BB6 RID: 23478 RVA: 0x001FB564 File Offset: 0x001F9764
		public string AppliesToOutcomesListString(RitualOutcomeEffectDef effectDef)
		{
			this.appliesToOutcomeListTmp.Clear();
			foreach (OutcomeChance outcomeChance in effectDef.outcomeChances)
			{
				if (this.AppliesToOutcome(effectDef, outcomeChance))
				{
					this.appliesToOutcomeListTmp.Add(outcomeChance.label.ToLower());
				}
			}
			return this.appliesToOutcomeListTmp.ToCommaListOr(false);
		}

		// Token: 0x06005BB7 RID: 23479 RVA: 0x001FB5E8 File Offset: 0x001F97E8
		public string AppliesToOutcomesString(RitualOutcomeEffectDef effectDef)
		{
			if (this.onlyPositiveOutcomes)
			{
				return "RitualAttachedOutcomes_Positive".Translate();
			}
			if (this.onlyNegativeOutcomes)
			{
				return "RitualAttachedOutcomes_Negative".Translate();
			}
			if (this.onlyBestOutcome)
			{
				return effectDef.outcomeChances.MaxBy((OutcomeChance o) => o.positivityIndex).label;
			}
			if (this.onlyWorstOutcome)
			{
				return effectDef.outcomeChances.MinBy((OutcomeChance o) => o.positivityIndex).label;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06005BB8 RID: 23480 RVA: 0x001FB69A File Offset: 0x001F989A
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.workerClass == null || this.workerClass.IsAbstract || this.workerClass.IsInterface)
			{
				yield return "worker class must be instantiable: " + this.workerClass;
			}
			if ((this.onlyPositiveOutcomes && this.onlyNegativeOutcomes) || (this.onlyPositiveOutcomes && this.onlyWorstOutcome) || (this.onlyNegativeOutcomes && this.onlyBestOutcome))
			{
				yield return "conflicting outcome positivity configuration";
			}
			yield break;
		}

		// Token: 0x04003569 RID: 13673
		public Type workerClass = typeof(RitualAttachableOutcomeEffectWorker);

		// Token: 0x0400356A RID: 13674
		public List<MemeDef> requiredMemeAny;

		// Token: 0x0400356B RID: 13675
		public List<RitualPatternDef> allowedRituals;

		// Token: 0x0400356C RID: 13676
		public List<RitualPatternDef> disallowedRituals;

		// Token: 0x0400356D RID: 13677
		public FactionDef requiredFaction;

		// Token: 0x0400356E RID: 13678
		public bool onlyPositiveOutcomes = true;

		// Token: 0x0400356F RID: 13679
		public bool onlyBestOutcome;

		// Token: 0x04003570 RID: 13680
		public bool onlyNegativeOutcomes;

		// Token: 0x04003571 RID: 13681
		public bool onlyWorstOutcome;

		// Token: 0x04003572 RID: 13682
		[MustTranslate]
		public string letterInfoText;

		// Token: 0x04003573 RID: 13683
		[MustTranslate]
		public string effectDesc;

		// Token: 0x04003574 RID: 13684
		private RitualAttachableOutcomeEffectWorker workerInstance;

		// Token: 0x04003575 RID: 13685
		private string requiredMemeList;

		// Token: 0x04003576 RID: 13686
		private List<string> appliesToOutcomeListTmp = new List<string>();
	}
}
