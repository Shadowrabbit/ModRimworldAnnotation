using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001622 RID: 5666
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x06007B2D RID: 31533 RVA: 0x00052CE0 File Offset: 0x00050EE0
		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
			}
		}

		// Token: 0x06007B2E RID: 31534 RVA: 0x00052898 File Offset: 0x00050A98
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06007B2F RID: 31535 RVA: 0x00052CF1 File Offset: 0x00050EF1
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x06007B30 RID: 31536 RVA: 0x00052D08 File Offset: 0x00050F08
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
			if (this.thingDef.Minifiable)
			{
				thing = thing.MakeMinified();
			}
			thing.stackCount = this.count;
			yield return thing;
			yield break;
		}

		// Token: 0x040050AB RID: 20651
		public const string PlayerStartWithTag = "PlayerStartsWith";
	}
}
