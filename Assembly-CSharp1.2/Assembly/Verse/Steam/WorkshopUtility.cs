using System;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x020008C3 RID: 2243
	internal static class WorkshopUtility
	{
		// Token: 0x060037D2 RID: 14290 RVA: 0x0002B220 File Offset: 0x00029420
		public static string GetLabel(this WorkshopInteractStage stage)
		{
			if (stage == WorkshopInteractStage.None)
			{
				return "None".Translate();
			}
			return ("WorkshopInteractStage_" + stage.ToString()).Translate();
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x0002B256 File Offset: 0x00029456
		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x0002B279 File Offset: 0x00029479
		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}
