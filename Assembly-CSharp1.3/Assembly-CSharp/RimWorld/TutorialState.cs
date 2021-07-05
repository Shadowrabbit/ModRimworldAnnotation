using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013DD RID: 5085
	public class TutorialState : IExposable
	{
		// Token: 0x06007BAE RID: 31662 RVA: 0x002B9804 File Offset: 0x002B7A04
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null || it.Destroyed || (it.Map == null && it.MapHeld == null));
			}
			Scribe_Collections.Look<Thing>(ref this.startingItems, "startingItems", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<CellRect>(ref this.roomRect, "roomRect", default(CellRect), false);
			Scribe_Values.Look<CellRect>(ref this.sandbagsRect, "sandbagsRect", default(CellRect), false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", -1, false);
			Scribe_Values.Look<bool>(ref this.introDone, "introDone", false, false);
			if (this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null);
			}
		}

		// Token: 0x06007BAF RID: 31663 RVA: 0x002B98ED File Offset: 0x002B7AED
		public void Notify_TutorialEnding()
		{
			this.startingItems.Clear();
			this.roomRect = default(CellRect);
			this.sandbagsRect = default(CellRect);
			this.endTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06007BB0 RID: 31664 RVA: 0x002B9922 File Offset: 0x002B7B22
		public void AddStartingItem(Thing t)
		{
			if (this.startingItems.Contains(t))
			{
				return;
			}
			this.startingItems.Add(t);
		}

		// Token: 0x04004466 RID: 17510
		public List<Thing> startingItems = new List<Thing>();

		// Token: 0x04004467 RID: 17511
		public CellRect roomRect;

		// Token: 0x04004468 RID: 17512
		public CellRect sandbagsRect;

		// Token: 0x04004469 RID: 17513
		public int endTick = -1;

		// Token: 0x0400446A RID: 17514
		public bool introDone;
	}
}
