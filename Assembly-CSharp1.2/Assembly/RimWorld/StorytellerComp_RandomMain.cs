using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200122F RID: 4655
	public class StorytellerComp_RandomMain : StorytellerComp
	{
		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x060065BB RID: 26043 RVA: 0x00045913 File Offset: 0x00043B13
		protected StorytellerCompProperties_RandomMain Props
		{
			get
			{
				return (StorytellerCompProperties_RandomMain)this.props;
			}
		}

		// Token: 0x060065BC RID: 26044 RVA: 0x00045920 File Offset: 0x00043B20
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
					if (base.UsableIncidentsInCategory(incidentCategoryDef, parms).TryRandomElementByWeight(new Func<IncidentDef, float>(base.IncidentChanceFinal), out incidentDef))
					{
						break;
					}
					list.Add(incidentCategoryDef);
					if (list.Count >= this.Props.categoryWeights.Count)
					{
						goto IL_FC;
					}
				}
				if (!this.Props.skipThreatBigIfRaidBeacon || !flag || incidentDef.category != IncidentCategoryDefOf.ThreatBig)
				{
					yield return new FiringIncident(incidentDef, this, parms);
				}
			}
			IL_FC:
			yield break;
		}

		// Token: 0x060065BD RID: 26045 RVA: 0x001F7194 File Offset: 0x001F5394
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

		// Token: 0x060065BE RID: 26046 RVA: 0x001F7248 File Offset: 0x001F5448
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
