using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001089 RID: 4233
	public class QuestPart_AssaultThings : QuestPart_MakeLord
	{
		// Token: 0x06005C41 RID: 23617 RVA: 0x0003FFF3 File Offset: 0x0003E1F3
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_AssaultThings(this.faction, this.things, 1f, false), base.Map, null);
		}

		// Token: 0x06005C42 RID: 23618 RVA: 0x001D9F18 File Offset: 0x001D8118
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04003DC8 RID: 15816
		public List<Thing> things = new List<Thing>();
	}
}
