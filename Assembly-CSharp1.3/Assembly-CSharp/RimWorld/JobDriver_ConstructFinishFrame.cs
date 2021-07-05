using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006DA RID: 1754
	public class JobDriver_ConstructFinishFrame : JobDriver
	{
		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060030F0 RID: 12528 RVA: 0x0011EDCC File Offset: 0x0011CFCC
		private Frame Frame
		{
			get
			{
				return (Frame)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x0011EDF2 File Offset: 0x0011CFF2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil build = new Toil();
			build.initAction = delegate()
			{
				GenClamor.DoClamor(build.actor, 15f, ClamorDefOf.Construction);
			};
			build.tickAction = delegate()
			{
				Pawn actor = build.actor;
				Frame frame = this.Frame;
				if (frame.resourceContainer.Count > 0)
				{
					actor.skills.Learn(SkillDefOf.Construction, 0.25f, false);
				}
				float num = actor.GetStatValue(StatDefOf.ConstructionSpeed, true) * 1.7f;
				if (frame.Stuff != null)
				{
					num *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor, null);
				}
				float workToBuild = frame.WorkToBuild;
				if (actor.Faction == Faction.OfPlayer)
				{
					float statValue = actor.GetStatValue(StatDefOf.ConstructSuccessChance, true);
					if (!TutorSystem.TutorialMode && Rand.Value < 1f - Mathf.Pow(statValue, num / workToBuild))
					{
						frame.FailConstruction(actor);
						this.ReadyForNextToil();
						return;
					}
				}
				if (frame.def.entityDefToBuild is TerrainDef)
				{
					this.Map.snowGrid.SetDepth(frame.Position, 0f);
				}
				frame.workDone += num;
				if (frame.workDone >= workToBuild)
				{
					frame.CompleteConstruction(actor);
					this.ReadyForNextToil();
					return;
				}
			};
			build.WithEffect(() => ((Frame)build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect, TargetIndex.A, null);
			build.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			build.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			build.FailOn(() => !GenConstruct.CanConstruct(this.Frame, this.pawn, true, false));
			build.defaultCompleteMode = ToilCompleteMode.Delay;
			build.defaultDuration = 5000;
			build.activeSkill = (() => SkillDefOf.Construction);
			yield return build;
			yield break;
		}

		// Token: 0x04001D5F RID: 7519
		private const int JobEndInterval = 5000;
	}
}
