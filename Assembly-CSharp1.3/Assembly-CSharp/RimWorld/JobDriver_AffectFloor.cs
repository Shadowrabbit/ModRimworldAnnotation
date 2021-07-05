using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006D5 RID: 1749
	public abstract class JobDriver_AffectFloor : JobDriver
	{
		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x060030CC RID: 12492
		protected abstract int BaseWorkAmount { get; }

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x060030CD RID: 12493
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x060030CE RID: 12494 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual StatDef SpeedStat
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x0011EAD5 File Offset: 0x0011CCD5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, ReservationLayerDefOf.Floor, errorOnFailed);
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x0011EAFB File Offset: 0x0011CCFB
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
			doWork.WithProgressBar(TargetIndex.A, () => 1f - this.workLeft / (float)this.BaseWorkAmount, false, -0.5f, false);
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x060030D1 RID: 12497
		protected abstract void DoEffect(IntVec3 c);

		// Token: 0x060030D2 RID: 12498 RVA: 0x0011EB0B File Offset: 0x0011CD0B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04001D58 RID: 7512
		private float workLeft = -1000f;

		// Token: 0x04001D59 RID: 7513
		protected bool clearSnow;
	}
}
