using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BFA RID: 3066
	public class JobDriver_Research : JobDriver
	{
		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06004823 RID: 18467 RVA: 0x000345B5 File Offset: 0x000327B5
		private ResearchProjectDef Project
		{
			get
			{
				return Find.ResearchManager.currentProj;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06004824 RID: 18468 RVA: 0x000345C1 File Offset: 0x000327C1
		private Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)base.TargetThingA;
			}
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x000345CE File Offset: 0x000327CE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x000345F0 File Offset: 0x000327F0
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
			research.WithEffect(EffecterDefOf.Research, TargetIndex.A);
			research.WithProgressBar(TargetIndex.A, delegate
			{
				ResearchProjectDef project = this.Project;
				if (project == null)
				{
					return 0f;
				}
				return project.ProgressPercent;
			}, false, -0.5f);
			research.defaultCompleteMode = ToilCompleteMode.Delay;
			research.defaultDuration = 4000;
			research.activeSkill = (() => SkillDefOf.Intellectual);
			yield return research;
			yield return Toils_General.Wait(2, TargetIndex.None);
			yield break;
		}

		// Token: 0x04003033 RID: 12339
		private const int JobEndInterval = 4000;
	}
}
