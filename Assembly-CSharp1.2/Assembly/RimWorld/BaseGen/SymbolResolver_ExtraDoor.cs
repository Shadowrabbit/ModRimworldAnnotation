using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E5A RID: 7770
	public class SymbolResolver_ExtraDoor : SymbolResolver
	{
		// Token: 0x0600A7A7 RID: 42919 RVA: 0x0030CB40 File Offset: 0x0030AD40
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec3 intVec = IntVec3.Invalid;
			int num = -1;
			for (int i = 0; i < 4; i++)
			{
				if (!this.WallHasDoor(rp.rect, new Rot4(i)))
				{
					for (int j = 0; j < 2; j++)
					{
						IntVec3 intVec2;
						if (this.TryFindRandomDoorSpawnCell(rp.rect, new Rot4(i), out intVec2))
						{
							int distanceToExistingDoors = this.GetDistanceToExistingDoors(intVec2, rp.rect);
							if (!intVec.IsValid || distanceToExistingDoors > num)
							{
								intVec = intVec2;
								num = distanceToExistingDoors;
								if (num == 2147483647)
								{
									break;
								}
							}
						}
					}
				}
			}
			if (intVec.IsValid)
			{
				ThingDef thingDef;
				if ((thingDef = rp.wallStuff) == null)
				{
					thingDef = (BaseGenUtility.WallStuffAt(intVec, map) ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false));
				}
				ThingDef stuff = thingDef;
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Door, stuff);
				thing.SetFaction(rp.faction, null);
				GenSpawn.Spawn(thing, intVec, BaseGen.globalSettings.map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600A7A8 RID: 42920 RVA: 0x0030CC2C File Offset: 0x0030AE2C
		private bool WallHasDoor(CellRect rect, Rot4 dir)
		{
			Map map = BaseGen.globalSettings.map;
			using (IEnumerator<IntVec3> enumerator = rect.GetEdgeCells(dir).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetDoor(map) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600A7A9 RID: 42921 RVA: 0x0030CC90 File Offset: 0x0030AE90
		private bool TryFindRandomDoorSpawnCell(CellRect rect, Rot4 dir, out IntVec3 found)
		{
			Map map = BaseGen.globalSettings.map;
			if (dir == Rot4.North)
			{
				if (rect.Width <= 2)
				{
					found = IntVec3.Invalid;
					return false;
				}
				int newX;
				if (!Rand.TryRangeInclusiveWhere(rect.minX + 1, rect.maxX - 1, delegate(int x)
				{
					IntVec3 cell = new IntVec3(x, 0, rect.maxZ + 1);
					IntVec3 cell2 = new IntVec3(x, 0, rect.maxZ - 1);
					return this.CanPassThrough(cell, map) && this.CanPassThrough(cell2, map);
				}, out newX))
				{
					found = IntVec3.Invalid;
					return false;
				}
				found = new IntVec3(newX, 0, rect.maxZ);
				return true;
			}
			else if (dir == Rot4.South)
			{
				if (rect.Width <= 2)
				{
					found = IntVec3.Invalid;
					return false;
				}
				int newX2;
				if (!Rand.TryRangeInclusiveWhere(rect.minX + 1, rect.maxX - 1, delegate(int x)
				{
					IntVec3 cell = new IntVec3(x, 0, rect.minZ - 1);
					IntVec3 cell2 = new IntVec3(x, 0, rect.minZ + 1);
					return this.CanPassThrough(cell, map) && this.CanPassThrough(cell2, map);
				}, out newX2))
				{
					found = IntVec3.Invalid;
					return false;
				}
				found = new IntVec3(newX2, 0, rect.minZ);
				return true;
			}
			else if (dir == Rot4.West)
			{
				if (rect.Height <= 2)
				{
					found = IntVec3.Invalid;
					return false;
				}
				int newZ;
				if (!Rand.TryRangeInclusiveWhere(rect.minZ + 1, rect.maxZ - 1, delegate(int z)
				{
					IntVec3 cell = new IntVec3(rect.minX - 1, 0, z);
					IntVec3 cell2 = new IntVec3(rect.minX + 1, 0, z);
					return this.CanPassThrough(cell, map) && this.CanPassThrough(cell2, map);
				}, out newZ))
				{
					found = IntVec3.Invalid;
					return false;
				}
				found = new IntVec3(rect.minX, 0, newZ);
				return true;
			}
			else
			{
				if (rect.Height <= 2)
				{
					found = IntVec3.Invalid;
					return false;
				}
				int newZ2;
				if (!Rand.TryRangeInclusiveWhere(rect.minZ + 1, rect.maxZ - 1, delegate(int z)
				{
					IntVec3 cell = new IntVec3(rect.maxX + 1, 0, z);
					IntVec3 cell2 = new IntVec3(rect.maxX - 1, 0, z);
					return this.CanPassThrough(cell, map) && this.CanPassThrough(cell2, map);
				}, out newZ2))
				{
					found = IntVec3.Invalid;
					return false;
				}
				found = new IntVec3(rect.maxX, 0, newZ2);
				return true;
			}
		}

		// Token: 0x0600A7AA RID: 42922 RVA: 0x0006EB05 File Offset: 0x0006CD05
		private bool CanPassThrough(IntVec3 cell, Map map)
		{
			return cell.InBounds(map) && cell.Standable(map) && cell.GetEdifice(map) == null;
		}

		// Token: 0x0600A7AB RID: 42923 RVA: 0x0030CEAC File Offset: 0x0030B0AC
		private int GetDistanceToExistingDoors(IntVec3 cell, CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			int num = int.MaxValue;
			foreach (IntVec3 intVec in rect.EdgeCells)
			{
				if (intVec.GetDoor(map) != null)
				{
					num = Mathf.Min(num, Mathf.Abs(cell.x - intVec.x) + Mathf.Abs(cell.z - intVec.z));
				}
			}
			return num;
		}
	}
}
