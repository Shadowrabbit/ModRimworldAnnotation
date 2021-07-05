using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C54 RID: 3156
	public class StorytellerComp_ThreatsGenerator : StorytellerComp
	{
		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x060049C4 RID: 18884 RVA: 0x00185A2B File Offset: 0x00183C2B
		protected StorytellerCompProperties_ThreatsGenerator Props
		{
			get
			{
				return (StorytellerCompProperties_ThreatsGenerator)this.props;
			}
		}

		// Token: 0x060049C5 RID: 18885 RVA: 0x00185A38 File Offset: 0x00183C38
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
