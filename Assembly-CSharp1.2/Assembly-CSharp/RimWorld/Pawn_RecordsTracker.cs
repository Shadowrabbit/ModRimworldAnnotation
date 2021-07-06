using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001519 RID: 5401
	public class Pawn_RecordsTracker : IExposable
	{
		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x060074AD RID: 29869 RVA: 0x0004EC99 File Offset: 0x0004CE99
		public float StoryRelevance
		{
			get
			{
				return (float)this.storyRelevance + this.storyRelevanceBonus;
			}
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x060074AE RID: 29870 RVA: 0x00238950 File Offset: 0x00236B50
		public Battle BattleActive
		{
			get
			{
				if (this.battleExitTick < Find.TickManager.TicksGame)
				{
					return null;
				}
				if (this.battleActive == null)
				{
					return null;
				}
				while (this.battleActive.AbsorbedBy != null)
				{
					this.battleActive = this.battleActive.AbsorbedBy;
				}
				return this.battleActive;
			}
		}

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x060074AF RID: 29871 RVA: 0x0004ECA9 File Offset: 0x0004CEA9
		public int LastBattleTick
		{
			get
			{
				return this.battleExitTick;
			}
		}

		// Token: 0x060074B0 RID: 29872 RVA: 0x002389A0 File Offset: 0x00236BA0
		public Pawn_RecordsTracker(Pawn pawn)
		{
			this.pawn = pawn;
			Rand.PushState();
			Rand.Seed = pawn.thingIDNumber * 681;
			this.storyRelevanceBonus = Rand.Range(0f, 100f);
			Rand.PopState();
		}

		// Token: 0x060074B1 RID: 29873 RVA: 0x0004ECB1 File Offset: 0x0004CEB1
		public void RecordsTick()
		{
			if (this.pawn.Dead)
			{
				return;
			}
			if (this.pawn.IsHashIntervalTick(80))
			{
				this.RecordsTickUpdate(80);
				this.battleActive = this.BattleActive;
			}
		}

		// Token: 0x060074B2 RID: 29874 RVA: 0x0004ECE4 File Offset: 0x0004CEE4
		public void RecordsTickMothballed(int interval)
		{
			this.RecordsTickUpdate(interval);
		}

		// Token: 0x060074B3 RID: 29875 RVA: 0x002389F8 File Offset: 0x00236BF8
		private void RecordsTickUpdate(int interval)
		{
			List<RecordDef> allDefsListForReading = DefDatabase<RecordDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].type == RecordType.Time && allDefsListForReading[i].Worker.ShouldMeasureTimeNow(this.pawn))
				{
					DefMap<RecordDef, float> defMap = this.records;
					RecordDef def = allDefsListForReading[i];
					defMap[def] += (float)interval;
				}
			}
			this.storyRelevance *= Math.Pow(0.20000000298023224, 0.0);
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x00238A88 File Offset: 0x00236C88
		public void Increment(RecordDef def)
		{
			if (def.type != RecordType.Int)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to increment record \"",
					def.defName,
					"\" whose record type is \"",
					def.type,
					"\"."
				}), false);
				return;
			}
			this.records[def] = Mathf.Round(this.records[def] + 1f);
		}

		// Token: 0x060074B5 RID: 29877 RVA: 0x00238B04 File Offset: 0x00236D04
		public void AddTo(RecordDef def, float value)
		{
			if (def.type == RecordType.Int)
			{
				this.records[def] = Mathf.Round(this.records[def] + Mathf.Round(value));
				return;
			}
			if (def.type == RecordType.Float)
			{
				DefMap<RecordDef, float> defMap = this.records;
				defMap[def] += value;
				return;
			}
			Log.Error(string.Concat(new object[]
			{
				"Tried to add value to record \"",
				def.defName,
				"\" whose record type is \"",
				def.type,
				"\"."
			}), false);
		}

		// Token: 0x060074B6 RID: 29878 RVA: 0x00238BA4 File Offset: 0x00236DA4
		public float GetValue(RecordDef def)
		{
			float num = this.records[def];
			if (def.type == RecordType.Int || def.type == RecordType.Time)
			{
				return Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x060074B7 RID: 29879 RVA: 0x0004ECED File Offset: 0x0004CEED
		public int GetAsInt(RecordDef def)
		{
			return Mathf.RoundToInt(this.records[def]);
		}

		// Token: 0x060074B8 RID: 29880 RVA: 0x0004ED00 File Offset: 0x0004CF00
		public void AccumulateStoryEvent(StoryEventDef def)
		{
			this.storyRelevance += (double)def.importance;
		}

		// Token: 0x060074B9 RID: 29881 RVA: 0x0004ED16 File Offset: 0x0004CF16
		public void EnterBattle(Battle battle)
		{
			this.battleActive = battle;
			this.battleExitTick = Find.TickManager.TicksGame + 5000;
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x00238BD8 File Offset: 0x00236DD8
		public void ExposeData()
		{
			this.battleActive = this.BattleActive;
			Scribe_Deep.Look<DefMap<RecordDef, float>>(ref this.records, "records", Array.Empty<object>());
			Scribe_Values.Look<double>(ref this.storyRelevance, "storyRelevance", 0.0, false);
			Scribe_References.Look<Battle>(ref this.battleActive, "battleActive", false);
			Scribe_Values.Look<int>(ref this.battleExitTick, "battleExitTick", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04004CFB RID: 19707
		public Pawn pawn;

		// Token: 0x04004CFC RID: 19708
		private DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();

		// Token: 0x04004CFD RID: 19709
		private double storyRelevance;

		// Token: 0x04004CFE RID: 19710
		private Battle battleActive;

		// Token: 0x04004CFF RID: 19711
		private int battleExitTick;

		// Token: 0x04004D00 RID: 19712
		private float storyRelevanceBonus;

		// Token: 0x04004D01 RID: 19713
		private const int UpdateTimeRecordsIntervalTicks = 80;

		// Token: 0x04004D02 RID: 19714
		private const float StoryRelevanceBonusRange = 100f;

		// Token: 0x04004D03 RID: 19715
		private const float StoryRelevanceMultiplierPerYear = 0.2f;
	}
}
