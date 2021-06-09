using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AD6 RID: 2774
	public class LordToil_ExitMapFighting : LordToil_ExitMap
	{
		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060041A0 RID: 16800 RVA: 0x00030E2A File Offset: 0x0002F02A
		public override DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x00030E31 File Offset: 0x0002F031
		public LordToil_ExitMapFighting(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false) : base(locomotion, canDig, interruptCurrentJob)
		{
		}
	}
}
