using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E1 RID: 481
	public sealed class TickManager : IExposable
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x0000FA64 File Offset: 0x0000DC64
		public int TicksGame
		{
			get
			{
				return this.ticksGameInt;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x0000FA6C File Offset: 0x0000DC6C
		public int TicksAbs
		{
			get
			{
				if (this.gameStartAbsTick == 0)
				{
					Log.ErrorOnce("Accessing TicksAbs but gameStartAbsTick is not set yet (you most likely want to use GenTicks.TicksAbs instead).", 1049580013, false);
					return this.ticksGameInt;
				}
				return this.ticksGameInt + this.gameStartAbsTick;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0000FA9A File Offset: 0x0000DC9A
		public int StartingYear
		{
			get
			{
				return this.startingYearInt;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x000A4D70 File Offset: 0x000A2F70
		public float TickRateMultiplier
		{
			get
			{
				if (this.slower.ForcedNormalSpeed)
				{
					if (this.curTimeSpeed == TimeSpeed.Paused)
					{
						return 0f;
					}
					return 1f;
				}
				else
				{
					switch (this.curTimeSpeed)
					{
					case TimeSpeed.Paused:
						return 0f;
					case TimeSpeed.Normal:
						return 1f;
					case TimeSpeed.Fast:
						return 3f;
					case TimeSpeed.Superfast:
						if (Find.Maps.Count == 0)
						{
							return 120f;
						}
						if (this.NothingHappeningInGame())
						{
							return 12f;
						}
						return 6f;
					case TimeSpeed.Ultrafast:
						if (Find.Maps.Count == 0)
						{
							return 150f;
						}
						return 15f;
					default:
						return -1f;
					}
				}
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0000FAA2 File Offset: 0x0000DCA2
		private float CurTimePerTick
		{
			get
			{
				if (this.TickRateMultiplier == 0f)
				{
					return 0f;
				}
				return 1f / (60f * this.TickRateMultiplier);
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0000FAC9 File Offset: 0x0000DCC9
		public bool Paused
		{
			get
			{
				return this.curTimeSpeed == TimeSpeed.Paused || Find.WindowStack.WindowsForcePause || LongEventHandler.ForcePause;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0000FAE6 File Offset: 0x0000DCE6
		public bool NotPlaying
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x0000FAFC File Offset: 0x0000DCFC
		// (set) Token: 0x06000C86 RID: 3206 RVA: 0x0000FB04 File Offset: 0x0000DD04
		public TimeSpeed CurTimeSpeed
		{
			get
			{
				return this.curTimeSpeed;IncidentWorker_PawnsArrive
			}
			set
			{
				this.curTimeSpeed = value;
			}
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0000FB0D File Offset: 0x0000DD0D
		public void TogglePaused()
		{
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.prePauseTimeSpeed = this.curTimeSpeed;
				this.curTimeSpeed = TimeSpeed.Paused;
				return;
			}
			if (this.prePauseTimeSpeed != this.curTimeSpeed)
			{
				this.curTimeSpeed = this.prePauseTimeSpeed;
				return;
			}
			this.curTimeSpeed = TimeSpeed.Normal;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0000FB4D File Offset: 0x0000DD4D
		public void Pause()
		{
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.TogglePaused();
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x000A4E18 File Offset: 0x000A3018
		private bool NothingHappeningInGame()
		{
			if (this.lastNothingHappeningCheckTick != this.TicksGame)
			{
				this.nothingHappeningCached = true;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
					for (int j = 0; j < list.Count; j++)
					{
						Pawn pawn = list[j];
						if (pawn.HostFaction == null && pawn.RaceProps.Humanlike && pawn.Awake())
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
					if (!this.nothingHappeningCached)
					{
						break;
					}
				}
				if (this.nothingHappeningCached)
				{
					for (int k = 0; k < maps.Count; k++)
					{
						if (maps[k].IsPlayerHome && maps[k].dangerWatcher.DangerRating >= StoryDanger.Low)
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
				}
				this.lastNothingHappeningCheckTick = this.TicksGame;
			}
			return this.nothingHappeningCached;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0000FB5D File Offset: 0x0000DD5D
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksGameInt, "ticksGame", 0, false);
			Scribe_Values.Look<int>(ref this.gameStartAbsTick, "gameStartAbsTick", 0, false);
			Scribe_Values.Look<int>(ref this.startingYearInt, "startingYear", 0, false);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x000A4F14 File Offset: 0x000A3114
		public void RegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.RegisterThing(t);
			}
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x000A4F34 File Offset: 0x000A3134
		public void DeRegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.DeregisterThing(t);
			}
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x000A4F54 File Offset: 0x000A3154
		private TickList TickListFor(Thing t)
		{
			switch (t.def.tickerType)
			{
			case TickerType.Never:
				return null;
			case TickerType.Normal:
				return this.tickListNormal;
			case TickerType.Rare:
				return this.tickListRare;
			case TickerType.Long:
				return this.tickListLong;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000A4FA4 File Offset: 0x000A31A4
		public void TickManagerUpdate()
		{
			if (!this.Paused)
			{
				float curTimePerTick = this.CurTimePerTick;
				if (Mathf.Abs(Time.deltaTime - curTimePerTick) < curTimePerTick * 0.1f)
				{
					this.realTimeToTickThrough += curTimePerTick;
				}
				else
				{
					this.realTimeToTickThrough += Time.deltaTime;
				}
				int num = 0;
				float tickRateMultiplier = this.TickRateMultiplier;
				this.clock.Reset();
				this.clock.Start();
				while (this.realTimeToTickThrough > 0f && (float)num < tickRateMultiplier * 2f)
				{
					this.DoSingleTick();
					this.realTimeToTickThrough -= curTimePerTick;
					num++;
					if (this.Paused || (float)this.clock.ElapsedMilliseconds > 1000f / this.WorstAllowedFPS)
					{
						break;
					}
				}
				if (this.realTimeToTickThrough > 0f)
				{
					this.realTimeToTickThrough = 0f;
				}
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x000A5088 File Offset: 0x000A3288
		public void DoSingleTick()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].MapPreTick();
			}
			if (!DebugSettings.fastEcology)
			{
				this.ticksGameInt++;
			}
			else
			{
				this.ticksGameInt += 2000;
			}
			Shader.SetGlobalFloat(ShaderPropertyIDs.GameSeconds, this.TicksGame.TicksToSeconds());
			this.tickListNormal.Tick();
			this.tickListRare.Tick();
			this.tickListLong.Tick();
			try
			{
				Find.DateNotifier.DateNotifierTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			try
			{
				Find.Scenario.TickScenario();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
			try
			{
				Find.World.WorldTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString(), false);
			}
			try
			{
				Find.StoryWatcher.StoryWatcherTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString(), false);
			}
			try
			{
				Find.GameEnder.GameEndTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString(), false);
			}
			try
			{
				Find.Storyteller.StorytellerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString(), false);
			}
			try
			{
				Find.TaleManager.TaleManagerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString(), false);
			}
			try
			{
				Find.QuestManager.QuestManagerTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString(), false);
			}
			try
			{
				Find.World.WorldPostTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString(), false);
			}
			for (int j = 0; j < maps.Count; j++)
			{
				maps[j].MapPostTick();
			}
			try
			{
				Find.History.HistoryTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString(), false);
			}
			GameComponentUtility.GameComponentTick();
			try
			{
				Find.LetterStack.LetterStackTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString(), false);
			}
			try
			{
				Find.Autosaver.AutosaverTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString(), false);
			}
			if (DebugViewSettings.logHourlyScreenshot && Find.TickManager.TicksGame >= this.lastAutoScreenshot + 2500)
			{
				ScreenshotTaker.QueueSilentScreenshot();
				this.lastAutoScreenshot = Find.TickManager.TicksGame / 2500 * 2500;
			}
			try
			{
				FilthMonitor.FilthMonitorTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString(), false);
			}
			UnityEngine.Debug.developerConsoleVisible = false;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x000A536C File Offset: 0x000A356C
		public void RemoveAllFromMap(Map map)
		{
			this.tickListNormal.RemoveWhere((Thing x) => x.Map == map);
			this.tickListRare.RemoveWhere((Thing x) => x.Map == map);
			this.tickListLong.RemoveWhere((Thing x) => x.Map == map);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0000FB95 File Offset: 0x0000DD95
		public void DebugSetTicksGame(int newTicksGame)
		{
			this.ticksGameInt = newTicksGame;
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0000FB9E File Offset: 0x0000DD9E
		public void Notify_GeneratedPotentiallyHostileMap()
		{
			this.Pause();
			this.slower.SignalForceNormalSpeedShort();
		}

		// Token: 0x04000AD8 RID: 2776
		private int ticksGameInt;

		// Token: 0x04000AD9 RID: 2777
		public int gameStartAbsTick;

		// Token: 0x04000ADA RID: 2778
		private float realTimeToTickThrough;

		// Token: 0x04000ADB RID: 2779
		private TimeSpeed curTimeSpeed = TimeSpeed.Normal;

		// Token: 0x04000ADC RID: 2780
		public TimeSpeed prePauseTimeSpeed;

		// Token: 0x04000ADD RID: 2781
		private int startingYearInt = 5500;

		// Token: 0x04000ADE RID: 2782
		private Stopwatch clock = new Stopwatch();

		// Token: 0x04000ADF RID: 2783
		private TickList tickListNormal = new TickList(TickerType.Normal);

		// Token: 0x04000AE0 RID: 2784
		private TickList tickListRare = new TickList(TickerType.Rare);

		// Token: 0x04000AE1 RID: 2785
		private TickList tickListLong = new TickList(TickerType.Long);

		// Token: 0x04000AE2 RID: 2786
		public TimeSlower slower = new TimeSlower();

		// Token: 0x04000AE3 RID: 2787
		private int lastAutoScreenshot;

		// Token: 0x04000AE4 RID: 2788
		private float WorstAllowedFPS = 22f;

		// Token: 0x04000AE5 RID: 2789
		private int lastNothingHappeningCheckTick = -1;

		// Token: 0x04000AE6 RID: 2790
		private bool nothingHappeningCached;
	}
}
