using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200161E RID: 5662
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06007B15 RID: 31509 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007B16 RID: 31510 RVA: 0x00052C3A File Offset: 0x00050E3A
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x06007B17 RID: 31511 RVA: 0x00052C56 File Offset: 0x00050E56
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "MapScatteredWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x0400509E RID: 20638
		public const string MapScatteredWithTag = "MapScatteredWith";
	}
}
