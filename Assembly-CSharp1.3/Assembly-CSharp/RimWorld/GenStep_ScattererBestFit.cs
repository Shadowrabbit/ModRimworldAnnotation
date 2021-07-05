using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001539 RID: 5433
	public abstract class GenStep_ScattererBestFit : GenStep_Scatterer
	{
		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x06008143 RID: 33091
		protected abstract IntVec2 Size { get; }

		// Token: 0x06008144 RID: 33092
		public abstract bool CollisionAt(IntVec3 cell, Map map);

		// Token: 0x06008145 RID: 33093 RVA: 0x002DBC24 File Offset: 0x002D9E24
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false)))
			{
				return false;
			}
			CellRect cellRect = CellRect.CenteredOn(c, this.Size.x, this.Size.z);
			if (!cellRect.FullyContainedWithin(CellRect.WholeMap(map)))
			{
				return false;
			}
			int num = 0;
			foreach (IntVec3 intVec in cellRect)
			{
				if (!intVec.InBounds(map))
				{
					return false;
				}
				List<Thing> thingList = intVec.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable)
					{
						return false;
					}
				}
				if (this.CollisionAt(intVec, map))
				{
					num++;
				}
			}
			if (num < GenStep_ScattererBestFit.bestOccupiedScore)
			{
				GenStep_ScattererBestFit.bestOccupiedScore = num;
				GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions = new IntVec3?(c);
			}
			return num == 0;
		}

		// Token: 0x06008146 RID: 33094 RVA: 0x002DBD50 File Offset: 0x002D9F50
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			GenStep_ScattererBestFit.bestOccupiedScore = int.MaxValue;
			GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions = null;
			if (this.CanScatterAt(map.Center, map))
			{
				result = map.Center;
				return true;
			}
			int num = Mathf.FloorToInt((float)(Mathf.Min(map.Size.x, map.Size.z) / 2));
			Vector3 v = IntVec3.North.ToVector3();
			for (int i = 5; i <= num; i += 5)
			{
				for (int j = 0; j < 5; j++)
				{
					float angle = Rand.Range(0f, 360f);
					Vector3 a = v.RotatedBy(angle);
					int num2 = Rand.Range(i - 5, i);
					IntVec3 intVec = map.Center + (a * (float)num2).ToIntVec3();
					if (this.CanScatterAt(intVec, map))
					{
						result = intVec;
						return true;
					}
				}
			}
			if (GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions != null)
			{
				result = GenStep_ScattererBestFit.bestCellWithLeastOccupiedCollisions.Value;
				return true;
			}
			if (this.warnOnFail)
			{
				Log.Warning("Scatterer " + this.ToString() + " could not find cell to generate at.");
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x040050A9 RID: 20649
		private const int DistanceStep = 5;

		// Token: 0x040050AA RID: 20650
		private const int AttemptsPerStep = 5;

		// Token: 0x040050AB RID: 20651
		private static IntVec3? bestCellWithLeastOccupiedCollisions;

		// Token: 0x040050AC RID: 20652
		private static int bestOccupiedScore;
	}
}
