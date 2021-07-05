using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000893 RID: 2195
	public static class CoverUtility
	{
		// Token: 0x06003671 RID: 13937 RVA: 0x0015BEDC File Offset: 0x0015A0DC
		public static List<CoverInfo> CalculateCoverGiverSet(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			List<CoverInfo> list = new List<CoverInfo>();
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo item;
				if (intVec.InBounds(map) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out item) && item.BlockChance > 0f)
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x0015BF44 File Offset: 0x0015A144
		public static float CalculateOverallBlockChance(LocalTargetInfo target, IntVec3 shooterLoc, Map map)
		{
			IntVec3 cell = target.Cell;
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = cell + GenAdj.AdjacentCells[i];
				CoverInfo coverInfo;
				if (intVec.InBounds(map) && CoverUtility.TryFindAdjustedCoverInCell(shooterLoc, target, intVec, map, out coverInfo))
				{
					num += (1f - num) * coverInfo.BlockChance;
				}
			}
			return num;
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x0015BFA8 File Offset: 0x0015A1A8
		private static bool TryFindAdjustedCoverInCell(IntVec3 shooterLoc, LocalTargetInfo target, IntVec3 adjCell, Map map, out CoverInfo result)
		{
			IntVec3 cell = target.Cell;
			Thing cover = adjCell.GetCover(map);
			if (cover == null || cover == target.Thing || shooterLoc == cell)
			{
				result = CoverInfo.Invalid;
				return false;
			}
			float angleFlat = (shooterLoc - cell).AngleFlat;
			float num = GenGeo.AngleDifferenceBetween((adjCell - cell).AngleFlat, angleFlat);
			if (!cell.AdjacentToCardinal(adjCell))
			{
				num *= 1.75f;
			}
			float num2 = cover.BaseBlockChance();
			if (num < 15f)
			{
				num2 *= 1f;
			}
			else if (num < 27f)
			{
				num2 *= 0.8f;
			}
			else if (num < 40f)
			{
				num2 *= 0.6f;
			}
			else if (num < 52f)
			{
				num2 *= 0.4f;
			}
			else
			{
				if (num >= 65f)
				{
					result = CoverInfo.Invalid;
					return false;
				}
				num2 *= 0.2f;
			}
			float lengthHorizontal = (shooterLoc - adjCell).LengthHorizontal;
			if (lengthHorizontal < 1.9f)
			{
				num2 *= 0.3333f;
			}
			else if (lengthHorizontal < 2.9f)
			{
				num2 *= 0.66666f;
			}
			result = new CoverInfo(cover, num2);
			return true;
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x0002A454 File Offset: 0x00028654
		public static float BaseBlockChance(this ThingDef def)
		{
			if (def.Fillage == FillCategory.Full)
			{
				return 0.75f;
			}
			return def.fillPercent;
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x0015C0E8 File Offset: 0x0015A2E8
		public static float BaseBlockChance(this Thing thing)
		{
			Building_Door building_Door = thing as Building_Door;
			if (building_Door != null && building_Door.Open)
			{
				return 0f;
			}
			return thing.def.BaseBlockChance();
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x0015C118 File Offset: 0x0015A318
		public static float TotalSurroundingCoverScore(IntVec3 c, Map map)
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (c2.InBounds(map))
				{
					Thing cover = c2.GetCover(map);
					if (cover != null)
					{
						num += cover.BaseBlockChance();
					}
				}
			}
			return num;
		}

		// Token: 0x040025F3 RID: 9715
		public const float CoverPercent_Corner = 0.75f;
	}
}
