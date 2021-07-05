using System;
using Steamworks;

namespace Verse.Steam
{
	// Token: 0x02000507 RID: 1287
	internal static class WorkshopUtility
	{
		// Token: 0x06002700 RID: 9984 RVA: 0x000F1020 File Offset: 0x000EF220
		public static string GetLabel(this WorkshopInteractStage stage)
		{
			if (stage == WorkshopInteractStage.None)
			{
				return "None".Translate();
			}
			return ("WorkshopInteractStage_" + stage.ToString()).Translate();
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000F1056 File Offset: 0x000EF256
		public static string GetLabel(this EItemUpdateStatus status)
		{
			return ("EItemUpdateStatus_" + status.ToString()).Translate();
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000F1079 File Offset: 0x000EF279
		public static string GetLabel(this EResult result)
		{
			return result.ToString().Substring(9);
		}
	}
}
