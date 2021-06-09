using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E33 RID: 7731
	public class SymbolResolver_BasePart_Outdoors_Division_Grid : SymbolResolver
	{
		// Token: 0x0600A725 RID: 42789 RVA: 0x00309B40 File Offset: 0x00307D40
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.rect.Width < 13 && rp.rect.Height < 13)
			{
				return false;
			}
			this.FillOptions(rp.rect);
			return this.optionsX.Any<Pair<int, int>>() && this.optionsZ.Any<Pair<int, int>>();
		}

		// Token: 0x0600A726 RID: 42790 RVA: 0x00309BA0 File Offset: 0x00307DA0
		public override void Resolve(ResolveParams rp)
		{
			this.FillOptions(rp.rect);
			if (!Rand.Chance(0.2f))
			{
				if (this.TryResolveRandomOption(0, 0, rp))
				{
					return;
				}
				if (this.TryResolveRandomOption(0, 1, rp))
				{
					return;
				}
			}
			if (this.TryResolveRandomOption(1, 0, rp))
			{
				return;
			}
			if (this.TryResolveRandomOption(2, 0, rp))
			{
				return;
			}
			if (this.TryResolveRandomOption(2, 1, rp))
			{
				return;
			}
			if (this.TryResolveRandomOption(999999, 999999, rp))
			{
				return;
			}
			Log.Warning("Grid resolver could not resolve any grid size. params=" + rp, false);
		}

		// Token: 0x0600A727 RID: 42791 RVA: 0x00309C2C File Offset: 0x00307E2C
		private void FillOptions(CellRect rect)
		{
			this.FillOptions(this.optionsX, rect.Width);
			this.FillOptions(this.optionsZ, rect.Height);
			if (this.optionsZ.Any((Pair<int, int> x) => x.First > 1))
			{
				this.optionsX.RemoveAll((Pair<int, int> x) => x.First >= 3 && this.GetRoomSize(x.First, x.Second, rect.Width) <= 7);
			}
			if (this.optionsX.Any((Pair<int, int> x) => x.First > 1))
			{
				this.optionsZ.RemoveAll((Pair<int, int> x) => x.First >= 3 && this.GetRoomSize(x.First, x.Second, rect.Height) <= 7);
			}
		}

		// Token: 0x0600A728 RID: 42792 RVA: 0x00309D04 File Offset: 0x00307F04
		private void FillOptions(List<Pair<int, int>> outOptions, int length)
		{
			outOptions.Clear();
			for (int i = 2; i <= 4; i++)
			{
				for (int j = 1; j <= 5; j++)
				{
					int roomSize = this.GetRoomSize(i, j, length);
					if (roomSize != -1 && roomSize >= 6 && roomSize >= 2 * j - 1)
					{
						outOptions.Add(new Pair<int, int>(i, j));
					}
				}
			}
		}

		// Token: 0x0600A729 RID: 42793 RVA: 0x00309D58 File Offset: 0x00307F58
		private int GetRoomSize(int roomsPerRow, int pathwayWidth, int totalLength)
		{
			int num = totalLength - (roomsPerRow - 1) * pathwayWidth;
			if (num % roomsPerRow != 0)
			{
				return -1;
			}
			return num / roomsPerRow;
		}

		// Token: 0x0600A72A RID: 42794 RVA: 0x00309D78 File Offset: 0x00307F78
		private bool TryResolveRandomOption(int maxWidthHeightDiff, int maxPathwayWidthDiff, ResolveParams rp)
		{
			SymbolResolver_BasePart_Outdoors_Division_Grid.options.Clear();
			for (int i = 0; i < this.optionsX.Count; i++)
			{
				int first = this.optionsX[i].First;
				int second = this.optionsX[i].Second;
				int roomSize = this.GetRoomSize(first, second, rp.rect.Width);
				for (int j = 0; j < this.optionsZ.Count; j++)
				{
					int first2 = this.optionsZ[j].First;
					int second2 = this.optionsZ[j].Second;
					int roomSize2 = this.GetRoomSize(first2, second2, rp.rect.Height);
					if (Mathf.Abs(roomSize - roomSize2) <= maxWidthHeightDiff && Mathf.Abs(second - second2) <= maxPathwayWidthDiff)
					{
						SymbolResolver_BasePart_Outdoors_Division_Grid.options.Add(new Pair<Pair<int, int>, Pair<int, int>>(this.optionsX[i], this.optionsZ[j]));
					}
				}
			}
			if (SymbolResolver_BasePart_Outdoors_Division_Grid.options.Any<Pair<Pair<int, int>, Pair<int, int>>>())
			{
				Pair<Pair<int, int>, Pair<int, int>> pair = SymbolResolver_BasePart_Outdoors_Division_Grid.options.RandomElement<Pair<Pair<int, int>, Pair<int, int>>>();
				this.ResolveOption(pair.First.First, pair.First.Second, pair.Second.First, pair.Second.Second, rp);
				return true;
			}
			return false;
		}

		// Token: 0x0600A72B RID: 42795 RVA: 0x00309EF8 File Offset: 0x003080F8
		private void ResolveOption(int roomsPerRowX, int pathwayWidthX, int roomsPerRowZ, int pathwayWidthZ, ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			int roomSize = this.GetRoomSize(roomsPerRowX, pathwayWidthX, rp.rect.Width);
			int roomSize2 = this.GetRoomSize(roomsPerRowZ, pathwayWidthZ, rp.rect.Height);
			ThingDef thingDef = null;
			if (pathwayWidthX >= 3)
			{
				if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
				{
					thingDef = ThingDefOf.StandingLamp;
				}
				else
				{
					thingDef = ThingDefOf.TorchLamp;
				}
			}
			TerrainDef floorDef = rp.pathwayFloorDef ?? BaseGenUtility.RandomBasicFloorDef(rp.faction, false);
			int num = roomSize;
			for (int i = 0; i < roomsPerRowX - 1; i++)
			{
				CellRect rect = new CellRect(rp.rect.minX + num, rp.rect.minZ, pathwayWidthX, rp.rect.Height);
				ResolveParams resolveParams = rp;
				resolveParams.rect = rect;
				resolveParams.floorDef = floorDef;
				resolveParams.streetHorizontal = new bool?(false);
				BaseGen.symbolStack.Push("street", resolveParams, null);
				num += roomSize + pathwayWidthX;
			}
			int num2 = roomSize2;
			for (int j = 0; j < roomsPerRowZ - 1; j++)
			{
				CellRect rect2 = new CellRect(rp.rect.minX, rp.rect.minZ + num2, rp.rect.Width, pathwayWidthZ);
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rect2;
				resolveParams2.floorDef = floorDef;
				resolveParams2.streetHorizontal = new bool?(true);
				BaseGen.symbolStack.Push("street", resolveParams2, null);
				num2 += roomSize2 + pathwayWidthZ;
			}
			num = 0;
			num2 = 0;
			this.children.Clear();
			for (int k = 0; k < roomsPerRowX; k++)
			{
				for (int l = 0; l < roomsPerRowZ; l++)
				{
					SymbolResolver_BasePart_Outdoors_Division_Grid.Child child = new SymbolResolver_BasePart_Outdoors_Division_Grid.Child();
					child.rect = new CellRect(rp.rect.minX + num, rp.rect.minZ + num2, roomSize, roomSize2);
					child.gridX = k;
					child.gridY = l;
					this.children.Add(child);
					num2 += roomSize2 + pathwayWidthZ;
				}
				num += roomSize + pathwayWidthX;
				num2 = 0;
			}
			this.MergeRandomChildren();
			this.children.Shuffle<SymbolResolver_BasePart_Outdoors_Division_Grid.Child>();
			for (int m = 0; m < this.children.Count; m++)
			{
				if (thingDef != null)
				{
					IntVec3 c = new IntVec3(this.children[m].rect.maxX + 1, 0, this.children[m].rect.maxZ);
					if (rp.rect.Contains(c) && c.Standable(map))
					{
						ResolveParams resolveParams3 = rp;
						resolveParams3.rect = CellRect.SingleCell(c);
						resolveParams3.singleThingDef = thingDef;
						BaseGen.symbolStack.Push("thing", resolveParams3, null);
					}
				}
				ResolveParams resolveParams4 = rp;
				resolveParams4.rect = this.children[m].rect;
				BaseGen.symbolStack.Push("basePart_outdoors", resolveParams4, null);
			}
		}

		// Token: 0x0600A72C RID: 42796 RVA: 0x0030A208 File Offset: 0x00308408
		private void MergeRandomChildren()
		{
			if (this.children.Count < 4)
			{
				return;
			}
			int num = GenMath.RoundRandom((float)this.children.Count / 6f);
			for (int i = 0; i < num; i++)
			{
				SymbolResolver_BasePart_Outdoors_Division_Grid.Child child = this.children.Find((SymbolResolver_BasePart_Outdoors_Division_Grid.Child x) => !x.merged);
				if (child == null)
				{
					break;
				}
				SymbolResolver_BasePart_Outdoors_Division_Grid.Child child3 = this.children.Find((SymbolResolver_BasePart_Outdoors_Division_Grid.Child x) => x != child && ((Mathf.Abs(x.gridX - child.gridX) == 1 && x.gridY == child.gridY) || (Mathf.Abs(x.gridY - child.gridY) == 1 && x.gridX == child.gridX)));
				if (child3 != null)
				{
					this.children.Remove(child);
					this.children.Remove(child3);
					SymbolResolver_BasePart_Outdoors_Division_Grid.Child child2 = new SymbolResolver_BasePart_Outdoors_Division_Grid.Child();
					child2.gridX = Mathf.Min(child.gridX, child3.gridX);
					child2.gridY = Mathf.Min(child.gridY, child3.gridY);
					child2.merged = true;
					child2.rect = CellRect.FromLimits(Mathf.Min(child.rect.minX, child3.rect.minX), Mathf.Min(child.rect.minZ, child3.rect.minZ), Mathf.Max(child.rect.maxX, child3.rect.maxX), Mathf.Max(child.rect.maxZ, child3.rect.maxZ));
					this.children.Add(child2);
				}
			}
		}

		// Token: 0x04007199 RID: 29081
		private List<Pair<int, int>> optionsX = new List<Pair<int, int>>();

		// Token: 0x0400719A RID: 29082
		private List<Pair<int, int>> optionsZ = new List<Pair<int, int>>();

		// Token: 0x0400719B RID: 29083
		private List<SymbolResolver_BasePart_Outdoors_Division_Grid.Child> children = new List<SymbolResolver_BasePart_Outdoors_Division_Grid.Child>();

		// Token: 0x0400719C RID: 29084
		private const int MinWidthOrHeight = 13;

		// Token: 0x0400719D RID: 29085
		private const int MinRoomsPerRow = 2;

		// Token: 0x0400719E RID: 29086
		private const int MaxRoomsPerRow = 4;

		// Token: 0x0400719F RID: 29087
		private const int MinPathwayWidth = 1;

		// Token: 0x040071A0 RID: 29088
		private const int MaxPathwayWidth = 5;

		// Token: 0x040071A1 RID: 29089
		private const int MinRoomSize = 6;

		// Token: 0x040071A2 RID: 29090
		private const float AllowNonSquareRoomsInTheFirstStepChance = 0.2f;

		// Token: 0x040071A3 RID: 29091
		private static List<Pair<Pair<int, int>, Pair<int, int>>> options = new List<Pair<Pair<int, int>, Pair<int, int>>>();

		// Token: 0x02001E34 RID: 7732
		private class Child
		{
			// Token: 0x040071A4 RID: 29092
			public CellRect rect;

			// Token: 0x040071A5 RID: 29093
			public int gridX;

			// Token: 0x040071A6 RID: 29094
			public int gridY;

			// Token: 0x040071A7 RID: 29095
			public bool merged;
		}
	}
}
