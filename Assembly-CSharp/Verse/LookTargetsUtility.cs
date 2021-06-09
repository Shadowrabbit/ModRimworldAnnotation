using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020007DF RID: 2015
	public static class LookTargetsUtility
	{
		// Token: 0x060032C8 RID: 13000 RVA: 0x00027D9C File Offset: 0x00025F9C
		public static bool IsValid(this LookTargets lookTargets)
		{
			return lookTargets != null && lookTargets.IsValid;
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x00027DA9 File Offset: 0x00025FA9
		public static GlobalTargetInfo TryGetPrimaryTarget(this LookTargets lookTargets)
		{
			if (lookTargets == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			return lookTargets.PrimaryTarget;
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x00027DBA File Offset: 0x00025FBA
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
