using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001240 RID: 4672
	public class StorytellerComp_SingleOnceFixed : StorytellerComp
	{
		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x060065FC RID: 26108 RVA: 0x00044070 File Offset: 0x00042270
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x060065FD RID: 26109 RVA: 0x00045B8F File Offset: 0x00043D8F
		private StorytellerCompProperties_SingleOnceFixed Props
		{
			get
			{
				return (StorytellerCompProperties_SingleOnceFixed)this.props;
			}
		}

		// Token: 0x060065FE RID: 26110 RVA: 0x00045B9C File Offset: 0x00043D9C
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			int num = this.IntervalsPassed;
			if (this.Props.minColonistCount > 0)
			{
				if (target.StoryState.lastFireTicks.ContainsKey(this.Props.incident))
				{
					yield break;
				}
				if (PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count < this.Props.minColonistCount)
				{
					yield break;
				}
				num -= target.StoryState.GetTicksFromColonistCount(this.Props.minColonistCount) / 1000;
			}
			if (num == this.Props.fireAfterDaysPassed * 60)
			{
				if (this.Props.skipIfColonistHasMinTitle != null)
				{
					List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
					for (int i = 0; i < allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Count; i++)
					{
						if (allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].royalty != null && allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].royalty.AllTitlesForReading.Any<RoyalTitle>() && allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists[i].royalty.MainTitle().seniority >= this.Props.skipIfColonistHasMinTitle.seniority)
						{
							yield break;
						}
					}
				}
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (!this.Props.skipIfOnExtremeBiome || (anyPlayerHomeMap != null && !anyPlayerHomeMap.Biome.isExtremeBiome))
				{
					IncidentDef incident = this.Props.incident;
					if (incident.TargetAllowed(target))
					{
						FiringIncident firingIncident = new FiringIncident(incident, this, this.GenerateParms(incident.category, target));
						yield return firingIncident;
					}
				}
			}
			yield break;
		}
	}
}
