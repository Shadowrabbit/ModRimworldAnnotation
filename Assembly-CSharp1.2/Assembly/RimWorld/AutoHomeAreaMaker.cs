using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF2 RID: 7154
	public static class AutoHomeAreaMaker
	{
		// Token: 0x06009D76 RID: 40310 RVA: 0x00068D93 File Offset: 0x00066F93
		private static bool ShouldAdd()
		{
			return Find.PlaySettings.autoHomeArea && Current.ProgramState == ProgramState.Playing;
		}

		// Token: 0x06009D77 RID: 40311 RVA: 0x00068DAB File Offset: 0x00066FAB
		public static void Notify_BuildingSpawned(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x06009D78 RID: 40312 RVA: 0x00068DAB File Offset: 0x00066FAB
		public static void Notify_BuildingClaimed(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x06009D79 RID: 40313 RVA: 0x002E1238 File Offset: 0x002DF438
		public static void MarkHomeAroundThing(Thing t)
		{
			if (!AutoHomeAreaMaker.ShouldAdd())
			{
				return;
			}
			CellRect cellRect = new CellRect(t.Position.x - t.RotatedSize.x / 2 - 4, t.Position.z - t.RotatedSize.z / 2 - 4, t.RotatedSize.x + 8, t.RotatedSize.z + 8);
			cellRect.ClipInsideMap(t.Map);
			foreach (IntVec3 c in cellRect)
			{
				t.Map.areaManager.Home[c] = true;
			}
		}

		// Token: 0x06009D7A RID: 40314 RVA: 0x002E1304 File Offset: 0x002DF504
		public static void Notify_ZoneCellAdded(IntVec3 c, Zone zone)
		{
			if (!AutoHomeAreaMaker.ShouldAdd())
			{
				return;
			}
			foreach (IntVec3 c2 in CellRect.CenteredOn(c, 4).ClipInsideMap(zone.Map))
			{
				zone.Map.areaManager.Home[c2] = true;
			}
		}

		// Token: 0x0400643A RID: 25658
		private const int BorderWidth = 4;
	}
}
