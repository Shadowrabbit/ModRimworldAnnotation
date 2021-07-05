using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AFD RID: 2813
	public class HistoryEventsManager : IExposable
	{
		// Token: 0x06004235 RID: 16949 RVA: 0x00162750 File Offset: 0x00160950
		public void RecordEvent(HistoryEvent historyEvent, bool canApplySelfTookThoughts = true)
		{
			try
			{
				IdeoUtility.Notify_HistoryEvent(historyEvent, canApplySelfTookThoughts);
			}
			catch (Exception arg)
			{
				Log.Error("Error while notifying ideos of a HistoryEvent: " + arg);
			}
			int item;
			if (!historyEvent.args.TryGetArg<int>(HistoryEventArgsNames.CustomGoodwill, out item))
			{
				item = 0;
			}
			Pawn pawn;
			if (historyEvent.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn) && pawn.IsColonist)
			{
				HistoryEventsManager.HistoryEventRecords historyEventRecords = this.colonistEvents[historyEvent.def];
				if (historyEventRecords.ticksGame == null)
				{
					historyEventRecords.ticksGame = new List<int>();
					historyEventRecords.customGoodwill = new List<int>();
					this.colonistEvents[historyEvent.def] = historyEventRecords;
				}
				historyEventRecords.ticksGame.Add(Find.TickManager.TicksGame);
				historyEventRecords.customGoodwill.Add(item);
				if (historyEventRecords.ticksGame.Count > historyEvent.def.maxRemembered)
				{
					historyEventRecords.ticksGame.RemoveRange(0, historyEventRecords.ticksGame.Count - historyEvent.def.maxRemembered);
					historyEventRecords.customGoodwill.RemoveRange(0, historyEventRecords.ticksGame.Count - historyEvent.def.maxRemembered);
				}
			}
			Faction key;
			if (historyEvent.args.TryGetArg<Faction>(HistoryEventArgsNames.AffectedFaction, out key))
			{
				DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords> defMap;
				if (!this.eventsAffectingFaction.TryGetValue(key, out defMap))
				{
					defMap = new DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords>();
					this.eventsAffectingFaction.Add(key, defMap);
				}
				HistoryEventsManager.HistoryEventRecords historyEventRecords2 = defMap[historyEvent.def];
				if (historyEventRecords2.ticksGame == null)
				{
					historyEventRecords2.ticksGame = new List<int>();
					historyEventRecords2.customGoodwill = new List<int>();
					defMap[historyEvent.def] = historyEventRecords2;
				}
				historyEventRecords2.ticksGame.Add(Find.TickManager.TicksGame);
				historyEventRecords2.customGoodwill.Add(item);
				if (historyEventRecords2.ticksGame.Count > historyEvent.def.maxRemembered)
				{
					historyEventRecords2.ticksGame.RemoveRange(0, historyEventRecords2.ticksGame.Count - historyEvent.def.maxRemembered);
					historyEventRecords2.customGoodwill.RemoveRange(0, historyEventRecords2.ticksGame.Count - historyEvent.def.maxRemembered);
				}
			}
		}

		// Token: 0x06004236 RID: 16950 RVA: 0x00162994 File Offset: 0x00160B94
		public void HistoryEventsManagerTick()
		{
			if (Find.TickManager.TicksGame % 10000 == 0)
			{
				foreach (KeyValuePair<Faction, DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords>> keyValuePair in this.eventsAffectingFaction)
				{
					this.CheckRemoveOldEvents(keyValuePair.Value);
				}
			}
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x00162A00 File Offset: 0x00160C00
		private void CheckRemoveOldEvents(DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords> ev)
		{
			HistoryEventsManager.<>c__DisplayClass14_0 CS$<>8__locals1;
			CS$<>8__locals1.ev = ev;
			HistoryEventsManager.tmpEvents.Clear();
			List<HistoryEventDef> allDefsListForReading = DefDatabase<HistoryEventDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				HistoryEventsManager.HistoryEventRecords historyEventRecords = CS$<>8__locals1.ev[allDefsListForReading[i]];
				if (historyEventRecords.ticksGame != null)
				{
					for (int j = 0; j < historyEventRecords.ticksGame.Count; j++)
					{
						float score = this.GetScore(historyEventRecords.ticksGame[j], historyEventRecords.customGoodwill[j]);
						HistoryEventsManager.tmpEvents.Add(new ValueTuple<HistoryEventDef, int, float>(allDefsListForReading[i], j, score));
					}
				}
			}
			if (HistoryEventsManager.tmpEvents.Count <= 5)
			{
				return;
			}
			if (HistoryEventsManager.tmpEvents.Count > 20)
			{
				HistoryEventsManager.tmpEvents.SortByDescending((ValueTuple<HistoryEventDef, int, float> x) => x.Item3);
				for (int k = 20; k < HistoryEventsManager.tmpEvents.Count; k++)
				{
					HistoryEventsManager.<CheckRemoveOldEvents>g__Remove|14_0(HistoryEventsManager.tmpEvents[k].Item1, HistoryEventsManager.tmpEvents[k].Item2, ref CS$<>8__locals1);
				}
				HistoryEventsManager.tmpEvents.RemoveRange(20, HistoryEventsManager.tmpEvents.Count - 20);
			}
			int num = HistoryEventsManager.tmpEvents.Count - 1;
			while (num >= 0 && HistoryEventsManager.tmpEvents.Count > 5)
			{
				if (HistoryEventsManager.tmpEvents[num].Item3 < 0.5f)
				{
					HistoryEventsManager.<CheckRemoveOldEvents>g__Remove|14_0(HistoryEventsManager.tmpEvents[num].Item1, HistoryEventsManager.tmpEvents[num].Item2, ref CS$<>8__locals1);
					HistoryEventsManager.tmpEvents.RemoveAt(num);
				}
				num--;
			}
			for (int l = 0; l < allDefsListForReading.Count; l++)
			{
				if (CS$<>8__locals1.ev[allDefsListForReading[l]].ticksGame != null)
				{
					CS$<>8__locals1.ev[allDefsListForReading[l]].ticksGame.RemoveAll((int x) => x == int.MinValue);
					CS$<>8__locals1.ev[allDefsListForReading[l]].customGoodwill.RemoveAll((int x) => x == int.MinValue);
				}
			}
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x00162C70 File Offset: 0x00160E70
		private float GetScore(int ticksOccurred, int goodwill)
		{
			float x = (Find.TickManager.TicksGame - ticksOccurred).TicksToDays();
			return HistoryEventsManager.DaysSinceEventToScoreFactor.Evaluate(x) * HistoryEventsManager.GoodwillImpactToScoreFactor.Evaluate((float)goodwill);
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x00162CA8 File Offset: 0x00160EA8
		public bool Any(HistoryEventDef def, Faction forFaction = null)
		{
			if (forFaction != null)
			{
				DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords> defMap;
				return this.eventsAffectingFaction.TryGetValue(forFaction, out defMap) && !defMap[def].ticksGame.NullOrEmpty<int>();
			}
			return !this.colonistEvents[def].ticksGame.NullOrEmpty<int>();
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x00162CF8 File Offset: 0x00160EF8
		public int GetLastTicksGame(HistoryEventDef def, Faction forFaction = null)
		{
			if (!this.Any(def, forFaction))
			{
				return -999999;
			}
			if (forFaction == null)
			{
				return this.colonistEvents[def].ticksGame[this.colonistEvents[def].ticksGame.Count - 1];
			}
			DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords> defMap;
			if (this.eventsAffectingFaction.TryGetValue(forFaction, out defMap))
			{
				return defMap[def].ticksGame[defMap[def].ticksGame.Count - 1];
			}
			return -999999;
		}

		// Token: 0x0600423B RID: 16955 RVA: 0x00162D84 File Offset: 0x00160F84
		public int GetRecentCountWithinTicks(HistoryEventDef def, int duration, Faction forFaction = null)
		{
			if (!this.Any(def, forFaction))
			{
				return 0;
			}
			if (forFaction == null)
			{
				return GenCollection.GetCountGreaterOrEqualInSortedList(this.colonistEvents[def].ticksGame, Find.TickManager.TicksGame - duration);
			}
			DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords> defMap;
			if (this.eventsAffectingFaction.TryGetValue(forFaction, out defMap))
			{
				return GenCollection.GetCountGreaterOrEqualInSortedList(defMap[def].ticksGame, Find.TickManager.TicksGame - duration);
			}
			return 0;
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x00162DF4 File Offset: 0x00160FF4
		public void GetRecent(HistoryEventDef def, int duration, List<int> outTicks, List<int> outCustomGoodwill = null, Faction forFaction = null)
		{
			outTicks.Clear();
			if (outCustomGoodwill != null)
			{
				outCustomGoodwill.Clear();
			}
			if (!this.Any(def, forFaction))
			{
				return;
			}
			if (forFaction != null)
			{
				DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords> defMap;
				if (this.eventsAffectingFaction.TryGetValue(forFaction, out defMap))
				{
					for (int i = defMap[def].ticksGame.Count - 1; i >= 0; i--)
					{
						if (defMap[def].ticksGame[i] < Find.TickManager.TicksGame - duration)
						{
							return;
						}
						outTicks.Add(defMap[def].ticksGame[i]);
						if (outCustomGoodwill != null)
						{
							outCustomGoodwill.Add(defMap[def].customGoodwill[i]);
						}
					}
					return;
				}
			}
			else
			{
				int num = this.colonistEvents[def].ticksGame.Count - 1;
				while (num >= 0 && this.colonistEvents[def].ticksGame[num] >= Find.TickManager.TicksGame - duration)
				{
					outTicks.Add(this.colonistEvents[def].ticksGame[num]);
					if (outCustomGoodwill != null)
					{
						outCustomGoodwill.Add(this.colonistEvents[def].customGoodwill[num]);
					}
					num--;
				}
			}
		}

		// Token: 0x0600423D RID: 16957 RVA: 0x00162F37 File Offset: 0x00161137
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords>>(ref this.colonistEvents, "colonistEvents", Array.Empty<object>());
			Scribe_Collections.Look<Faction, DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords>>(ref this.eventsAffectingFaction, "eventsAffectingFaction", LookMode.Reference, LookMode.Deep, ref this.tmpFactions, ref this.tmpDefMaps);
		}

		// Token: 0x06004240 RID: 16960 RVA: 0x00163051 File Offset: 0x00161251
		[CompilerGenerated]
		internal static void <CheckRemoveOldEvents>g__Remove|14_0(HistoryEventDef def, int index, ref HistoryEventsManager.<>c__DisplayClass14_0 A_2)
		{
			A_2.ev[def].ticksGame[index] = int.MinValue;
			A_2.ev[def].customGoodwill[index] = int.MinValue;
		}

		// Token: 0x0400285A RID: 10330
		private DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords> colonistEvents = new DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords>();

		// Token: 0x0400285B RID: 10331
		private Dictionary<Faction, DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords>> eventsAffectingFaction = new Dictionary<Faction, DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords>>();

		// Token: 0x0400285C RID: 10332
		private List<Faction> tmpFactions;

		// Token: 0x0400285D RID: 10333
		private List<DefMap<HistoryEventDef, HistoryEventsManager.HistoryEventRecords>> tmpDefMaps;

		// Token: 0x0400285E RID: 10334
		private const int CheckRemoveOldEventsTicksInterval = 10000;

		// Token: 0x0400285F RID: 10335
		private const int MinCountToRemoveOld = 5;

		// Token: 0x04002860 RID: 10336
		private const int MaxCountToKeep = 20;

		// Token: 0x04002861 RID: 10337
		private const float RemoveOldScoreThreshold = 0.5f;

		// Token: 0x04002862 RID: 10338
		private static readonly SimpleCurve DaysSinceEventToScoreFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 0.1f),
				true
			},
			{
				new CurvePoint(300f, 0f),
				true
			}
		};

		// Token: 0x04002863 RID: 10339
		private static readonly SimpleCurve GoodwillImpactToScoreFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.55f),
				true
			},
			{
				new CurvePoint(20f, 1f),
				true
			},
			{
				new CurvePoint(100f, 10f),
				true
			},
			{
				new CurvePoint(200f, 15f),
				true
			}
		};

		// Token: 0x04002864 RID: 10340
		private static List<ValueTuple<HistoryEventDef, int, float>> tmpEvents = new List<ValueTuple<HistoryEventDef, int, float>>();

		// Token: 0x0200204C RID: 8268
		private struct HistoryEventRecords : IExposable
		{
			// Token: 0x0600B8F4 RID: 47348 RVA: 0x003BF181 File Offset: 0x003BD381
			public void ExposeData()
			{
				Scribe_Collections.Look<int>(ref this.ticksGame, "ticksGame", LookMode.Value, Array.Empty<object>());
				Scribe_Collections.Look<int>(ref this.customGoodwill, "customGoodwill", LookMode.Value, Array.Empty<object>());
			}

			// Token: 0x04007BD0 RID: 31696
			public List<int> ticksGame;

			// Token: 0x04007BD1 RID: 31697
			public List<int> customGoodwill;
		}
	}
}
