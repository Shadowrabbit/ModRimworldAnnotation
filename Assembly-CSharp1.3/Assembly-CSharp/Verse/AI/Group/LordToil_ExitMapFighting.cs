using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000671 RID: 1649
	public class LordToil_ExitMapFighting : LordToil_ExitMap
	{
		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002EC3 RID: 11971 RVA: 0x00117060 File Offset: 0x00115260
		public override DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x00117067 File Offset: 0x00115267
		public LordToil_ExitMapFighting(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false) : base(locomotion, canDig, interruptCurrentJob)
		{
		}
	}
}
