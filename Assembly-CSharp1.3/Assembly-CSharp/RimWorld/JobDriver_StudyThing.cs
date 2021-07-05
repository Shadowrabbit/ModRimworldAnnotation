using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200073E RID: 1854
	public class JobDriver_StudyThing : JobDriver
	{
		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003369 RID: 13161 RVA: 0x00122865 File Offset: 0x00120A65
		private Thing StudyThing
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600336A RID: 13162 RVA: 0x00125233 File Offset: 0x00123433
		private CompStudiable StudyComp
		{
			get
			{
				return this.StudyThing.TryGetComp<CompStudiable>();
			}
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x00125240 File Offset: 0x00123440
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.StudyThing, this.job, 1, -1, null, errorOnFailed) && (!this.StudyThing.def.hasInteractionCell || this.pawn.ReserveSittableOrSpot(this.StudyThing.InteractionCell, this.job, errorOnFailed));
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x001252A2 File Offset: 0x001234A2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => this.StudyThing.Map.designationManager.DesignationOn(this.StudyThing, DesignationDefOf.Study) == null || this.StudyComp == null || this.StudyComp.Completed);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil studyThing = new Toil();
			studyThing.tickAction = delegate()
			{
				Pawn actor = studyThing.actor;
				float num = actor.GetStatValue(StatDefOf.ResearchSpeed, true);
				num *= this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
				this.StudyComp.DoResearch(num);
				actor.skills.Learn(SkillDefOf.Intellectual, 0.1f, false);
			};
			studyThing.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			studyThing.WithEffect(EffecterDefOf.Research, TargetIndex.A, null);
			studyThing.WithProgressBar(TargetIndex.A, () => this.StudyComp.ProgressPercent, false, -0.5f, false);
			studyThing.defaultCompleteMode = ToilCompleteMode.Delay;
			studyThing.defaultDuration = 4000;
			studyThing.activeSkill = (() => SkillDefOf.Intellectual);
			yield return studyThing;
			yield break;
		}

		// Token: 0x04001E07 RID: 7687
		private const int JobEndInterval = 4000;
	}
}
