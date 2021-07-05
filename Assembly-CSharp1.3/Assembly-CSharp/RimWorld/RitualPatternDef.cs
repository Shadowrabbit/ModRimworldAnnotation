using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7D RID: 3965
	public class RitualPatternDef : Def
	{
		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06005DF1 RID: 24049 RVA: 0x00203DE4 File Offset: 0x00201FE4
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					if (this.iconPathOverride != null)
					{
						this.icon = ContentFinder<Texture2D>.Get(this.iconPathOverride, true);
					}
					else
					{
						this.icon = null;
					}
				}
				return this.icon;
			}
		}

		// Token: 0x06005DF2 RID: 24050 RVA: 0x00203E20 File Offset: 0x00202020
		public void Fill(Precept_Ritual ritual)
		{
			ritual.nameMaker = this.nameMaker;
			if (!this.ritualObligationTriggers.NullOrEmpty<RitualObligationTriggerProperties>())
			{
				ritual.obligationTriggers.Clear();
				foreach (RitualObligationTriggerProperties ritualObligationTriggerProperties in this.ritualObligationTriggers)
				{
					RitualObligationTrigger instance = ritualObligationTriggerProperties.GetInstance(ritual);
					ritual.obligationTriggers.Add(instance);
					instance.Init(ritualObligationTriggerProperties);
				}
			}
			if (this.ritualOutcomeEffect != null)
			{
				ritual.outcomeEffect = this.ritualOutcomeEffect.GetInstance();
			}
			Precept_Ritual ritual2 = ritual;
			RitualObligationTargetFilterDef ritualObligationTargetFilterDef = this.ritualObligationTargetFilter;
			ritual2.obligationTargetFilter = ((ritualObligationTargetFilterDef != null) ? ritualObligationTargetFilterDef.GetInstance() : null);
			if (ritual.obligationTargetFilter != null)
			{
				ritual.obligationTargetFilter.parent = ritual;
			}
			Precept_Ritual ritual3 = ritual;
			RitualTargetFilterDef ritualTargetFilterDef = this.ritualTargetFilter;
			ritual3.targetFilter = ((ritualTargetFilterDef != null) ? ritualTargetFilterDef.GetInstance() : null);
			ritual.behavior = this.ritualBehavior.GetInstance();
			ritual.ritualOnlyForIdeoMembers = this.ritualOnlyForIdeoMembers;
			ritual.ritualExpectedDesc = this.ritualExpectedDesc;
			ritual.ritualExpectedPostfix = this.ritualExpectedPostfix;
			ritual.descOverride = this.descOverride;
			ritual.shortDescOverride = this.shortDescOverride;
			ritual.iconPathOverride = this.iconPathOverride;
			ritual.patternGroupTag = this.patternGroupTag;
			ritual.minTechLevel = this.minTechLevel;
			ritual.maxTechLevel = this.maxTechLevel;
			ritual.playsIdeoMusic = this.playsIdeoMusic;
			ritual.ritualExplanation = this.ritualExplanation;
			ritual.canBeAnytime = this.canStartAnytime;
			ritual.mergeGizmosForObligations = this.mergeGizmosForObligations;
			if (this.alwaysStartAnytime)
			{
				ritual.isAnytime = true;
			}
			else if (this.canStartAnytime)
			{
				Precept_Ritual precept_Ritual = ritual.ideo.PreceptsListForReading.OfType<Precept_Ritual>().FirstOrDefault((Precept_Ritual p) => p != ritual && p.behavior != null && p.behavior.def == this.ritualBehavior);
				int num = ritual.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Count((Precept_Ritual r) => r != ritual && r.isAnytime && r.def.ritualPatternBase != null && r.def.ritualPatternBase.canStartAnytime);
				int num2 = ritual.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Count((Precept_Ritual r) => r != ritual && !r.isAnytime && r.def.ritualPatternBase != null && r.def.ritualPatternBase.canStartAnytime);
				if (precept_Ritual != null)
				{
					ritual.isAnytime = false;
					precept_Ritual.isAnytime = false;
				}
				else if (num != num2)
				{
					ritual.isAnytime = (num2 > num);
				}
				else
				{
					ritual.isAnytime = Rand.Bool;
				}
			}
			else
			{
				ritual.isAnytime = false;
			}
			if (ritual.SupportsAttachableOutcomeEffect)
			{
				ritual.attachableOutcomeEffect = (from d in DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs
				where base.<Fill>g__ValidateAttachedOutcome|0(d, false)
				select d).RandomElementWithFallback(null);
				if (ritual.attachableOutcomeEffect == null)
				{
					ritual.attachableOutcomeEffect = (from d in DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs
					where base.<Fill>g__ValidateAttachedOutcome|0(d, true)
					select d).RandomElementWithFallback(null);
				}
			}
			ritual.generatedAttachedReward = true;
			ritual.sourcePattern = this;
		}

		// Token: 0x06005DF3 RID: 24051 RVA: 0x002041B0 File Offset: 0x002023B0
		public bool CanFactionUse(FactionDef faction)
		{
			return faction == null || RitualPatternDef.CanUseWithTechLevel(faction.techLevel, this.minTechLevel, this.maxTechLevel);
		}

		// Token: 0x06005DF4 RID: 24052 RVA: 0x002041CE File Offset: 0x002023CE
		public static bool CanUseWithTechLevel(TechLevel level, TechLevel min, TechLevel max)
		{
			return min <= level && (max == TechLevel.Undefined || max >= level);
		}

		// Token: 0x0400363E RID: 13886
		public RulePackDef nameMaker;

		// Token: 0x0400363F RID: 13887
		public FloatRange ritualFreeStartIntervalDaysRange;

		// Token: 0x04003640 RID: 13888
		public List<RitualObligationTriggerProperties> ritualObligationTriggers;

		// Token: 0x04003641 RID: 13889
		public RitualObligationTargetFilterDef ritualObligationTargetFilter;

		// Token: 0x04003642 RID: 13890
		public RitualTargetFilterDef ritualTargetFilter;

		// Token: 0x04003643 RID: 13891
		public RitualBehaviorDef ritualBehavior;

		// Token: 0x04003644 RID: 13892
		public RitualOutcomeEffectDef ritualOutcomeEffect;

		// Token: 0x04003645 RID: 13893
		public bool ritualOnlyForIdeoMembers = true;

		// Token: 0x04003646 RID: 13894
		public bool canStartAnytime;

		// Token: 0x04003647 RID: 13895
		public bool alwaysStartAnytime;

		// Token: 0x04003648 RID: 13896
		public bool playsIdeoMusic = true;

		// Token: 0x04003649 RID: 13897
		public bool ignoreConsumableBuildingRequirement;

		// Token: 0x0400364A RID: 13898
		public bool mergeGizmosForObligations;

		// Token: 0x0400364B RID: 13899
		public TechLevel minTechLevel;

		// Token: 0x0400364C RID: 13900
		public TechLevel maxTechLevel;

		// Token: 0x0400364D RID: 13901
		public int gracePeriodTicksSinceGameStarted;

		// Token: 0x0400364E RID: 13902
		[MustTranslate]
		public string ritualExpectedDesc;

		// Token: 0x0400364F RID: 13903
		[MustTranslate]
		public string ritualExpectedPostfix;

		// Token: 0x04003650 RID: 13904
		[MustTranslate]
		public string shortDescOverride;

		// Token: 0x04003651 RID: 13905
		[MustTranslate]
		public string descOverride;

		// Token: 0x04003652 RID: 13906
		[MustTranslate]
		public string ritualExplanation;

		// Token: 0x04003653 RID: 13907
		[NoTranslate]
		public string iconPathOverride;

		// Token: 0x04003654 RID: 13908
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04003655 RID: 13909
		[NoTranslate]
		public string patternGroupTag;

		// Token: 0x04003656 RID: 13910
		private Texture2D icon;
	}
}
