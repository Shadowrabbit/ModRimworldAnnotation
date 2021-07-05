using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000178 RID: 376
	public class AnimalPenBalanceCalculator
	{
		// Token: 0x06000A89 RID: 2697 RVA: 0x00039B99 File Offset: 0x00037D99
		public AnimalPenBalanceCalculator(Map map, bool considerInProgressMovement)
		{
			this.map = map;
			this.considerInProgressMovement = considerInProgressMovement;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00039BC1 File Offset: 0x00037DC1
		public void MarkDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00039BCC File Offset: 0x00037DCC
		public bool IsBetterPen(CompAnimalPenMarker markerA, CompAnimalPenMarker markerB, bool leavingMarkerB, Pawn animal)
		{
			this.RecalculateIfDirty();
			District district = markerA.parent.GetDistrict(RegionType.Set_Passable);
			District district2 = markerB.parent.GetDistrict(RegionType.Set_Passable);
			if (district == district2)
			{
				return false;
			}
			float bodySize = animal.BodySize;
			float num = this.TotalBodySizeIn(district) + bodySize;
			float num2 = this.TotalBodySizeIn(district2) + (leavingMarkerB ? (-bodySize) : bodySize);
			float num3 = num / (float)district.CellCount;
			float num4 = num2 / (float)district2.CellCount;
			return num3 * 1.2f < num4;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00039C40 File Offset: 0x00037E40
		public float TotalBodySizeIn(District district)
		{
			this.RecalculateIfDirty();
			float num = 0f;
			foreach (AnimalPenBalanceCalculator.AnimalMembershipInfo animalMembershipInfo in this.membership)
			{
				if (animalMembershipInfo.pen == district)
				{
					num += animalMembershipInfo.animal.BodySize;
				}
			}
			return num;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00039CB0 File Offset: 0x00037EB0
		private void RecalculateIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.membership.Clear();
			foreach (Pawn pawn in this.map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					District district = null;
					if (this.considerInProgressMovement && pawn.roping.IsRopedByPawn && pawn.roping.RopedByPawn.jobs.curDriver is JobDriver_RopeToPen)
					{
						Thing thing = pawn.roping.RopedByPawn.CurJob.GetTarget(TargetIndex.C).Thing;
						District district2 = (thing != null) ? thing.GetDistrict(RegionType.Set_Passable) : null;
						if (district2 != null && !district2.TouchesMapEdge)
						{
							district = district2;
						}
					}
					if (district == null)
					{
						CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
						district = ((currentPenOf != null) ? currentPenOf.parent.GetDistrict(RegionType.Set_Passable) : null);
					}
					this.membership.Add(new AnimalPenBalanceCalculator.AnimalMembershipInfo
					{
						animal = pawn,
						pen = district
					});
				}
			}
		}

		// Token: 0x040008EE RID: 2286
		private const float DensityTolerance = 0.2f;

		// Token: 0x040008EF RID: 2287
		private readonly Map map;

		// Token: 0x040008F0 RID: 2288
		private bool considerInProgressMovement;

		// Token: 0x040008F1 RID: 2289
		private readonly List<AnimalPenBalanceCalculator.AnimalMembershipInfo> membership = new List<AnimalPenBalanceCalculator.AnimalMembershipInfo>();

		// Token: 0x040008F2 RID: 2290
		private bool dirty = true;

		// Token: 0x02001945 RID: 6469
		private struct AnimalMembershipInfo
		{
			// Token: 0x04006105 RID: 24837
			public Pawn animal;

			// Token: 0x04006106 RID: 24838
			public District pen;
		}
	}
}
