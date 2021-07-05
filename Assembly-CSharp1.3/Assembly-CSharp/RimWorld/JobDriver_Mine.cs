using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000727 RID: 1831
	public class JobDriver_Mine : JobDriver
	{
		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x060032DB RID: 13019 RVA: 0x00123C84 File Offset: 0x00121E84
		private Thing MineTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x00123CA5 File Offset: 0x00121EA5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.MineTarget, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x00123CC7 File Offset: 0x00121EC7
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
						DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, (float)num, 0f, -1f, mine.actor, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
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
			mine.WithProgressBar(TargetIndex.A, () => 1f - (float)this.MineTarget.HitPoints / (float)this.MineTarget.MaxHitPoints, false, -0.5f, false);
			mine.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			mine.activeSkill = (() => SkillDefOf.Mining);
			yield return mine;
			yield break;
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x00123CD8 File Offset: 0x00121ED8
		private void ResetTicksToPickHit()
		{
			float num = this.pawn.GetStatValue(StatDefOf.MiningSpeed, true);
			if (num < 0.6f && this.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.6f;
			}
			this.ticksToPickHit = (int)Math.Round((double)(100f / num));
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x00123D2B File Offset: 0x00121F2B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}

		// Token: 0x04001DDB RID: 7643
		private int ticksToPickHit = -1000;

		// Token: 0x04001DDC RID: 7644
		private Effecter effecter;

		// Token: 0x04001DDD RID: 7645
		public const int BaseTicksBetweenPickHits = 100;

		// Token: 0x04001DDE RID: 7646
		private const int BaseDamagePerPickHit_NaturalRock = 80;

		// Token: 0x04001DDF RID: 7647
		private const int BaseDamagePerPickHit_NotNaturalRock = 40;

		// Token: 0x04001DE0 RID: 7648
		private const float MinMiningSpeedFactorForNPCs = 0.6f;
	}
}
