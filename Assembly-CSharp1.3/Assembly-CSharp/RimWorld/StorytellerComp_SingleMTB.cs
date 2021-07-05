using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4F RID: 3151
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060049B9 RID: 18873 RVA: 0x00185973 File Offset: 0x00183B73
		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)this.props;
			}
		}

		// Token: 0x060049BA RID: 18874 RVA: 0x00185980 File Offset: 0x00183B80
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (this.Props.incident == null || !this.Props.incident.TargetAllowed(target))
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x00185997 File Offset: 0x00183B97
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
