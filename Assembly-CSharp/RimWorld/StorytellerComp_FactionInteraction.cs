using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001229 RID: 4649
	public class StorytellerComp_FactionInteraction : StorytellerComp
	{
		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06006597 RID: 26007 RVA: 0x000457D7 File Offset: 0x000439D7
		private StorytellerCompProperties_FactionInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_FactionInteraction)this.props;
			}
		}

		// Token: 0x06006598 RID: 26008 RVA: 0x000457E4 File Offset: 0x000439E4
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (this.Props.minDanger != StoryDanger.None)
			{
				Map map = target as Map;
				if (map == null || map.dangerWatcher.DangerRating < this.Props.minDanger)
				{
					yield break;
				}
			}
			float num = StorytellerUtility.AllyIncidentFraction(this.Props.fullAlliesOnly);
			if (num <= 0f)
			{
				yield break;
			}
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), this.Props.minDaysPassed, 60f, 0f, this.Props.minSpacingDays, this.Props.baseIncidentsPerYear, this.Props.baseIncidentsPerYear, num);
			int num2;
			for (int i = 0; i < incCount; i = num2 + 1)
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms, false))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x06006599 RID: 26009 RVA: 0x000457FB File Offset: 0x000439FB
		public override string ToString()
		{
			return base.ToString() + " (" + this.Props.incident.defName + ")";
		}
	}
}
