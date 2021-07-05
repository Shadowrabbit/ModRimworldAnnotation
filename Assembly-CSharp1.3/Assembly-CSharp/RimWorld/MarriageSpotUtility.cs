using System;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200107D RID: 4221
	public static class MarriageSpotUtility
	{
		// Token: 0x0600646D RID: 25709 RVA: 0x0021DCBC File Offset: 0x0021BEBC
		public static bool IsValidMarriageSpot(IntVec3 cell, Map map, StringBuilder outFailReason = null)
		{
			if (!cell.Standable(map))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("MarriageSpotNotStandable".Translate());
				}
				return false;
			}
			return cell.Roofed(map) || JoyUtility.EnjoyableOutsideNow(map, outFailReason);
		}

		// Token: 0x0600646E RID: 25710 RVA: 0x0021DCF8 File Offset: 0x0021BEF8
		public static bool IsValidMarriageSpotFor(IntVec3 cell, Pawn firstFiance, Pawn secondFiance, StringBuilder outFailReason = null)
		{
			if (!firstFiance.Spawned || !secondFiance.Spawned)
			{
				Log.Warning("Can't check if a marriage spot is valid because one of the fiances isn't spawned.");
				return false;
			}
			if (firstFiance.Map != secondFiance.Map)
			{
				return false;
			}
			if (!MarriageSpotUtility.IsValidMarriageSpot(cell, firstFiance.Map, outFailReason))
			{
				return false;
			}
			if (!cell.Roofed(firstFiance.Map))
			{
				if (!JoyUtility.EnjoyableOutsideNow(firstFiance, outFailReason))
				{
					return false;
				}
				if (!JoyUtility.EnjoyableOutsideNow(secondFiance, outFailReason))
				{
					return false;
				}
			}
			if (cell.GetDangerFor(firstFiance, firstFiance.Map) != Danger.None)
			{
				if (outFailReason != null)
				{
					outFailReason.Append("MarriageSpotDangerous".Translate(firstFiance.LabelShort, firstFiance));
				}
				return false;
			}
			if (cell.GetDangerFor(secondFiance, secondFiance.Map) != Danger.None)
			{
				if (outFailReason != null)
				{
					outFailReason.Append("MarriageSpotDangerous".Translate(secondFiance.LabelShort, secondFiance));
				}
				return false;
			}
			if (cell.IsForbidden(firstFiance))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("MarriageSpotForbidden".Translate(firstFiance.LabelShort, firstFiance));
				}
				return false;
			}
			if (cell.IsForbidden(secondFiance))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("MarriageSpotForbidden".Translate(secondFiance.LabelShort, secondFiance));
				}
				return false;
			}
			if (!firstFiance.CanReserve(cell, 1, -1, null, false) || !secondFiance.CanReserve(cell, 1, -1, null, false))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("MarriageSpotReserved".Translate());
				}
				return false;
			}
			if (!firstFiance.CanReach(cell, PathEndMode.OnCell, Danger.None, false, false, TraverseMode.ByPawn))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("MarriageSpotUnreachable".Translate(firstFiance.LabelShort, firstFiance));
				}
				return false;
			}
			if (!secondFiance.CanReach(cell, PathEndMode.OnCell, Danger.None, false, false, TraverseMode.ByPawn))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("MarriageSpotUnreachable".Translate(secondFiance.LabelShort, secondFiance));
				}
				return false;
			}
			if (!firstFiance.IsPrisoner && !secondFiance.IsPrisoner)
			{
				Room room = cell.GetRoom(firstFiance.Map);
				if (room != null && room.IsPrisonCell)
				{
					if (outFailReason != null)
					{
						outFailReason.Append("MarriageSpotInPrisonCell".Translate());
					}
					return false;
				}
			}
			return true;
		}
	}
}
