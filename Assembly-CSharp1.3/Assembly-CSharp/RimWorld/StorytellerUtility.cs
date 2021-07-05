using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C59 RID: 3161
	public static class StorytellerUtility
	{
		// Token: 0x060049D1 RID: 18897 RVA: 0x00185C08 File Offset: 0x00183E08
		public static IncidentParms DefaultParmsNow(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			if (incCat == null)
			{
				Log.Warning("Trying to get default parms for null incident category.");
			}
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = target;
			if (incCat.needsParmsPoints)
			{
				incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(target);
			}
			return incidentParms;
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00185C44 File Offset: 0x00183E44
		public static float GetProgressScore(IIncidentTarget target)
		{
			int num = 0;
			foreach (Pawn pawn in target.PlayerPawnsForStoryteller)
			{
				if (!pawn.IsQuestLodger() && pawn.IsFreeColonist)
				{
					num++;
				}
			}
			return (float)num * 1f + target.PlayerWealthForStoryteller * 0.0001f;
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x00185CB8 File Offset: 0x00183EB8
		public static float DefaultThreatPointsNow(IIncidentTarget target)
		{
			float playerWealthForStoryteller = target.PlayerWealthForStoryteller;
			float num = StorytellerUtility.PointsPerWealthCurve.Evaluate(playerWealthForStoryteller);
			float num2 = 0f;
			foreach (Pawn pawn in target.PlayerPawnsForStoryteller)
			{
				if (!pawn.IsQuestLodger())
				{
					float num3 = 0f;
					if (pawn.IsFreeColonist)
					{
						num3 = StorytellerUtility.PointsPerColonistByWealthCurve.Evaluate(playerWealthForStoryteller);
					}
					else if (pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && !pawn.Downed && pawn.training.CanAssignToTrain(TrainableDefOf.Release).Accepted)
					{
						num3 = 0.08f * pawn.kindDef.combatPower;
						if (target is Caravan)
						{
							num3 *= 0.7f;
						}
					}
					if (num3 > 0f)
					{
						if (pawn.ParentHolder != null && pawn.ParentHolder is Building_CryptosleepCasket)
						{
							num3 *= 0.3f;
						}
						num3 = Mathf.Lerp(num3, num3 * pawn.health.summaryHealth.SummaryHealthPercent, 0.65f);
						num2 += num3;
					}
				}
			}
			float num4 = (num + num2) * target.IncidentPointsRandomFactorRange.RandomInRange;
			float totalThreatPointsFactor = Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor;
			float num5 = Mathf.Lerp(1f, totalThreatPointsFactor, Find.Storyteller.difficulty.adaptationEffectFactor);
			return Mathf.Clamp(num4 * num5 * Find.Storyteller.difficulty.threatScale * Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate((float)GenDate.DaysPassed), 35f, 10000f);
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x00185E84 File Offset: 0x00184084
		public static float DefaultSiteThreatPointsNow()
		{
			return SiteTuning.ThreatPointsToSiteThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.World)) * SiteTuning.SitePointRandomFactorRange.RandomInRange;
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x00185EB4 File Offset: 0x001840B4
		public static float AllyIncidentFraction(bool fullAlliesOnly)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (!allFactionsListForReading[i].Hidden && !allFactionsListForReading[i].IsPlayer && !allFactionsListForReading[i].temporary)
				{
					if (allFactionsListForReading[i].def.CanEverBeNonHostile)
					{
						num2++;
					}
					if (allFactionsListForReading[i].PlayerRelationKind == FactionRelationKind.Ally || (!fullAlliesOnly && !allFactionsListForReading[i].HostileTo(Faction.OfPlayer)))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				return -1f;
			}
			float x = (float)num / Mathf.Max((float)num2, 1f);
			return StorytellerUtility.AllyIncidentFractionFromAllyFraction.Evaluate(x);
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x00185F7C File Offset: 0x0018417C
		public static void ShowFutureIncidentsDebugLogFloatMenu(bool currentMapOnly)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("-All comps-", delegate()
			{
				StorytellerUtility.DebugLogTestFutureIncidents(currentMapOnly, null, null, 300);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			List<StorytellerComp> storytellerComps = Find.Storyteller.storytellerComps;
			for (int i = 0; i < storytellerComps.Count; i++)
			{
				StorytellerComp comp = storytellerComps[i];
				string text = comp.ToString();
				if (!text.NullOrEmpty())
				{
					list.Add(new FloatMenuOption(text, delegate()
					{
						StorytellerUtility.DebugLogTestFutureIncidents(currentMapOnly, comp, null, 300);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x0018604C File Offset: 0x0018424C
		public static void DebugLogTestFutureIncidents(bool currentMapOnly, StorytellerComp onlyThisComp = null, QuestPart onlyThisQuestPart = null, int numTestDays = 100)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<IIncidentTarget, int> incCountsForTarget;
			int[] incCountsForComp;
			List<Pair<IncidentDef, IncidentParms>> allIncidents;
			int threatBigCount;
			StorytellerUtility.DebugGetFutureIncidents(numTestDays, currentMapOnly, out incCountsForTarget, out incCountsForComp, out allIncidents, out threatBigCount, stringBuilder, onlyThisComp, null, onlyThisQuestPart);
			new StringBuilder();
			string text = "Test future incidents for " + Find.Storyteller.def;
			if (onlyThisComp != null)
			{
				text = string.Concat(new object[]
				{
					text,
					" (",
					onlyThisComp,
					")"
				});
			}
			text = string.Concat(new string[]
			{
				text,
				" (",
				Find.TickManager.TicksGame.TicksToDays().ToString("F1"),
				"d - ",
				(Find.TickManager.TicksGame + numTestDays * 60000).TicksToDays().ToString("F1"),
				"d)"
			});
			StorytellerUtility.DebugLogIncidentsInternal(allIncidents, threatBigCount, incCountsForTarget, incCountsForComp, numTestDays, stringBuilder.ToString(), text);
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x00186140 File Offset: 0x00184340
		public static void DebugLogTestFutureIncidents(ThreatsGeneratorParams parms)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Dictionary<IIncidentTarget, int> incCountsForTarget;
			int[] incCountsForComp;
			List<Pair<IncidentDef, IncidentParms>> allIncidents;
			int threatBigCount;
			StorytellerUtility.DebugGetFutureIncidents(20, true, out incCountsForTarget, out incCountsForComp, out allIncidents, out threatBigCount, stringBuilder, null, parms, null);
			new StringBuilder();
			string header = string.Concat(new object[]
			{
				"Test future incidents for ThreatsGenerator ",
				parms,
				" (",
				20,
				" days, difficulty ",
				Find.Storyteller.difficultyDef,
				")"
			});
			StorytellerUtility.DebugLogIncidentsInternal(allIncidents, threatBigCount, incCountsForTarget, incCountsForComp, 20, stringBuilder.ToString(), header);
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x001861CC File Offset: 0x001843CC
		private static void DebugLogIncidentsInternal(List<Pair<IncidentDef, IncidentParms>> allIncidents, int threatBigCount, Dictionary<IIncidentTarget, int> incCountsForTarget, int[] incCountsForComp, int numTestDays, string incidentList, string header)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(header);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Points guess:            " + StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap));
			stringBuilder.AppendLine("Incident count:          " + incCountsForTarget.Sum((KeyValuePair<IIncidentTarget, int> x) => x.Value));
			stringBuilder.AppendLine("Incident count per day:  " + ((float)incCountsForTarget.Sum((KeyValuePair<IIncidentTarget, int> x) => x.Value) / (float)numTestDays).ToString("F2"));
			stringBuilder.AppendLine("ThreatBig count:         " + threatBigCount);
			stringBuilder.AppendLine("ThreatBig count per day: " + ((float)threatBigCount / (float)numTestDays).ToString("F2"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Incident count per def:");
			using (IEnumerator<IncidentDef> enumerator = (from x in (from x in allIncidents
			select x.First).Distinct<IncidentDef>()
			orderby x.category.defName, x.defName
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IncidentDef inc = enumerator.Current;
					int num = (from i in allIncidents
					where i.First == inc
					select i).Count<Pair<IncidentDef, IncidentParms>>();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"  ",
						inc.category.defName.PadRight(20),
						" ",
						inc.defName.PadRight(35),
						" ",
						num
					}));
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Incident count per target:");
			foreach (KeyValuePair<IIncidentTarget, int> keyValuePair in from kvp in incCountsForTarget
			orderby kvp.Value
			select kvp)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"  ",
					keyValuePair.Key.ToString().PadRight(30),
					" ",
					keyValuePair.Value
				}));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Incidents per StorytellerComp:");
			for (int j = 0; j < incCountsForComp.Length; j++)
			{
				stringBuilder.AppendLine("  M" + j.ToString().PadRight(5) + incCountsForComp[j]);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Full incident record:");
			stringBuilder.Append(incidentList);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x0018653C File Offset: 0x0018473C
		public static void DebugGetFutureIncidents(int numTestDays, bool currentMapOnly, out Dictionary<IIncidentTarget, int> incCountsForTarget, out int[] incCountsForComp, out List<Pair<IncidentDef, IncidentParms>> allIncidents, out int threatBigCount, StringBuilder outputSb = null, StorytellerComp onlyThisComp = null, ThreatsGeneratorParams onlyThisThreatsGenerator = null, QuestPart onlyThisQuestPart = null)
		{
			int ticksGame = Find.TickManager.TicksGame;
			IncidentQueue incidentQueue = Find.Storyteller.incidentQueue;
			List<IIncidentTarget> allIncidentTargets = Find.Storyteller.AllIncidentTargets;
			StorytellerUtility.tmpOldStoryStates.Clear();
			for (int i = 0; i < allIncidentTargets.Count; i++)
			{
				IIncidentTarget incidentTarget = allIncidentTargets[i];
				StorytellerUtility.tmpOldStoryStates.Add(incidentTarget, incidentTarget.StoryState);
				new StoryState(incidentTarget).CopyTo(incidentTarget.StoryState);
			}
			Find.Storyteller.incidentQueue = new IncidentQueue();
			int num = numTestDays * 60;
			incCountsForComp = new int[Find.Storyteller.storytellerComps.Count];
			incCountsForTarget = new Dictionary<IIncidentTarget, int>();
			allIncidents = new List<Pair<IncidentDef, IncidentParms>>();
			threatBigCount = 0;
			Func<FiringIncident, bool> <>9__0;
			for (int j = 0; j < num; j++)
			{
				IEnumerable<FiringIncident> enumerable;
				if (onlyThisThreatsGenerator != null)
				{
					enumerable = ThreatsGenerator.MakeIntervalIncidents(onlyThisThreatsGenerator, Find.CurrentMap, ticksGame);
				}
				else if (onlyThisComp != null)
				{
					enumerable = Find.Storyteller.MakeIncidentsForInterval(onlyThisComp, Find.Storyteller.AllIncidentTargets);
				}
				else if (onlyThisQuestPart != null)
				{
					IEnumerable<FiringIncident> source = Find.Storyteller.MakeIncidentsForInterval();
					Func<FiringIncident, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((FiringIncident x) => x.sourceQuestPart == onlyThisQuestPart));
					}
					enumerable = source.Where(predicate);
				}
				else
				{
					enumerable = Find.Storyteller.MakeIncidentsForInterval();
				}
				foreach (FiringIncident firingIncident in enumerable)
				{
					if (firingIncident == null)
					{
						Log.Error("Null incident generated.");
					}
					if (!currentMapOnly || firingIncident.parms.target == Find.CurrentMap)
					{
						firingIncident.parms.target.StoryState.Notify_IncidentFired(firingIncident);
						allIncidents.Add(new Pair<IncidentDef, IncidentParms>(firingIncident.def, firingIncident.parms));
						if (!incCountsForTarget.ContainsKey(firingIncident.parms.target))
						{
							incCountsForTarget[firingIncident.parms.target] = 0;
						}
						Dictionary<IIncidentTarget, int> dictionary = incCountsForTarget;
						IIncidentTarget target = firingIncident.parms.target;
						int num2 = dictionary[target];
						dictionary[target] = num2 + 1;
						string text;
						if (firingIncident.def.category == IncidentCategoryDefOf.ThreatBig)
						{
							threatBigCount++;
							text = "T ";
						}
						else if (firingIncident.def.category == IncidentCategoryDefOf.ThreatSmall)
						{
							text = "S ";
						}
						else
						{
							text = "  ";
						}
						string text2;
						if (onlyThisThreatsGenerator != null)
						{
							text2 = "";
						}
						else
						{
							int num3 = Find.Storyteller.storytellerComps.IndexOf(firingIncident.source);
							if (num3 >= 0)
							{
								incCountsForComp[num3]++;
								text2 = "M" + num3 + " ";
							}
							else
							{
								text2 = "";
							}
						}
						text2 = text2.PadRight(4);
						if (outputSb != null)
						{
							outputSb.AppendLine(string.Concat(new object[]
							{
								text2,
								text,
								(Find.TickManager.TicksGame.TicksToDays().ToString("F1") + "d").PadRight(6),
								" ",
								firingIncident
							}));
						}
					}
				}
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1000);
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			Find.Storyteller.incidentQueue = incidentQueue;
			for (int k = 0; k < allIncidentTargets.Count; k++)
			{
				StorytellerUtility.tmpOldStoryStates[allIncidentTargets[k]].CopyTo(allIncidentTargets[k].StoryState);
			}
			StorytellerUtility.tmpOldStoryStates.Clear();
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x00186908 File Offset: 0x00184B08
		public static void DebugLogTestIncidentTargets()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Available incident targets:\n");
			foreach (IIncidentTarget incidentTarget in Find.Storyteller.AllIncidentTargets)
			{
				stringBuilder.AppendLine(incidentTarget.ToString());
				foreach (IncidentTargetTagDef arg in incidentTarget.IncidentTargetTags())
				{
					stringBuilder.AppendLine("  " + arg);
				}
				stringBuilder.AppendLine("");
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04002CD8 RID: 11480
		public const float GlobalPointsMin = 35f;

		// Token: 0x04002CD9 RID: 11481
		public const float GlobalPointsMax = 10000f;

		// Token: 0x04002CDA RID: 11482
		public const float BuildingWealthFactor = 0.5f;

		// Token: 0x04002CDB RID: 11483
		private static readonly SimpleCurve PointsPerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(14000f, 0f),
				true
			},
			{
				new CurvePoint(400000f, 2400f),
				true
			},
			{
				new CurvePoint(700000f, 3600f),
				true
			},
			{
				new CurvePoint(1000000f, 4200f),
				true
			}
		};

		// Token: 0x04002CDC RID: 11484
		public const float FixedWeathModeMaxThreatLevelInYears = 12f;

		// Token: 0x04002CDD RID: 11485
		public static readonly SimpleCurve FixedWealthModeMapWealthFromTimeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 10000f),
				true
			},
			{
				new CurvePoint(180f, 180000f),
				true
			},
			{
				new CurvePoint(720f, 1000000f),
				true
			},
			{
				new CurvePoint(1800f, 2500000f),
				true
			}
		};

		// Token: 0x04002CDE RID: 11486
		private const float PointsPerTameNonDownedCombatTrainableAnimalCombatPower = 0.08f;

		// Token: 0x04002CDF RID: 11487
		private const float PointsPerPlayerPawnFactorInContainer = 0.3f;

		// Token: 0x04002CE0 RID: 11488
		private const float PointsPerPlayerPawnHealthSummaryLerpAmount = 0.65f;

		// Token: 0x04002CE1 RID: 11489
		private static readonly SimpleCurve PointsPerColonistByWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 15f),
				true
			},
			{
				new CurvePoint(10000f, 15f),
				true
			},
			{
				new CurvePoint(400000f, 140f),
				true
			},
			{
				new CurvePoint(1000000f, 200f),
				true
			}
		};

		// Token: 0x04002CE2 RID: 11490
		public const float CaravanWealthPointsFactor = 0.7f;

		// Token: 0x04002CE3 RID: 11491
		public const float CaravanAnimalPointsFactor = 0.7f;

		// Token: 0x04002CE4 RID: 11492
		public static readonly FloatRange CaravanPointsRandomFactorRange = new FloatRange(0.7f, 0.9f);

		// Token: 0x04002CE5 RID: 11493
		private static readonly SimpleCurve AllyIncidentFractionFromAllyFraction = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(0.25f, 0.6f),
				true
			}
		};

		// Token: 0x04002CE6 RID: 11494
		public const float ProgressScorePerWealth = 0.0001f;

		// Token: 0x04002CE7 RID: 11495
		public const float ProgressScorePerFreeColonist = 1f;

		// Token: 0x04002CE8 RID: 11496
		private static Dictionary<IIncidentTarget, StoryState> tmpOldStoryStates = new Dictionary<IIncidentTarget, StoryState>();
	}
}
