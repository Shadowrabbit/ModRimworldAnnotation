using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DB6 RID: 3510
	public class LordToil_BestowingCeremony_Wait : LordToil_Wait
	{
		// Token: 0x06004FFD RID: 20477 RVA: 0x00038240 File Offset: 0x00036440
		public LordToil_BestowingCeremony_Wait(Pawn target)
		{
			this.target = target;
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x001B637C File Offset: 0x001B457C
		public override void Init()
		{
			Messages.Message("MessageBestowerWaiting".Translate(this.target.Named("TARGET"), this.lord.ownedPawns[0].Named("BESTOWER")), new LookTargets(new Pawn[]
			{
				this.target,
				this.lord.ownedPawns[0]
			}), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x0003824F File Offset: 0x0003644F
		protected override void DecoratePawnDuty(PawnDuty duty)
		{
			duty.focus = this.target;
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x00038262 File Offset: 0x00036462
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x00038277 File Offset: 0x00036477
		public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn bestower, Pawn forPawn)
		{
			if (forPawn == this.target)
			{
				yield return new FloatMenuOption("BeginCeremony".Translate(bestower.Named("BESTOWER")), delegate()
				{
					((LordJob_BestowingCeremony)this.lord.LordJob).StartCeremony(forPawn);
				}, MenuOptionPriority.High, null, null, 0f, null, null);
			}
			yield break;
		}

		// Token: 0x040033B3 RID: 13235
		public Pawn target;
	}
}
