using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC5 RID: 3269
	public class JobGiver_ManTurretsNearSelf : JobGiver_ManTurrets
	{
		// Token: 0x06004BA4 RID: 19364 RVA: 0x0003044E File Offset: 0x0002E64E
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
