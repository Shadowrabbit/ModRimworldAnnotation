using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000568 RID: 1384
	public class SubSoundDef : Editable
	{
		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x060028C5 RID: 10437 RVA: 0x000F74F4 File Offset: 0x000F56F4
		public FloatRange Duration
		{
			get
			{
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				foreach (ResolvedGrain resolvedGrain in this.resolvedGrains)
				{
					num = Mathf.Min(num, resolvedGrain.duration);
					num2 = Mathf.Max(num2, resolvedGrain.duration);
				}
				if (num == float.PositiveInfinity || num2 == float.NegativeInfinity)
				{
					return new FloatRange(0f, 0f);
				}
				return new FloatRange(num / Mathf.Abs(this.pitchRange.min), num2 / Mathf.Abs(this.pitchRange.max));
			}
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x000F75B0 File Offset: 0x000F57B0
		public virtual void TryPlay(SoundInfo info)
		{
			if (this.resolvedGrains.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot play ",
					this.parentDef,
					" (subSound ",
					this,
					"_: No resolved grains."
				}));
				return;
			}
			if (!Find.SoundRoot.oneShotManager.CanAddPlayingOneShot(this.parentDef, info))
			{
				return;
			}
			if (Current.Game != null && !this.gameSpeedRange.Includes((int)Find.TickManager.CurTimeSpeed))
			{
				return;
			}
			ResolvedGrain resolvedGrain = this.RandomizedResolvedGrain();
			ResolvedGrain_Clip resolvedGrain_Clip = resolvedGrain as ResolvedGrain_Clip;
			if (resolvedGrain_Clip != null)
			{
				if (SampleOneShot.TryMakeAndPlay(this, resolvedGrain_Clip.clip, info) == null)
				{
					return;
				}
				SoundSlotManager.Notify_Played(this.parentDef.slot, resolvedGrain_Clip.clip.length);
			}
			this.Notify_GrainPlayed(resolvedGrain);
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000F767C File Offset: 0x000F587C
		public void Notify_GrainPlayed(ResolvedGrain chosenGrain)
		{
			if (this.distinctResolvedGrainsCount > 1)
			{
				if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
				{
					while (this.recentlyPlayedResolvedGrains.Count >= this.numToAvoid)
					{
						this.recentlyPlayedResolvedGrains.Dequeue();
					}
					if (this.recentlyPlayedResolvedGrains.Count < this.numToAvoid)
					{
						this.recentlyPlayedResolvedGrains.Enqueue(chosenGrain);
						return;
					}
				}
				else if (this.repeatMode == RepeatSelectMode.NeverTwice)
				{
					this.lastPlayedResolvedGrain = chosenGrain;
				}
			}
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x000F76EC File Offset: 0x000F58EC
		public bool IsSameOrHasSameTag(SubSoundDef other)
		{
			return this == other || (!this.tag.NullOrEmpty() && !other.tag.NullOrEmpty() && this.tag == other.tag);
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x000F7724 File Offset: 0x000F5924
		public ResolvedGrain RandomizedResolvedGrain()
		{
			ResolvedGrain chosenGrain = null;
			for (;;)
			{
				chosenGrain = this.resolvedGrains.RandomElement<ResolvedGrain>();
				if (this.distinctResolvedGrainsCount <= 1)
				{
					break;
				}
				if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
				{
					if (!(from g in this.recentlyPlayedResolvedGrains
					where g.Equals(chosenGrain)
					select g).Any<ResolvedGrain>())
					{
						break;
					}
				}
				else if (this.repeatMode != RepeatSelectMode.NeverTwice || !chosenGrain.Equals(this.lastPlayedResolvedGrain))
				{
					break;
				}
			}
			return chosenGrain;
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x000F77A3 File Offset: 0x000F59A3
		public float RandomizedVolume()
		{
			return this.volumeRange.RandomInRange / 100f;
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x000F77B6 File Offset: 0x000F59B6
		public override void ResolveReferences()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.resolvedGrains.Clear();
				foreach (AudioGrain audioGrain in this.grains)
				{
					foreach (ResolvedGrain item in audioGrain.GetResolvedGrains())
					{
						this.resolvedGrains.Add(item);
					}
				}
				this.distinctResolvedGrainsCount = this.resolvedGrains.Distinct<ResolvedGrain>().Count<ResolvedGrain>();
				this.numToAvoid = Mathf.FloorToInt((float)this.distinctResolvedGrainsCount / 2f);
				if (this.distinctResolvedGrainsCount >= 6)
				{
					this.numToAvoid++;
				}
			});
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000F77C9 File Offset: 0x000F59C9
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.resolvedGrains.Count == 0)
			{
				yield return "No grains resolved.";
			}
			if (this.sustainAttack + this.sustainRelease > this.sustainLoopDurationRange.TrueMin)
			{
				yield return "Attack + release < min loop duration. Sustain samples will cut off.";
			}
			if (this.distRange.min > this.distRange.max)
			{
				yield return "Dist range min/max are reversed.";
			}
			if (this.gameSpeedRange.max == 0)
			{
				yield return "gameSpeedRange should have max value greater than 0";
			}
			if (this.gameSpeedRange.min > this.gameSpeedRange.max)
			{
				yield return "gameSpeedRange min/max are reversed.";
			}
			foreach (SoundParameterMapping soundParameterMapping in this.paramMappings)
			{
				if (soundParameterMapping.inParam == null || soundParameterMapping.outParam == null)
				{
					yield return "At least one parameter mapping is missing an in or out parameter.";
					break;
				}
				if (soundParameterMapping.outParam != null)
				{
					Type neededFilter = soundParameterMapping.outParam.NeededFilterType;
					if (neededFilter != null && !(from fil in this.filters
					where fil.GetType() == neededFilter
					select fil).Any<SoundFilter>())
					{
						yield return "A parameter wants to modify the " + neededFilter.ToString() + " filter, but this sound doesn't have it.";
					}
				}
			}
			List<SoundParameterMapping>.Enumerator enumerator = default(List<SoundParameterMapping>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000F77D9 File Offset: 0x000F59D9
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x0400193F RID: 6463
		[Description("A name to help you identify the sound.")]
		[DefaultValue("UnnamedSubSoundDef")]
		[MayTranslate]
		public string name = "UnnamedSubSoundDef";

		// Token: 0x04001940 RID: 6464
		[Description("Whether this sound plays on the camera or in the world.\n\nThis must match what the game expects from the sound Def with this name.")]
		[DefaultValue(false)]
		public bool onCamera;

		// Token: 0x04001941 RID: 6465
		[Description("Whether to mute this subSound while the game is paused (either by the pausing in play or by opening a menu)")]
		[DefaultValue(false)]
		public bool muteWhenPaused;

		// Token: 0x04001942 RID: 6466
		[Description("Whether this subSound's tempo should be affected by the current tick rate.")]
		[DefaultValue(false)]
		public bool tempoAffectedByGameSpeed;

		// Token: 0x04001943 RID: 6467
		[Description("The sound grains used for this sample. The game will choose one of these randomly when the sound plays. Sustainers choose one for each sample as it begins.")]
		public List<AudioGrain> grains = new List<AudioGrain>();

		// Token: 0x04001944 RID: 6468
		[EditSliderRange(0f, 100f)]
		[Description("This sound will play at a random volume inside this range.\n\nSustainers will choose a different random volume for each sample.")]
		[DefaultFloatRange(50f, 50f)]
		public FloatRange volumeRange = new FloatRange(50f, 50f);

		// Token: 0x04001945 RID: 6469
		[EditSliderRange(0.05f, 2f)]
		[Description("This sound will play at a random pitch inside this range.\n\nSustainers will choose a different random pitch for each sample.")]
		[DefaultFloatRange(1f, 1f)]
		public FloatRange pitchRange = FloatRange.One;

		// Token: 0x04001946 RID: 6470
		[EditSliderRange(0f, 200f)]
		[Description("This sound will play max volume when it is under minDistance from the camera.\n\nIt will fade out linearly until the camera distance reaches its max.")]
		[DefaultFloatRange(25f, 70f)]
		public FloatRange distRange = new FloatRange(25f, 70f);

		// Token: 0x04001947 RID: 6471
		[Description("When the sound chooses the next grain, you may use this setting to have it avoid repeating the last grain, or avoid repeating any of the grains in the last X played, X being half the total number of grains defined.")]
		[DefaultValue(RepeatSelectMode.NeverLastHalf)]
		public RepeatSelectMode repeatMode = RepeatSelectMode.NeverLastHalf;

		// Token: 0x04001948 RID: 6472
		[Description("Mappings between game parameters (like fire size or wind speed) and properties of the sound.")]
		[DefaultEmptyList(typeof(SoundParameterMapping))]
		public List<SoundParameterMapping> paramMappings = new List<SoundParameterMapping>();

		// Token: 0x04001949 RID: 6473
		[Description("The filters to be applied to this sound.")]
		[DefaultEmptyList(typeof(SoundFilter))]
		public List<SoundFilter> filters = new List<SoundFilter>();

		// Token: 0x0400194A RID: 6474
		[Description("A range of possible times between when this sound is triggered and when it will actually start playing.")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange startDelayRange = FloatRange.Zero;

		// Token: 0x0400194B RID: 6475
		[Description("A range of game speeds this sound can be played on.")]
		public IntRange gameSpeedRange = new IntRange(0, 999);

		// Token: 0x0400194C RID: 6476
		[Description("One shots sharing the same tag are treated as the same sound when determining importance.")]
		[NoTranslate]
		public string tag;

		// Token: 0x0400194D RID: 6477
		[Description("If true, each sample in the sustainer will be looped and ended only after sustainerLoopDurationRange. If not, the sounds will just play once and end after their own length.")]
		[DefaultValue(true)]
		public bool sustainLoop = true;

		// Token: 0x0400194E RID: 6478
		[EditSliderRange(0f, 10f)]
		[Description("The range of durations that individual looped samples in the sustainer will have. Each sample ends after a time randomly chosen in this range.\n\nOnly used if the sustainer is looped.")]
		[DefaultFloatRange(9999f, 9999f)]
		public FloatRange sustainLoopDurationRange = new FloatRange(9999f, 9999f);

		// Token: 0x0400194F RID: 6479
		[EditSliderRange(-2f, 2f)]
		[Description("The time between when one sample ends and the next starts.\n\nSet to negative if you wish samples to overlap.")]
		[LoadAlias("sustainInterval")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange sustainIntervalRange = FloatRange.Zero;

		// Token: 0x04001950 RID: 6480
		[EditSliderRange(0f, 2f)]
		[Description("The fade-in time of each sample. The sample will start at 0 volume and fade in over this number of seconds.")]
		[DefaultValue(0f)]
		public float sustainAttack;

		// Token: 0x04001951 RID: 6481
		[Description("Skip the attack on the first sustainer sample.")]
		[DefaultValue(true)]
		public bool sustainSkipFirstAttack = true;

		// Token: 0x04001952 RID: 6482
		[EditSliderRange(0f, 2f)]
		[Description("The fade-out time of each sample. At this number of seconds before the sample ends, it will start fading out. Its volume will be zero at the moment it finishes fading out.")]
		[DefaultValue(0f)]
		public float sustainRelease;

		// Token: 0x04001953 RID: 6483
		[Unsaved(false)]
		public SoundDef parentDef;

		// Token: 0x04001954 RID: 6484
		[Unsaved(false)]
		private List<ResolvedGrain> resolvedGrains = new List<ResolvedGrain>();

		// Token: 0x04001955 RID: 6485
		[Unsaved(false)]
		private ResolvedGrain lastPlayedResolvedGrain;

		// Token: 0x04001956 RID: 6486
		[Unsaved(false)]
		private int numToAvoid;

		// Token: 0x04001957 RID: 6487
		[Unsaved(false)]
		private int distinctResolvedGrainsCount;

		// Token: 0x04001958 RID: 6488
		[Unsaved(false)]
		private Queue<ResolvedGrain> recentlyPlayedResolvedGrains = new Queue<ResolvedGrain>();
	}
}
