using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF7 RID: 3319
	public class JobGiver_GetJoyInBed : JobGiver_GetJoy
	{
		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06004C4E RID: 19534 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanDoDuringMedicalRest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x00036374 File Offset: 0x00034574
		protected override bool JoyGiverAllowed(JoyGiverDef def)
		{
			return def.canDoWhileInBed;
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x0003637C File Offset: 0x0003457C
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJobWhileInBed(pawn);
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x001A98CC File Offset: 0x001A7ACC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.CurJob == null || !pawn.InBed() || !pawn.Awake() || pawn.needs.joy == null)
			{
				return null;
			}
			if (pawn.needs.joy.CurLevel > 0.5f)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x04003242 RID: 12866
		private const float MaxJoyLevel = 0.5f;
	}
}
