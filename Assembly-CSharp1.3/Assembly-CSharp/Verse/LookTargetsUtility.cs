using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200047F RID: 1151
	public static class LookTargetsUtility
	{
		// Token: 0x060022FF RID: 8959 RVA: 0x000DBF52 File Offset: 0x000DA152
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000DBF5F File Offset: 0x000DA15F
		public static GlobalTargetInfo TryGetPrimaryTarget(this LookTargets lookTargets)
		{
			if (lookTargets == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			return lookTargets.PrimaryTarget;
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x000DBF70 File Offset: 0x000DA170
		public static void TryHighlight(this LookTargets lookTargets, bool arrow = true, bool colonistBar = true, bool circleOverlay = false)
		{
			if (lookTargets == null)
			{
				return;
			}
			lookTargets.Highlight(arrow, colonistBar, circleOverlay);
		}
	}
}
