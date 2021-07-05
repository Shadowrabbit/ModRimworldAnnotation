using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x020005AC RID: 1452
	public static class ToilEffects
	{
		// Token: 0x06002A68 RID: 10856 RVA: 0x000FF240 File Offset: 0x000FD440
		public static Toil PlaySoundAtStart(this Toil toil, SoundDef sound)
		{
			toil.AddPreInitAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000FF280 File Offset: 0x000FD480
		public static Toil PlaySoundAtEnd(this Toil toil, SoundDef sound)
		{
			toil.AddFinishAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000FF2C0 File Offset: 0x000FD4C0
		public static Toil PlaySustainerOrSound(this Toil toil, SoundDef soundDef, float pitchFactor = 1f)
		{
			return toil.PlaySustainerOrSound(() => soundDef, pitchFactor);
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000FF2F0 File Offset: 0x000FD4F0
		public static Toil PlaySustainerOrSound(this Toil toil, Func<SoundDef> soundDefGetter, float pitchFactor = 1f)
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
						info.pitchFactor = pitchFactor;
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

		// Token: 0x06002A6C RID: 10860 RVA: 0x000FF354 File Offset: 0x000FD554
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, TargetIndex ind, Color? overrideColor = null)
		{
			return toil.WithEffect(() => effectDef, ind, overrideColor);
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000FF384 File Offset: 0x000FD584
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, Func<LocalTargetInfo> effectTargetGetter, Color? overrideColor = null)
		{
			return toil.WithEffect(() => effectDef, effectTargetGetter, overrideColor);
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x000FF3B4 File Offset: 0x000FD5B4
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, TargetIndex ind, Color? overrideColor = null)
		{
			return toil.WithEffect(effecterDefGetter, () => toil.actor.CurJob.GetTarget(ind), overrideColor);
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x000FF3F0 File Offset: 0x000FD5F0
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Thing thing, Color? overrideColor = null)
		{
			return toil.WithEffect(effecterDefGetter, () => thing, overrideColor);
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x000FF420 File Offset: 0x000FD620
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Func<LocalTargetInfo> effectTargetGetter, Color? overrideColor = null)
		{
			Effecter effecter = null;
			toil.AddPreTickAction(delegate
			{
				if (effecter == null)
				{
					EffecterDef effecterDef = effecterDefGetter();
					if (effecterDef == null)
					{
						return;
					}
					effecter = effecterDef.Spawn();
					effecter.Trigger(toil.actor, effectTargetGetter().ToTargetInfo(toil.actor.Map));
					if (overrideColor == null)
					{
						return;
					}
					using (List<SubEffecter>.Enumerator enumerator = effecter.children.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SubEffecter_Sprayer subEffecter_Sprayer;
							if ((subEffecter_Sprayer = (enumerator.Current as SubEffecter_Sprayer)) != null)
							{
								subEffecter_Sprayer.colorOverride = overrideColor;
							}
						}
						return;
					}
				}
				effecter.EffectTick(toil.actor, effectTargetGetter().ToTargetInfo(toil.actor.Map));
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

		// Token: 0x06002A71 RID: 10865 RVA: 0x000FF48C File Offset: 0x000FD68C
		public static Toil WithProgressBar(this Toil toil, TargetIndex ind, Func<float> progressGetter, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f, bool alwaysShow = false)
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
					mote.alwaysShow = alwaysShow;
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

		// Token: 0x06002A72 RID: 10866 RVA: 0x000FF508 File Offset: 0x000FD708
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ, false);
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000FF540 File Offset: 0x000FD740
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, int toilDuration, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toilDuration, interpolateBetweenActorAndTarget, offsetZ, false);
		}
	}
}
