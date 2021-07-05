using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C41 RID: 3137
	public class StorytellerComp_FactionInteraction : StorytellerComp
	{
		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06004991 RID: 18833 RVA: 0x001853BC File Offset: 0x001835BC
		private StorytellerCompProperties_FactionInteraction Props
		{
			get
			{
				return (StorytellerCompProperties_FactionInteraction)this.props;
			}
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x001853C9 File Offset: 0x001835C9
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			Map map = target as Map;
			if (this.Props.minDanger != StoryDanger.None && (map == null || map.dangerWatcher.DangerRating < this.Props.minDanger))
			{
				yield break;
			}
			if (this.Props.minWealth > 0f && (map == null || map.wealthWatcher.WealthTotal < this.Props.minWealth))
			{
				yield break;
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
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x001853E0 File Offset: 0x001835E0
		public override string ToString()
		{
			return base.ToString() + " (" + this.Props.incident.defName + ")";
		}
	}
}
