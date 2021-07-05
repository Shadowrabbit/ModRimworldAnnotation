using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001502 RID: 5378
	public class StatWorker_LeatherAmount : StatWorker
	{
		// Token: 0x0600801B RID: 32795 RVA: 0x002D5BD7 File Offset: 0x002D3DD7
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
