using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015BB RID: 5563
	public class SitePartWorker_MechCluster : SitePartWorker
	{
		// Token: 0x060078C4 RID: 30916 RVA: 0x0024AE3C File Offset: 0x0024903C
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in map.listerThings.AllThings)
			{
				if ((thing.def.building != null && thing.def.building.buildingTags != null && thing.def.building.buildingTags.Contains("MechClusterMember")) || (thing is Pawn && thing.def.race.IsMechanoid))
				{
					list.Add(thing);
				}
			}
			lookTargets = new LookTargets(list);
			return arrivedLetterPart;
		}

		// Token: 0x060078C5 RID: 30917 RVA: 0x000515D6 File Offset: 0x0004F7D6
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, 750f);
			return sitePartParams;
		}

		// Token: 0x04004F8A RID: 20362
		public const float MinPoints = 750f;
	}
}
