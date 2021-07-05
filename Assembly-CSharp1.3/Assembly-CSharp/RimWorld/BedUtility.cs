using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106F RID: 4207
	public static class BedUtility
	{
		// Token: 0x060063CE RID: 25550 RVA: 0x0021B5DF File Offset: 0x002197DF
		public static int GetSleepingSlotsCount(IntVec2 bedSize)
		{
			return bedSize.x;
		}

		// Token: 0x060063CF RID: 25551 RVA: 0x0021B5E8 File Offset: 0x002197E8
		public static IntVec3 GetSleepingSlotPos(int index, IntVec3 bedCenter, Rot4 bedRot, IntVec2 bedSize)
		{
			int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(bedSize);
			if (index < 0 || index >= sleepingSlotsCount)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to get sleeping slot pos with index ",
					index,
					", but there are only ",
					sleepingSlotsCount,
					" sleeping slots available."
				}));
				return bedCenter;
			}
			CellRect cellRect = GenAdj.OccupiedRect(bedCenter, bedRot, bedSize);
			if (bedRot == Rot4.North)
			{
				return new IntVec3(cellRect.minX + index, bedCenter.y, cellRect.minZ);
			}
			if (bedRot == Rot4.East)
			{
				return new IntVec3(cellRect.minX, bedCenter.y, cellRect.maxZ - index);
			}
			if (bedRot == Rot4.South)
			{
				return new IntVec3(cellRect.minX + index, bedCenter.y, cellRect.maxZ);
			}
			return new IntVec3(cellRect.maxX, bedCenter.y, cellRect.maxZ - index);
		}

		// Token: 0x060063D0 RID: 25552 RVA: 0x0021B6D8 File Offset: 0x002198D8
		public static bool WillingToShareBed(Pawn pawn1, Pawn pawn2)
		{
			if (pawn1 == pawn2)
			{
				return true;
			}
			if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed, pawn1) || !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed, pawn2))
			{
				return false;
			}
			if (pawn1.relations.GetDirectRelation(PawnRelationDefOf.Spouse, pawn2) != null)
			{
				if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_Spouse, pawn1) || !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_Spouse, pawn2))
				{
					return false;
				}
			}
			else if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_NonSpouse, pawn1) || !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.SharedBed_NonSpouse, pawn2))
			{
				return false;
			}
			return true;
		}
	}
}
