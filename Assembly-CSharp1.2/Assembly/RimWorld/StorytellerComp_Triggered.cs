using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001247 RID: 4679
	public class StorytellerComp_Triggered : StorytellerComp
	{
		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06006617 RID: 26135 RVA: 0x00045C97 File Offset: 0x00043E97
		private StorytellerCompProperties_Triggered Props
		{
			get
			{
				return (StorytellerCompProperties_Triggered)this.props;
			}
		}

		// Token: 0x06006618 RID: 26136 RVA: 0x001F7C40 File Offset: 0x001F5E40
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
					if (this.Props.incident.Worker.CanFireNow(parms, false))
					{
						QueuedIncident qi = new QueuedIncident(new FiringIncident(this.Props.incident, this, parms), Find.TickManager.TicksGame + this.Props.delayTicks, 0);
						Find.Storyteller.incidentQueue.Add(qi);
					}
				}
			}
		}

		// Token: 0x06006619 RID: 26137 RVA: 0x00045CA4 File Offset: 0x00043EA4
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}
	}
}
