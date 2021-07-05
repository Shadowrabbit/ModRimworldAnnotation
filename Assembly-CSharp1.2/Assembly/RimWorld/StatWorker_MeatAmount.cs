using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6D RID: 7533
	public class StatWorker_MeatAmount : StatWorker
	{
		// Token: 0x0600A3BF RID: 41919 RVA: 0x0006CA28 File Offset: 0x0006AC28
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			foreach (Dialog_InfoCard.Hyperlink hyperlink in base.GetInfoCardHyperlinks(statRequest))
			{
				yield return hyperlink;
			}
			IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
			if (!statRequest.HasThing || statRequest.Thing.def.race == null || statRequest.Thing.def.race.meatDef == null)
			{
				yield break;
			}
			yield return new Dialog_InfoCard.Hyperlink(statRequest.Thing.def.race.meatDef, -1);
			yield break;
			yield break;
		}
	}
}
