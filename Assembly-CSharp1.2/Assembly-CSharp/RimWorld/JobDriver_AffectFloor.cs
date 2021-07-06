using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B4A RID: 2890
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060043F8 RID: 17400
		protected abstract int BaseWorkAmount { get; }

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060043F9 RID: 17401
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x060043FA RID: 17402 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x000324E0 File Offset: 0x000306E0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed);
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x00032506 File Offset: 0x00030706
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.ignoreDesignations && this.Map.designationManager.DesignationAt(this.TargetLocA, this.DesDef) == null);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = (float)this.BaseWorkAmount;
			};
			doWork.tickAction = delegate()
			{
				float num = (this.SpeedStat != null) ? doWork.actor.GetStatValue(this.SpeedStat, true) : 1f;
				num *= 1.7f;
				this.workLeft -= num;
				if (doWork.actor.skills != null)
				{
					doWork.actor.skills.Learn(SkillDefOf.Construction, 0.1f, false);
				}
				if (this.clearSnow)
				{
					this.Map.snowGrid.SetDepth(this.TargetLocA, 0f);
				}
				if (this.workLeft <= 0f)
				{
					this.DoEffect(this.TargetLocA);
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
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / (float)this.BaseWorkAmount, false, -0.5f);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x060043FD RID: 17405
		protected abstract void DoEffect(IntVec3 c);

		// Token: 0x060043FE RID: 17406 RVA: 0x00032516 File Offset: 0x00030716
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04002E43 RID: 11843
		private float workLeft = -1000f;

		// Token: 0x04002E44 RID: 11844
		protected bool clearSnow;
	}
}
