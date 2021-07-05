using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001017 RID: 4119
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x0600611E RID: 24862 RVA: 0x0001276E File Offset: 0x0001096E
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x0021007F File Offset: 0x0020E27F
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x06006120 RID: 24864 RVA: 0x0021009B File Offset: 0x0020E29B
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "MapScatteredWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x04003768 RID: 14184
		public const string MapScatteredWithTag = "MapScatteredWith";
	}
}
