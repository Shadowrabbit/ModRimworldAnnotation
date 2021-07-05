using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125C RID: 4700
	public class Alert_NeedColonistBeds : Alert
	{
		// Token: 0x060070A6 RID: 28838 RVA: 0x0025865A File Offset: 0x0025685A
		public Alert_NeedColonistBeds()
		{
			this.defaultLabel = "NeedColonistBeds".Translate();
			this.defaultExplanation = "NeedColonistBedsDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x060070A7 RID: 28839 RVA: 0x00258694 File Offset: 0x00256894
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed > 30)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedColonistBeds(maps[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060070A8 RID: 28840 RVA: 0x002586E4 File Offset: 0x002568E4
		private bool NeedColonistBeds(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building_Bed building_Bed = allBuildingsColonist[i] as Building_Bed;
				if (building_Bed != null && building_Bed.ForColonists && !building_Bed.Medical && building_Bed.def.building.bed_humanlike)
				{
					if (building_Bed.SleepingSlotsCount == 1)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
			int num3 = 0;
			int num4 = 0;
			IEnumerable<Pawn> enumerable = from p in map.mapPawns.FreeColonists
			where !p.IsSlave
			select p;
			foreach (Pawn pawn in enumerable)
			{
				if ((pawn.Spawned || pawn.BrieflyDespawned()) && !Alert_NeedColonistBeds.tmpAssignedPawns.Contains(pawn))
				{
					List<DirectPawnRelation> list = LovePartnerRelationUtility.ExistingLovePartners(pawn, false);
					if (list.NullOrEmpty<DirectPawnRelation>())
					{
						num3++;
						Alert_NeedColonistBeds.tmpAssignedPawns.Add(pawn);
					}
					else
					{
						Pawn pawn2 = null;
						int num5 = int.MaxValue;
						for (int j = 0; j < list.Count; j++)
						{
							Pawn otherPawn = list[j].otherPawn;
							if (otherPawn != null && otherPawn.Spawned && otherPawn.Map == pawn.Map && otherPawn.Faction == Faction.OfPlayer && otherPawn.HostFaction == null && !Alert_NeedColonistBeds.tmpAssignedPawns.Contains(otherPawn))
							{
								int num6 = LovePartnerRelationUtility.ExistingLovePartnersCount(otherPawn, false);
								if (pawn2 == null || num6 < num5)
								{
									pawn2 = otherPawn;
									num5 = num6;
								}
							}
						}
						if (pawn2 != null)
						{
							Alert_NeedColonistBeds.tmpAssignedPawns.Add(pawn2);
							Alert_NeedColonistBeds.tmpAssignedPawns.Add(pawn);
							num4++;
						}
					}
				}
			}
			foreach (Pawn item in enumerable)
			{
				if (!Alert_NeedColonistBeds.tmpAssignedPawns.Contains(item))
				{
					num3++;
				}
			}
			Alert_NeedColonistBeds.tmpAssignedPawns.Clear();
			for (int k = 0; k < num4; k++)
			{
				if (num2 > 0)
				{
					num2--;
				}
				else
				{
					num -= 2;
				}
			}
			for (int l = 0; l < num3; l++)
			{
				if (num2 > 0)
				{
					num2--;
				}
				else
				{
					num--;
				}
			}
			return num < 0 || num2 < 0;
		}

		// Token: 0x04003E1D RID: 15901
		private static List<Pawn> tmpAssignedPawns = new List<Pawn>();
	}
}
