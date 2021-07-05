using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B9 RID: 4281
	public static class BrieflyDespawnedUtil
	{
		// Token: 0x0600664E RID: 26190 RVA: 0x00228B22 File Offset: 0x00226D22
		public static bool BrieflyDespawned(this Pawn pawn)
		{
			return !pawn.Spawned && pawn.ParentHolder is PawnFlyer;
		}
	}
}
