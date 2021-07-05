using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006DC RID: 1756
	public class JobDriver_FixBrokenDownBuilding : JobDriver
	{
		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060030FC RID: 12540 RVA: 0x0011EEE4 File Offset: 0x0011D0E4
		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060030FD RID: 12541 RVA: 0x0011EF0C File Offset: 0x0011D10C
		private Thing Components
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x0011EF30 File Offset: 0x0011D130
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Building, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Components, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x0011EF81 File Offset: 0x0011D181
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(1000, TargetIndex.None);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.WithEffect(this.Building.def.repairEffect, TargetIndex.A, null);
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

		// Token: 0x04001D62 RID: 7522
		private const TargetIndex BuildingInd = TargetIndex.A;

		// Token: 0x04001D63 RID: 7523
		private const TargetIndex ComponentInd = TargetIndex.B;

		// Token: 0x04001D64 RID: 7524
		private const int TicksDuration = 1000;
	}
}
