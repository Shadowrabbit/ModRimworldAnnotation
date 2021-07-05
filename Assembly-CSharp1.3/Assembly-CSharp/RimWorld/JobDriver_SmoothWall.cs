using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006E1 RID: 1761
	public class JobDriver_SmoothWall : JobDriver
	{
		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x0600311D RID: 12573 RVA: 0x0011F1E4 File Offset: 0x0011D3E4
		protected int BaseWorkAmount
		{
			get
			{
				return 6500;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x0600311E RID: 12574 RVA: 0x0011F1EB File Offset: 0x0011D3EB
		protected DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothWall;
			}
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x0011F1F4 File Offset: 0x0011D3F4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.job.targetA.Cell, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x0011F24F File Offset: 0x0011D44F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.ignoreDesignations && this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef) == null);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = (float)this.BaseWorkAmount;
			};
			doWork.tickAction = delegate()
			{
				float num = doWork.actor.GetStatValue(StatDefOf.SmoothingSpeed, true) * 1.7f;
				this.workLeft -= num;
				if (doWork.actor.skills != null)
				{
					doWork.actor.skills.Learn(SkillDefOf.Construction, 0.1f, false);
				}
				if (this.workLeft <= 0f)
				{
					this.DoEffect();
					Designation designation = this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef);
					if (designation != null)
					{
						designation.Delete();
					}
					this.ReadyForNextToil();
					return;
				}
			};
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / (float)this.BaseWorkAmount, false, -0.5f, false);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x0011F260 File Offset: 0x0011D460
		protected void DoEffect()
		{
			SmoothableWallUtility.Notify_SmoothedByPawn(SmoothableWallUtility.SmoothWall(base.TargetA.Thing, this.pawn), this.pawn);
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x0011F291 File Offset: 0x0011D491
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04001D6B RID: 7531
		private float workLeft = -1000f;
	}
}
