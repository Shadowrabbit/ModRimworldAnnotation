using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E73 RID: 3699
	public class Pawn_RecordsTracker : IExposable
	{
		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06005621 RID: 22049 RVA: 0x001D3228 File Offset: 0x001D1428
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

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06005622 RID: 22050 RVA: 0x001D3277 File Offset: 0x001D1477
		public int LastBattleTick
		{
			get
			{
				return this.battleExitTick;
			}
		}

		// Token: 0x06005623 RID: 22051 RVA: 0x001D327F File Offset: 0x001D147F
		public Pawn_RecordsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005624 RID: 22052 RVA: 0x001D3299 File Offset: 0x001D1499
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

		// Token: 0x06005625 RID: 22053 RVA: 0x001D32CC File Offset: 0x001D14CC
		public void RecordsTickMothballed(int interval)
		{
			this.RecordsTickUpdate(interval);
		}

		// Token: 0x06005626 RID: 22054 RVA: 0x001D32D8 File Offset: 0x001D14D8
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
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x001D3344 File Offset: 0x001D1544
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
				}));
				return;
			}
			this.records[def] = Mathf.Round(this.records[def] + 1f);
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x001D33C0 File Offset: 0x001D15C0
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
			}));
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x001D3460 File Offset: 0x001D1660
		public float GetValue(RecordDef def)
		{
			float num = this.records[def];
			if (def.type == RecordType.Int || def.type == RecordType.Time)
			{
				return Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x001D3493 File Offset: 0x001D1693
		public int GetAsInt(RecordDef def)
		{
			return Mathf.RoundToInt(this.records[def]);
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x001D34A6 File Offset: 0x001D16A6
		public void EnterBattle(Battle battle)
		{
			this.battleActive = battle;
			this.battleExitTick = Find.TickManager.TicksGame + 5000;
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x001D34C8 File Offset: 0x001D16C8
		public void ExposeData()
		{
			this.battleActive = this.BattleActive;
			Scribe_Deep.Look<DefMap<RecordDef, float>>(ref this.records, "records", Array.Empty<object>());
			Scribe_References.Look<Battle>(ref this.battleActive, "battleActive", false);
			Scribe_Values.Look<int>(ref this.battleExitTick, "battleExitTick", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x040032F4 RID: 13044
		public Pawn pawn;

		// Token: 0x040032F5 RID: 13045
		private DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();

		// Token: 0x040032F6 RID: 13046
		private Battle battleActive;

		// Token: 0x040032F7 RID: 13047
		private int battleExitTick;

		// Token: 0x040032F8 RID: 13048
		private const int UpdateTimeRecordsIntervalTicks = 80;
	}
}
