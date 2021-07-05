using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001019 RID: 4121
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06006126 RID: 24870 RVA: 0x002100D1 File Offset: 0x0020E2D1
		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
			}
		}

		// Token: 0x06006127 RID: 24871 RVA: 0x0020F70C File Offset: 0x0020D90C
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06006128 RID: 24872 RVA: 0x002100E2 File Offset: 0x0020E2E2
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x06006129 RID: 24873 RVA: 0x002100F9 File Offset: 0x0020E2F9
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
			if (this.thingDef.Minifiable)
			{
				thing = thing.MakeMinified();
			}
			if (this.thingDef.IsIngestible && this.thingDef.ingestible.IsMeal)
			{
				FoodUtility.GenerateGoodIngredients(thing, Faction.OfPlayer.ideos.PrimaryIdeo);
			}
			thing.stackCount = this.count;
			yield return thing;
			yield break;
		}

		// Token: 0x04003769 RID: 14185
		public const string PlayerStartWithTag = "PlayerStartsWith";
	}
}
