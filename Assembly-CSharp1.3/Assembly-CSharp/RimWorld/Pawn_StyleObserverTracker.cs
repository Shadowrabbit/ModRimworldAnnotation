using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E77 RID: 3703
	public class Pawn_StyleObserverTracker : IExposable
	{
		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x06005694 RID: 22164 RVA: 0x001D64DF File Offset: 0x001D46DF
		public int StyleDominanceThoughtIndex
		{
			get
			{
				return this.styleDominanceThoughtIndex;
			}
		}

		// Token: 0x06005695 RID: 22165 RVA: 0x001D64E7 File Offset: 0x001D46E7
		public Pawn_StyleObserverTracker()
		{
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x001D64F6 File Offset: 0x001D46F6
		public Pawn_StyleObserverTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005697 RID: 22167 RVA: 0x001D650C File Offset: 0x001D470C
		private bool CellValid(IntVec3 cell)
		{
			return cell.InBounds(this.pawn.Map) && !cell.Fogged(this.pawn.Map) && GenSight.LineOfSight(this.pawn.Position, cell, this.pawn.Map, false, null, 0, 0);
		}

		// Token: 0x06005698 RID: 22168 RVA: 0x001D6564 File Offset: 0x001D4764
		public void StyleObserverTick()
		{
			if (this.pawn.IsHashIntervalTick(900) && ModsConfig.IdeologyActive && this.pawn.Ideo != null && this.pawn.Spawned)
			{
				int lastIndex = this.styleDominanceThoughtIndex;
				this.styleDominanceThoughtIndex = -1;
				if (this.pawn.Awake() && this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
				{
					Room room = this.pawn.GetRoom(RegionType.Set_All);
					IntVec3 center;
					if (room != null && CellFinder.TryFindRandomCellNear(this.pawn.Position, this.pawn.Map, 5, (IntVec3 x) => this.CellValid(x), out center, -1))
					{
						float styleDominanceFromCellsCenteredOn = IdeoUtility.GetStyleDominanceFromCellsCenteredOn(center, this.pawn.Position, this.pawn.Map, this.pawn.Ideo);
						float pointsThreshold = (!room.IsDoorway && (float)room.CellCount < 10f) ? ((float)room.CellCount) : 10f;
						this.UpdateStyleDominanceThoughtIndex(styleDominanceFromCellsCenteredOn, pointsThreshold, lastIndex);
					}
				}
			}
		}

		// Token: 0x06005699 RID: 22169 RVA: 0x001D6688 File Offset: 0x001D4888
		private void UpdateStyleDominanceThoughtIndex(float styleDominance, float pointsThreshold, int lastIndex)
		{
			if (styleDominance >= pointsThreshold)
			{
				this.styleDominanceThoughtIndex = 0;
			}
			else if (styleDominance <= -pointsThreshold)
			{
				this.styleDominanceThoughtIndex = 1;
			}
			else
			{
				this.styleDominanceThoughtIndex = -1;
			}
			if (lastIndex != this.styleDominanceThoughtIndex)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x001D66DF File Offset: 0x001D48DF
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.styleDominanceThoughtIndex, "styleDominanceThoughtIndex", -1, false);
		}

		// Token: 0x0400331E RID: 13086
		public Pawn pawn;

		// Token: 0x0400331F RID: 13087
		private int styleDominanceThoughtIndex = -1;

		// Token: 0x04003320 RID: 13088
		private const int StyleObservationCenterCellRadius = 5;

		// Token: 0x04003321 RID: 13089
		private const int StyleObservationInterval = 900;

		// Token: 0x04003322 RID: 13090
		private const float BaseDominancePointsThreshold = 10f;
	}
}
