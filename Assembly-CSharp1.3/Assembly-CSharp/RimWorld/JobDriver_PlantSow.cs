using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000742 RID: 1858
	public class JobDriver_PlantSow : JobDriver
	{
		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x0600337F RID: 13183 RVA: 0x00125634 File Offset: 0x00123834
		private Plant Plant
		{
			get
			{
				return (Plant)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x0012565A File Offset: 0x0012385A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.sowWorkDone, "sowWorkDone", 0f, false);
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x000FC76A File Offset: 0x000FA96A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x00125678 File Offset: 0x00123878
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch).FailOn(() => PlantUtility.AdjacentSowBlocker(this.job.plantDefToSow, this.TargetA.Cell, this.Map) != null).FailOn(() => !this.job.plantDefToSow.CanNowPlantAt(this.TargetLocA, this.Map, false));
			Toil sowToil = new Toil();
			sowToil.initAction = delegate()
			{
				this.TargetThingA = GenSpawn.Spawn(this.job.plantDefToSow, this.TargetLocA, this.Map, WipeMode.Vanish);
				this.pawn.Reserve(this.TargetThingA, sowToil.actor.CurJob, 1, -1, null, true);
				Plant plant = (Plant)this.TargetThingA;
				plant.Growth = 0f;
				plant.sown = true;
			};
			sowToil.tickAction = delegate()
			{
				Pawn actor = sowToil.actor;
				if (actor.skills != null)
				{
					actor.skills.Learn(SkillDefOf.Plants, 0.085f, false);
				}
				float statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
				Plant plant = this.Plant;
				if (plant.LifeStage != PlantLifeStage.Sowing)
				{
					Log.Error(this + " getting sowing work while not in Sowing life stage.");
				}
				this.sowWorkDone += statValue;
				if (this.sowWorkDone >= plant.def.plant.sowWork)
				{
					plant.Growth = 0.0001f;
					this.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlag.Things);
					actor.records.Increment(RecordDefOf.PlantsSown);
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedPlant, actor.Named(HistoryEventArgsNames.Doer)), true);
					if (plant.def.plant.humanFoodPlant)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedHumanFoodPlant, actor.Named(HistoryEventArgsNames.Doer)), true);
					}
					this.ReadyForNextToil();
					return;
				}
			};
			sowToil.defaultCompleteMode = ToilCompleteMode.Never;
			sowToil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			sowToil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			sowToil.WithEffect(EffecterDefOf.Sow, TargetIndex.A, null);
			sowToil.WithProgressBar(TargetIndex.A, () => this.sowWorkDone / this.Plant.def.plant.sowWork, true, -0.5f, false);
			sowToil.PlaySustainerOrSound(() => SoundDefOf.Interact_Sow, 1f);
			sowToil.AddFinishAction(delegate
			{
				if (this.TargetThingA != null)
				{
					Plant plant = (Plant)sowToil.actor.CurJob.GetTarget(TargetIndex.A).Thing;
					if (this.sowWorkDone < plant.def.plant.sowWork && !this.TargetThingA.Destroyed)
					{
						this.TargetThingA.Destroy(DestroyMode.Vanish);
					}
				}
			});
			sowToil.activeSkill = (() => SkillDefOf.Plants);
			yield return sowToil;
			yield break;
		}

		// Token: 0x04001E0B RID: 7691
		private float sowWorkDone;
	}
}
