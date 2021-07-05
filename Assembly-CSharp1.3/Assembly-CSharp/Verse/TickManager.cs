using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200013E RID: 318
	public sealed class TickManager : IExposable
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00028E8D File Offset: 0x0002708D
		public int TicksGame
		{
			get
			{
				return this.ticksGameInt;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00028E95 File Offset: 0x00027095
		public int TicksAbs
		{
			get
			{
				if (this.gameStartAbsTick == 0)
				{
					Log.ErrorOnce("Accessing TicksAbs but gameStartAbsTick is not set yet (you most likely want to use GenTicks.TicksAbs instead).", 1049580013);
					return this.ticksGameInt;
				}
				return this.ticksGameInt + this.gameStartAbsTick;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00028EC2 File Offset: 0x000270C2
		public int StartingYear
		{
			get
			{
				return this.startingYearInt;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x00028ECC File Offset: 0x000270CC
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
						if (Find.Maps.Count == 0 || TickManager.UltraSpeedBoost)
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

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00028F79 File Offset: 0x00027179
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

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x00028FA0 File Offset: 0x000271A0
		public bool Paused
		{
			get
			{
				return this.curTimeSpeed == TimeSpeed.Paused || Find.WindowStack.WindowsForcePause || LongEventHandler.ForcePause || Find.TilePicker.Active;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00028FC9 File Offset: 0x000271C9
		public bool NotPlaying
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x00028FDF File Offset: 0x000271DF
		// (set) Token: 0x060008CD RID: 2253 RVA: 0x00028FE7 File Offset: 0x000271E7
		public TimeSpeed CurTimeSpeed
		{
			get
			{
				return this.curTimeSpeed;
			}
			set
			{
				this.curTimeSpeed = value;
			}
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00028FF0 File Offset: 0x000271F0
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

		// Token: 0x060008CF RID: 2255 RVA: 0x00029030 File Offset: 0x00027230
		public void Pause()
		{
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.TogglePaused();
			}
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00029040 File Offset: 0x00027240
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

		// Token: 0x060008D1 RID: 2257 RVA: 0x0002913A File Offset: 0x0002733A
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksGameInt, "ticksGame", 0, false);
			Scribe_Values.Look<int>(ref this.gameStartAbsTick, "gameStartAbsTick", 0, false);
			Scribe_Values.Look<int>(ref this.startingYearInt, "startingYear", 0, false);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00029174 File Offset: 0x00027374
		public void RegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.RegisterThing(t);
			}
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00029194 File Offset: 0x00027394
		public void DeRegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.DeregisterThing(t);
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000291B4 File Offset: 0x000273B4
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

		// Token: 0x060008D5 RID: 2261 RVA: 0x00029204 File Offset: 0x00027404
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

		// Token: 0x060008D6 RID: 2262 RVA: 0x000292E8 File Offset: 0x000274E8
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
				Log.Error(ex.ToString());
			}
			try
			{
				Find.Scenario.TickScenario();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			try
			{
				Find.World.WorldTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
			try
			{
				Find.StoryWatcher.StoryWatcherTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString());
			}
			try
			{
				Find.GameEnder.GameEndTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString());
			}
			try
			{
				Find.Storyteller.StorytellerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString());
			}
			try
			{
				Find.TaleManager.TaleManagerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString());
			}
			try
			{
				Find.QuestManager.QuestManagerTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString());
			}
			try
			{
				Find.World.WorldPostTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString());
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
				Log.Error(ex10.ToString());
			}
			GameComponentUtility.GameComponentTick();
			try
			{
				Find.LetterStack.LetterStackTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString());
			}
			try
			{
				Find.Autosaver.AutosaverTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString());
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
				Log.Error(ex13.ToString());
			}
			try
			{
				Find.TransportShipManager.ShipObjectsTick();
			}
			catch (Exception ex14)
			{
				Log.Error(ex14.ToString());
			}
			UnityEngine.Debug.developerConsoleVisible = false;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000295E4 File Offset: 0x000277E4
		public void RemoveAllFromMap(Map map)
		{
			this.tickListNormal.RemoveWhere((Thing x) => x.Map == map);
			this.tickListRare.RemoveWhere((Thing x) => x.Map == map);
			this.tickListLong.RemoveWhere((Thing x) => x.Map == map);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00029643 File Offset: 0x00027843
		public void DebugSetTicksGame(int newTicksGame)
		{
			this.ticksGameInt = newTicksGame;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002964C File Offset: 0x0002784C
		public void Notify_GeneratedPotentiallyHostileMap()
		{
			this.Pause();
			this.slower.SignalForceNormalSpeedShort();
		}

		// Token: 0x04000815 RID: 2069
		[TweakValue("Gameplay", 0f, 100f)]
		private static bool UltraSpeedBoost;

		// Token: 0x04000816 RID: 2070
		private int ticksGameInt;

		// Token: 0x04000817 RID: 2071
		public int gameStartAbsTick;

		// Token: 0x04000818 RID: 2072
		private float realTimeToTickThrough;

		// Token: 0x04000819 RID: 2073
		private TimeSpeed curTimeSpeed = TimeSpeed.Normal;

		// Token: 0x0400081A RID: 2074
		public TimeSpeed prePauseTimeSpeed;

		// Token: 0x0400081B RID: 2075
		private int startingYearInt = 5500;

		// Token: 0x0400081C RID: 2076
		private Stopwatch clock = new Stopwatch();

		// Token: 0x0400081D RID: 2077
		private TickList tickListNormal = new TickList(TickerType.Normal);

		// Token: 0x0400081E RID: 2078
		private TickList tickListRare = new TickList(TickerType.Rare);

		// Token: 0x0400081F RID: 2079
		private TickList tickListLong = new TickList(TickerType.Long);

		// Token: 0x04000820 RID: 2080
		public TimeSlower slower = new TimeSlower();

		// Token: 0x04000821 RID: 2081
		private int lastAutoScreenshot;

		// Token: 0x04000822 RID: 2082
		private float WorstAllowedFPS = 22f;

		// Token: 0x04000823 RID: 2083
		private int lastNothingHappeningCheckTick = -1;

		// Token: 0x04000824 RID: 2084
		private bool nothingHappeningCached;
	}
}
