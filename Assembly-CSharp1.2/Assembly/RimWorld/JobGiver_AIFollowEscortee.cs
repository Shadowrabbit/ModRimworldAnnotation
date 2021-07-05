using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C99 RID: 3225
	public class JobGiver_AIFollowEscortee : JobGiver_AIFollowPawn
	{
		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06004B28 RID: 19240 RVA: 0x00035A08 File Offset: 0x00033C08
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 120;
			}
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x00035A0C File Offset: 0x00033C0C
		protected override Pawn GetFollowee(Pawn pawn)
		{
			return (Pawn)pawn.mindState.duty.focus.Thing;
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x000356F5 File Offset: 0x000338F5
		protected override float GetRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
