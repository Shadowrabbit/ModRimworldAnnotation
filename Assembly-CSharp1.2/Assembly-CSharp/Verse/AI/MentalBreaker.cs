using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A16 RID: 2582
	public class MentalBreaker : IExposable
	{
		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06003DA6 RID: 15782 RVA: 0x0002E657 File Offset: 0x0002C857
		public float BreakThresholdExtreme
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) * 0.14285715f;
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06003DA7 RID: 15783 RVA: 0x0002E670 File Offset: 0x0002C870
		public float BreakThresholdMajor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true) * 0.5714286f;
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06003DA8 RID: 15784 RVA: 0x0002E689 File Offset: 0x0002C889
		public float BreakThresholdMinor
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.MentalBreakThreshold, true);
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06003DA9 RID: 15785 RVA: 0x0002E69C File Offset: 0x0002C89C
		private bool CanDoRandomMentalBreaks
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06003DAA RID: 15786 RVA: 0x0002E6CC File Offset: 0x0002C8CC
		public bool BreakExtremeIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && this.CurMood < this.BreakThresholdExtreme;
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06003DAB RID: 15787 RVA: 0x0002E6EB File Offset: 0x0002C8EB
		public bool BreakMajorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdMajor;
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06003DAC RID: 15788 RVA: 0x0002E712 File Offset: 0x0002C912
		public bool BreakMinorIsImminent
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && !this.BreakMajorIsImminent && this.CurMood < this.BreakThresholdMinor;
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06003DAD RID: 15789 RVA: 0x0002E741 File Offset: 0x0002C941
		public bool BreakExtremeIsApproaching
		{
			get
			{
				return this.pawn.MentalStateDef == null && !this.BreakExtremeIsImminent && this.CurMood < this.BreakThresholdExtreme + 0.1f;
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003DAE RID: 15790 RVA: 0x0002E76E File Offset: 0x0002C96E
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

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003DAF RID: 15791 RVA: 0x0002E79D File Offset: 0x0002C99D
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

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003DB0 RID: 15792 RVA: 0x0002E7AD File Offset: 0x0002C9AD
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

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00006B8B File Offset: 0x00004D8B
		public MentalBreaker()
		{
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x0002E7DD File Offset: 0x0002C9DD
		public MentalBreaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06003DB3 RID: 15795 RVA: 0x0002E7EC File Offset: 0x0002C9EC
		internal void Reset()
		{
			this.ticksBelowExtreme = 0;
			this.ticksBelowMajor = 0;
			this.ticksBelowMinor = 0;
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x001761EC File Offset: 0x001743EC
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksUntilCanDoMentalBreak, "ticksUntilCanDoMentalBreak", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowExtreme, "ticksBelowExtreme", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMajor, "ticksBelowMajor", 0, false);
			Scribe_Values.Look<int>(ref this.ticksBelowMinor, "ticksBelowMinor", 0, false);
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x00176244 File Offset: 0x00174444
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

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00176394 File Offset: 0x00174594
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

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00176414 File Offset: 0x00174614
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

		// Token: 0x06003DB8 RID: 15800 RVA: 0x001764F0 File Offset: 0x001746F0
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

		// Token: 0x06003DB9 RID: 15801 RVA: 0x0002E803 File Offset: 0x0002CA03
		public void Notify_RecoveredFromMentalState()
		{
			this.ticksUntilCanDoMentalBreak = 15000;
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x0002E810 File Offset: 0x0002CA10
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

		// Token: 0x06003DBB RID: 15803 RVA: 0x001765B0 File Offset: 0x001747B0
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

		// Token: 0x06003DBC RID: 15804 RVA: 0x0017678C File Offset: 0x0017498C
		internal void LogPossibleMentalBreaks()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.pawn + " current possible mood mental breaks:");
			stringBuilder.AppendLine("CurrentDesiredMoodBreakIntensity: " + this.CurrentDesiredMoodBreakIntensity);
			foreach (MentalBreakDef arg in this.CurrentPossibleMoodBreaks)
			{
				stringBuilder.AppendLine("  " + arg);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04002ABC RID: 10940
		private Pawn pawn;

		// Token: 0x04002ABD RID: 10941
		private int ticksUntilCanDoMentalBreak;

		// Token: 0x04002ABE RID: 10942
		private int ticksBelowExtreme;

		// Token: 0x04002ABF RID: 10943
		private int ticksBelowMajor;

		// Token: 0x04002AC0 RID: 10944
		private int ticksBelowMinor;

		// Token: 0x04002AC1 RID: 10945
		private const int CheckInterval = 150;

		// Token: 0x04002AC2 RID: 10946
		private const float ExtremeBreakMTBDays = 0.5f;

		// Token: 0x04002AC3 RID: 10947
		private const float MajorBreakMTBDays = 0.8f;

		// Token: 0x04002AC4 RID: 10948
		private const float MinorBreakMTBDays = 4f;

		// Token: 0x04002AC5 RID: 10949
		private const int MinTicksBelowToBreak = 2000;

		// Token: 0x04002AC6 RID: 10950
		private const int MinTicksSinceRecoveryToBreak = 15000;

		// Token: 0x04002AC7 RID: 10951
		private const float MajorBreakMoodFraction = 0.5714286f;

		// Token: 0x04002AC8 RID: 10952
		private const float ExtremeBreakMoodFraction = 0.14285715f;

		// Token: 0x04002AC9 RID: 10953
		private static List<Thought> tmpThoughts = new List<Thought>();
	}
}
