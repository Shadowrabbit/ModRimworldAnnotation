using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006F6 RID: 1782
	public class JobDriver_BuildSnowman : JobDriver
	{
		// Token: 0x0600319C RID: 12700 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x00120BBE File Offset: 0x0011EDBE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil doWork = new Toil();
			doWork.initAction = delegate()
			{
				this.workLeft = 2300f;
			};
			doWork.tickAction = delegate()
			{
				this.workLeft -= doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed, true) * 1.7f;
				if (this.workLeft <= 0f)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Snowman, null);
					thing.SetFaction(this.pawn.Faction, null);
					GenSpawn.Spawn(thing, this.TargetLocA, this.Map, WipeMode.Vanish);
					this.ReadyForNextToil();
					return;
				}
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
			};
			doWork.defaultCompleteMode = ToilCompleteMode.Never;
			doWork.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			doWork.activeSkill = (() => SkillDefOf.Construction);
			yield return doWork;
			yield break;
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x00120BCE File Offset: 0x0011EDCE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04001D93 RID: 7571
		private float workLeft = -1000f;

		// Token: 0x04001D94 RID: 7572
		protected const int BaseWorkAmount = 2300;
	}
}
