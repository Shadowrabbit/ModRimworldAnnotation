using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000974 RID: 2420
	public class JobDriver_Wait : JobDriver
	{
		// Token: 0x06003B42 RID: 15170 RVA: 0x0016D560 File Offset: 0x0016B760
		public override string GetReport()
		{
			if (this.job.def != JobDefOf.Wait_Combat)
			{
				return base.GetReport();
			}
			if (this.pawn.RaceProps.Humanlike && this.pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return "ReportStanding".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x0002D765 File Offset: 0x0002B965
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				base.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.pawn.Position);
				this.pawn.pather.StopDead();
				this.CheckForAutoAttack();
			};
			toil.tickAction = delegate()
			{
				if (this.job.expiryInterval == -1 && this.job.def == JobDefOf.Wait_Combat && !this.pawn.Drafted)
				{
					Log.Error(this.pawn + " in eternal WaitCombat without being drafted.", false);
					base.ReadyForNextToil();
					return;
				}
				if ((Find.TickManager.TicksGame + this.pawn.thingIDNumber) % 4 == 0)
				{
					this.CheckForAutoAttack();
				}
			};
			this.DecorateWaitToil(toil);
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			if (this.pawn.mindState != null && this.pawn.mindState.duty != null && this.pawn.mindState.duty.focus != null)
			{
				LocalTargetInfo focusLocal = this.pawn.mindState.duty.focus;
				toil.handlingFacing = false;
				Toil toil2 = toil;
				toil2.tickAction = (Action)Delegate.Combine(toil2.tickAction, new Action(delegate()
				{
					this.pawn.rotationTracker.FaceTarget(focusLocal);
				}));
			}
			yield return toil;
			yield break;
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DecorateWaitToil(Toil wait)
		{
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x0002D775 File Offset: 0x0002B975
		public override void Notify_StanceChanged()
		{
			if (this.pawn.stances.curStance is Stance_Mobile)
			{
				this.CheckForAutoAttack();
			}
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x0016D5BC File Offset: 0x0016B7BC
		private void CheckForAutoAttack()
		{
			if (this.pawn.Downed)
			{
				return;
			}
			if (this.pawn.stances.FullBodyBusy)
			{
				return;
			}
			this.collideWithPawns = false;
			bool flag = !this.pawn.WorkTagIsDisabled(WorkTags.Violent);
			bool flag2 = this.pawn.RaceProps.ToolUser && this.pawn.Faction == Faction.OfPlayer && !this.pawn.WorkTagIsDisabled(WorkTags.Firefighting);
			if (flag || flag2)
			{
				Fire fire = null;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.pawn.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (flag)
							{
								Pawn pawn = thingList[j] as Pawn;
								if (pawn != null && !pawn.Downed && this.pawn.HostileTo(pawn) && GenHostility.IsActiveThreatTo(pawn, this.pawn.Faction))
								{
									this.pawn.meleeVerbs.TryMeleeAttack(pawn, null, false);
									this.collideWithPawns = true;
									return;
								}
							}
							if (flag2)
							{
								Fire fire2 = thingList[j] as Fire;
								if (fire2 != null && (fire == null || fire2.fireSize < fire.fireSize || i == 8) && (fire2.parent == null || fire2.parent != this.pawn))
								{
									fire = fire2;
								}
							}
						}
					}
				}
				if (fire != null && (!this.pawn.InMentalState || this.pawn.MentalState.def.allowBeatfire))
				{
					this.pawn.natives.TryBeatFire(fire);
					return;
				}
				if (flag && this.job.canUseRangedWeapon && this.pawn.Faction != null && this.job.def == JobDefOf.Wait_Combat && (this.pawn.drafter == null || this.pawn.drafter.FireAtWill))
				{
					Verb currentEffectiveVerb = this.pawn.CurrentEffectiveVerb;
					if (currentEffectiveVerb != null && !currentEffectiveVerb.verbProps.IsMeleeAttack)
					{
						TargetScanFlags targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedLOSToNonPawns | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
						if (currentEffectiveVerb.IsIncendiary())
						{
							targetScanFlags |= TargetScanFlags.NeedNonBurning;
						}
						Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this.pawn, targetScanFlags, null, 0f, 9999f);
						if (thing != null)
						{
							this.pawn.TryStartAttack(thing);
							this.collideWithPawns = true;
							return;
						}
					}
				}
			}
		}

		// Token: 0x0400292C RID: 10540
		private const int TargetSearchInterval = 4;
	}
}
