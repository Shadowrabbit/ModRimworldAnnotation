using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B2A RID: 2858
	public class QuestPart_Filter_AllThingsHacked : QuestPart_Filter
	{
		// Token: 0x06004313 RID: 17171 RVA: 0x001667F4 File Offset: 0x001649F4
		protected override bool Pass(SignalArgs args)
		{
			if (this.things.NullOrEmpty<Thing>())
			{
				return false;
			}
			foreach (Thing thing in this.things)
			{
				if (!thing.IsHackable() || !thing.IsHacked())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x00166868 File Offset: 0x00164A68
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040028D0 RID: 10448
		public List<Thing> things = new List<Thing>();
	}
}
