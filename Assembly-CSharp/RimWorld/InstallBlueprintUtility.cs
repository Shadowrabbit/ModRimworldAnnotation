using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200166C RID: 5740
	public static class InstallBlueprintUtility
	{
		// Token: 0x06007D2A RID: 32042 RVA: 0x00255E20 File Offset: 0x00254020
		public static void CancelBlueprintsFor(Thing th)
		{
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(th);
			if (blueprint_Install != null)
			{
				blueprint_Install.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x06007D2B RID: 32043 RVA: 0x00255E40 File Offset: 0x00254040
		public static Blueprint_Install ExistingBlueprintFor(Thing th)
		{
			ThingDef installBlueprintDef = th.GetInnerIfMinified().def.installBlueprintDef;
			if (installBlueprintDef == null)
			{
				return null;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsMatching(ThingRequest.ForDef(installBlueprintDef));
				for (int j = 0; j < list.Count; j++)
				{
					Blueprint_Install blueprint_Install = list[j] as Blueprint_Install;
					if (blueprint_Install != null && blueprint_Install.MiniToInstallOrBuildingToReinstall == th)
					{
						return blueprint_Install;
					}
				}
			}
			return null;
		}
	}
}
