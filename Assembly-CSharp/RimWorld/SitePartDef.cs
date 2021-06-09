using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDF RID: 4063
	public class SitePartDef : Def
	{
		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x0600589C RID: 22684 RVA: 0x0003D942 File Offset: 0x0003BB42
		public SitePartWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SitePartWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x0600589D RID: 22685 RVA: 0x001D0648 File Offset: 0x001CE848
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (!this.expandingIconTexture.NullOrEmpty())
					{
						this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
					}
					else
					{
						this.expandingIconTextureInt = BaseContent.BadTex;
					}
				}
				return this.expandingIconTextureInt;
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x0600589E RID: 22686 RVA: 0x001D0698 File Offset: 0x001CE898
		public List<GenStepDef> ExtraGenSteps
		{
			get
			{
				if (this.extraGenSteps == null)
				{
					this.extraGenSteps = new List<GenStepDef>();
					List<GenStepDef> allDefsListForReading = DefDatabase<GenStepDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].linkWithSite == this)
						{
							this.extraGenSteps.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.extraGenSteps;
			}
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x001D06F8 File Offset: 0x001CE8F8
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x001D0758 File Offset: 0x001CE958
		public bool FactionCanOwn(Faction faction)
		{
			return (!this.requiresFaction || faction != null) && (this.minFactionTechLevel == TechLevel.Undefined || (faction != null && faction.def.techLevel >= this.minFactionTechLevel)) && (faction == null || (!faction.IsPlayer && !faction.defeated && !faction.Hidden)) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x060058A1 RID: 22689 RVA: 0x001D07C0 File Offset: 0x001CE9C0
		public bool CompatibleWith(SitePartDef part)
		{
			for (int i = 0; i < part.excludesTags.Count; i++)
			{
				if (this.tags.Contains(part.excludesTags[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < this.excludesTags.Count; j++)
			{
				if (part.tags.Contains(this.excludesTags[j]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04003AD4 RID: 15060
		public ThingDef conditionCauserDef;

		// Token: 0x04003AD5 RID: 15061
		public float activeThreatDisturbanceFactor = 1f;

		// Token: 0x04003AD6 RID: 15062
		public bool defaultHidden;

		// Token: 0x04003AD7 RID: 15063
		public Type workerClass = typeof(SitePartWorker);

		// Token: 0x04003AD8 RID: 15064
		[NoTranslate]
		public string siteTexture;

		// Token: 0x04003AD9 RID: 15065
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x04003ADA RID: 15066
		public bool applyFactionColorToSiteTexture;

		// Token: 0x04003ADB RID: 15067
		public bool showFactionInInspectString;

		// Token: 0x04003ADC RID: 15068
		public bool requiresFaction;

		// Token: 0x04003ADD RID: 15069
		public TechLevel minFactionTechLevel;

		// Token: 0x04003ADE RID: 15070
		[MustTranslate]
		public string approachOrderString;

		// Token: 0x04003ADF RID: 15071
		[MustTranslate]
		public string approachingReportString;

		// Token: 0x04003AE0 RID: 15072
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04003AE1 RID: 15073
		[NoTranslate]
		public List<string> excludesTags = new List<string>();

		// Token: 0x04003AE2 RID: 15074
		[MustTranslate]
		public string arrivedLetter;

		// Token: 0x04003AE3 RID: 15075
		[MustTranslate]
		public string arrivedLetterLabelPart;

		// Token: 0x04003AE4 RID: 15076
		public List<HediffDef> arrivedLetterHediffHyperlinks;

		// Token: 0x04003AE5 RID: 15077
		public LetterDef arrivedLetterDef;

		// Token: 0x04003AE6 RID: 15078
		public bool wantsThreatPoints;

		// Token: 0x04003AE7 RID: 15079
		public float minThreatPoints;

		// Token: 0x04003AE8 RID: 15080
		public bool increasesPopulation;

		// Token: 0x04003AE9 RID: 15081
		public bool badEvenIfNoMap;

		// Token: 0x04003AEA RID: 15082
		public float forceExitAndRemoveMapCountdownDurationDays = 4f;

		// Token: 0x04003AEB RID: 15083
		public bool handlesWorldObjectTimeoutInspectString;

		// Token: 0x04003AEC RID: 15084
		public string mainPartAllThreatsLabel;

		// Token: 0x04003AED RID: 15085
		[Unsaved(false)]
		private SitePartWorker workerInt;

		// Token: 0x04003AEE RID: 15086
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x04003AEF RID: 15087
		[Unsaved(false)]
		private List<GenStepDef> extraGenSteps;
	}
}
