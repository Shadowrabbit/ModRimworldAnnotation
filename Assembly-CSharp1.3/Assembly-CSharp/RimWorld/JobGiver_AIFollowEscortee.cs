using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200077E RID: 1918
	public class JobGiver_AIFollowEscortee : JobGiver_AIFollowPawn
	{
		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060034D0 RID: 13520 RVA: 0x0012B198 File Offset: 0x00129398
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 120;
			}
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x0012B19C File Offset: 0x0012939C
		protected override Pawn GetFollowee(Pawn pawn)
		{
			return (Pawn)pawn.mindState.duty.focus.Thing;
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x0012867C File Offset: 0x0012687C
		protected override float GetRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
