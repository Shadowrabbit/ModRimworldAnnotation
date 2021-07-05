using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C46 RID: 3142
	public class StorytellerComp_RandomMain : StorytellerComp
	{
		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060049A2 RID: 18850 RVA: 0x0018564B File Offset: 0x0018384B
		protected StorytellerCompProperties_RandomMain Props
		{
			get
			{
				return (StorytellerCompProperties_RandomMain)this.props;
			}
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x00185658 File Offset: 0x00183858
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Rand.MTBEventOccurs(this.Props.mtbDays, 60000f, 1000f))
			{
				bool flag = target.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
				List<IncidentCategoryDef> list = new List<IncidentCategoryDef>();
				IncidentParms parms;
				IncidentDef incidentDef;
				for (;;)
				{
					IncidentCategoryDef incidentCategoryDef = this.ChooseRandomCategory(target, list);
					parms = this.GenerateParms(incidentCategoryDef, target);
					if (base.TrySelectRandomIncident(base.UsableIncidentsInCategory(incidentCategoryDef, parms), out incidentDef))
					{
						break;
					}
					list.Add(incidentCategoryDef);
					if (list.Count >= this.Props.categoryWeights.Count)
					{
						goto IL_F1;
					}
				}
				if (!this.Props.skipThreatBigIfRaidBeacon || !flag || incidentDef.category != IncidentCategoryDefOf.ThreatBig)
				{
					yield return new FiringIncident(incidentDef, this, parms);
				}
			}
			IL_F1:
			yield break;
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x00185670 File Offset: 0x00183870
		private IncidentCategoryDef ChooseRandomCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
		{
			if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig))
			{
				int num = Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick;
				if (target.StoryState.LastThreatBigTick >= 0 && (float)num > 60000f * this.Props.maxThreatBigIntervalDays)
				{
					return IncidentCategoryDefOf.ThreatBig;
				}
			}
			return (from cw in this.Props.categoryWeights
			where !skipCategories.Contains(cw.category)
			select cw).RandomElementByWeight((IncidentCategoryEntry cw) => cw.weight).category;
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x00185724 File Offset: 0x00183924
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incCat, target);
			if (incidentParms.points >= 0f)
			{
				incidentParms.points *= this.Props.randomPointsFactorRange.RandomInRange;
			}
			return incidentParms;
		}
	}
}
