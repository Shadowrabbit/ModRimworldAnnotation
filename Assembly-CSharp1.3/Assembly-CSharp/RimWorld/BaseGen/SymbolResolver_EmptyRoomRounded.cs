using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015B9 RID: 5561
	public class SymbolResolver_EmptyRoomRounded : SymbolResolver
	{
		// Token: 0x0600830A RID: 33546 RVA: 0x002EA120 File Offset: 0x002E8320
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.cornerRadius != null)
			{
				int num = Mathf.Min(rp.rect.Width, rp.rect.Height);
				int? num2 = rp.cornerRadius * 2;
				int num3 = num;
				if (num2.GetValueOrDefault() > num3 & num2 != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600830B RID: 33547 RVA: 0x002EA1A8 File Offset: 0x002E83A8
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, true);
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.CorrespondingTerrainDef(thingDef, true, rp.faction);
			int num = Mathf.Min(rp.rect.Width, rp.rect.Height);
			int num2 = rp.cornerRadius ?? (num / 4);
			if (num2 > num / 2)
			{
				num2 = Mathf.FloorToInt((float)num / 2f);
			}
			CellRect cellRect = new CellRect(rp.rect.minX + num2, rp.rect.minZ, rp.rect.Width - 2 * num2, rp.rect.Height);
			CellRect cellRect2 = new CellRect(rp.rect.minX, rp.rect.minZ + num2, num2, rp.rect.Height - 2 * num2);
			CellRect cellRect3 = new CellRect(rp.rect.maxX - num2 + 1, rp.rect.minZ + num2, num2, rp.rect.Height - 2 * num2);
			foreach (IntVec3 intVec in rp.rect.Corners)
			{
				int newX = (intVec.x < rp.rect.CenterCell.x) ? 1 : -1;
				int newZ = (intVec.z < rp.rect.CenterCell.z) ? 1 : -1;
				IntVec3 a = new IntVec3(newX, 0, newZ);
				IntVec3 intVec2 = intVec + a * num2;
				CellRect cellRect4 = new CellRect(Mathf.Min(intVec2.x, intVec.x), Mathf.Min(intVec2.z, intVec.z), num2, num2);
				foreach (IntVec3 intVec3 in GenRadial.RadialCellsAround(intVec2, (float)(num2 - 1), (float)num2))
				{
					if (cellRect4.Contains(intVec3))
					{
						ResolveParams resolveParams = rp;
						resolveParams.wallStuff = thingDef;
						resolveParams.rect = CellRect.CenteredOn(intVec3, 1, 1);
						BaseGen.symbolStack.Push("edgeWalls", resolveParams, null);
					}
				}
				foreach (IntVec3 intVec4 in GenRadial.RadialCellsAround(intVec2, (float)num2, true))
				{
					if (cellRect4.Contains(intVec4))
					{
						ResolveParams resolveParams2 = rp;
						resolveParams2.rect = CellRect.CenteredOn(intVec4, 1, 1);
						resolveParams2.floorDef = floorDef;
						if (rp.noRoof == null || !rp.noRoof.Value)
						{
							BaseGen.symbolStack.Push("roof", resolveParams2, null);
						}
						BaseGen.symbolStack.Push("floor", resolveParams2, null);
						BaseGen.symbolStack.Push("clear", resolveParams2, null);
					}
				}
			}
			this.wallCells.Clear();
			if ((float)cellRect.Area > 0f)
			{
				this.wallCells.AddRange(cellRect.GetEdgeCells(Rot4.North));
				this.wallCells.AddRange(cellRect.GetEdgeCells(Rot4.South));
			}
			if ((float)cellRect2.Area > 0f)
			{
				this.wallCells.AddRange(cellRect2.GetEdgeCells(Rot4.West));
			}
			if ((float)cellRect3.Area > 0f)
			{
				this.wallCells.AddRange(cellRect3.GetEdgeCells(Rot4.East));
			}
			ResolveParams resolveParams3 = rp;
			foreach (IntVec3 center in this.wallCells)
			{
				resolveParams3.wallStuff = thingDef;
				resolveParams3.rect = CellRect.CenteredOn(center, 1, 1);
				BaseGen.symbolStack.Push("edgeWalls", resolveParams3, null);
			}
			ResolveParams resolveParams4 = rp;
			resolveParams4.floorDef = floorDef;
			foreach (CellRect rect in new CellRect[]
			{
				cellRect,
				cellRect2,
				cellRect3
			})
			{
				if ((float)rect.Area > 0f)
				{
					resolveParams4.rect = rect;
					if (rp.noRoof == null || !rp.noRoof.Value)
					{
						BaseGen.symbolStack.Push("roof", resolveParams4, null);
					}
					BaseGen.symbolStack.Push("floor", resolveParams4, null);
					BaseGen.symbolStack.Push("clear", resolveParams4, null);
				}
			}
			this.wallCells.Clear();
		}

		// Token: 0x040051F5 RID: 20981
		private List<IntVec3> wallCells = new List<IntVec3>();
	}
}
