using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BE1 RID: 3041
	public class JobDriver_Mine : JobDriver
	{
		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x0600478C RID: 18316 RVA: 0x0018EA98 File Offset: 0x0018CC98
		private Thing MineTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x000340E0 File Offset: 0x000322E0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.MineTarget, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x00034102 File Offset: 0x00032302
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnCellMissingDesignation(TargetIndex.A, DesignationDefOf.Mine);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil mine = new Toil();
			mine.tickAction = delegate()
			{
				Pawn actor = mine.actor;
				Thing mineTarget = this.MineTarget;
				if (this.ticksToPickHit < -100)
				{
					this.ResetTicksToPickHit();
				}
				if (actor.skills != null && (mineTarget.Faction != actor.Faction || actor.Faction == null))
				{
					actor.skills.Learn(SkillDefOf.Mining, 0.07f, false);
				}
				this.ticksToPickHit--;
				if (this.ticksToPickHit <= 0)
				{
					IntVec3 position = mineTarget.Position;
					if (this.effecter == null)
					{
						this.effecter = EffecterDefOf.Mine.Spawn();
					}
					this.effecter.Trigger(actor, mineTarget);
					int num = mineTarget.def.building.isNaturalRock ? 80 : 40;
					Mineable mineable = mineTarget as Mineable;
					if (mineable == null || mineTarget.HitPoints > num)
					{
						DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, (float)num, 0f, -1f, mine.actor, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
						mineTarget.TakeDamage(dinfo);
					}
					else
					{
						mineable.Notify_TookMiningDamage(mineTarget.HitPoints, mine.actor);
						mineable.HitPoints = 0;
						mineable.DestroyMined(actor);
					}
					if (mineTarget.Destroyed)
					{
						actor.Map.mineStrikeManager.CheckStruckOre(position, mineTarget.def, actor);
						actor.records.Increment(RecordDefOf.CellsMined);
						if (this.pawn.Faction != Faction.OfPlayer)
						{
							List<Thing> thingList = position.GetThingList(this.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								thingList[i].SetForbidden(true, false);
							}
						}
						if (this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsVeryValuable(mineTarget.def))
						{
							TaleRecorder.RecordTale(TaleDefOf.MinedValuable, new object[]
							{
								this.pawn,
								mineTarget.def.building.mineableThing
							});
						}
						if (this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsValuable(mineTarget.def) && !this.pawn.Map.IsPlayerHome)
						{
							TaleRecorder.RecordTale(TaleDefOf.CaravanRemoteMining, new object[]
							{
								this.pawn,
								mineTarget.def.building.mineableThing
							});
						}
						this.ReadyForNextToil();
						return;
					}
					this.ResetTicksToPickHit();
				}
			};
			mine.defaultCompleteMode = ToilCompleteMode.Never;
			mine.WithProgressBar(TargetIndex.A, () => 1f - (float)this.MineTarget.HitPoints / (float)this.MineTarget.MaxHitPoints, false, -0.5f);
			mine.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			mine.activeSkill = (() => SkillDefOf.Mining);
			yield return mine;
			yield break;
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x00198440 File Offset: 0x00196640
		private void ResetTicksToPickHit()
		{
			float num = this.pawn.GetStatValue(StatDefOf.MiningSpeed, true);
			if (num < 0.6f && this.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.6f;
			}
			this.ticksToPickHit = (int)Math.Round((double)(100f / num));
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x00034112 File Offset: 0x00032312
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}

		// Token: 0x04002FE1 RID: 12257
		private int ticksToPickHit = -1000;

		// Token: 0x04002FE2 RID: 12258
		private Effecter effecter;

		// Token: 0x04002FE3 RID: 12259
		public const int BaseTicksBetweenPickHits = 100;

		// Token: 0x04002FE4 RID: 12260
		private const int BaseDamagePerPickHit_NaturalRock = 80;

		// Token: 0x04002FE5 RID: 12261
		private const int BaseDamagePerPickHit_NotNaturalRock = 40;

		// Token: 0x04002FE6 RID: 12262
		private const float MinMiningSpeedFactorForNPCs = 0.6f;
	}
}
