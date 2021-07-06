using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001300 RID: 4864
	public class RetainedCaravanData : IExposable
	{
		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06006991 RID: 27025 RVA: 0x00048016 File Offset: 0x00046216
		public bool HasDestinationTile
		{
			get
			{
				return this.destinationTile != -1;
			}
		}

		// Token: 0x06006992 RID: 27026 RVA: 0x00048024 File Offset: 0x00046224
		public RetainedCaravanData(Map map)
		{
			this.map = map;
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x00208314 File Offset: 0x00206514
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.shouldPassStoryState, "shouldPassStoryState", false, false);
			Scribe_Values.Look<int>(ref this.nextTile, "nextTile", -1, false);
			Scribe_Values.Look<float>(ref this.nextTileCostLeftPct, "nextTileCostLeftPct", -1f, false);
			Scribe_Values.Look<bool>(ref this.paused, "paused", false, false);
			Scribe_Values.Look<int>(ref this.destinationTile, "destinationTile", 0, false);
			Scribe_Deep.Look<CaravanArrivalAction>(ref this.arrivalAction, "arrivalAction", Array.Empty<object>());
		}

		// Token: 0x06006994 RID: 27028 RVA: 0x00048041 File Offset: 0x00046241
		public void Notify_GeneratedTempIncidentMapFor(Caravan caravan)
		{
			if (!this.map.Parent.def.isTempIncidentMapOwner)
			{
				return;
			}
			this.Set(caravan);
		}

		// Token: 0x06006995 RID: 27029 RVA: 0x00208394 File Offset: 0x00206594
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

		// Token: 0x06006996 RID: 27030 RVA: 0x00208478 File Offset: 0x00206678
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

		// Token: 0x0400464E RID: 17998
		private Map map;

		// Token: 0x0400464F RID: 17999
		private bool shouldPassStoryState;

		// Token: 0x04004650 RID: 18000
		private int nextTile = -1;

		// Token: 0x04004651 RID: 18001
		private float nextTileCostLeftPct;

		// Token: 0x04004652 RID: 18002
		private bool paused;

		// Token: 0x04004653 RID: 18003
		private int destinationTile = -1;

		// Token: 0x04004654 RID: 18004
		private CaravanArrivalAction arrivalAction;
	}
}
