using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001620 RID: 5664
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x06007B21 RID: 31521 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007B22 RID: 31522 RVA: 0x00052898 File Offset: 0x00050A98
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06007B23 RID: 31523 RVA: 0x00052C9F File Offset: 0x00050E9F
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}
	}
}
