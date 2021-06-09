using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200044F RID: 1103
	public class Pawn_AgeTracker : IExposable
	{
		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x000192A4 File Offset: 0x000174A4
		// (set) Token: 0x06001BA9 RID: 7081 RVA: 0x000192AC File Offset: 0x000174AC
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

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001BAA RID: 7082 RVA: 0x000192B5 File Offset: 0x000174B5
		public int AgeBiologicalYears
		{
			get
			{
				return (int)(this.ageBiologicalTicksInt / 3600000L);
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001BAB RID: 7083 RVA: 0x000192C5 File Offset: 0x000174C5
		public float AgeBiologicalYearsFloat
		{
			get
			{
				return (float)this.ageBiologicalTicksInt / 3600000f;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001BAC RID: 7084 RVA: 0x000192D4 File Offset: 0x000174D4
		// (set) Token: 0x06001BAD RID: 7085 RVA: 0x000192DC File Offset: 0x000174DC
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
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001BAE RID: 7086 RVA: 0x000192EC File Offset: 0x000174EC
		// (set) Token: 0x06001BAF RID: 7087 RVA: 0x000192FB File Offset: 0x000174FB
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

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001BB0 RID: 7088 RVA: 0x0001930B File Offset: 0x0001750B
		public int AgeChronologicalYears
		{
			get
			{
				return (int)(this.AgeChronologicalTicks / 3600000L);
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001BB1 RID: 7089 RVA: 0x0001931B File Offset: 0x0001751B
		public float AgeChronologicalYearsFloat
		{
			get
			{
				return (float)this.AgeChronologicalTicks / 3600000f;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x0001932A File Offset: 0x0001752A
		public int BirthYear
		{
			get
			{
				return GenDate.Year(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001BB3 RID: 7091 RVA: 0x0001933C File Offset: 0x0001753C
		public int BirthDayOfSeasonZeroBased
		{
			get
			{
				return GenDate.DayOfSeason(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x0001934E File Offset: 0x0001754E
		public int BirthDayOfYear
		{
			get
			{
				return GenDate.DayOfYear(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001BB5 RID: 7093 RVA: 0x00019360 File Offset: 0x00017560
		public Quadrum BirthQuadrum
		{
			get
			{
				return GenDate.Quadrum(this.birthAbsTicksInt, 0f);
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x000ED438 File Offset: 0x000EB638
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

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x000ED490 File Offset: 0x000EB690
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
					text = text + "\nnextLifeStageChangeTick: " + this.nextLifeStageChangeTick;
				}
				return text;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x00019372 File Offset: 0x00017572
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

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x00019389 File Offset: 0x00017589
		public LifeStageDef CurLifeStage
		{
			get
			{
				return this.CurLifeStageRace.def;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x00019396 File Offset: 0x00017596
		public LifeStageAge CurLifeStageRace
		{
			get
			{
				return this.pawn.RaceProps.lifeStageAges[this.CurLifeStageIndex];
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001BBB RID: 7099 RVA: 0x000ED5E0 File Offset: 0x000EB7E0
		public PawnKindLifeStage CurKindLifeStage
		{
			get
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					Log.ErrorOnce("Tried to get CurKindLifeStage from humanlike pawn " + this.pawn, 8888811, false);
					return null;
				}
				return this.pawn.kindDef.lifeStages[this.CurLifeStageIndex];
			}
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x000193B3 File Offset: 0x000175B3
		public Pawn_AgeTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000193E1 File Offset: 0x000175E1
		public void ExposeData()
		{
			Scribe_Values.Look<long>(ref this.ageBiologicalTicksInt, "ageBiologicalTicks", 0L, false);
			Scribe_Values.Look<long>(ref this.birthAbsTicksInt, "birthAbsTicks", 0L, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.cachedLifeStageIndex = -1;
			}
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x00019418 File Offset: 0x00017618
		public void AgeTick()
		{
			this.ageBiologicalTicksInt += 1L;
			if ((long)Find.TickManager.TicksGame >= this.nextLifeStageChangeTick)
			{
				this.RecalculateLifeStageIndex();
			}
			if (this.ageBiologicalTicksInt % 3600000L == 0L)
			{
				this.BirthdayBiological();
			}
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000ED638 File Offset: 0x000EB838
		public void AgeTickMothballed(int interval)
		{
			long num = this.ageBiologicalTicksInt;
			this.ageBiologicalTicksInt += (long)interval;
			while ((long)Find.TickManager.TicksGame >= this.nextLifeStageChangeTick)
			{
				this.RecalculateLifeStageIndex();
			}
			int num2 = (int)(num / 3600000L);
			while ((long)num2 < this.ageBiologicalTicksInt / 3600000L)
			{
				this.BirthdayBiological();
				num2 += 3600000;
			}
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000ED6A4 File Offset: 0x000EB8A4
		private void RecalculateLifeStageIndex()
		{
			int num = -1;
			List<LifeStageAge> lifeStageAges = this.pawn.RaceProps.lifeStageAges;
			for (int i = lifeStageAges.Count - 1; i >= 0; i--)
			{
				if (lifeStageAges[i].minAge <= this.AgeBiologicalYearsFloat + 1E-06f)
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
			if (this.cachedLifeStageIndex < lifeStageAges.Count - 1)
			{
				float num2 = lifeStageAges[this.cachedLifeStageIndex + 1].minAge - this.AgeBiologicalYearsFloat;
				int num3 = (Current.ProgramState == ProgramState.Playing) ? Find.TickManager.TicksGame : 0;
				this.nextLifeStageChangeTick = (long)num3 + (long)Mathf.Ceil(num2 * 3600000f);
				return;
			}
			this.nextLifeStageChangeTick = long.MaxValue;
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000ED7A4 File Offset: 0x000EB9A4
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

		// Token: 0x06001BC2 RID: 7106 RVA: 0x00019457 File Offset: 0x00017657
		public void DebugForceBirthdayBiological()
		{
			this.BirthdayBiological();
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000ED8DC File Offset: 0x000EBADC
		private void CheckChangePawnKindName()
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

		// Token: 0x06001BC4 RID: 7108 RVA: 0x0001945F File Offset: 0x0001765F
		public void DebugMake1YearOlder()
		{
			this.ageBiologicalTicksInt += 3600000L;
			this.birthAbsTicksInt -= 3600000L;
			this.RecalculateLifeStageIndex();
		}

		// Token: 0x04001411 RID: 5137
		private Pawn pawn;

		// Token: 0x04001412 RID: 5138
		private long ageBiologicalTicksInt = -1L;

		// Token: 0x04001413 RID: 5139
		private long birthAbsTicksInt = -1L;

		// Token: 0x04001414 RID: 5140
		private int cachedLifeStageIndex = -1;

		// Token: 0x04001415 RID: 5141
		private long nextLifeStageChangeTick = -1L;

		// Token: 0x04001416 RID: 5142
		private const float BornAtLongitude = 0f;
	}
}
