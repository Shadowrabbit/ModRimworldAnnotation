using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200056B RID: 1387
	public abstract class Sample
	{
		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x000F7C1A File Offset: 0x000F5E1A
		public float AgeRealTime
		{
			get
			{
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x060028DB RID: 10459 RVA: 0x000F7C28 File Offset: 0x000F5E28
		public int AgeTicks
		{
			get
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					return Find.TickManager.TicksGame - this.startTick;
				}
				return (int)(this.AgeRealTime * 60f);
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x060028DC RID: 10460
		public abstract float ParentStartRealTime { get; }

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060028DD RID: 10461
		public abstract float ParentStartTick { get; }

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060028DE RID: 10462
		public abstract float ParentHashCode { get; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060028DF RID: 10463
		public abstract SoundParams ExternalParams { get; }

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060028E0 RID: 10464
		public abstract SoundInfo Info { get; }

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060028E1 RID: 10465 RVA: 0x000F7C54 File Offset: 0x000F5E54
		public Map Map
		{
			get
			{
				return this.Info.Maker.Map;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060028E2 RID: 10466 RVA: 0x000F7C77 File Offset: 0x000F5E77
		protected bool TestPlaying
		{
			get
			{
				return this.Info.testPlay;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000F7C84 File Offset: 0x000F5E84
		protected float MappedVolumeMultiplier
		{
			get
			{
				float num = 1f;
				foreach (float num2 in this.volumeInMappings.Values)
				{
					num *= num2;
				}
				return num;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060028E4 RID: 10468 RVA: 0x000F7CE0 File Offset: 0x000F5EE0
		protected float ContextVolumeMultiplier
		{
			get
			{
				if (this.TestPlaying || SoundDefHelper.CorrectContextNow(this.subDef.parentDef, this.Map))
				{
					return 1f;
				}
				return 0f;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060028E5 RID: 10469 RVA: 0x000F7D10 File Offset: 0x000F5F10
		protected virtual float Volume
		{
			get
			{
				if (this.subDef.muteWhenPaused && Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused && !this.TestPlaying)
				{
					return 0f;
				}
				return this.resolvedVolume * this.Info.volumeFactor * this.MappedVolumeMultiplier * this.ContextVolumeMultiplier;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060028E6 RID: 10470 RVA: 0x000F7D6C File Offset: 0x000F5F6C
		public float SanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.Volume, this.subDef.parentDef);
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060028E7 RID: 10471 RVA: 0x000F7D84 File Offset: 0x000F5F84
		protected virtual float Pitch
		{
			get
			{
				float num = this.resolvedPitch * this.Info.pitchFactor;
				if (this.subDef.tempoAffectedByGameSpeed && Current.ProgramState == ProgramState.Playing && !this.TestPlaying && !Find.TickManager.Paused)
				{
					num *= Find.TickManager.TickRateMultiplier;
				}
				return num;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060028E8 RID: 10472 RVA: 0x000F7DDB File Offset: 0x000F5FDB
		public float SanitizedPitch
		{
			get
			{
				return AudioSourceUtility.GetSanitizedPitch(this.Pitch, this.subDef.parentDef);
			}
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000F7DF4 File Offset: 0x000F5FF4
		public Sample(SubSoundDef def)
		{
			this.subDef = def;
			this.resolvedVolume = def.RandomizedVolume();
			this.resolvedPitch = def.pitchRange.RandomInRange;
			this.startRealTime = Time.realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.startTick = 0;
			}
			foreach (SoundParamTarget_Volume key in (from m in this.subDef.paramMappings
			select m.outParam).OfType<SoundParamTarget_Volume>())
			{
				this.volumeInMappings.Add(key, 0f);
			}
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x000F7EDC File Offset: 0x000F60DC
		public virtual void Update()
		{
			this.source.pitch = this.SanitizedPitch;
			this.ApplyMappedParameters();
			this.source.volume = this.SanitizedVolume;
			if (this.source.volume < 0.001f)
			{
				this.source.mute = true;
			}
			else
			{
				this.source.mute = false;
			}
			if (this.subDef.tempoAffectedByGameSpeed && !this.TestPlaying)
			{
				if (Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused)
				{
					if (this.source.isPlaying)
					{
						this.source.Pause();
						return;
					}
				}
				else if (!this.source.isPlaying)
				{
					this.source.UnPause();
				}
			}
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x000F7F98 File Offset: 0x000F6198
		public void ApplyMappedParameters()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.paramUpdateMode != SoundParamUpdateMode.OncePerSample || !this.mappingsApplied)
				{
					soundParameterMapping.Apply(this);
				}
			}
			this.mappingsApplied = true;
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x000F7FF1 File Offset: 0x000F61F1
		public void SignalMappedVolume(float value, SoundParamTarget sourceParam)
		{
			this.volumeInMappings[sourceParam] = value;
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x000F8000 File Offset: 0x000F6200
		public virtual void SampleCleanup()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.curve.HasView)
				{
					soundParameterMapping.curve.View.ClearDebugInputFrom(this);
				}
			}
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x000F8058 File Offset: 0x000F6258
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Sample_",
				this.subDef.name,
				" volume=",
				this.source.volume,
				" at ",
				this.source.transform.position.ToIntVec3()
			});
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x000F80C6 File Offset: 0x000F62C6
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.startRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x0400195E RID: 6494
		public SubSoundDef subDef;

		// Token: 0x0400195F RID: 6495
		public AudioSource source;

		// Token: 0x04001960 RID: 6496
		public float startRealTime;

		// Token: 0x04001961 RID: 6497
		public int startTick;

		// Token: 0x04001962 RID: 6498
		public float resolvedVolume;

		// Token: 0x04001963 RID: 6499
		public float resolvedPitch;

		// Token: 0x04001964 RID: 6500
		private bool mappingsApplied;

		// Token: 0x04001965 RID: 6501
		private Dictionary<SoundParamTarget, float> volumeInMappings = new Dictionary<SoundParamTarget, float>();
	}
}
