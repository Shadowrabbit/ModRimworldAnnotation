using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C51 RID: 3153
	public class StorytellerComp_SingleOnceFixed : StorytellerComp
	{
		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060049BE RID: 18878 RVA: 0x0017B75F File Offset: 0x0017995F
		protected int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x060049BF RID: 18879 RVA: 0x001859D7 File Offset: 0x00183BD7
		private StorytellerCompProperties_SingleOnceFixed Props
		{
			get
			{
				return (StorytellerCompProperties_SingleOnceFixed)this.props;
			}
		}

		// Token: 0x060049C0 RID: 18880 RVA: 0x001859E4 File Offset: 0x00183BE4
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
