using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000731 RID: 1841
	public class JobDriver_Resurrect : JobDriver
	{
		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x00124344 File Offset: 0x00122544
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06003317 RID: 13079 RVA: 0x0012436C File Offset: 0x0012256C
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x00124390 File Offset: 0x00122590
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Corpse, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x001243E1 File Offset: 0x001225E1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(600, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return toil;
			yield return Toils_General.Do(new Action(this.Resurrect));
			yield break;
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x001243F4 File Offset: 0x001225F4
		private void Resurrect()
		{
			Pawn innerPawn = this.Corpse.InnerPawn;
			ResurrectionUtility.ResurrectWithSideEffects(innerPawn);
			Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
			this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
		}

		// Token: 0x04001DF0 RID: 7664
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04001DF1 RID: 7665
		private const TargetIndex ItemInd = TargetIndex.B;

		// Token: 0x04001DF2 RID: 7666
		private const int DurationTicks = 600;
	}
}
