using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B54 RID: 2900
	public class QuestPart_AssaultThings : QuestPart_MakeLord
	{
		// Token: 0x060043DE RID: 17374 RVA: 0x00168FF7 File Offset: 0x001671F7
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_AssaultThings(this.faction, this.things, 1f, false), base.Map, null);
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x00169024 File Offset: 0x00167224
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04002929 RID: 10537
		public List<Thing> things = new List<Thing>();
	}
}
