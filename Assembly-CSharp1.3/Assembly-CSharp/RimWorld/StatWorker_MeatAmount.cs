using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001501 RID: 5377
	public class StatWorker_MeatAmount : StatWorker
	{
		// Token: 0x06008018 RID: 32792 RVA: 0x002D5BB7 File Offset: 0x002D3DB7
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
