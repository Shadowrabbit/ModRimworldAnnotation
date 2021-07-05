using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000743 RID: 1859
	public abstract class JobDriver_PlantWork : JobDriver
	{
		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06003384 RID: 13188 RVA: 0x00125688 File Offset: 0x00123888
		protected Plant Plant
		{
			get
			{
				return (Plant)this.job.targetA.Thing;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06003385 RID: 13189 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual DesignationDef RequiredDesignation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x001256A0 File Offset: 0x001238A0
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

		// Token: 0x06003387 RID: 13191 RVA: 0x001256FE File Offset: 0x001238FE
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
				Plant plant2 = this.Plant;
				num *= Mathf.Lerp(3.3f, 1f, plant2.Growth);
				this.workDone += num;
				if (this.workDone >= plant2.def.plant.harvestWork)
				{
					if (plant2.def.plant.harvestedThingDef != null)
					{
						StatDef stat = plant2.def.plant.harvestedThingDef.IsDrug ? StatDefOf.DrugHarvestYield : StatDefOf.PlantHarvestYield;
						float statValue = actor.GetStatValue(stat, true);
						if (actor.RaceProps.Humanlike && plant2.def.plant.harvestFailable && !plant2.Blighted && Rand.Value > statValue)
						{
							MoteMaker.ThrowText((this.pawn.DrawPos + plant2.DrawPos) / 2f, this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
						}
						else
						{
							int num2 = plant2.YieldNow();
							if (statValue > 1f)
							{
								num2 = GenMath.RoundRandom((float)num2 * statValue);
							}
							if (num2 > 0)
							{
								Thing thing = ThingMaker.MakeThing(plant2.def.plant.harvestedThingDef, null);
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
					plant2.def.plant.soundHarvestFinish.PlayOneShot(actor);
					plant2.PlantCollected(this.pawn);
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
			Toil cut2 = cut;
			Plant plant = this.Plant;
			cut2.WithEffect((plant != null && plant.def.plant.IsTree) ? EffecterDefOf.Harvest_Tree : EffecterDefOf.Harvest_Plant, TargetIndex.A, null);
			cut.WithProgressBar(TargetIndex.A, () => this.workDone / this.Plant.def.plant.harvestWork, true, -0.5f, false);
			cut.PlaySustainerOrSound(() => this.Plant.def.plant.soundHarvesting, 1f);
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

		// Token: 0x06003388 RID: 13192 RVA: 0x0012570E File Offset: 0x0012390E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void Init()
		{
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual Toil PlantWorkDoneToil()
		{
			return null;
		}

		// Token: 0x04001E0C RID: 7692
		private float workDone;

		// Token: 0x04001E0D RID: 7693
		protected float xpPerTick;

		// Token: 0x04001E0E RID: 7694
		protected const TargetIndex PlantInd = TargetIndex.A;
	}
}
