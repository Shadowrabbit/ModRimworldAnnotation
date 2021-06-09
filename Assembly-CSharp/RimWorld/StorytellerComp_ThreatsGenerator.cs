using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001244 RID: 4676
	public class StorytellerComp_ThreatsGenerator : StorytellerComp
	{
		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x0600660A RID: 26122 RVA: 0x00045C0D File Offset: 0x00043E0D
		protected StorytellerCompProperties_ThreatsGenerator Props
		{
			get
			{
				return (StorytellerCompProperties_ThreatsGenerator)this.props;
			}
		}

		// Token: 0x0600660B RID: 26123 RVA: 0x00045C1A File Offset: 0x00043E1A
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = target as Map;
			foreach (FiringIncident firingIncident in ThreatsGenerator.MakeIntervalIncidents(this.Props.parms, target, (map != null) ? map.generationTick : 0))
			{
				firingIncident.source = this;
				yield return firingIncident;
			}
			IEnumerator<FiringIncident> enumerator = null;
			yield break;
			yield break;
		}
	}
}
