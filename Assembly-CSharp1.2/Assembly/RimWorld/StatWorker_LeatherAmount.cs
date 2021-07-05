using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6F RID: 7535
	public class StatWorker_LeatherAmount : StatWorker
	{
		// Token: 0x0600A3CB RID: 41931 RVA: 0x0006CA9B File Offset: 0x0006AC9B
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			foreach (Dialog_InfoCard.Hyperlink hyperlink in base.GetInfoCardHyperlinks(statRequest))
			{
				yield return hyperlink;
			}
			IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
			if (!statRequest.HasThing || statRequest.Thing.def.race == null || statRequest.Thing.def.race.leatherDef == null)
			{
				yield break;
			}
			yield return new Dialog_InfoCard.Hyperlink(statRequest.Thing.def.race.leatherDef, -1);
			yield break;
			yield break;
		}
	}
}
