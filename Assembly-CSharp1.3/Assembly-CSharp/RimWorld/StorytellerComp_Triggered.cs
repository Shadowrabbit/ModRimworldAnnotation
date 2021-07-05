using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C56 RID: 3158
	public class StorytellerComp_Triggered : StorytellerComp
	{
		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x00185A6F File Offset: 0x00183C6F
		private StorytellerCompProperties_Triggered Props
		{
			get
			{
				return (StorytellerCompProperties_Triggered)this.props;
			}
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x00185A7C File Offset: 0x00183C7C
		public override void Notify_PawnEvent(Pawn p, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			if (!p.RaceProps.Humanlike || !p.IsColonist)
			{
				return;
			}
			if (ev == AdaptationEvent.Died || ev == AdaptationEvent.Kidnapped || ev == AdaptationEvent.LostBecauseMapClosed || ev == AdaptationEvent.Downed)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction)
				{
					if (pawn.RaceProps.Humanlike && !pawn.Downed)
					{
						return;
					}
				}
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.Props.incident.category, anyPlayerHomeMap);
					if (this.Props.incident.Worker.CanFireNow(parms))
					{
						QueuedIncident qi = new QueuedIncident(new FiringIncident(this.Props.incident, this, parms), Find.TickManager.TicksGame + this.Props.delayTicks, 0);
						Find.Storyteller.incidentQueue.Add(qi);
					}
				}
			}
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x00185B84 File Offset: 0x00183D84
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
