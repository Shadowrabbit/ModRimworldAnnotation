using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000871 RID: 2161
	public class LordToil_BestowingCeremony_Wait : LordToil_Wait
	{
		// Token: 0x060038FD RID: 14589 RVA: 0x0013F342 File Offset: 0x0013D542
		public LordToil_BestowingCeremony_Wait(Pawn target, Pawn bestower)
		{
			this.target = target;
			this.bestower = bestower;
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x0013F358 File Offset: 0x0013D558
		public override void Init()
		{
			Messages.Message("MessageBestowerWaiting".Translate(this.target.Named("TARGET"), this.lord.ownedPawns[0].Named("BESTOWER")), new LookTargets(new Pawn[]
			{
				this.target,
				this.lord.ownedPawns[0]
			}), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x0013F3D2 File Offset: 0x0013D5D2
		protected override void DecoratePawnDuty(PawnDuty duty)
		{
			duty.focus = this.target;
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x0013F3E5 File Offset: 0x0013D5E5
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x0013F3FA File Offset: 0x0013D5FA
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			if (p == this.bestower)
			{
				LordJob_BestowingCeremony lordJob = (LordJob_BestowingCeremony)this.lord.LordJob;
				yield return new Command_BestowerCeremony(lordJob, this.bestower, this.target, delegate(List<Pawn> pawns)
				{
					this.lord.AddPawns(pawns);
					lordJob.colonistParticipants.AddRange(pawns);
					this.lord.ReceiveMemo(LordJob_BestowingCeremony.MemoCeremonyStarted);
					foreach (Pawn pawn in pawns)
					{
						if (pawn.drafter != null)
						{
							pawn.drafter.Drafted = false;
						}
						if (!pawn.Awake())
						{
							RestUtility.WakeUp(pawn);
						}
					}
				});
			}
			yield break;
		}

		// Token: 0x04001F52 RID: 8018
		public Pawn target;

		// Token: 0x04001F53 RID: 8019
		public Pawn bestower;
	}
}
