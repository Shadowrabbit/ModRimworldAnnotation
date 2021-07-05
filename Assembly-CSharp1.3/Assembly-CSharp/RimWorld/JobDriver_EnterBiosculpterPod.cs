using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200070E RID: 1806
	public class JobDriver_EnterBiosculpterPod : JobDriver
	{
		// Token: 0x06003226 RID: 12838 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x0012200F File Offset: 0x0012020F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.job.targetA.Thing.TryGetComp<CompBiosculpterPod>().CanAccept(base.GetActor()));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return JobDriver_EnterBiosculpterPod.PrepareToEnterToil(TargetIndex.A);
			Toil enter = new Toil();
			enter.initAction = delegate()
			{
				Pawn actor = enter.actor;
				CompBiosculpterPod compBiosculpterPod = actor.CurJob.targetA.Thing.TryGetComp<CompBiosculpterPod>();
				if (compBiosculpterPod == null)
				{
					return;
				}
				compBiosculpterPod.TryAcceptPawn(actor);
			};
			enter.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return enter;
			yield break;
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x00122020 File Offset: 0x00120220
		public static Toil PrepareToEnterToil(TargetIndex podIndex)
		{
			Toil prepare = Toils_General.Wait(70, TargetIndex.None);
			prepare.FailOnCannotTouch(podIndex, PathEndMode.InteractionCell);
			prepare.WithProgressBarToilDelay(podIndex, false, -0.5f);
			prepare.PlaySustainerOrSound(delegate()
			{
				Thing thing = prepare.actor.CurJob.GetTarget(podIndex).Thing;
				if (thing == null)
				{
					return null;
				}
				CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
				if (compBiosculpterPod == null)
				{
					return null;
				}
				return compBiosculpterPod.Props.enterSound;
			}, 1f);
			return prepare;
		}

		// Token: 0x04001DB6 RID: 7606
		public const int EnterPodDelay = 70;
	}
}
