using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195F RID: 6495
	public class Alert_ThroneroomInvalidConfiguration : Alert
	{
		// Token: 0x06008FC1 RID: 36801 RVA: 0x00060592 File Offset: 0x0005E792
		public Alert_ThroneroomInvalidConfiguration()
		{
			this.defaultLabel = "ThroneroomInvalidConfiguration".Translate();
			this.defaultExplanation = "ThroneroomInvalidConfigurationDesc".Translate();
		}

		// Token: 0x06008FC2 RID: 36802 RVA: 0x000605C4 File Offset: 0x0005E7C4
		public override TaggedString GetExplanation()
		{
			return base.GetExplanation() + "\n\n" + Alert_ThroneroomInvalidConfiguration.validationInfo;
		}

		// Token: 0x06008FC3 RID: 36803 RVA: 0x002963B0 File Offset: 0x002945B0
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.Throne))
				{
					Building_Throne building_Throne = (Building_Throne)thing;
					Alert_ThroneroomInvalidConfiguration.validationInfo = RoomRoleWorker_ThroneRoom.Validate(building_Throne.GetRoom(RegionType.Set_Passable));
					if (Alert_ThroneroomInvalidConfiguration.validationInfo != null)
					{
						return AlertReport.CulpritIs(building_Throne);
					}
				}
			}
			return false;
		}

		// Token: 0x04005B82 RID: 23426
		private static string validationInfo;
	}
}
