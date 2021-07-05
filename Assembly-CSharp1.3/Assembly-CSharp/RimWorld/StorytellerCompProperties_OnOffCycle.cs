using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C42 RID: 3138
	public class StorytellerCompProperties_OnOffCycle : StorytellerCompProperties
	{
		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06004995 RID: 18837 RVA: 0x00185407 File Offset: 0x00183607
		public IncidentCategoryDef IncidentCategory
		{
			get
			{
				if (this.incident != null)
				{
					return this.incident.category;
				}
				return this.category;
			}
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00185423 File Offset: 0x00183623
		public StorytellerCompProperties_OnOffCycle()
		{
			this.compClass = typeof(StorytellerComp_OnOffCycle);
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00185446 File Offset: 0x00183646
		public override IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.incident != null && this.category != null)
			{
				yield return "incident and category should not both be defined";
			}
			if (this.onDays <= 0f)
			{
				yield return "onDays must be above zero";
			}
			if (this.numIncidentsRange.TrueMax <= 0f)
			{
				yield return "numIncidentRange not configured";
			}
			if (this.minSpacingDays * this.numIncidentsRange.TrueMax > this.onDays * 0.9f)
			{
				yield return "minSpacingDays too high compared to max number of incidents.";
			}
			yield break;
		}

		// Token: 0x04002CB6 RID: 11446
		public float onDays;

		// Token: 0x04002CB7 RID: 11447
		public float offDays;

		// Token: 0x04002CB8 RID: 11448
		public float minSpacingDays;

		// Token: 0x04002CB9 RID: 11449
		public FloatRange numIncidentsRange = FloatRange.Zero;

		// Token: 0x04002CBA RID: 11450
		public SimpleCurve acceptFractionByDaysPassedCurve;

		// Token: 0x04002CBB RID: 11451
		public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

		// Token: 0x04002CBC RID: 11452
		public SimpleCurve acceptPercentFactorPerProgressScoreCurve;

		// Token: 0x04002CBD RID: 11453
		public IncidentDef incident;

		// Token: 0x04002CBE RID: 11454
		private IncidentCategoryDef category;

		// Token: 0x04002CBF RID: 11455
		public float forceRaidEnemyBeforeDaysPassed;
	}
}
