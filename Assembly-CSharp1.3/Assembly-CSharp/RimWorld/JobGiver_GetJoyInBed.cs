using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007DD RID: 2013
	public class JobGiver_GetJoyInBed : JobGiver_GetJoy
	{
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x0600360B RID: 13835 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool CanDoDuringMedicalRest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x001320BF File Offset: 0x001302BF
		protected override bool JoyGiverAllowed(JoyGiverDef def)
		{
			return def.canDoWhileInBed;
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x001320C7 File Offset: 0x001302C7
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJobWhileInBed(pawn);
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x001320D8 File Offset: 0x001302D8
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

		// Token: 0x04001ECD RID: 7885
		private const float MaxJoyLevel = 0.5f;
	}
}
