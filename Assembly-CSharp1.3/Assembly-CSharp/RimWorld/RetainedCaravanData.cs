using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CDC RID: 3292
	public class RetainedCaravanData : IExposable
	{
		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06004CE0 RID: 19680 RVA: 0x0019A575 File Offset: 0x00198775
		public bool HasDestinationTile
		{
			get
			{
				return this.destinationTile != -1;
			}
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x0019A583 File Offset: 0x00198783
		public RetainedCaravanData(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x0019A5A0 File Offset: 0x001987A0
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.shouldPassStoryState, "shouldPassStoryState", false, false);
			Scribe_Values.Look<int>(ref this.nextTile, "nextTile", -1, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeftPct, "nextTileCostLeftPct", -1f, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x0019A620 File Offset: 0x00198820
		public void Notify_GeneratedTempIncidentMapFor(Caravan caravan)
		{
			if (!this.map.Parent.def.isTempIncidentMapOwner)
			{
				return;
			}
			this.Set(caravan);
		}

		// Token: 0x06004CE4 RID: 19684 RVA: 0x0019A644 File Offset: 0x00198844
		public void Notify_CaravanFormed(Caravan caravan)
		{
			if (this.shouldPassStoryState)
			{
				this.shouldPassStoryState = false;
				this.map.StoryState.CopyTo(caravan.StoryState);
			}
			if (this.nextTile != -1 && this.nextTile != caravan.Tile && caravan.CanReach(this.nextTile))
			{
				caravan.pather.StartPath(this.nextTile, null, true, true);
				caravan.pather.nextTileCostLeft = caravan.pather.nextTileCostTotal * this.nextTileCostLeftPct;
				caravan.pather.Paused = this.paused;
				caravan.tweener.ResetTweenedPosToRoot();
			}
			if (this.HasDestinationTile && this.destinationTile != caravan.Tile)
			{
				caravan.pather.StartPath(this.destinationTile, this.arrivalAction, true, true);
				this.destinationTile = -1;
				this.arrivalAction = null;
			}
		}

		// Token: 0x06004CE5 RID: 19685 RVA: 0x0019A728 File Offset: 0x00198928
		private void Set(Caravan caravan)
		{
			caravan.StoryState.CopyTo(this.map.StoryState);
			this.shouldPassStoryState = true;
			if (caravan.pather.Moving)
			{
				this.nextTile = caravan.pather.nextTile;
				this.nextTileCostLeftPct = caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal;
				this.paused = caravan.pather.Paused;
				this.destinationTile = caravan.pather.Destination;
				this.arrivalAction = caravan.pather.ArrivalAction;
				return;
			}
			this.nextTile = -1;
			this.nextTileCostLeftPct = 0f;
			this.paused = false;
			this.destinationTile = -1;
			this.arrivalAction = null;
		}

		// Token: 0x04002E82 RID: 11906
		private Map map;

		// Token: 0x04002E83 RID: 11907
		private bool shouldPassStoryState;

		// Token: 0x04002E84 RID: 11908
		private int nextTile = -1;

		// Token: 0x04002E85 RID: 11909
		private float nextTileCostLeftPct;

		// Token: 0x04002E86 RID: 11910
		private bool paused;

		// Token: 0x04002E87 RID: 11911
		private int destinationTile = -1;

		// Token: 0x04002E88 RID: 11912
		private CaravanArrivalAction arrivalAction;
	}
}
