using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002EC RID: 748
	public class Pawn_AgeTracker : IExposable
	{
		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x0007BA5E File Offset: 0x00079C5E
		// (set) Token: 0x06001543 RID: 5443 RVA: 0x0007BA66 File Offset: 0x00079C66
		public long BirthAbsTicks
		{
			get
			{
				return this.birthAbsTicksInt;
			}
			set
			{
				this.birthAbsTicksInt = value;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001544 RID: 5444 RVA: 0x0007BA6F File Offset: 0x00079C6F
		public int AgeBiologicalYears
		{
			get
			{
				return (int)(this.ageBiologicalTicksInt / 3600000L);
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x0007BA7F File Offset: 0x00079C7F
		public float AgeBiologicalYearsFloat
		{
			get
			{
				return (float)this.ageBiologicalTicksInt / 3600000f;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x0007BA8E File Offset: 0x00079C8E
		// (set) Token: 0x06001547 RID: 5447 RVA: 0x0007BA96 File Offset: 0x00079C96
		public long AgeBiologicalTicks
		{
			get
			{
				return this.ageBiologicalTicksInt;
			}
			set
			{
				this.ageBiologicalTicksInt = value;
				this.cachedLifeStageIndex = -1;
				this.CalculateInitialGrowth();
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x0007BAAC File Offset: 0x00079CAC
		// (set) Token: 0x06001549 RID: 5449 RVA: 0x0007BABB File Offset: 0x00079CBB
		public long AgeChronologicalTicks
		{
			get
			{
				return (long)GenTicks.TicksAbs - this.birthAbsTicksInt;
			}
			set
			{
				this.BirthAbsTicks = (long)GenTicks.TicksAbs - value;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x0007BACB File Offset: 0x00079CCB
		public int AgeChronologicalYears
		{
			get
			{
				return (int)(this.AgeChronologicalTicks / 3600000L);
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600154B RID: 5451 RVA: 0x0007BADB File Offset: 0x00079CDB
		public float AgeChronologicalYearsFloat
		{
			get
			{
				return (float)this.AgeChronologicalTicks / 3600000f;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x0007BAEA File Offset: 0x00079CEA
		public int BirthYear
		{
			get
			{
				return GenDate.Year(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x0007BAFC File Offset: 0x00079CFC
		public int BirthDayOfSeasonZeroBased
		{
			get
			{
				return GenDate.DayOfSeason(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x0007BB0E File Offset: 0x00079D0E
		public int BirthDayOfYear
		{
			get
			{
				return GenDate.DayOfYear(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x0007BB20 File Offset: 0x00079D20
		public Quadrum BirthQuadrum
		{
			get
			{
				return GenDate.Quadrum(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x0007BB34 File Offset: 0x00079D34
		public string AgeNumberString
		{
			get
			{
				string text = this.AgeBiologicalYearsFloat.ToStringApproxAge();
				if (this.AgeChronologicalYears != this.AgeBiologicalYears)
				{
					text = string.Concat(new object[]
					{
						text,
						" (",
						this.AgeChronologicalYears,
						")"
					});
				}
				return text;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001551 RID: 5457 RVA: 0x0007BB8C File Offset: 0x00079D8C
		public string AgeTooltipString
		{
			get
			{
				int value;
				int value2;
				int value3;
				float num;
				this.ageBiologicalTicksInt.TicksToPeriod(out value, out value2, out value3, out num);
				int value4;
				int value5;
				int value6;
				((long)GenTicks.TicksAbs - this.birthAbsTicksInt).TicksToPeriod(out value4, out value5, out value6, out num);
				string value7 = "FullDate".Translate(Find.ActiveLanguageWorker.OrdinalNumber(this.BirthDayOfSeasonZeroBased + 1, Gender.None), this.BirthQuadrum.Label(), this.BirthYear);
				string text = "Born".Translate(value7) + "\n" + "AgeChronological".Translate(value4, value5, value6) + "\n" + "AgeBiological".Translate(value, value2, value3);
				if (Prefs.DevMode)
				{
					text += "\n\nDev mode info:";
					text = text + "\nageBiologicalTicksInt: " + this.ageBiologicalTicksInt;
					text = text + "\nbirthAbsTicksInt: " + this.birthAbsTicksInt;
					text = text + "\ngrowth: " + this.growth;
					text = text + "\nage reversal demand deadline: " + ((int)Math.Abs(this.AgeReversalDemandedDeadlineTicks)).ToStringTicksToPeriod(true, false, true, true) + ((this.AgeReversalDemandedDeadlineTicks < 0L) ? " past deadline" : " in future");
				}
				return text;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x0007BD18 File Offset: 0x00079F18
		public int CurLifeStageIndex
		{
			get
			{
				if (this.cachedLifeStageIndex < 0)
				{
					this.RecalculateLifeStageIndex();
				}
				return this.cachedLifeStageIndex;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001553 RID: 5459 RVA: 0x0007BD2F File Offset: 0x00079F2F
		public LifeStageDef CurLifeStage
		{
			get
			{
				return this.CurLifeStageRace.def;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x0007BD3C File Offset: 0x00079F3C
		public LifeStageAge CurLifeStageRace
		{
			get
			{
				return this.pawn.RaceProps.lifeStageAges[this.CurLifeStageIndex];
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001555 RID: 5461 RVA: 0x0007BD5C File Offset: 0x00079F5C
		public PawnKindLifeStage CurKindLifeStage
		{
			get
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					Log.ErrorOnce("Tried to get CurKindLifeStage from humanlike pawn " + this.pawn, 8888811);
					return null;
				}
				return this.pawn.kindDef.lifeStages[this.CurLifeStageIndex];
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x0007BDB2 File Offset: 0x00079FB2
		public float Growth
		{
			get
			{
				return this.growth;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06001557 RID: 5463 RVA: 0x0007BDBC File Offset: 0x00079FBC
		private long TicksToAdulthood
		{
			get
			{
				return (long)Mathf.FloorToInt(this.pawn.RaceProps.lifeStageAges[this.pawn.RaceProps.lifeStageAges.Count - 1].minAge * 3600000f) - this.AgeBiologicalTicks;
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x0007BE0D File Offset: 0x0007A00D
		public long AgeReversalDemandedDeadlineTicks
		{
			get
			{
				return this.ageReversalDemandedAtAgeTicks - this.AgeBiologicalTicks;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001559 RID: 5465 RVA: 0x0007BE1C File Offset: 0x0007A01C
		public Pawn_AgeTracker.AgeReversalReason LastAgeReversalReason
		{
			get
			{
				return this.lastAgeReversalReason;
			}
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0007BE24 File Offset: 0x0007A024
		public Pawn_AgeTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0007BE88 File Offset: 0x0007A088
		public void ExposeData()
		{
			Scribe_Values.Look<long>(ref this.ageBiologicalTicksInt, "ageBiologicalTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.birthAbsTicksInt, "birthAbsTicks", 0L, false);
			Scribe_Values.Look<float>(ref this.growth, "growth", -1f, false);
			Scribe_Values.Look<long>(ref this.nextGrowthCheckTick, "nextGrowthCheckTick", -1L, false);
			Scribe_Values.Look<long>(ref this.ageReversalDemandedAtAgeTicks, "ageReversalDemandedAtAgeTicks", long.MaxValue, false);
			Scribe_Values.Look<Pawn_AgeTracker.AgeReversalReason>(ref this.lastAgeReversalReason, "lastAgeReversalReason", Pawn_AgeTracker.AgeReversalReason.Initial, false);
			Scribe_Values.Look<bool>(ref this.initializedAgeReversalDemand, "initializedAgeReversalDemand", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.cachedLifeStageIndex = -1;
			}
			if (this.ageReversalDemandedAtAgeTicks == 9223372036854775807L)
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
			}
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x0007BF4C File Offset: 0x0007A14C
		public void AgeTick()
		{
			this.ageBiologicalTicksInt += 1L;
			if ((long)Find.TickManager.TicksGame >= this.nextGrowthCheckTick)
			{
				this.CalculateGrowth(240);
			}
			if (this.pawn.IsHashIntervalTick(60000))
			{
				this.CheckAgeReversalDemand();
			}
			if (this.ageBiologicalTicksInt % 3600000L == 0L)
			{
				this.BirthdayBiological();
			}
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x0007BFB4 File Offset: 0x0007A1B4
		public void AgeTickMothballed(int interval)
		{
			long num = this.ageBiologicalTicksInt;
			this.ageBiologicalTicksInt += (long)interval;
			this.CalculateGrowth(interval);
			this.CheckAgeReversalDemand();
			int num2 = (int)(num / 3600000L);
			while ((long)num2 < this.ageBiologicalTicksInt / 3600000L)
			{
				this.BirthdayBiological();
				num2 += 3600000;
			}
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x0007C010 File Offset: 0x0007A210
		private void CheckAgeReversalDemand()
		{
			if (!ModsConfig.IdeologyActive || this.initializedAgeReversalDemand || this.pawn.Faction != Faction.OfPlayer || this.pawn.MapHeld == null)
			{
				return;
			}
			if (ExpectationsUtility.CurrentExpectationFor(this.pawn.MapHeld).order < ThoughtDefOf.AgeReversalDemanded.minExpectationForNegativeThought.order)
			{
				this.ResetAgeReversalDemand(this.lastAgeReversalReason, false);
				return;
			}
			this.initializedAgeReversalDemand = true;
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x0007C088 File Offset: 0x0007A288
		private void CalculateInitialGrowth()
		{
			this.growth = Mathf.Clamp01(this.AgeBiologicalYearsFloat / this.pawn.RaceProps.lifeStageAges[this.pawn.RaceProps.lifeStageAges.Count - 1].minAge);
			this.nextGrowthCheckTick = (long)(Find.TickManager.TicksGame + 240);
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0007C0F0 File Offset: 0x0007A2F0
		private void CalculateGrowth(int interval)
		{
			if (this.growth >= 1f)
			{
				this.nextGrowthCheckTick = long.MaxValue;
				return;
			}
			this.growth += PawnUtility.BodyResourceGrowthSpeed(this.pawn) * (float)interval / Mathf.Max((float)this.TicksToAdulthood, 1f);
			this.growth = Mathf.Min(this.growth, 1f);
			this.nextGrowthCheckTick = (long)(Find.TickManager.TicksGame + 240);
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x0007C17C File Offset: 0x0007A37C
		private void RecalculateLifeStageIndex()
		{
			int num = -1;
			if (this.growth < 0f)
			{
				this.CalculateInitialGrowth();
			}
			float num2 = Mathf.Lerp(0f, this.pawn.RaceProps.lifeStageAges[this.pawn.RaceProps.lifeStageAges.Count - 1].minAge, this.growth);
			List<LifeStageAge> lifeStageAges = this.pawn.RaceProps.lifeStageAges;
			for (int i = lifeStageAges.Count - 1; i >= 0; i--)
			{
				if (lifeStageAges[i].minAge <= num2 + 1E-06f)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				num = 0;
			}
			bool flag = this.cachedLifeStageIndex != num;
			this.cachedLifeStageIndex = num;
			if (flag && !this.pawn.RaceProps.Humanlike)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.pawn.Drawer.renderer.graphics.SetAllGraphicsDirty();
				});
				this.CheckChangePawnKindName();
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x0007C264 File Offset: 0x0007A464
		private void BirthdayBiological()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (HediffGiver_Birthday hediffGiver_Birthday in AgeInjuryUtility.RandomHediffsToGainOnBirthday(this.pawn, this.AgeBiologicalYears))
			{
				if (hediffGiver_Birthday.TryApply(this.pawn, null))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("    - " + hediffGiver_Birthday.hediff.LabelCap);
				}
			}
			if (this.pawn.RaceProps.Humanlike && PawnUtility.ShouldSendNotificationAbout(this.pawn) && stringBuilder.Length > 0)
			{
				string str = "BirthdayBiologicalAgeInjuries".Translate(this.pawn, this.AgeBiologicalYears, stringBuilder).AdjustedFor(this.pawn, "PAWN", true);
				Find.LetterStack.ReceiveLetter("LetterLabelBirthday".Translate(), str, LetterDefOf.NegativeEvent, this.pawn, null, null, null, null);
			}
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x0007C39C File Offset: 0x0007A59C
		public void ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason reason, bool cancelInitialization = false)
		{
			int num;
			if (reason == Pawn_AgeTracker.AgeReversalReason.Recruited)
			{
				num = this.RecruitedPawnAgeReversalDemandInDays.RandomInRange;
			}
			else if (reason == Pawn_AgeTracker.AgeReversalReason.ViaTreatment)
			{
				num = 60;
			}
			else
			{
				num = this.NewPawnAgeReversalDemandInDays.RandomInRange;
			}
			long num2 = (long)(num * 60000);
			long num3 = Math.Max(this.AgeBiologicalTicks, 72000000L) + num2;
			if (reason == Pawn_AgeTracker.AgeReversalReason.Recruited && num3 < this.ageReversalDemandedAtAgeTicks)
			{
				return;
			}
			this.ageReversalDemandedAtAgeTicks = num3;
			this.lastAgeReversalReason = reason;
			if (cancelInitialization)
			{
				this.initializedAgeReversalDemand = false;
			}
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0007C419 File Offset: 0x0007A619
		public void Notify_IdeoChanged()
		{
			if (this.pawn.Ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded))
			{
				this.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Recruited, false);
			}
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0007C43A File Offset: 0x0007A63A
		public void DebugForceBirthdayBiological()
		{
			this.BirthdayBiological();
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0007C444 File Offset: 0x0007A644
		public void CheckChangePawnKindName()
		{
			NameSingle nameSingle = this.pawn.Name as NameSingle;
			if (nameSingle == null || !nameSingle.Numerical)
			{
				return;
			}
			string kindLabel = this.pawn.KindLabel;
			if (nameSingle.NameWithoutNumber == kindLabel)
			{
				return;
			}
			int number = nameSingle.Number;
			string text = this.pawn.KindLabel + " " + number;
			if (!NameUseChecker.NameSingleIsUsed(text))
			{
				this.pawn.Name = new NameSingle(text, true);
				return;
			}
			this.pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(this.pawn, NameStyle.Numeric, null);
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0007C4DF File Offset: 0x0007A6DF
		public void DebugMakeOlder(long ticks)
		{
			this.ageBiologicalTicksInt += ticks;
			this.birthAbsTicksInt -= ticks;
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x04000F0C RID: 3852
		private Pawn pawn;

		// Token: 0x04000F0D RID: 3853
		private long ageBiologicalTicksInt = -1L;

		// Token: 0x04000F0E RID: 3854
		private long birthAbsTicksInt = -1L;

		// Token: 0x04000F0F RID: 3855
		private long nextGrowthCheckTick = -1L;

		// Token: 0x04000F10 RID: 3856
		private float growth = -1f;

		// Token: 0x04000F11 RID: 3857
		private long ageReversalDemandedAtAgeTicks;

		// Token: 0x04000F12 RID: 3858
		private Pawn_AgeTracker.AgeReversalReason lastAgeReversalReason;

		// Token: 0x04000F13 RID: 3859
		private bool initializedAgeReversalDemand;

		// Token: 0x04000F14 RID: 3860
		private int cachedLifeStageIndex = -1;

		// Token: 0x04000F15 RID: 3861
		private const float BornAtLongitude = 0f;

		// Token: 0x04000F16 RID: 3862
		private const int GrowthInterval = 240;

		// Token: 0x04000F17 RID: 3863
		public const int AgeReversalDemandMinAgeYears = 20;

		// Token: 0x04000F18 RID: 3864
		private const int TreatedPawnAgeReversalDemandInDays = 60;

		// Token: 0x04000F19 RID: 3865
		private readonly IntRange NewPawnAgeReversalDemandInDays = new IntRange(20, 40);

		// Token: 0x04000F1A RID: 3866
		private readonly IntRange RecruitedPawnAgeReversalDemandInDays = new IntRange(15, 20);

		// Token: 0x02001A24 RID: 6692
		public enum AgeReversalReason
		{
			// Token: 0x04006421 RID: 25633
			Initial,
			// Token: 0x04006422 RID: 25634
			Recruited,
			// Token: 0x04006423 RID: 25635
			ViaTreatment
		}
	}
}
