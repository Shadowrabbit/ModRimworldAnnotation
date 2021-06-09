using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE9 RID: 7145
	public class TutorialState : IExposable
	{
		// Token: 0x06009D3E RID: 40254 RVA: 0x002DFD40 File Offset: 0x002DDF40
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

		// Token: 0x06009D3F RID: 40255 RVA: 0x00068AA8 File Offset: 0x00066CA8
		public void Notify_TutorialEnding()
		{
			this.startingItems.Clear();
			this.roomRect = default(CellRect);
			this.sandbagsRect = default(CellRect);
			this.endTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06009D40 RID: 40256 RVA: 0x00068ADD File Offset: 0x00066CDD
		public void AddStartingItem(Thing t)
		{
			if (this.startingItems.Contains(t))
			{
				return;
			}
			this.startingItems.Add(t);
		}

		// Token: 0x04006413 RID: 25619
		public List<Thing> startingItems = new List<Thing>();

		// Token: 0x04006414 RID: 25620
		public CellRect roomRect;

		// Token: 0x04006415 RID: 25621
		public CellRect sandbagsRect;

		// Token: 0x04006416 RID: 25622
		public int endTick = -1;

		// Token: 0x04006417 RID: 25623
		public bool introDone;
	}
}
