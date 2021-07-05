using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C69 RID: 3177
	public static class ComplexUtility
	{
		// Token: 0x06004A35 RID: 18997 RVA: 0x001883EC File Offset: 0x001865EC
		public static bool TryFindRandomSpawnCell(ThingDef def, IEnumerable<IntVec3> cells, Map map, out IntVec3 spawnPosition, int gap = 1, Rot4? rot = null)
		{
			foreach (IntVec3 intVec in cells.InRandomOrder(null))
			{
				CellRect cellRect = GenAdj.OccupiedRect(intVec, rot ?? Rot4.North, def.Size).ExpandedBy(gap);
				bool flag = false;
				foreach (IntVec3 c in cellRect)
				{
					if (!c.InBounds(map) || c.GetEdifice(map) != null || c.GetFirstPawn(map) != null)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					spawnPosition = intVec;
					return true;
				}
			}
			spawnPosition = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06004A36 RID: 18998 RVA: 0x001884E8 File Offset: 0x001866E8
		public static string SpawnRoomEnteredTrigger(List<CellRect> room, Map map)
		{
			string text = "RoomEntered" + Find.UniqueIDsManager.GetNextSignalTagID();
			foreach (CellRect rect in room)
			{
				RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
				rectTrigger.signalTag = text;
				rectTrigger.Rect = rect;
				GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			}
			return text;
		}

		// Token: 0x06004A37 RID: 18999 RVA: 0x00188578 File Offset: 0x00186778
		public static string SpawnRadialDistanceTrigger(IEnumerable<Thing> things, Map map, int radius)
		{
			string text = "RandomTrigger" + Find.UniqueIDsManager.GetNextSignalTagID();
			foreach (Thing thing in things)
			{
				RadialTrigger radialTrigger = (RadialTrigger)ThingMaker.MakeThing(ThingDefOf.RadialTrigger, null);
				radialTrigger.signalTag = text;
				radialTrigger.maxRadius = radius;
				radialTrigger.lineOfSight = true;
				GenSpawn.Spawn(radialTrigger, thing.Position, map, WipeMode.Vanish);
			}
			return text;
		}
	}
}
