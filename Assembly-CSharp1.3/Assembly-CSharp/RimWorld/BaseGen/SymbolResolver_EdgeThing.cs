using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015B6 RID: 5558
	public class SymbolResolver_EdgeThing : SymbolResolver
	{
		// Token: 0x060082FF RID: 33535 RVA: 0x002E9968 File Offset: 0x002E7B68
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.singleThingDef != null)
			{
				bool avoidOtherEdgeThings = rp.edgeThingAvoidOtherEdgeThings ?? false;
				bool mustReachMapEdge = rp.edgeThingMustReachMapEdge ?? false;
				if (rp.thingRot != null)
				{
					IntVec3 intVec;
					if (!this.TryFindSpawnCell(rp.rect, rp.singleThingDef, rp.thingRot.Value, avoidOtherEdgeThings, mustReachMapEdge, out intVec))
					{
						return false;
					}
				}
				else if (!rp.singleThingDef.rotatable)
				{
					IntVec3 intVec;
					if (!this.TryFindSpawnCell(rp.rect, rp.singleThingDef, Rot4.North, avoidOtherEdgeThings, mustReachMapEdge, out intVec))
					{
						return false;
					}
				}
				else
				{
					bool flag = false;
					for (int i = 0; i < 4; i++)
					{
						IntVec3 intVec;
						if (this.TryFindSpawnCell(rp.rect, rp.singleThingDef, new Rot4(i), avoidOtherEdgeThings, mustReachMapEdge, out intVec))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008300 RID: 33536 RVA: 0x002E9A60 File Offset: 0x002E7C60
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.singleThingDef ?? (from x in DefDatabase<ThingDef>.AllDefsListForReading
			where (x.IsWeapon || x.IsMedicine || x.IsDrug) && x.graphicData != null && !x.destroyOnDrop && x.size.x <= rp.rect.Width && x.size.z <= rp.rect.Width && x.size.x <= rp.rect.Height && x.size.z <= rp.rect.Height
			select x).RandomElement<ThingDef>();
			IntVec3 invalid = IntVec3.Invalid;
			Rot4 value = Rot4.North;
			bool avoidOtherEdgeThings = rp.edgeThingAvoidOtherEdgeThings ?? false;
			bool mustReachMapEdge = rp.edgeThingMustReachMapEdge ?? false;
			if (rp.thingRot != null)
			{
				if (!this.TryFindSpawnCell(rp.rect, thingDef, rp.thingRot.Value, avoidOtherEdgeThings, mustReachMapEdge, out invalid))
				{
					return;
				}
				value = rp.thingRot.Value;
			}
			else if (!thingDef.rotatable)
			{
				if (!this.TryFindSpawnCell(rp.rect, thingDef, Rot4.North, avoidOtherEdgeThings, mustReachMapEdge, out invalid))
				{
					return;
				}
				value = Rot4.North;
			}
			else
			{
				this.randomRotations.Shuffle<int>();
				bool flag = false;
				for (int i = 0; i < this.randomRotations.Count; i++)
				{
					if (this.TryFindSpawnCell(rp.rect, thingDef, new Rot4(this.randomRotations[i]), avoidOtherEdgeThings, mustReachMapEdge, out invalid))
					{
						value = new Rot4(this.randomRotations[i]);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			ResolveParams rp2 = rp;
			rp2.rect = CellRect.SingleCell(invalid);
			rp2.thingRot = new Rot4?(value);
			rp2.singleThingDef = thingDef;
			BaseGen.symbolStack.Push("thing", rp2, null);
		}

		// Token: 0x06008301 RID: 33537 RVA: 0x002E9C28 File Offset: 0x002E7E28
		private bool TryFindSpawnCell(CellRect rect, ThingDef thingDef, Rot4 rot, bool avoidOtherEdgeThings, bool mustReachMapEdge, out IntVec3 spawnCell)
		{
			if (avoidOtherEdgeThings)
			{
				spawnCell = IntVec3.Invalid;
				int num = -1;
				for (int i = 0; i < this.MaxTriesToAvoidOtherEdgeThings; i++)
				{
					IntVec3 intVec;
					if (this.TryFindSpawnCell(rect, thingDef, rot, mustReachMapEdge, out intVec))
					{
						int distanceSquaredToExistingEdgeThing = this.GetDistanceSquaredToExistingEdgeThing(intVec, rect, thingDef);
						if (!spawnCell.IsValid || distanceSquaredToExistingEdgeThing > num)
						{
							spawnCell = intVec;
							num = distanceSquaredToExistingEdgeThing;
							if (num == 2147483647)
							{
								break;
							}
						}
					}
				}
				return spawnCell.IsValid;
			}
			return this.TryFindSpawnCell(rect, thingDef, rot, mustReachMapEdge, out spawnCell);
		}

		// Token: 0x06008302 RID: 33538 RVA: 0x002E9CA8 File Offset: 0x002E7EA8
		private bool TryFindSpawnCell(CellRect rect, ThingDef thingDef, Rot4 rot, bool mustReachMapEdge, out IntVec3 spawnCell)
		{
			Map map = BaseGen.globalSettings.map;
			IntVec3 zero = IntVec3.Zero;
			IntVec2 size = thingDef.size;
			GenAdj.AdjustForRotation(ref zero, ref size, rot);
			CellRect empty = CellRect.Empty;
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false);
			Func<IntVec3, bool> <>9__3;
			Predicate<CellRect> basePredicate = delegate(CellRect x)
			{
				IEnumerable<IntVec3> cells = x.Cells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__3) == null)
				{
					predicate = (<>9__3 = ((IntVec3 y) => y.Standable(map)));
				}
				if (cells.All(predicate))
				{
					if (!GenSpawn.WouldWipeAnythingWith(x, thingDef, map, (Thing z) => z.def.category == ThingCategory.Building) && (thingDef.category != ThingCategory.Item || x.CenterCell.GetFirstItem(map) == null))
					{
						return !mustReachMapEdge || map.reachability.CanReachMapEdge(x.CenterCell, traverseParms);
					}
				}
				return false;
			};
			bool flag = false;
			if (thingDef.category == ThingCategory.Building)
			{
				flag = rect.TryFindRandomInnerRectTouchingEdge(size, out empty, (CellRect x) => basePredicate(x) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(x, map) && GenConstruct.TerrainCanSupport(x, map, thingDef));
				if (!flag)
				{
					flag = rect.TryFindRandomInnerRectTouchingEdge(size, out empty, (CellRect x) => basePredicate(x) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(x, map));
				}
			}
			if (!flag && !rect.TryFindRandomInnerRectTouchingEdge(size, out empty, basePredicate))
			{
				spawnCell = IntVec3.Invalid;
				return false;
			}
			foreach (IntVec3 intVec in empty)
			{
				if (GenAdj.OccupiedRect(intVec, rot, thingDef.size) == empty)
				{
					spawnCell = intVec;
					return true;
				}
			}
			Log.Error("We found a valid rect but we couldn't find the root position. This should never happen.");
			spawnCell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06008303 RID: 33539 RVA: 0x002E9E08 File Offset: 0x002E8008
		private int GetDistanceSquaredToExistingEdgeThing(IntVec3 cell, CellRect rect, ThingDef thingDef)
		{
			Map map = BaseGen.globalSettings.map;
			int num = int.MaxValue;
			foreach (IntVec3 intVec in rect.EdgeCells)
			{
				List<Thing> thingList = intVec.GetThingList(map);
				bool flag = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def == thingDef)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					num = Mathf.Min(num, cell.DistanceToSquared(intVec));
				}
			}
			return num;
		}

		// Token: 0x040051F3 RID: 20979
		private List<int> randomRotations = new List<int>
		{
			0,
			1,
			2,
			3
		};

		// Token: 0x040051F4 RID: 20980
		private int MaxTriesToAvoidOtherEdgeThings = 4;
	}
}
