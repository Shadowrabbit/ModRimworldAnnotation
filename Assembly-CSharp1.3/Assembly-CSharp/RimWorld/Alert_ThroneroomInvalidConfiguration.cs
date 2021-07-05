using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001270 RID: 4720
	public class Alert_ThroneroomInvalidConfiguration : Alert
	{
		// Token: 0x060070F6 RID: 28918 RVA: 0x0025A3F3 File Offset: 0x002585F3
		public Alert_ThroneroomInvalidConfiguration()
		{
			this.defaultLabel = "ThroneroomInvalidConfiguration".Translate();
			this.defaultExplanation = "ThroneroomInvalidConfigurationDesc".Translate();
		}

		// Token: 0x060070F7 RID: 28919 RVA: 0x0025A425 File Offset: 0x00258625
		public override TaggedString GetExplanation()
		{
			return base.GetExplanation() + "\n\n" + Alert_ThroneroomInvalidConfiguration.validationInfo;
		}

		// Token: 0x060070F8 RID: 28920 RVA: 0x0025A444 File Offset: 0x00258644
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.Throne))
				{
					Building_Throne building_Throne = (Building_Throne)thing;
					Alert_ThroneroomInvalidConfiguration.validationInfo = RoomRoleWorker_ThroneRoom.Validate(building_Throne.GetRoom(RegionType.Set_All));
					if (Alert_ThroneroomInvalidConfiguration.validationInfo != null)
					{
						return AlertReport.CulpritIs(building_Throne);
					}
				}
			}
			return false;
		}

		// Token: 0x04003E31 RID: 15921
		private static string validationInfo;
	}
}
