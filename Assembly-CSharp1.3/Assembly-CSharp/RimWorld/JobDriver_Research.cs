using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000730 RID: 1840
	public class JobDriver_Research : JobDriver
	{
		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06003311 RID: 13073 RVA: 0x001242B7 File Offset: 0x001224B7
		private ResearchProjectDef Project
		{
			get
			{
				return Find.ResearchManager.currentProj;
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06003312 RID: 13074 RVA: 0x001242C3 File Offset: 0x001224C3
		private Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)base.TargetThingA;
			}
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x001242D0 File Offset: 0x001224D0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed) && (!this.ResearchBench.def.hasInteractionCell || this.pawn.ReserveSittableOrSpot(this.ResearchBench.InteractionCell, this.job, errorOnFailed));
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x00124332 File Offset: 0x00122532
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil research = new Toil();
			research.tickAction = delegate()
			{
				Pawn actor = research.actor;
				float num = actor.GetStatValue(StatDefOf.ResearchSpeed, true);
				num *= this.TargetThingA.GetStatValue(StatDefOf.ResearchSpeedFactor, true);
				Find.ResearchManager.ResearchPerformed(num, actor);
				actor.skills.Learn(SkillDefOf.Intellectual, 0.1f, false);
				actor.GainComfortFromCellIfPossible(true);
			};
			research.FailOn(() => this.Project == null);
			research.FailOn(() => !this.Project.CanBeResearchedAt(this.ResearchBench, false));
			research.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			research.WithEffect(EffecterDefOf.Research, TargetIndex.A, null);
			research.WithProgressBar(TargetIndex.A, delegate
			{
				ResearchProjectDef project = this.Project;
				if (project == null)
				{
					return 0f;
				}
				return project.ProgressPercent;
			}, false, -0.5f, false);
			research.defaultCompleteMode = ToilCompleteMode.Delay;
			research.defaultDuration = 4000;
			research.activeSkill = (() => SkillDefOf.Intellectual);
			yield return research;
			yield return Toils_General.Wait(2, TargetIndex.None);
			yield break;
		}

		// Token: 0x04001DEF RID: 7663
		private const int JobEndInterval = 4000;
	}
}
