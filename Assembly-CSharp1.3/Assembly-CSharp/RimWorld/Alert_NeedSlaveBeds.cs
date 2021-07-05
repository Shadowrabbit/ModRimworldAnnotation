using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125D RID: 4701
	public class Alert_NeedSlaveBeds : Alert
	{
		// Token: 0x060070AA RID: 28842 RVA: 0x002589A0 File Offset: 0x00256BA0
		public Alert_NeedSlaveBeds()
		{
			this.defaultLabel = "NeedSlaveBeds".Translate();
			this.defaultExplanation = "NeedSlaveBedsDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x060070AB RID: 28843 RVA: 0x002589DC File Offset: 0x00256BDC
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed > 30)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedSlaveBeds(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060070AC RID: 28844 RVA: 0x00258A2C File Offset: 0x00256C2C
		private bool NeedSlaveBeds(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			int num = 0;
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building_Bed building_Bed = allBuildingsColonist[i] as Building_Bed;
				if (building_Bed != null && building_Bed.ForSlaves && !building_Bed.Medical && building_Bed.def.building.bed_humanlike)
				{
					num += building_Bed.TotalSleepingSlots;
				}
			}
			int num2 = 0;
			foreach (Pawn pawn in map.mapPawns.FreeColonists)
			{
				if ((pawn.Spawned || pawn.BrieflyDespawned()) && pawn.IsSlave)
				{
					num2++;
				}
			}
			return num < num2;
		}
	}
}
