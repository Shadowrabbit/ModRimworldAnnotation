using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000942 RID: 2370
	public abstract class Sample
	{
		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06003A21 RID: 14881 RVA: 0x0002CCC6 File Offset: 0x0002AEC6
		public float AgeRealTime
		{
			get
			{
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06003A22 RID: 14882 RVA: 0x0002CCD4 File Offset: 0x0002AED4
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

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06003A23 RID: 14883
		public abstract float ParentStartRealTime { get; }

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06003A24 RID: 14884
		public abstract float ParentStartTick { get; }

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06003A25 RID: 14885
		public abstract float ParentHashCode { get; }

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06003A26 RID: 14886
		public abstract SoundParams ExternalParams { get; }

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06003A27 RID: 14887
		public abstract SoundInfo Info { get; }

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x00168F60 File Offset: 0x00167160
		public Map Map
		{
			get
			{
				return this.Info.Maker.Map;
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06003A29 RID: 14889 RVA: 0x0002CCFD File Offset: 0x0002AEFD
		protected bool TestPlaying
		{
			get
			{
				return this.Info.testPlay;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06003A2A RID: 14890 RVA: 0x00168F84 File Offset: 0x00167184
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

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06003A2B RID: 14891 RVA: 0x0002CD0A File Offset: 0x0002AF0A
		protected float ContextVolumeMultiplier
		{
			get
			{
				if (SoundDefHelper.CorrectContextNow(this.subDef.parentDef, this.Map))
				{
					return 1f;
				}
				return 0f;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06003A2C RID: 14892 RVA: 0x00168FE0 File Offset: 0x001671E0
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

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06003A2D RID: 14893 RVA: 0x0002CD2F File Offset: 0x0002AF2F
		public float SanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.Volume, this.subDef.parentDef);
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06003A2E RID: 14894 RVA: 0x0016903C File Offset: 0x0016723C
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

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06003A2F RID: 14895 RVA: 0x0002CD47 File Offset: 0x0002AF47
		public float SanitizedPitch
		{
			get
			{
				return AudioSourceUtility.GetSanitizedPitch(this.Pitch, this.subDef.parentDef);
			}
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x00169094 File Offset: 0x00167294
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

		// Token: 0x06003A31 RID: 14897 RVA: 0x0016917C File Offset: 0x0016737C
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

		// Token: 0x06003A32 RID: 14898 RVA: 0x00169238 File Offset: 0x00167438
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

		// Token: 0x06003A33 RID: 14899 RVA: 0x0002CD5F File Offset: 0x0002AF5F
		public void SignalMappedVolume(float value, SoundParamTarget sourceParam)
		{
			this.volumeInMappings[sourceParam] = value;
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x00169294 File Offset: 0x00167494
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

		// Token: 0x06003A35 RID: 14901 RVA: 0x001692EC File Offset: 0x001674EC
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

		// Token: 0x06003A36 RID: 14902 RVA: 0x0002CD6E File Offset: 0x0002AF6E
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.startRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x0400285D RID: 10333
		public SubSoundDef subDef;

		// Token: 0x0400285E RID: 10334
		public AudioSource source;

		// Token: 0x0400285F RID: 10335
		public float startRealTime;

		// Token: 0x04002860 RID: 10336
		public int startTick;

		// Token: 0x04002861 RID: 10337
		public float resolvedVolume;

		// Token: 0x04002862 RID: 10338
		public float resolvedPitch;

		// Token: 0x04002863 RID: 10339
		private bool mappingsApplied;

		// Token: 0x04002864 RID: 10340
		private Dictionary<SoundParamTarget, float> volumeInMappings = new Dictionary<SoundParamTarget, float>();
	}
}
