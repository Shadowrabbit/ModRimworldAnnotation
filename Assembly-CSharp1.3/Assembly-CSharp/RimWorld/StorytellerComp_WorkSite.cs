using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C58 RID: 3160
	public class StorytellerComp_WorkSite : StorytellerComp
	{
		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x060049CD RID: 18893 RVA: 0x00185BC4 File Offset: 0x00183DC4
		public StorytellerCompProperties_WorkSite Props
		{
			get
			{
				return (StorytellerCompProperties_WorkSite)this.props;
			}
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x00185BD1 File Offset: 0x00183DD1
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Find.TickManager.TicksGame < 600000)
			{
				yield break;
			}
			float num = QuestNode_Root_WorkSite.BestAppearanceFrequency();
			if (this.Props.incident == null || !this.Props.incident.TargetAllowed(target) || num <= 0f)
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(this.Props.baseMtbDays / num, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x00185BE8 File Offset: 0x00183DE8
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
