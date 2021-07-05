using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABE RID: 2750
	public class SitePartDef : Def
	{
		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x0600410F RID: 16655 RVA: 0x0015EA1D File Offset: 0x0015CC1D
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

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004110 RID: 16656 RVA: 0x0015EA50 File Offset: 0x0015CC50
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

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x0015EAA0 File Offset: 0x0015CCA0
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

		// Token: 0x06004112 RID: 16658 RVA: 0x0015EB00 File Offset: 0x0015CD00
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0015EB60 File Offset: 0x0015CD60
		public bool FactionCanOwn(Faction faction)
		{
			return (!this.requiresFaction || faction != null) && (this.minFactionTechLevel == TechLevel.Undefined || (faction != null && faction.def.techLevel >= this.minFactionTechLevel)) && (faction == null || (!faction.IsPlayer && !faction.defeated && !faction.Hidden)) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0015EBC8 File Offset: 0x0015CDC8
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

		// Token: 0x04002688 RID: 9864
		public ThingDef conditionCauserDef;

		// Token: 0x04002689 RID: 9865
		public float activeThreatDisturbanceFactor = 1f;

		// Token: 0x0400268A RID: 9866
		public bool defaultHidden;

		// Token: 0x0400268B RID: 9867
		public Type workerClass = typeof(SitePartWorker);

		// Token: 0x0400268C RID: 9868
		[NoTranslate]
		public string siteTexture;

		// Token: 0x0400268D RID: 9869
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x0400268E RID: 9870
		public bool applyFactionColorToSiteTexture;

		// Token: 0x0400268F RID: 9871
		public bool showFactionInInspectString;

		// Token: 0x04002690 RID: 9872
		public bool requiresFaction;

		// Token: 0x04002691 RID: 9873
		public bool disallowsAutomaticDetectionTimerStart;

		// Token: 0x04002692 RID: 9874
		public TechLevel minFactionTechLevel;

		// Token: 0x04002693 RID: 9875
		[MustTranslate]
		public string approachOrderString;

		// Token: 0x04002694 RID: 9876
		[MustTranslate]
		public string approachingReportString;

		// Token: 0x04002695 RID: 9877
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04002696 RID: 9878
		[NoTranslate]
		public List<string> excludesTags = new List<string>();

		// Token: 0x04002697 RID: 9879
		[MustTranslate]
		public string arrivedLetter;

		// Token: 0x04002698 RID: 9880
		[MustTranslate]
		public string arrivedLetterLabelPart;

		// Token: 0x04002699 RID: 9881
		public List<HediffDef> arrivedLetterHediffHyperlinks;

		// Token: 0x0400269A RID: 9882
		public LetterDef arrivedLetterDef;

		// Token: 0x0400269B RID: 9883
		public bool wantsThreatPoints;

		// Token: 0x0400269C RID: 9884
		public float minThreatPoints;

		// Token: 0x0400269D RID: 9885
		public bool increasesPopulation;

		// Token: 0x0400269E RID: 9886
		public bool badEvenIfNoMap;

		// Token: 0x0400269F RID: 9887
		public float forceExitAndRemoveMapCountdownDurationDays = 4f;

		// Token: 0x040026A0 RID: 9888
		public bool handlesWorldObjectTimeoutInspectString;

		// Token: 0x040026A1 RID: 9889
		public string mainPartAllThreatsLabel;

		// Token: 0x040026A2 RID: 9890
		public IntVec3? minMapSize;

		// Token: 0x040026A3 RID: 9891
		public List<SitePartDef.WorkSiteLootThing> lootTable;

		// Token: 0x040026A4 RID: 9892
		public float selectionWeight;

		// Token: 0x040026A5 RID: 9893
		[Unsaved(false)]
		private SitePartWorker workerInt;

		// Token: 0x040026A6 RID: 9894
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x040026A7 RID: 9895
		[Unsaved(false)]
		private List<GenStepDef> extraGenSteps;

		// Token: 0x0200202D RID: 8237
		public class WorkSiteLootThing
		{
			// Token: 0x04007B58 RID: 31576
			public ThingDef thing;

			// Token: 0x04007B59 RID: 31577
			public float weight;
		}
	}
}
