using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E3 RID: 5091
	public static class AutoHomeAreaMaker
	{
		// Token: 0x06007BD5 RID: 31701 RVA: 0x002BAC7C File Offset: 0x002B8E7C
		private static bool ShouldAdd()
		{
			return Find.PlaySettings.autoHomeArea && Current.ProgramState == ProgramState.Playing;
		}

		// Token: 0x06007BD6 RID: 31702 RVA: 0x002BAC94 File Offset: 0x002B8E94
		public static void Notify_BuildingSpawned(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x06007BD7 RID: 31703 RVA: 0x002BAC94 File Offset: 0x002B8E94
		public static void Notify_BuildingClaimed(Thing b)
		{
			if (!AutoHomeAreaMaker.ShouldAdd() || !b.def.building.expandHomeArea || b.Faction != Faction.OfPlayer)
			{
				return;
			}
			AutoHomeAreaMaker.MarkHomeAroundThing(b);
		}

		// Token: 0x06007BD8 RID: 31704 RVA: 0x002BACC4 File Offset: 0x002B8EC4
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

		// Token: 0x06007BD9 RID: 31705 RVA: 0x002BAD90 File Offset: 0x002B8F90
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

		// Token: 0x04004493 RID: 17555
		private const int BorderWidth = 4;
	}
}
