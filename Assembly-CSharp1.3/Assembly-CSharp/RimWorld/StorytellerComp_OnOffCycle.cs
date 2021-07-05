using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C43 RID: 3139
	public class StorytellerComp_OnOffCycle : StorytellerComp
	{
		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06004998 RID: 18840 RVA: 0x00185456 File Offset: 0x00183656
		protected StorytellerCompProperties_OnOffCycle Props
		{
			get
			{
				return (StorytellerCompProperties_OnOffCycle)this.props;
			}
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00185463 File Offset: 0x00183663
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

		// Token: 0x0600499A RID: 18842 RVA: 0x0018547C File Offset: 0x0018367C
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
				if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms))
				{
					return null;
				}
				def = IncidentDefOf.RaidEnemy;
			}
			else if (this.Props.incident != null)
			{
				if (!this.Props.incident.Worker.CanFireNow(parms))
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

		// Token: 0x0600499B RID: 18843 RVA: 0x00185548 File Offset: 0x00183748
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
