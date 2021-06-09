using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E51 RID: 7761
	public class SymbolResolver_EnsureCanHoldRoof : SymbolResolver
	{
		// Token: 0x0600A788 RID: 42888 RVA: 0x0030C2F4 File Offset: 0x0030A4F4
		public override void Resolve(ResolveParams rp)
		{
			ThingDef wallStuff = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			do
			{
				this.CalculateRoofsAboutToCollapse(rp.rect);
				this.CalculateEdgeRoofs(rp.rect);
			}
			while (this.TrySpawnPillar(rp.faction, wallStuff));
		}

		// Token: 0x0600A789 RID: 42889 RVA: 0x0030C340 File Offset: 0x0030A540
		private void CalculateRoofsAboutToCollapse(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Clear();
			SymbolResolver_EnsureCanHoldRoof.visited.Clear();
			Predicate<IntVec3> <>9__0;
			foreach (IntVec3 intVec in rect)
			{
				if (intVec.Roofed(map) && !RoofCollapseCellsFinder.ConnectsToRoofHolder(intVec, map, SymbolResolver_EnsureCanHoldRoof.visited))
				{
					FloodFiller floodFiller = map.floodFiller;
					IntVec3 root = intVec;
					Predicate<IntVec3> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((IntVec3 x) => x.Roofed(map)));
					}
					floodFiller.FloodFill(root, passCheck, delegate(IntVec3 x)
					{
						SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Add(x);
					}, int.MaxValue, false, null);
				}
			}
			foreach (IntVec3 intVec2 in rect)
			{
				if (intVec2.Roofed(map) && !SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(intVec2) && !RoofCollapseUtility.WithinRangeOfRoofHolder(intVec2, map, false))
				{
					SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Add(intVec2);
				}
			}
		}

		// Token: 0x0600A78A RID: 42890 RVA: 0x0030C4A0 File Offset: 0x0030A6A0
		private void CalculateEdgeRoofs(CellRect rect)
		{
			SymbolResolver_EnsureCanHoldRoof.edgeRoofs.Clear();
			foreach (IntVec3 intVec in SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse)
			{
				for (int i = 0; i < 4; i++)
				{
					IntVec3 item = intVec + GenAdj.CardinalDirections[i];
					if (!SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(item))
					{
						SymbolResolver_EnsureCanHoldRoof.edgeRoofs.Add(intVec);
						break;
					}
				}
			}
		}

		// Token: 0x0600A78B RID: 42891 RVA: 0x0030C530 File Offset: 0x0030A730
		private bool TrySpawnPillar(Faction faction, ThingDef wallStuff)
		{
			if (!SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Any<IntVec3>())
			{
				return false;
			}
			Map map = BaseGen.globalSettings.map;
			IntVec3 bestCell = IntVec3.Invalid;
			float bestScore = 0f;
			map.floodFiller.FloodFill(IntVec3.Invalid, (IntVec3 x) => SymbolResolver_EnsureCanHoldRoof.roofsAboutToCollapse.Contains(x), delegate(IntVec3 x)
			{
				float pillarSpawnScore = this.GetPillarSpawnScore(x);
				if (pillarSpawnScore > 0f && (!bestCell.IsValid || pillarSpawnScore >= bestScore))
				{
					bestCell = x;
					bestScore = pillarSpawnScore;
				}
			}, int.MaxValue, false, SymbolResolver_EnsureCanHoldRoof.edgeRoofs);
			if (bestCell.IsValid)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, wallStuff);
				thing.SetFaction(faction, null);
				GenSpawn.Spawn(thing, bestCell, map, WipeMode.Vanish);
				return true;
			}
			return false;
		}

		// Token: 0x0600A78C RID: 42892 RVA: 0x0030C5F0 File Offset: 0x0030A7F0
		private float GetPillarSpawnScore(IntVec3 c)
		{
			Map map = BaseGen.globalSettings.map;
			if (c.Impassable(map) || c.GetFirstBuilding(map) != null || c.GetFirstItem(map) != null || c.GetFirstPawn(map) != null)
			{
				return 0f;
			}
			bool flag = true;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[i];
				if (!c2.InBounds(map) || !c2.Walkable(map))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return 2f;
			}
			return 1f;
		}

		// Token: 0x040071CC RID: 29132
		private static HashSet<IntVec3> roofsAboutToCollapse = new HashSet<IntVec3>();

		// Token: 0x040071CD RID: 29133
		private static List<IntVec3> edgeRoofs = new List<IntVec3>();

		// Token: 0x040071CE RID: 29134
		private static HashSet<IntVec3> visited = new HashSet<IntVec3>();
	}
}
