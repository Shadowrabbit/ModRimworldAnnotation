using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C28 RID: 3112
	public abstract class JobDriver_PlantWork : JobDriver
	{
		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06004920 RID: 18720 RVA: 0x00034C5F File Offset: 0x00032E5F
		protected Plant Plant
		{
			get
			{
				return (Plant)this.job.targetA.Thing;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06004921 RID: 18721 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual DesignationDef RequiredDesignation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x0019C4BC File Offset: 0x0019A6BC
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			if (target.IsValid && !this.pawn.Reserve(target, this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x00034C76 File Offset: 0x00032E76
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.Init();
			yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (this.RequiredDesignation != null) ? ((Thing t) => this.Map.designationManager.DesignationOn(t, this.RequiredDesignation) != null) : null);
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			Toil toil = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			if (this.RequiredDesignation != null)
			{
				toil.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			yield return toil;
			Toil cut = new Toil();
			cut.tickAction = delegate()
			{
				Pawn actor = cut.actor;
				if (actor.skills != null)
				{
					actor.skills.Learn(SkillDefOf.Plants, this.xpPerTick, false);
				}
				float num = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
				Plant plant = this.Plant;
				num *= Mathf.Lerp(3.3f, 1f, plant.Growth);
				this.workDone += num;
				if (this.workDone >= plant.def.plant.harvestWork)
				{
					if (plant.def.plant.harvestedThingDef != null)
					{
						if (actor.RaceProps.Humanlike && plant.def.plant.harvestFailable && !plant.Blighted && Rand.Value > actor.GetStatValue(StatDefOf.PlantHarvestYield, true))
						{
							MoteMaker.ThrowText((this.pawn.DrawPos + plant.DrawPos) / 2f, this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
						}
						else
						{
							int num2 = plant.YieldNow();
							if (num2 > 0)
							{
								Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef, null);
								thing.stackCount = num2;
								if (actor.Faction != Faction.OfPlayer)
								{
									thing.SetForbidden(true, true);
								}
								Find.QuestManager.Notify_PlantHarvested(actor, thing);
								GenPlace.TryPlaceThing(thing, actor.Position, this.Map, ThingPlaceMode.Near, null, null, default(Rot4));
								actor.records.Increment(RecordDefOf.PlantsHarvested);
							}
						}
					}
					plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
					plant.PlantCollected();
					this.workDone = 0f;
					this.ReadyForNextToil();
					return;
				}
			};
			cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			if (this.RequiredDesignation != null)
			{
				cut.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			cut.defaultCompleteMode = ToilCompleteMode.Never;
			cut.WithEffect(EffecterDefOf.Harvest, TargetIndex.A);
			cut.WithProgressBar(TargetIndex.A, () => this.workDone / this.Plant.def.plant.harvestWork, true, -0.5f);
			cut.PlaySustainerOrSound(() => this.Plant.def.plant.soundHarvesting);
			cut.activeSkill = (() => SkillDefOf.Plants);
			yield return cut;
			Toil toil2 = this.PlantWorkDoneToil();
			if (toil2 != null)
			{
				yield return toil2;
			}
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x00034C86 File Offset: 0x00032E86
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void Init()
		{
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual Toil PlantWorkDoneToil()
		{
			return null;
		}

		// Token: 0x040030AE RID: 12462
		private float workDone;

		// Token: 0x040030AF RID: 12463
		protected float xpPerTick;

		// Token: 0x040030B0 RID: 12464
		protected const TargetIndex PlantInd = TargetIndex.A;
	}
}
