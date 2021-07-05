using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B8B RID: 2955
	public class JobDriver_BuildSnowman : JobDriver
	{
		// Token: 0x0600456F RID: 17775 RVA: 0x0002D6EB File Offset: 0x0002B8EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x00033028 File Offset: 0x00031228
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

		// Token: 0x06004571 RID: 17777 RVA: 0x00033038 File Offset: 0x00031238
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
		}

		// Token: 0x04002EF3 RID: 12019
		private float workLeft = -1000f;

		// Token: 0x04002EF4 RID: 12020
		protected const int BaseWorkAmount = 2300;
	}
}
