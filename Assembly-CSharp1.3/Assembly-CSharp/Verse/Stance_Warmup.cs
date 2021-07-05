using System;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020002F8 RID: 760
	public class Stance_Warmup : Stance_Busy
	{
		// Token: 0x0600160F RID: 5647 RVA: 0x0008052E File Offset: 0x0007E72E
		public Stance_Warmup()
		{
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00080540 File Offset: 0x0007E740
		public Stance_Warmup(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
			if (focusTarg.HasThing && focusTarg.Thing is Pawn)
			{
				Pawn pawn = (Pawn)focusTarg.Thing;
				this.targetStartedDowned = pawn.Downed;
				if (pawn.apparel != null)
				{
					for (int i = 0; i < pawn.apparel.WornApparelCount; i++)
					{
						ShieldBelt shieldBelt = pawn.apparel.WornApparel[i] as ShieldBelt;
						if (shieldBelt != null)
						{
							shieldBelt.KeepDisplaying();
						}
					}
				}
			}
			if (verb != null)
			{
				if (verb.verbProps.soundAiming != null)
				{
					SoundInfo info = SoundInfo.InMap(verb.caster, MaintenanceType.PerTick);
					if (verb.CasterIsPawn)
					{
						info.pitchFactor = 1f / verb.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true);
					}
					this.sustainer = verb.verbProps.soundAiming.TrySpawnSustainer(info);
				}
				if (verb.verbProps.warmupEffecter != null && verb.Caster != null)
				{
					this.effecter = verb.verbProps.warmupEffecter.Spawn(verb.Caster, verb.Caster.Map, 1f);
					this.effecter.Trigger(verb.Caster, focusTarg.ToTargetInfo(verb.Caster.Map));
				}
			}
			this.drawAimPie = (verb != null && verb.verbProps.drawAimPie);
		}

		// Token: 0x06001611 RID: 5649 RVA: 0x000806AD File Offset: 0x0007E8AD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.targetStartedDowned, "targetStartDowned", false, false);
			Scribe_Values.Look<bool>(ref this.drawAimPie, "drawAimPie", false, false);
		}

		// Token: 0x06001612 RID: 5650 RVA: 0x000806DC File Offset: 0x0007E8DC
		public override void StanceDraw()
		{
			if (this.drawAimPie && Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				GenDraw.DrawAimPie(this.stanceTracker.pawn, this.focusTarg, (int)((float)this.ticksLeft * this.pieSizeFactor), 0.2f);
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00080734 File Offset: 0x0007E934
		public override void StanceTick()
		{
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.Maintain();
			}
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.verb.Caster, this.focusTarg.ToTargetInfo(this.verb.Caster.Map));
			}
			if (!this.targetStartedDowned && this.focusTarg.HasThing && this.focusTarg.Thing is Pawn && ((Pawn)this.focusTarg.Thing).Downed)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
				return;
			}
			if (this.focusTarg.HasThing && (!this.focusTarg.Thing.Spawned || this.verb == null || !this.verb.CanHitTargetFrom(base.Pawn.Position, this.focusTarg)))
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
				return;
			}
			if (this.focusTarg == base.Pawn.mindState.enemyTarget)
			{
				base.Pawn.mindState.Notify_EngagedTarget();
			}
			base.StanceTick();
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x0008087A File Offset: 0x0007EA7A
		public void Interrupt()
		{
			base.Expire();
			Effecter effecter = this.effecter;
			if (effecter == null)
			{
				return;
			}
			effecter.Cleanup();
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x00080892 File Offset: 0x0007EA92
		protected override void Expire()
		{
			Verb verb = this.verb;
			if (verb != null)
			{
				verb.WarmupComplete();
			}
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			base.Expire();
		}

		// Token: 0x04000F4F RID: 3919
		private Sustainer sustainer;

		// Token: 0x04000F50 RID: 3920
		private Effecter effecter;

		// Token: 0x04000F51 RID: 3921
		private bool targetStartedDowned;

		// Token: 0x04000F52 RID: 3922
		private bool drawAimPie = true;
	}
}
