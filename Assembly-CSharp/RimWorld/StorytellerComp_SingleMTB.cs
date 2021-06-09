using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200123D RID: 4669
	public class StorytellerComp_SingleMTB : StorytellerComp
	{
		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x060065EF RID: 26095 RVA: 0x00045B01 File Offset: 0x00043D01
		private StorytellerCompProperties_SingleMTB Props
		{
			get
			{
				return (StorytellerCompProperties_SingleMTB)this.props;
			}
		}

		// Token: 0x060065F0 RID: 26096 RVA: 0x00045B0E File Offset: 0x00043D0E
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!this.Props.incident.TargetAllowed(target))
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms, false))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x060065F1 RID: 26097 RVA: 0x00045B25 File Offset: 0x00043D25
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
