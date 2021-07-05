using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B5C RID: 2908
	public class JobDriver_FixBrokenDownBuilding : JobDriver
	{
		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06004469 RID: 17513 RVA: 0x00190258 File Offset: 0x0018E458
		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x0600446A RID: 17514 RVA: 0x00190280 File Offset: 0x0018E480
		private Thing Components
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x001902A4 File Offset: 0x0018E4A4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Building, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Components, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x00032911 File Offset: 0x00030B11
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(1000, TargetIndex.None);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.WithEffect(this.Building.def.repairEffect, TargetIndex.A);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.activeSkill = (() => SkillDefOf.Construction);
			yield return toil;
			yield return new Toil
			{
				initAction = delegate()
				{
					this.Components.Destroy(DestroyMode.Vanish);
					if (Rand.Value > this.pawn.GetStatValue(StatDefOf.FixBrokenDownBuildingSuccessChance, true))
					{
						MoteMaker.ThrowText((this.pawn.DrawPos + this.Building.DrawPos) / 2f, base.Map, "TextMote_FixBrokenDownBuildingFail".Translate(), 3.65f);
						return;
					}
					this.Building.GetComp<CompBreakdownable>().Notify_Repaired();
				}
			};
			yield break;
		}

		// Token: 0x04002E72 RID: 11890
		private const TargetIndex BuildingInd = TargetIndex.A;

		// Token: 0x04002E73 RID: 11891
		private const TargetIndex ComponentInd = TargetIndex.B;

		// Token: 0x04002E74 RID: 11892
		private const int TicksDuration = 1000;
	}
}
