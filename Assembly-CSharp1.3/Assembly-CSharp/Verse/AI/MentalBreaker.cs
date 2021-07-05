using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x020005C6 RID: 1478
	public class MentalBreaker : IExposable
	{
		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06002AFF RID: 11007 RVA: 0x00101B8D File Offset: 0x000FFD8D
		public float BreakThresholdExtreme
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) * 0.14285715f;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002B00 RID: 11008 RVA: 0x00101BA6 File Offset: 0x000FFDA6
		public float BreakThresholdMajor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) * 0.5714286f;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06002B01 RID: 11009 RVA: 0x00101BBF File Offset: 0x000FFDBF
		public float BreakThresholdMinor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true);
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06002B02 RID: 11010 RVA: 0x00101BD2 File Offset: 0x000FFDD2
		private bool CanDoRandomMentalBreaks
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002B03 RID: 11011 RVA: 0x00101C02 File Offset: 0x000FFE02
		public bool BreakExtremeIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.CurMood < this.BreakThresholdExtreme;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06002B04 RID: 11012 RVA: 0x00101C21 File Offset: 0x000FFE21
		public bool BreakMajorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdMajor;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06002B05 RID: 11013 RVA: 0x00101C48 File Offset: 0x000FFE48
		public bool BreakMinorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && !this.BreakMajorIsImminent && this.CurMood < this.BreakThresholdMinor;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002B06 RID: 11014 RVA: 0x00101C77 File Offset: 0x000FFE77
		public bool BreakExtremeIsApproaching
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdExtreme + 0.1f;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002B07 RID: 11015 RVA: 0x00101CA4 File Offset: 0x000FFEA4
		public float CurMood
		{
			get
			{
				if (this.pawn.needs.mood == null)
				{
					return 0.5f;
				}
				return this.pawn.needs.mood.CurLevel;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002B08 RID: 11016 RVA: 0x00101CD3 File Offset: 0x000FFED3
		private IEnumerable<MentalBreakDef> CurrentPossibleMoodBreaks
		{
			get
			{
				MentalBreakIntensity intensity;
				Func<MentalBreakDef, bool> <>9__0;
				MentalBreakIntensity intensity2;
				for (intensity = this.CurrentDesiredMoodBreakIntensity; intensity != MentalBreakIntensity.None; intensity = (MentalBreakIntensity)(intensity2 - MentalBreakIntensity.Minor))
				{
					IEnumerable<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
					Func<MentalBreakDef, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((MentalBreakDef d) => d.intensity == intensity && d.Worker.BreakCanOccur(this.pawn)));
					}
					IEnumerable<MentalBreakDef> enumerable = allDefsListForReading.Where(predicate);
					bool flag = false;
					foreach (MentalBreakDef mentalBreakDef in enumerable)
					{
						yield return mentalBreakDef;
						flag = true;
					}
					IEnumerator<MentalBreakDef> enumerator = null;
					if (flag)
					{
						yield break;
					}
					intensity2 = intensity;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002B09 RID: 11017 RVA: 0x00101CE3 File Offset: 0x000FFEE3
		private MentalBreakIntensity CurrentDesiredMoodBreakIntensity
		{
			get
			{
				if (this.ticksBelowExtreme >= 2000)
				{
					return MentalBreakIntensity.Extreme;
				}
				if (this.ticksBelowMajor >= 2000)
				{
					return MentalBreakIntensity.Major;
				}
				if (this.ticksBelowMinor >= 2000)
				{
					return MentalBreakIntensity.Minor;
				}
				return MentalBreakIntensity.None;
			}
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000033AC File Offset: 0x000015AC
		public MentalBreaker()
		{
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x00101D13 File Offset: 0x000FFF13
		public MentalBreaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x00101D22 File Offset: 0x000FFF22
		internal void Reset()
		{
			this.ticksBelowExtreme = 0;
			this.ticksBelowMajor = 0;
			this.ticksBelowMinor = 0;
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x00101D3C File Offset: 0x000FFF3C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksUntilCanDoMentalBreak, "ticksUntilCanDoMentalBreak", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowExtreme, "ticksBelowExtreme", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMajor, "ticksBelowMajor", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMinor, "ticksBelowMinor", 0, false);
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x00101D94 File Offset: 0x000FFF94
		public void MentalBreakerTick()
		{
			if (this.ticksUntilCanDoMentalBreak > 0 && this.pawn.Awake())
			{
				this.ticksUntilCanDoMentalBreak--;
			}
			if (this.CanDoRandomMentalBreaks && this.pawn.MentalStateDef == null && this.pawn.IsHashIntervalTick(150))
			{
				if (!DebugSettings.enableRandomMentalStates)
				{
					return;
				}
				if (this.CurMood < this.BreakThresholdExtreme)
				{
					this.ticksBelowExtreme += 150;
				}
				else
				{
					this.ticksBelowExtreme = 0;
				}
				if (this.CurMood < this.BreakThresholdMajor)
				{
					this.ticksBelowMajor += 150;
				}
				else
				{
					this.ticksBelowMajor = 0;
				}
				if (this.CurMood < this.BreakThresholdMinor)
				{
					this.ticksBelowMinor += 150;
				}
				else
				{
					this.ticksBelowMinor = 0;
				}
				if (this.TestMoodMentalBreak() && this.TryDoRandomMoodCausedMentalBreak())
				{
					return;
				}
				if (this.pawn.story != null)
				{
					List<Trait> allTraits = this.pawn.story.traits.allTraits;
					for (int i = 0; i < allTraits.Count; i++)
					{
						if (allTraits[i].CurrentData.MentalStateGiver.CheckGive(this.pawn, 150))
						{
							return;
						}
					}
				}
			}
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x00101EE4 File Offset: 0x001000E4
		private bool TestMoodMentalBreak()
		{
			if (this.ticksUntilCanDoMentalBreak > 0)
			{
				return false;
			}
			if (this.ticksBelowExtreme > 2000)
			{
				return Rand.MTBEventOccurs(0.5f, 60000f, 150f);
			}
			if (this.ticksBelowMajor > 2000)
			{
				return Rand.MTBEventOccurs(0.8f, 60000f, 150f);
			}
			return this.ticksBelowMinor > 2000 && Rand.MTBEventOccurs(4f, 60000f, 150f);
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x00101F64 File Offset: 0x00100164
		public bool TryDoRandomMoodCausedMentalBreak()
		{
			if (!this.CanDoRandomMentalBreaks || this.pawn.Downed || !this.pawn.Awake() || this.pawn.InMentalState)
			{
				return false;
			}
			if (this.pawn.Faction != Faction.OfPlayer && this.CurrentDesiredMoodBreakIntensity != MentalBreakIntensity.Extreme)
			{
				return false;
			}
			if (QuestUtility.AnyQuestDisablesRandomMoodCausedMentalBreaksFor(this.pawn))
			{
				return false;
			}
			MentalBreakDef mentalBreakDef;
			if (!this.CurrentPossibleMoodBreaks.TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(this.pawn, true), out mentalBreakDef))
			{
				return false;
			}
			Thought thought = this.RandomFinalStraw();
			TaggedString taggedString = "MentalStateReason_Mood".Translate();
			if (thought != null)
			{
				taggedString += "\n\n" + "FinalStraw".Translate(thought.LabelCap);
			}
			return mentalBreakDef.Worker.TryStart(this.pawn, taggedString, true);
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x00102040 File Offset: 0x00100240
		private Thought RandomFinalStraw()
		{
			this.pawn.needs.mood.thoughts.GetAllMoodThoughts(MentalBreaker.tmpThoughts);
			float num = 0f;
			for (int i = 0; i < MentalBreaker.tmpThoughts.Count; i++)
			{
				float num2 = MentalBreaker.tmpThoughts[i].MoodOffset();
				if (num2 < num)
				{
					num = num2;
				}
			}
			float maxMoodOffset = num * 0.5f;
			Thought result = null;
			(from x in MentalBreaker.tmpThoughts
			where x.MoodOffset() <= maxMoodOffset
			select x).TryRandomElementByWeight((Thought x) => -x.MoodOffset(), out result);
			MentalBreaker.tmpThoughts.Clear();
			return result;
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x001020FE File Offset: 0x001002FE
		public void Notify_RecoveredFromMentalState()
		{
			this.ticksUntilCanDoMentalBreak = 15000;
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x0010210B File Offset: 0x0010030B
		public float MentalBreakThresholdFor(MentalBreakIntensity intensity)
		{
			switch (intensity)
			{
			case MentalBreakIntensity.Minor:
				return this.BreakThresholdMinor;
			case MentalBreakIntensity.Major:
				return this.BreakThresholdMajor;
			case MentalBreakIntensity.Extreme:
				return this.BreakThresholdExtreme;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x00102140 File Offset: 0x00100340
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn.ToString());
			stringBuilder.AppendLine("   ticksUntilCanDoMentalBreak=" + this.ticksUntilCanDoMentalBreak);
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   ticksBelowExtreme=",
				this.ticksBelowExtreme,
				"/",
				2000
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   ticksBelowSerious=",
				this.ticksBelowMajor,
				"/",
				2000
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"   ticksBelowMinor=",
				this.ticksBelowMinor,
				"/",
				2000
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current desired mood break intensity: " + this.CurrentDesiredMoodBreakIntensity.ToString());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Current possible mood breaks:");
			float num = (from d in this.CurrentPossibleMoodBreaks
			select d.Worker.CommonalityFor(this.pawn, true)).Sum();
			foreach (MentalBreakDef mentalBreakDef in this.CurrentPossibleMoodBreaks)
			{
				float num2 = mentalBreakDef.Worker.CommonalityFor(this.pawn, true);
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   ",
					mentalBreakDef,
					"     ",
					(num2 / num).ToStringPercent()
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x0010231C File Offset: 0x0010051C
		internal void LogPossibleMentalBreaks()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn + " current possible mood mental breaks:");
			stringBuilder.AppendLine("CurrentDesiredMoodBreakIntensity: " + this.CurrentDesiredMoodBreakIntensity);
			foreach (MentalBreakDef arg in this.CurrentPossibleMoodBreaks)
			{
				stringBuilder.AppendLine("  " + arg);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001A5B RID: 6747
		private Pawn pawn;

		// Token: 0x04001A5C RID: 6748
		private int ticksUntilCanDoMentalBreak;

		// Token: 0x04001A5D RID: 6749
		private int ticksBelowExtreme;

		// Token: 0x04001A5E RID: 6750
		private int ticksBelowMajor;

		// Token: 0x04001A5F RID: 6751
		private int ticksBelowMinor;

		// Token: 0x04001A60 RID: 6752
		private const int CheckInterval = 150;

		// Token: 0x04001A61 RID: 6753
		private const float ExtremeBreakMTBDays = 0.5f;

		// Token: 0x04001A62 RID: 6754
		private const float MajorBreakMTBDays = 0.8f;

		// Token: 0x04001A63 RID: 6755
		private const float MinorBreakMTBDays = 4f;

		// Token: 0x04001A64 RID: 6756
		private const int MinTicksBelowToBreak = 2000;

		// Token: 0x04001A65 RID: 6757
		private const int MinTicksSinceRecoveryToBreak = 15000;

		// Token: 0x04001A66 RID: 6758
		private const float MajorBreakMoodFraction = 0.5714286f;

		// Token: 0x04001A67 RID: 6759
		private const float ExtremeBreakMoodFraction = 0.14285715f;

		// Token: 0x04001A68 RID: 6760
		private static List<Thought> tmpThoughts = new List<Thought>();
	}
}
