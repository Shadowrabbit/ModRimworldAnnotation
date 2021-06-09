using System;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000468 RID: 1128
	public class Stance_Warmup : Stance_Busy
	{
		// Token: 0x06001CA3 RID: 7331 RVA: 0x00019E3E File Offset: 0x0001803E
		public Stance_Warmup()
		{
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000F15C4 File Offset: 0x000EF7C4
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

		// Token: 0x06001CA5 RID: 7333 RVA: 0x00019E4D File Offset: 0x0001804D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.targetStartedDowned, "targetStartDowned", false, false);
			Scribe_Values.Look<bool>(ref this.drawAimPie, "drawAimPie", false, false);
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000F1734 File Offset: 0x000EF934
		public override void StanceDraw()
		{
			if (this.drawAimPie && Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				GenDraw.DrawAimPie(this.stanceTracker.pawn, this.focusTarg, (int)((float)this.ticksLeft * this.pieSizeFactor), 0.2f);
			}
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000F178C File Offset: 0x000EF98C
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

		// Token: 0x06001CA8 RID: 7336 RVA: 0x00019E79 File Offset: 0x00018079
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

		// Token: 0x06001CA9 RID: 7337 RVA: 0x00019E91 File Offset: 0x00018091
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

		// Token: 0x0400147D RID: 5245
		private Sustainer sustainer;

		// Token: 0x0400147E RID: 5246
		private Effecter effecter;

		// Token: 0x0400147F RID: 5247
		private bool targetStartedDowned;

		// Token: 0x04001480 RID: 5248
		private bool drawAimPie = true;
	}
}
