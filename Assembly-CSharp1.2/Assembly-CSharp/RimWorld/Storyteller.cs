using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200120A RID: 4618
	public class Storyteller : IExposable
	{
		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x060064F2 RID: 25842 RVA: 0x001F4D30 File Offset: 0x001F2F30
		public List<IIncidentTarget> AllIncidentTargets
		{
			get
			{
				Storyteller.tmpAllIncidentTargets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Storyteller.tmpAllIncidentTargets.Add(maps[i]);
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled)
					{
						Storyteller.tmpAllIncidentTargets.Add(caravans[j]);
					}
				}
				Storyteller.tmpAllIncidentTargets.Add(Find.World);
				return Storyteller.tmpAllIncidentTargets;
			}
		}

		// Token: 0x060064F3 RID: 25843 RVA: 0x00045232 File Offset: 0x00043432
		public static void StorytellerStaticUpdate()
		{
			Storyteller.tmpAllIncidentTargets.Clear();
		}

		// Token: 0x060064F4 RID: 25844 RVA: 0x0004523E File Offset: 0x0004343E
		public Storyteller()
		{
		}

		// Token: 0x060064F5 RID: 25845 RVA: 0x00045267 File Offset: 0x00043467
		public Storyteller(StorytellerDef def, DifficultyDef difficulty) : this(def, difficulty, new Difficulty(difficulty))
		{
		}

		// Token: 0x060064F6 RID: 25846 RVA: 0x001F4DC0 File Offset: 0x001F2FC0
		public Storyteller(StorytellerDef def, DifficultyDef difficulty, Difficulty difficultyValues)
		{
			this.def = def;
			this.difficulty = difficulty;
			this.difficultyValues = difficultyValues;
			this.InitializeStorytellerComps();
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x001F4E10 File Offset: 0x001F3010
		private void InitializeStorytellerComps()
		{
			this.storytellerComps = new List<StorytellerComp>();
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				if (this.def.comps[i].Enabled)
				{
					StorytellerComp storytellerComp = (StorytellerComp)Activator.CreateInstance(this.def.comps[i].compClass);
					storytellerComp.props = this.def.comps[i];
					storytellerComp.Initialize();
					this.storytellerComps.Add(storytellerComp);
				}
			}
		}

		// Token: 0x060064F8 RID: 25848 RVA: 0x001F4EA8 File Offset: 0x001F30A8
		public void ExposeData()
		{
			Scribe_Defs.Look<StorytellerDef>(ref this.def, "def");
			Scribe_Defs.Look<DifficultyDef>(ref this.difficulty, "difficulty");
			Scribe_Deep.Look<IncidentQueue>(ref this.incidentQueue, "incidentQueue", Array.Empty<object>());
			if (this.difficulty == null)
			{
				Log.Error("Loaded storyteller without difficulty", false);
				this.difficulty = DifficultyDefOf.Rough;
			}
			if (this.difficulty.isCustom)
			{
				Scribe_Deep.Look<Difficulty>(ref this.difficultyValues, "customDifficulty", new object[]
				{
					this.difficulty
				});
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.difficultyValues = new Difficulty(this.difficulty);
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.InitializeStorytellerComps();
			}
		}

		// Token: 0x060064F9 RID: 25849 RVA: 0x001F4F60 File Offset: 0x001F3160
		public void StorytellerTick()
		{
			this.incidentQueue.IncidentQueueTick();
			if (Find.TickManager.TicksGame % 1000 == 0)
			{
				if (!DebugSettings.enableStoryteller)
				{
					return;
				}
				foreach (FiringIncident fi in this.MakeIncidentsForInterval())
				{
					this.TryFire(fi);
				}
			}
		}

		// Token: 0x060064FA RID: 25850 RVA: 0x001F4FD4 File Offset: 0x001F31D4
		public bool TryFire(FiringIncident fi)
		{
			if (fi.def.Worker.CanFireNow(fi.parms, false) && fi.def.Worker.TryExecute(fi.parms))
			{
				fi.parms.target.StoryState.Notify_IncidentFired(fi);
				return true;
			}
			return false;
		}

		// Token: 0x060064FB RID: 25851 RVA: 0x00045277 File Offset: 0x00043477
		public IEnumerable<FiringIncident> MakeIncidentsForInterval()
		{
			//所有事件目标
			List<IIncidentTarget> targets = this.AllIncidentTargets;
			int num;
			
			for (int i = 0; i < this.storytellerComps.Count; i = num + 1)
			{
				//每个组件创建一个事件
				foreach (FiringIncident firingIncident in this.MakeIncidentsForInterval(this.storytellerComps[i], targets))
				{
					yield return firingIncident;
				}
				IEnumerator<FiringIncident> enumerator = null;
				num = i;
			}
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (quests[i].State == QuestState.Ongoing)
				{
					List<QuestPart> parts = quests[i].PartsListForReading;
					for (int j = 0; j < parts.Count; j = num + 1)
					{
						IIncidentMakerQuestPart incidentMakerQuestPart = parts[j] as IIncidentMakerQuestPart;
						if (incidentMakerQuestPart != null && ((QuestPartActivable)parts[j]).State == QuestPartState.Enabled)
						{
							foreach (FiringIncident firingIncident2 in incidentMakerQuestPart.MakeIntervalIncidents())
							{
								firingIncident2.sourceQuestPart = parts[j];
								firingIncident2.parms.quest = quests[i];
								yield return firingIncident2;
							}
							IEnumerator<FiringIncident> enumerator = null;
						}
						num = j;
					}
					parts = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060064FC RID: 25852 RVA: 0x00045287 File Offset: 0x00043487
		public IEnumerable<FiringIncident> MakeIncidentsForInterval(StorytellerComp comp, List<IIncidentTarget> targets)
		{
			//需要度过N天
			if (GenDate.DaysPassedFloat <= comp.props.minDaysPassed)
			{
				yield break;
			}
			int num;
			
			for (int i = 0; i < targets.Count; i = num + 1)
			{
				IIncidentTarget incidentTarget = targets[i];
				bool flag = false;
				bool flag2 = comp.props.allowedTargetTags.NullOrEmpty<IncidentTargetTagDef>();
				foreach (IncidentTargetTagDef item in incidentTarget.IncidentTargetTags())
				{
					//禁用目标存在 并且当前当前目标在内
					if (!comp.props.disallowedTargetTags.NullOrEmpty<IncidentTargetTagDef>() && comp.props.disallowedTargetTags.Contains(item))
					{
						flag = true;
						break;
					}
					//允许目标标签列表包含当前
					if (!flag2 && comp.props.allowedTargetTags.Contains(item))
					{
						flag2 = true;
					}
				}
				if (!flag && flag2)
				{
					foreach (FiringIncident firingIncident in comp.MakeIntervalIncidents(incidentTarget))
					{
						//当前描述AI难度允许大型威胁 或者当前事件不属于大型威胁
						if (Find.Storyteller.difficultyValues.allowBigThreats || firingIncident.def.category != IncidentCategoryDefOf.ThreatBig)
						{
							yield return firingIncident;
						}
					}
					IEnumerator<FiringIncident> enumerator2 = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060064FD RID: 25853 RVA: 0x001F502C File Offset: 0x001F322C
		public void Notify_PawnEvent(Pawn pawn, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			Find.StoryWatcher.watcherAdaptation.Notify_PawnEvent(pawn, ev, dinfo);
			for (int i = 0; i < this.storytellerComps.Count; i++)
			{
				this.storytellerComps[i].Notify_PawnEvent(pawn, ev, dinfo);
			}
		}

		// Token: 0x060064FE RID: 25854 RVA: 0x0004529E File Offset: 0x0004349E
		public void Notify_DefChanged()
		{
			this.InitializeStorytellerComps();
		}

		// Token: 0x060064FF RID: 25855 RVA: 0x001F5078 File Offset: 0x001F3278
		public string DebugString()
		{
			if (Time.frameCount % 60 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("GLOBAL STORYTELLER STATS");
				stringBuilder.AppendLine("------------------------");
				stringBuilder.AppendLine("Storyteller: ".PadRight(40) + this.def.label);
				stringBuilder.AppendLine("Adaptation days: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.AdaptDays.ToString("F1"));
				stringBuilder.AppendLine("Adapt points factor: ".PadRight(40) + Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor.ToString("F2"));
				stringBuilder.AppendLine("Time points factor: ".PadRight(40) + Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate((float)GenDate.DaysPassed).ToString("F2"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Ally incident fraction (neutral or ally): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(false).ToString("F2"));
				stringBuilder.AppendLine("Ally incident fraction (ally only): ".PadRight(40) + StorytellerUtility.AllyIncidentFraction(true).ToString("F2"));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(StorytellerUtilityPopulation.DebugReadout().TrimEndNewlines());
				IIncidentTarget incidentTarget = Find.WorldSelector.SingleSelectedObject as IIncidentTarget;
				if (incidentTarget == null)
				{
					incidentTarget = Find.CurrentMap;
				}
				if (incidentTarget != null)
				{
					Map map = incidentTarget as Map;
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("STATS FOR INCIDENT TARGET: " + incidentTarget);
					stringBuilder.AppendLine("------------------------");
					stringBuilder.AppendLine("Progress score: ".PadRight(40) + StorytellerUtility.GetProgressScore(incidentTarget).ToString("F2"));
					stringBuilder.AppendLine("Base points: ".PadRight(40) + StorytellerUtility.DefaultThreatPointsNow(incidentTarget).ToString("F0"));
					stringBuilder.AppendLine("Points factor random range: ".PadRight(40) + incidentTarget.IncidentPointsRandomFactorRange);
					stringBuilder.AppendLine("Wealth: ".PadRight(40) + incidentTarget.PlayerWealthForStoryteller.ToString("F0"));
					if (map != null)
					{
						if (Find.Storyteller.difficultyValues.fixedWealthMode)
						{
							stringBuilder.AppendLine(string.Format("- Wealth calculated using fixed model curve, time factor: {0:F1}", Find.Storyteller.difficultyValues.fixedWealthTimeFactor));
							stringBuilder.AppendLine("- Map age: ".PadRight(40) + map.AgeInDays.ToString("F1"));
						}
						stringBuilder.AppendLine("- Items: ".PadRight(40) + map.wealthWatcher.WealthItems.ToString("F0"));
						stringBuilder.AppendLine("- Buildings: ".PadRight(40) + map.wealthWatcher.WealthBuildings.ToString("F0"));
						stringBuilder.AppendLine("- Floors: ".PadRight(40) + map.wealthWatcher.WealthFloorsOnly.ToString("F0"));
						stringBuilder.AppendLine("- Pawns: ".PadRight(40) + map.wealthWatcher.WealthPawns.ToString("F0"));
					}
					stringBuilder.AppendLine("Pawn count human: ".PadRight(40) + (from p in incidentTarget.PlayerPawnsForStoryteller
					where p.def.race.Humanlike
					select p).Count<Pawn>());
					stringBuilder.AppendLine("Pawn count animal: ".PadRight(40) + (from p in incidentTarget.PlayerPawnsForStoryteller
					where p.def.race.Animal
					select p).Count<Pawn>());
					if (map != null)
					{
						stringBuilder.AppendLine("StoryDanger: ".PadRight(40) + map.dangerWatcher.DangerRating);
						stringBuilder.AppendLine("FireDanger: ".PadRight(40) + map.fireWatcher.FireDanger.ToString("F2"));
						stringBuilder.AppendLine("LastThreatBigTick days ago: ".PadRight(40) + (Find.TickManager.TicksGame - map.storyState.LastThreatBigTick).ToStringTicksToDays("F1"));
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("LIST OF ALL INCIDENT TARGETS");
				stringBuilder.AppendLine("------------------------");
				for (int i = 0; i < this.AllIncidentTargets.Count; i++)
				{
					stringBuilder.AppendLine(i + ". " + this.AllIncidentTargets[i].ToString());
				}
				this.debugStringCached = stringBuilder.ToString();
			}
			return this.debugStringCached;
		}

		// Token: 0x0400432F RID: 17199
		public StorytellerDef def;

		// Token: 0x04004330 RID: 17200
		public DifficultyDef difficulty;

		// Token: 0x04004331 RID: 17201
		public Difficulty difficultyValues = new Difficulty();

		// Token: 0x04004332 RID: 17202
		public List<StorytellerComp> storytellerComps;

		// Token: 0x04004333 RID: 17203
		public IncidentQueue incidentQueue = new IncidentQueue();

		// Token: 0x04004334 RID: 17204
		public static readonly Vector2 PortraitSizeTiny = new Vector2(116f, 124f);

		// Token: 0x04004335 RID: 17205
		public static readonly Vector2 PortraitSizeLarge = new Vector2(580f, 620f);

		// Token: 0x04004336 RID: 17206
		public const int IntervalsPerDay = 60;

		// Token: 0x04004337 RID: 17207
		public const int CheckInterval = 1000;

		// Token: 0x04004338 RID: 17208
		private static List<IIncidentTarget> tmpAllIncidentTargets = new List<IIncidentTarget>();

		// Token: 0x04004339 RID: 17209
		private string debugStringCached = "Generating data...";
	}
}
