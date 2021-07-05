using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FED RID: 4077
	public class SitePartWorker_MechCluster : SitePartWorker
	{
		// Token: 0x06006002 RID: 24578 RVA: 0x0020C068 File Offset: 0x0020A268
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x06006003 RID: 24579 RVA: 0x0020C1B3 File Offset: 0x0020A3B3
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, 750f);
			return sitePartParams;
		}

		// Token: 0x04003722 RID: 14114
		public const float MinPoints = 750f;
	}
}
