using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x020009B2 RID: 2482
	public static class ToilEffects
	{
		// Token: 0x06003C7A RID: 15482 RVA: 0x00172658 File Offset: 0x00170858
		public static Toil PlaySoundAtStart(this Toil toil, SoundDef sound)
		{
			toil.AddPreInitAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x00172698 File Offset: 0x00170898
		public static Toil PlaySoundAtEnd(this Toil toil, SoundDef sound)
		{
			toil.AddFinishAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x001726D8 File Offset: 0x001708D8
		public static Toil PlaySustainerOrSound(this Toil toil, SoundDef soundDef)
		{
			return toil.PlaySustainerOrSound(() => soundDef);
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x00172704 File Offset: 0x00170904
		public static Toil PlaySustainerOrSound(this Toil toil, Func<SoundDef> soundDefGetter)
		{
			Sustainer sustainer = null;
			toil.AddPreInitAction(delegate
			{
				SoundDef soundDef = soundDefGetter();
				if (soundDef != null && !soundDef.sustain)
				{
					soundDef.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
				}
			});
			toil.AddPreTickAction(delegate
			{
				if (sustainer == null || sustainer.Ended)
				{
					SoundDef soundDef = soundDefGetter();
					if (soundDef != null && soundDef.sustain)
					{
						SoundInfo info = SoundInfo.InMap(toil.actor, MaintenanceType.PerTick);
						sustainer = soundDef.TrySpawnSustainer(info);
						return;
					}
				}
				else
				{
					sustainer.Maintain();
				}
			});
			return toil;
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x00172760 File Offset: 0x00170960
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, TargetIndex ind)
		{
			return toil.WithEffect(() => effectDef, ind);
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x00172790 File Offset: 0x00170990
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, TargetIndex ind)
		{
			return toil.WithEffect(effecterDefGetter, () => toil.actor.CurJob.GetTarget(ind));
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x001727CC File Offset: 0x001709CC
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Thing thing)
		{
			return toil.WithEffect(effecterDefGetter, () => thing);
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x001727FC File Offset: 0x001709FC
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Func<LocalTargetInfo> effectTargetGetter)
		{
			Effecter effecter = null;
			toil.AddPreTickAction(delegate
			{
				if (effecter != null)
				{
					effecter.EffectTick(toil.actor, effectTargetGetter().ToTargetInfo(toil.actor.Map));
					return;
				}
				EffecterDef effecterDef = effecterDefGetter();
				if (effecterDef == null)
				{
					return;
				}
				effecter = effecterDef.Spawn();
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x00172860 File Offset: 0x00170A60
		public static Toil WithProgressBar(this Toil toil, TargetIndex ind, Func<float> progressGetter, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			Effecter effecter = null;
			toil.AddPreTickAction(delegate
			{
				if (toil.actor.Faction != Faction.OfPlayer)
				{
					return;
				}
				if (effecter == null)
				{
					EffecterDef progressBar = EffecterDefOf.ProgressBar;
					effecter = progressBar.Spawn();
					return;
				}
				LocalTargetInfo target = toil.actor.CurJob.GetTarget(ind);
				if (!target.IsValid || (target.HasThing && !target.Thing.Spawned))
				{
					effecter.EffectTick(toil.actor, TargetInfo.Invalid);
				}
				else if (interpolateBetweenActorAndTarget)
				{
					effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), toil.actor);
				}
				else
				{
					effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), TargetInfo.Invalid);
				}
				MoteProgressBar mote = ((SubEffecter_ProgressBar)effecter.children[0]).mote;
				if (mote != null)
				{
					mote.progress = Mathf.Clamp01(progressGetter());
					mote.offsetZ = offsetZ;
				}
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x001728D4 File Offset: 0x00170AD4
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ);
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x00172908 File Offset: 0x00170B08
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, int toilDuration, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toilDuration, interpolateBetweenActorAndTarget, offsetZ);
		}
	}
}
