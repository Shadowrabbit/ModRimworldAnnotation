using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122B RID: 4651
	public class StorytellerCompProperties_OnOffCycle : StorytellerCompProperties
	{
		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x060065A3 RID: 26019 RVA: 0x0004584C File Offset: 0x00043A4C
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

		// Token: 0x060065A4 RID: 26020 RVA: 0x00045868 File Offset: 0x00043A68
		public StorytellerCompProperties_OnOffCycle()
		{
			this.compClass = typeof(StorytellerComp_OnOffCycle);
		}

		// Token: 0x060065A5 RID: 26021 RVA: 0x0004588B File Offset: 0x00043A8B
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

		// Token: 0x040043A7 RID: 17319
		public float onDays;

		// Token: 0x040043A8 RID: 17320
		public float offDays;

		// Token: 0x040043A9 RID: 17321
		public float minSpacingDays;

		// Token: 0x040043AA RID: 17322
		public FloatRange numIncidentsRange = FloatRange.Zero;

		// Token: 0x040043AB RID: 17323
		public SimpleCurve acceptFractionByDaysPassedCurve;

		// Token: 0x040043AC RID: 17324
		public SimpleCurve acceptPercentFactorPerThreatPointsCurve;

		// Token: 0x040043AD RID: 17325
		public SimpleCurve acceptPercentFactorPerProgressScoreCurve;

		// Token: 0x040043AE RID: 17326
		public IncidentDef incident;

		// Token: 0x040043AF RID: 17327
		private IncidentCategoryDef category;

		// Token: 0x040043B0 RID: 17328
		public float forceRaidEnemyBeforeDaysPassed;
	}
}
