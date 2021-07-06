using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122D RID: 4653
	public class StorytellerComp_OnOffCycle : StorytellerComp
	{
		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x060065AE RID: 26030 RVA: 0x000458C5 File Offset: 0x00043AC5
		protected StorytellerCompProperties_OnOffCycle Props
		{
			get
			{
				return (StorytellerCompProperties_OnOffCycle)this.props;
			}
		}

		// Token: 0x060065AF RID: 26031 RVA: 0x000458D2 File Offset: 0x00043AD2
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float num = 1f;
			if (this.Props.acceptFractionByDaysPassedCurve != null)
			{
				num *= this.Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
			}
			if (this.Props.acceptPercentFactorPerThreatPointsCurve != null)
			{
				num *= this.Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
			}
			if (this.Props.acceptPercentFactorPerProgressScoreCurve != null)
			{
				num *= this.Props.acceptPercentFactorPerProgressScoreCurve.Evaluate(StorytellerUtility.GetProgressScore(target));
			}
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf(this), this.Props.minDaysPassed, this.Props.onDays, this.Props.offDays, this.Props.minSpacingDays, this.Props.numIncidentsRange.min, this.Props.numIncidentsRange.max, num);
			int num2;
			for (int i = 0; i < incCount; i = num2 + 1)
			{
				FiringIncident firingIncident = this.GenerateIncident(target);
				if (firingIncident != null)
				{
					yield return firingIncident;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x060065B0 RID: 26032 RVA: 0x001F6E90 File Offset: 0x001F5090
		private FiringIncident GenerateIncident(IIncidentTarget target)
		{
			if (this.Props.IncidentCategory == null)
			{
				return null;
			}
			IncidentParms parms = this.GenerateParms(this.Props.IncidentCategory, target);
			IncidentDef def;
			if ((float)GenDate.DaysPassed < this.Props.forceRaidEnemyBeforeDaysPassed)
			{
				if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms, false))
				{
					return null;
				}
				def = IncidentDefOf.RaidEnemy;
			}
			else if (this.Props.incident != null)
			{
				if (!this.Props.incident.Worker.CanFireNow(parms, false))
				{
					return null;
				}
				def = this.Props.incident;
			}
			else if (!base.UsableIncidentsInCategory(this.Props.IncidentCategory, parms).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out def))
			{
				return null;
			}
			return new FiringIncident(def, this, null)
			{
				parms = parms
			};
		}

		// Token: 0x060065B1 RID: 26033 RVA: 0x001F6F60 File Offset: 0x001F5160
		public override string ToString()
		{
			if (this.Props.incident == null && this.Props.IncidentCategory == null)
			{
				return "";
			}
			return base.ToString() + " (" + ((this.Props.incident != null) ? this.Props.incident.defName : this.Props.IncidentCategory.defName) + ")";
		}
	}
}
