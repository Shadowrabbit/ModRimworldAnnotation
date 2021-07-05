using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001018 RID: 4120
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x06006122 RID: 24866 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006123 RID: 24867 RVA: 0x0020F70C File Offset: 0x0020D90C
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06006124 RID: 24868 RVA: 0x002100BA File Offset: 0x0020E2BA
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
