using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C34 RID: 3124
	public class Storyteller : IExposable
	{
		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x0600494F RID: 18767 RVA: 0x00184224 File Offset: 0x00182424
		[Obsolete("Use \"difficulty\" instead.")]
		public Difficulty difficultyValues
		{
			get
			{
				return this.difficulty;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06004950 RID: 18768 RVA: 0x0018422C File Offset: 0x0018242C
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

		// Token: 0x06004951 RID: 18769 RVA: 0x001842BA File Offset: 0x001824BA
		public static void StorytellerStaticUpdate()
		{
			Storyteller.tmpAllIncidentTargets.Clear();
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x001842C6 File Offset: 0x001824C6
		public Storyteller()
		{
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x001842EF File Offset: 0x001824EF
		public Storyteller(StorytellerDef def, DifficultyDef difficultyDef) : this(def, difficultyDef, new Difficulty(difficultyDef))
		{
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x00184300 File Offset: 0x00182500
		public Storyteller(StorytellerDef def, DifficultyDef difficultyDef, Difficulty difficulty)
		{
			this.def = def;
			this.difficultyDef = difficultyDef;
			this.difficulty = difficulty;
			this.InitializeStorytellerComps();
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x00184350 File Offset: 0x00182550
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

		// Token: 0x06004956 RID: 18774 RVA: 0x001843E8 File Offset: 0x001825E8
		public void ExposeData()
		{
			Scribe_Defs.Look<StorytellerDef>(ref this.def, "def");
			Scribe_Defs.Look<DifficultyDef>(ref this.difficultyDef, "difficulty");
			Scribe_Deep.Look<IncidentQueue>(ref this.incidentQueue, "incidentQueue", Array.Empty<object>());
			if (this.difficultyDef == null)
			{
				Log.Error("Loaded storyteller without difficulty");
				this.difficultyDef = DifficultyDefOf.Rough;
			}
			if (this.difficultyDef.isCustom)
			{
				Scribe_Deep.Look<Difficulty>(ref this.difficulty, "customDifficulty", new object[]
				{
					this.difficultyDef
				});
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.difficulty = new Difficulty(this.difficultyDef);
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.InitializeStorytellerComps();
			}
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x0018449C File Offset: 0x0018269C
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

		// Token: 0x06004958 RID: 18776 RVA: 0x00184510 File Offset: 0x00182710
		public bool TryFire(FiringIncident fi)
		{
			if (fi.def.Worker.CanFireNow(fi.parms) && fi.def.Worker.TryExecute(fi.parms))
			{
				fi.parms.target.StoryState.Notify_IncidentFired(fi);
				return true;
			}
			return false;
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x00184566 File Offset: 0x00182766
		public IEnumerable<FiringIncident> MakeIncidentsForInterval()
		{
			List<IIncidentTarget> targets = this.AllIncidentTargets;
			int num;
			for (int i = 0; i < this.storytellerComps.Count; i = num + 1)
			{
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
			yield break;
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x00184576 File Offset: 0x00182776
		public IEnumerable<FiringIncident> MakeIncidentsForInterval(StorytellerComp comp, List<IIncidentTarget> targets)
		{
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
					if (!comp.props.disallowedTargetTags.NullOrEmpty<IncidentTargetTagDef>() && comp.props.disallowedTargetTags.Contains(item))
					{
						flag = true;
						break;
					}
					if (!flag2 && comp.props.allowedTargetTags.Contains(item))
					{
						flag2 = true;
					}
				}
				if (!flag && flag2)
				{
					foreach (FiringIncident firingIncident in comp.MakeIntervalIncidents(incidentTarget))
					{
						if (Find.Storyteller.difficulty.allowBigThreats || firingIncident.def.category != IncidentCategoryDefOf.ThreatBig)
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

		// Token: 0x0600495B RID: 18779 RVA: 0x00184590 File Offset: 0x00182790
		public void Notify_PawnEvent(Pawn pawn, AdaptationEvent ev, DamageInfo? dinfo = null)
		{
			Find.StoryWatcher.watcherAdaptation.Notify_PawnEvent(pawn, ev, dinfo);
			for (int i = 0; i < this.storytellerComps.Count; i++)
			{
				this.storytellerComps[i].Notify_PawnEvent(pawn, ev, dinfo);
			}
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x001845D9 File Offset: 0x001827D9
		public void Notify_DefChanged()
		{
			this.InitializeStorytellerComps();
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x001845E4 File Offset: 0x001827E4
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
						if (Find.Storyteller.difficulty.fixedWealthMode)
						{
							stringBuilder.AppendLine(string.Format("- Wealth calculated using fixed model curve, time factor: {0:F1}", Find.Storyteller.difficulty.fixedWealthTimeFactor));
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

		// Token: 0x04002C9A RID: 11418
		public StorytellerDef def;

		// Token: 0x04002C9B RID: 11419
		public DifficultyDef difficultyDef;

		// Token: 0x04002C9C RID: 11420
		public Difficulty difficulty = new Difficulty();

		// Token: 0x04002C9D RID: 11421
		public List<StorytellerComp> storytellerComps;

		// Token: 0x04002C9E RID: 11422
		public IncidentQueue incidentQueue = new IncidentQueue();

		// Token: 0x04002C9F RID: 11423
		public static readonly Vector2 PortraitSizeTiny = new Vector2(116f, 124f);

		// Token: 0x04002CA0 RID: 11424
		public static readonly Vector2 PortraitSizeLarge = new Vector2(580f, 620f);

		// Token: 0x04002CA1 RID: 11425
		public const int IntervalsPerDay = 60;

		// Token: 0x04002CA2 RID: 11426
		public const int CheckInterval = 1000;

		// Token: 0x04002CA3 RID: 11427
		private static List<IIncidentTarget> tmpAllIncidentTargets = new List<IIncidentTarget>();

		// Token: 0x04002CA4 RID: 11428
		private string debugStringCached = "Generating data...";
	}
}
