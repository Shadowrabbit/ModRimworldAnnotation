using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001043 RID: 4163
	public static class InstallBlueprintUtility
	{
		// Token: 0x0600626D RID: 25197 RVA: 0x00215C78 File Offset: 0x00213E78
		public static void CancelBlueprintsFor(Thing th)
		{
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(th);
			if (blueprint_Install != null)
			{
				blueprint_Install.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x0600626E RID: 25198 RVA: 0x00215C98 File Offset: 0x00213E98
		public static Blueprint_Install ExistingBlueprintFor(Thing th)
		{
			List<Map> maps = Find.Maps;
			Thing innerIfMinified = th.GetInnerIfMinified();
			for (int i = 0; i < maps.Count; i++)
			{
				Blueprint_Install result;
				if (maps[i].listerBuildings.TryGetReinstallBlueprint(innerIfMinified, out result))
				{
					return result;
				}
			}
			return null;
		}
	}
}
