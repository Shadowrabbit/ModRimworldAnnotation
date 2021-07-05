using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000106 RID: 262
	public class SoundDef : Def
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00021504 File Offset: 0x0001F704
		private bool HasSubSoundsOnCamera
		{
			get
			{
				for (int i = 0; i < this.subSounds.Count; i++)
				{
					if (this.subSounds[i].onCamera)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00021540 File Offset: 0x0001F740
		public bool HasSubSoundsInWorld
		{
			get
			{
				for (int i = 0; i < this.subSounds.Count; i++)
				{
					if (!this.subSounds[i].onCamera)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00021579 File Offset: 0x0001F779
		public int MaxSimultaneousSamples
		{
			get
			{
				return this.maxSimultaneous * this.subSounds.Count;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00021590 File Offset: 0x0001F790
		public FloatRange Duration
		{
			get
			{
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				foreach (SubSoundDef subSoundDef in this.subSounds)
				{
					num = Mathf.Min(num, subSoundDef.Duration.min);
					num2 = Mathf.Max(num2, subSoundDef.Duration.max);
				}
				return new FloatRange((num == float.PositiveInfinity) ? 0f : num, (num2 == float.NegativeInfinity) ? 0f : num2);
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00021634 File Offset: 0x0001F834
		public override void ResolveReferences()
		{
			for (int i = 0; i < this.subSounds.Count; i++)
			{
				this.subSounds[i].parentDef = this;
				this.subSounds[i].ResolveReferences();
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002167A File Offset: 0x0001F87A
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.slot != "" && !this.HasSubSoundsOnCamera)
			{
				yield return "Sound slots only work for on-camera sounds.";
			}
			if (this.HasSubSoundsInWorld && this.context != SoundContext.MapOnly)
			{
				yield return "Sounds with non-on-camera subsounds should use MapOnly context.";
			}
			if (this.priorityMode == VoicePriorityMode.PrioritizeNewest && this.sustain)
			{
				yield return "PrioritizeNewest is not supported with sustainers.";
			}
			if (this.maxVoices < 1)
			{
				yield return "Max voices is less than 1.";
			}
			if (!this.sustain && (this.sustainStartSound != null || this.sustainStopSound != null))
			{
				yield return "Sustainer start and end sounds only work with sounds defined as sustainers.";
			}
			int num;
			if (!this.sustain)
			{
				for (int i = 0; i < this.subSounds.Count; i = num + 1)
				{
					if (this.subSounds[i].startDelayRange.TrueMax > 0.001f)
					{
						yield return "startDelayRange is only supported on sustainers.";
					}
					num = i;
				}
			}
			List<SoundDef> defs = DefDatabase<SoundDef>.AllDefsListForReading;
			for (int i = 0; i < defs.Count; i = num + 1)
			{
				if (!defs[i].eventNames.NullOrEmpty<string>())
				{
					for (int j = 0; j < defs[i].eventNames.Count; j = num + 1)
					{
						if (defs[i].eventNames[j] == this.defName)
						{
							yield return this.defName + " is also defined in the eventNames list for " + defs[i];
						}
						num = j;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002168C File Offset: 0x0001F88C
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (this.testSustainer == null)
			{
				if (widgetRow.ButtonIcon(TexButton.Play, null, null, null, null, true))
				{
					this.ResolveReferences();
					SoundInfo info;
					if (this.HasSubSoundsInWorld)
					{
						IntVec3 mapPosition = Find.CameraDriver.MapPosition;
						info = SoundInfo.InMap(new TargetInfo(mapPosition, Find.CurrentMap, false), MaintenanceType.PerFrame);
						for (int i = 0; i < 5; i++)
						{
							FleckMaker.ThrowDustPuff(mapPosition, Find.CurrentMap, 1.5f);
						}
					}
					else
					{
						info = SoundInfo.OnCamera(MaintenanceType.PerFrame);
					}
					info.testPlay = true;
					if (this.sustain)
					{
						this.testSustainer = this.TrySpawnSustainer(info);
						return;
					}
					this.PlayOneShot(info);
					return;
				}
			}
			else
			{
				this.testSustainer.Maintain();
				if (widgetRow.ButtonIcon(TexButton.Stop, null, null, null, null, true))
				{
					this.testSustainer.End();
					this.testSustainer = null;
				}
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00021790 File Offset: 0x0001F990
		public static SoundDef Named(string defName)
		{
			SoundDef namedSilentFail = DefDatabase<SoundDef>.GetNamedSilentFail(defName);
			if (namedSilentFail != null)
			{
				return namedSilentFail;
			}
			if (!Prefs.DevMode)
			{
				object obj = SoundDef.undefinedSoundDefsLock;
				lock (obj)
				{
					if (SoundDef.undefinedSoundDefs.ContainsKey(defName))
					{
						return SoundDef.UndefinedDefNamed(defName);
					}
				}
			}
			List<SoundDef> allDefsListForReading = DefDatabase<SoundDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].eventNames.Count > 0)
				{
					for (int j = 0; j < allDefsListForReading[i].eventNames.Count; j++)
					{
						if (allDefsListForReading[i].eventNames[j] == defName)
						{
							return allDefsListForReading[i];
						}
					}
				}
			}
			if (DefDatabase<SoundDef>.DefCount == 0)
			{
				Log.Warning("Tried to get SoundDef named " + defName + ", but sound defs aren't loaded yet (is it a static variable initialized before play data?).");
			}
			return SoundDef.UndefinedDefNamed(defName);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00021894 File Offset: 0x0001FA94
		private static SoundDef UndefinedDefNamed(string defName)
		{
			object obj = SoundDef.undefinedSoundDefsLock;
			SoundDef soundDef;
			lock (obj)
			{
				if (!SoundDef.undefinedSoundDefs.TryGetValue(defName, out soundDef))
				{
					soundDef = new SoundDef();
					soundDef.isUndefined = true;
					soundDef.defName = defName;
					SoundDef.undefinedSoundDefs.Add(defName, soundDef);
				}
			}
			return soundDef;
		}

		// Token: 0x04000638 RID: 1592
		[Description("If checked, this sound is a sustainer.\n\nSustainers are used for sounds with a defined beginning and end (as opposed to OneShots, which just fire at a given instant).\n\nThis value must match what the game expects from the SubSoundDef with this name.")]
		[DefaultValue(false)]
		public bool sustain;

		// Token: 0x04000639 RID: 1593
		[Description("When the sound is allowed to play: only when the map view is active, only when the world view is active, or always (map + world + main menu).")]
		[DefaultValue(SoundContext.Any)]
		public SoundContext context;

		// Token: 0x0400063A RID: 1594
		[Description("Event names for this sound. \n\nThe code will look up sounds to play them according to their name. If the code finds the event name it wants in this list, it will trigger this sound.\n\nThe Def name is also used as an event name. Obsolete")]
		public List<string> eventNames = new List<string>();

		// Token: 0x0400063B RID: 1595
		[Description("For one-shots, this is the number of individual sounds from this Def than can be playing at a time.\n\n For sustainers, this is the number of sustainers that can be running with this sound (each of which can have sub-sounds). Sustainers can fade in and out as you move the camera or objects move, to keep the nearest ones audible.\n\nThis setting may not work for on-camera sounds.")]
		[DefaultValue(4)]
		public int maxVoices = 4;

		// Token: 0x0400063C RID: 1596
		[Description("The number of instances of this sound that can play at almost exactly the same moment. Handles cases like six gunners all firing their identical guns at the same time because a target came into view of all of them at the same time. Ordinarily this would make a painfully loud sound, but you can reduce it with this.")]
		[DefaultValue(3)]
		public int maxSimultaneous = 3;

		// Token: 0x0400063D RID: 1597
		[Description("If the system has to not play some instances of this sound because of maxVoices, this determines which ones are ignored.\n\nYou should use PrioritizeNewest for things like gunshots, so older still-playing samples are overridden by newer, more important ones.\n\nSustained sounds should usually prioritize nearest, so if a new fire starts burning nearby it can override a more distant one.")]
		[DefaultValue(VoicePriorityMode.PrioritizeNewest)]
		public VoicePriorityMode priorityMode;

		// Token: 0x0400063E RID: 1598
		[Description("The special sound slot this sound takes. If a sound with this slot is playing, new sounds in this slot will not play.\n\nOnly works for on-camera sounds.")]
		[DefaultValue("")]
		public string slot = "";

		// Token: 0x0400063F RID: 1599
		[LoadAlias("sustainerStartSound")]
		[Description("The name of the SoundDef that will be played when this sustainer starts.")]
		[DefaultValue("")]
		public SoundDef sustainStartSound;

		// Token: 0x04000640 RID: 1600
		[LoadAlias("sustainerStopSound")]
		[Description("The name of the SoundDef that will be played when this sustainer ends.")]
		[DefaultValue("")]
		public SoundDef sustainStopSound;

		// Token: 0x04000641 RID: 1601
		[Description("After a sustainer is ended, the sound will fade out over this many real-time seconds.")]
		[DefaultValue(0f)]
		public float sustainFadeoutTime;

		// Token: 0x04000642 RID: 1602
		[Description("All the sounds that will play when this set is triggered.")]
		public List<SubSoundDef> subSounds = new List<SubSoundDef>();

		// Token: 0x04000643 RID: 1603
		[Unsaved(false)]
		public bool isUndefined;

		// Token: 0x04000644 RID: 1604
		[Unsaved(false)]
		public Sustainer testSustainer;

		// Token: 0x04000645 RID: 1605
		private static Dictionary<string, SoundDef> undefinedSoundDefs = new Dictionary<string, SoundDef>();

		// Token: 0x04000646 RID: 1606
		private static object undefinedSoundDefsLock = new object();
	}
}
