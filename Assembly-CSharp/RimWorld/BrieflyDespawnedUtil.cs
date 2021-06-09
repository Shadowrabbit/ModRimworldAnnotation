using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001716 RID: 5910
	public static class BrieflyDespawnedUtil
	{
		// Token: 0x06008241 RID: 33345 RVA: 0x0005779D File Offset: 0x0005599D
		public static bool BrieflyDespawned(this Pawn pawn)
		{
			return !pawn.Spawned && pawn.ParentHolder is PawnFlyer;
		}
	}
}
