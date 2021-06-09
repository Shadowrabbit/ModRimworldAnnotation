using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.QuestGen;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000582 RID: 1410
	public static class DebugActionsQuests
	{
		// Token: 0x060023B1 RID: 9137 RVA: 0x0001E324 File Offset: 0x0001C524
		[DebugAction("Quests", "Generate quest", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GenerateQuest()
		{
			DebugActionsQuests.GenerateQuests(1, false);
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x0001E32D File Offset: 0x0001C52D
		[DebugAction("Quests", "Generate quests x10", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GenerateQuests10()
		{
			DebugActionsQuests.GenerateQuests(10, false);
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x0001E337 File Offset: 0x0001C537
		[DebugAction("Quests", "Generate quests x30", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GenerateQuests30()
		{
			DebugActionsQuests.GenerateQuests(30, false);
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0010EBA4 File Offset: 0x0010CDA4
		[DebugAction("Quests", "Generate quests (1x for each points)", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GenerateQuestsSamples()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<QuestScriptDef> enumerator = (from x in DefDatabase<QuestScriptDef>.AllDefs
			where x.IsRootAny
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestScriptDef scriptDef = enumerator.Current;
					list.Add(new DebugMenuOption(scriptDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						foreach (float num in DebugActionsUtility.PointsOptions(false))
						{
							try
							{
								if (!scriptDef.CanRun(num))
								{
									Log.Error(string.Concat(new object[]
									{
										"Cannot generate quest ",
										scriptDef.defName,
										" for ",
										num,
										" points!"
									}), false);
								}
								else
								{
									Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(scriptDef, num);
									quest.name = num + ": " + quest.name;
								}
							}
							catch (Exception ex)
							{
								Log.Error(string.Concat(new object[]
								{
									"Exception generating quest ",
									scriptDef.defName,
									" for ",
									num,
									" points!\n\n",
									ex.Message,
									"\n-------------\n",
									ex.StackTrace
								}), false);
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(from op in list
			orderby op.label
			select op));
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x0010EC78 File Offset: 0x0010CE78
		private static void GenerateQuests(int count, bool logDescOnly)
		{
			DebugActionsQuests.<>c__DisplayClass5_0 CS$<>8__locals1 = new DebugActionsQuests.<>c__DisplayClass5_0();
			CS$<>8__locals1.count = count;
			CS$<>8__locals1.logDescOnly = logDescOnly;
			CS$<>8__locals1.generateQuest = delegate(QuestScriptDef script, Slate slate)
			{
				int num = 0;
				for (int i = 0; i < CS$<>8__locals1.count; i++)
				{
					if (script.IsRootDecree)
					{
						Pawn pawn = slate.Get<Pawn>("asker", null, false);
						if (pawn.royalty.AllTitlesForReading.NullOrEmpty<RoyalTitle>())
						{
							pawn.royalty.SetTitle(Faction.Empire, RoyalTitleDefOf.Knight, false, false, true);
							Messages.Message("Dev: Gave " + RoyalTitleDefOf.Knight.label + " title to " + pawn.LabelCap, pawn, MessageTypeDefOf.NeutralEvent, false);
						}
						Find.CurrentMap.StoryState.RecordDecreeFired(script);
					}
					if (CS$<>8__locals1.count != 1 && !script.CanRun(slate))
					{
						num++;
					}
					else if (!CS$<>8__locals1.logDescOnly)
					{
						Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(script, slate);
						if (!quest.hidden)
						{
							QuestUtility.SendLetterQuestAvailable(quest);
						}
					}
					else
					{
						Quest quest2 = QuestUtility.GenerateQuestAndMakeAvailable(script, slate);
						string text = quest2.name;
						if (slate.Exists("points", false))
						{
							text = string.Concat(new object[]
							{
								text,
								"(",
								slate.Get<float>("points", 0f, false),
								" points)"
							});
						}
						if (slate.Exists("population", false))
						{
							text = string.Concat(new object[]
							{
								text,
								"(",
								slate.Get<int>("population", 0, false),
								" population)"
							});
						}
						text += "\n--------------\n" + quest2.description + "\n--------------";
						Log.Message(text, false);
						Find.QuestManager.Remove(quest2);
					}
				}
				if (num != 0)
				{
					Messages.Message("Dev: Generated only " + (CS$<>8__locals1.count - num) + " quests.", MessageTypeDefOf.RejectInput, false);
				}
			};
			CS$<>8__locals1.selectPoints = delegate(QuestScriptDef script, Slate slate, Action next)
			{
				List<DebugMenuOption> list2 = new List<DebugMenuOption>();
				foreach (float localPoints2 in DebugActionsUtility.PointsOptions(false))
				{
					float localPoints = localPoints2;
					string text = localPoints2.ToString("F0") + " points";
					if (script != null)
					{
						if (script.IsRootDecree)
						{
							slate.Set<Pawn>("asker", PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.RandomElement<Pawn>(), false);
						}
						if (script == QuestScriptDefOf.LongRangeMineralScannerLump)
						{
							slate.Set<ThingDef>("targetMineableThing", ThingDefOf.Gold, false);
							slate.Set<ThingDef>("targetMineable", ThingDefOf.MineableGold, false);
							slate.Set<Pawn>("worker", PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>(), false);
						}
						slate.Set<float>("points", localPoints, false);
						if (!script.CanRun(slate))
						{
							text += " [not now]";
						}
					}
					list2.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
					{
						slate.Set<float>("points", localPoints, false);
						next();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
			};
			CS$<>8__locals1.selectPopulation = delegate(Slate slate, Action next)
			{
				List<DebugMenuOption> list2 = new List<DebugMenuOption>();
				list2.Add(new DebugMenuOption("*Don't set", DebugMenuOptionMode.Action, next));
				foreach (int localPopulation2 in DebugActionsUtility.PopulationOptions())
				{
					int localPopulation = localPopulation2;
					list2.Add(new DebugMenuOption(localPopulation2.ToString("F0") + " colony population", DebugMenuOptionMode.Action, delegate()
					{
						slate.Set<int>("population", localPopulation, false);
						next();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*Natural random", DebugMenuOptionMode.Action, delegate()
			{
				Slate slate = new Slate();
				CS$<>8__locals1.selectPoints(null, slate, delegate
				{
					float points = slate.Get<float>("points", 0f, false);
					QuestScriptDef script = NaturalRandomQuestChooser.ChooseNaturalRandomQuest(points, Find.CurrentMap);
					if (script.affectedByPopulation)
					{
						CS$<>8__locals1.selectPopulation(slate, delegate
						{
							CS$<>8__locals1.generateQuest(script, slate);
						});
						return;
					}
					CS$<>8__locals1.generateQuest(script, slate);
				});
			}));
			using (IEnumerator<QuestScriptDef> enumerator = (from x in DefDatabase<QuestScriptDef>.AllDefs
			where x.IsRootAny
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugActionsQuests.<>c__DisplayClass5_7 CS$<>8__locals2 = new DebugActionsQuests.<>c__DisplayClass5_7();
					CS$<>8__locals2.CS$<>8__locals5 = CS$<>8__locals1;
					CS$<>8__locals2.scriptDef = enumerator.Current;
					QuestScriptDef localScriptDef = CS$<>8__locals2.scriptDef;
					string defName = localScriptDef.defName;
					list.Add(new DebugMenuOption(defName, DebugMenuOptionMode.Action, delegate()
					{
						Slate slate = new Slate();
						if (localScriptDef.affectedByPoints && localScriptDef.affectedByPopulation)
						{
							Action <>9__14;
							CS$<>8__locals2.CS$<>8__locals5.selectPoints(localScriptDef, slate, delegate
							{
								Action<Slate, Action> selectPopulation = CS$<>8__locals2.CS$<>8__locals5.selectPopulation;
								Slate slate = slate;
								Action arg;
								if ((arg = <>9__14) == null)
								{
									arg = (<>9__14 = delegate()
									{
										CS$<>8__locals2.CS$<>8__locals5.generateQuest(localScriptDef, slate);
									});
								}
								selectPopulation(slate, arg);
							});
							return;
						}
						if (CS$<>8__locals2.scriptDef.affectedByPoints)
						{
							CS$<>8__locals2.CS$<>8__locals5.selectPoints(localScriptDef, slate, delegate
							{
								CS$<>8__locals2.CS$<>8__locals5.generateQuest(localScriptDef, slate);
							});
							return;
						}
						if (localScriptDef.affectedByPopulation)
						{
							CS$<>8__locals2.CS$<>8__locals5.selectPopulation(slate, delegate
							{
								CS$<>8__locals2.CS$<>8__locals5.generateQuest(localScriptDef, slate);
							});
							return;
						}
						CS$<>8__locals2.CS$<>8__locals5.generateQuest(localScriptDef, slate);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(from op in list
			orderby op.label
			select op));
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x0010EE08 File Offset: 0x0010D008
		[DebugAction("Quests", "QuestPart test", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestQuestPart()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localQuestPartType2 in typeof(QuestPart).AllSubclassesNonAbstract())
			{
				Type localQuestPartType = localQuestPartType2;
				list.Add(new DebugMenuOption(localQuestPartType.Name, DebugMenuOptionMode.Action, delegate()
				{
					Quest quest = Quest.MakeRaw();
					quest.name = "DEBUG QUEST (" + localQuestPartType.Name + ")";
					QuestPart questPart = (QuestPart)Activator.CreateInstance(localQuestPartType);
					quest.AddPart(questPart);
					questPart.AssignDebugData();
					quest.description = "A debug quest to test " + localQuestPartType.Name + "\n\n" + Scribe.saver.DebugOutputFor(questPart);
					Find.QuestManager.Add(quest);
					Find.LetterStack.ReceiveLetter("Dev: Quest", quest.description, LetterDefOf.PositiveEvent, LookTargets.Invalid, null, quest, null, null);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x0010EEA0 File Offset: 0x0010D0A0
		[DebugAction("Quests", "Log generated quest savedata", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		public static void QuestExample()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (QuestScriptDef localRuleDef2 in DefDatabase<QuestScriptDef>.AllDefs)
			{
				QuestScriptDef localRuleDef = localRuleDef2;
				list.Add(new DebugMenuOption(localRuleDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
					{
						float localPoints = localPoints2;
						list2.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
						{
							Slate slate = new Slate();
							slate.Set<float>("points", localPoints, false);
							Quest saveable = QuestGen.Generate(localRuleDef, slate);
							Log.Message(Scribe.saver.DebugOutputFor(saveable), false);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x0010EF2C File Offset: 0x0010D12C
		[DebugOutput]
		public static void QuestRewardsSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<QuestScriptDef>.Enumerator enumerator = DefDatabase<QuestScriptDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugActionsQuests.<>c__DisplayClass9_0 CS$<>8__locals1 = new DebugActionsQuests.<>c__DisplayClass9_0();
					CS$<>8__locals1.quest = enumerator.Current;
					if (CS$<>8__locals1.quest.IsRootAny)
					{
						QuestScriptDef localQuest = CS$<>8__locals1.quest;
						list.Add(new DebugMenuOption(CS$<>8__locals1.quest.defName, DebugMenuOptionMode.Action, delegate()
						{
							Dictionary<float, int> numQuestsRating1 = new Dictionary<float, int>();
							Dictionary<float, int> numQuestsRating2 = new Dictionary<float, int>();
							Dictionary<float, int> numQuestsRating3 = new Dictionary<float, int>();
							Dictionary<float, float> rewardRating1min = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating1max = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating1accumulated = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating2min = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating2max = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating2accumulated = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating3min = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating3max = new Dictionary<float, float>();
							Dictionary<float, float> rewardRating3accumulated = new Dictionary<float, float>();
							foreach (float num in DebugActionsQuests.QuestRewardDebugPointLevels)
							{
								if (num >= CS$<>8__locals1.quest.rootMinPoints)
								{
									numQuestsRating1.Add(num, 0);
									numQuestsRating2.Add(num, 0);
									numQuestsRating3.Add(num, 0);
									Slate slate = new Slate();
									slate.Set<float>("points", num, false);
									slate.Set<bool>("debugDontGenerateRewardThings", true, false);
									rewardRating1min.Add(num, 9999999f);
									rewardRating2min.Add(num, 9999999f);
									rewardRating3min.Add(num, 9999999f);
									rewardRating1max.Add(num, -9999999f);
									rewardRating2max.Add(num, -9999999f);
									rewardRating3max.Add(num, -9999999f);
									rewardRating1accumulated.Add(num, 0f);
									rewardRating2accumulated.Add(num, 0f);
									rewardRating3accumulated.Add(num, 0f);
									for (int j = 0; j < 20; j++)
									{
										DebugActionsQuests.lastQuestGeneratedRewardValue = 0f;
										Quest quest = QuestGen.Generate(localQuest, slate.DeepCopy());
										float num2 = DebugActionsQuests.lastQuestGeneratedRewardValue;
										if (quest.challengeRating == 1)
										{
											Dictionary<float, int> numQuestsRating = numQuestsRating1;
											float key = num;
											int num3 = numQuestsRating[key];
											numQuestsRating[key] = num3 + 1;
											rewardRating1min[num] = Mathf.Min(rewardRating1min[num], num2);
											rewardRating1max[num] = Mathf.Max(rewardRating1max[num], num2);
											Dictionary<float, float> dictionary = rewardRating1accumulated;
											key = num;
											dictionary[key] += num2;
										}
										else if (quest.challengeRating == 2)
										{
											Dictionary<float, int> numQuestsRating4 = numQuestsRating2;
											float key = num;
											int num3 = numQuestsRating4[key];
											numQuestsRating4[key] = num3 + 1;
											rewardRating2min[num] = Mathf.Min(rewardRating2min[num], num2);
											rewardRating2max[num] = Mathf.Max(rewardRating2max[num], num2);
											Dictionary<float, float> dictionary = rewardRating2accumulated;
											key = num;
											dictionary[key] += num2;
										}
										else if (quest.challengeRating == 3)
										{
											Dictionary<float, int> numQuestsRating5 = numQuestsRating3;
											float key = num;
											int num3 = numQuestsRating5[key];
											numQuestsRating5[key] = num3 + 1;
											rewardRating3min[num] = Mathf.Min(rewardRating3min[num], num2);
											rewardRating3max[num] = Mathf.Max(rewardRating3max[num], num2);
											Dictionary<float, float> dictionary = rewardRating3accumulated;
											key = num;
											dictionary[key] += num2;
										}
									}
								}
							}
							IEnumerable<float> questRewardDebugPointLevels2 = DebugActionsQuests.QuestRewardDebugPointLevels;
							TableDataGetter<float>[] array = new TableDataGetter<float>[13];
							array[0] = new TableDataGetter<float>("points", (float v) => v.ToString());
							array[1] = new TableDataGetter<float>("rating 1\nquest count\nof " + 20, (float v) => numQuestsRating1[v].ToString());
							array[2] = new TableDataGetter<float>("rating 1\nrewardValue\nmin", delegate(float v)
							{
								if (rewardRating1min[v] != 9999999f)
								{
									return ((int)rewardRating1min[v]).ToString();
								}
								return "-";
							});
							array[3] = new TableDataGetter<float>("rating 1\nrewardValue\navg", delegate(float v)
							{
								if (rewardRating1accumulated[v] > 0f)
								{
									return ((int)(rewardRating1accumulated[v] / (float)numQuestsRating1[v])).ToString();
								}
								return "-";
							});
							array[4] = new TableDataGetter<float>("rating 1\nrewardValue\nmax", delegate(float v)
							{
								if (rewardRating1max[v] != -9999999f)
								{
									return ((int)rewardRating1max[v]).ToString();
								}
								return "-";
							});
							array[5] = new TableDataGetter<float>("rating 2\nquest count\nof " + 20, (float v) => numQuestsRating2[v].ToString());
							array[6] = new TableDataGetter<float>("rating 2\nrewardValue\nmin", delegate(float v)
							{
								if (rewardRating2min[v] != 9999999f)
								{
									return ((int)rewardRating2min[v]).ToString();
								}
								return "-";
							});
							array[7] = new TableDataGetter<float>("rating 2\nrewardValue\navg", delegate(float v)
							{
								if (rewardRating2accumulated[v] > 0f)
								{
									return ((int)(rewardRating2accumulated[v] / (float)numQuestsRating2[v])).ToString();
								}
								return "-";
							});
							array[8] = new TableDataGetter<float>("rating 2\nrewardValue\nmax", delegate(float v)
							{
								if (rewardRating2max[v] != -9999999f)
								{
									return ((int)rewardRating2max[v]).ToString();
								}
								return "-";
							});
							array[9] = new TableDataGetter<float>("rating 3\nquest count\nof " + 20, (float v) => numQuestsRating3[v].ToString());
							array[10] = new TableDataGetter<float>("rating 3\nrewardValue\nmin", delegate(float v)
							{
								if (rewardRating3min[v] != 9999999f)
								{
									return ((int)rewardRating3min[v]).ToString();
								}
								return "-";
							});
							array[11] = new TableDataGetter<float>("rating 3\nrewardValue\navg", delegate(float v)
							{
								if (rewardRating3accumulated[v] > 0f)
								{
									return ((int)(rewardRating3accumulated[v] / (float)numQuestsRating3[v])).ToString();
								}
								return "-";
							});
							array[12] = new TableDataGetter<float>("rating 3\nrewardValue\nmax", delegate(float v)
							{
								if (rewardRating3max[v] != -9999999f)
								{
									return ((int)rewardRating3max[v]).ToString();
								}
								return "-";
							});
							DebugTables.MakeTablesDialog<float>(questRewardDebugPointLevels2, array);
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x0010EFF0 File Offset: 0x0010D1F0
		[DebugOutput]
		public static void QuestDefs()
		{
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(Find.World), false);
			IEnumerable<QuestScriptDef> dataSources = from x in DefDatabase<QuestScriptDef>.AllDefs
			orderby x.IsRootRandomSelected descending, x.IsRootDecree descending
			select x;
			TableDataGetter<QuestScriptDef>[] array = new TableDataGetter<QuestScriptDef>[13];
			array[0] = new TableDataGetter<QuestScriptDef>("defName", (QuestScriptDef d) => d.defName);
			array[1] = new TableDataGetter<QuestScriptDef>("points\nmin", delegate(QuestScriptDef d)
			{
				if (d.rootMinPoints <= 0f)
				{
					return "";
				}
				return d.rootMinPoints.ToString();
			});
			array[2] = new TableDataGetter<QuestScriptDef>("progress\nmin", delegate(QuestScriptDef d)
			{
				if (d.rootMinProgressScore <= 0f)
				{
					return "";
				}
				return d.rootMinProgressScore.ToString();
			});
			array[3] = new TableDataGetter<QuestScriptDef>("increases\npop", (QuestScriptDef d) => d.rootIncreasesPopulation.ToStringCheckBlank());
			array[4] = new TableDataGetter<QuestScriptDef>("root\nweight", delegate(QuestScriptDef d)
			{
				if (d.rootSelectionWeight <= 0f)
				{
					return "";
				}
				return d.rootSelectionWeight.ToString();
			});
			array[5] = new TableDataGetter<QuestScriptDef>("decree\nweight", delegate(QuestScriptDef d)
			{
				if (d.decreeSelectionWeight <= 0f)
				{
					return "";
				}
				return d.decreeSelectionWeight.ToString();
			});
			array[6] = new TableDataGetter<QuestScriptDef>("decree\ntags", (QuestScriptDef d) => d.decreeTags.ToCommaList(false));
			array[7] = new TableDataGetter<QuestScriptDef>("auto\naccept", (QuestScriptDef d) => d.autoAccept.ToStringCheckBlank());
			array[8] = new TableDataGetter<QuestScriptDef>("expiry\ndays", delegate(QuestScriptDef d)
			{
				if (d.expireDaysRange.TrueMax <= 0f)
				{
					return "";
				}
				return d.expireDaysRange.ToString();
			});
			array[9] = new TableDataGetter<QuestScriptDef>("CanRun\nnow", (QuestScriptDef d) => d.CanRun(slate).ToStringCheckBlank());
			array[10] = new TableDataGetter<QuestScriptDef>("canGiveRoyalFavor", (QuestScriptDef d) => d.canGiveRoyalFavor.ToStringCheckBlank());
			array[11] = new TableDataGetter<QuestScriptDef>("possible rewards", delegate(QuestScriptDef d)
			{
				RewardsGeneratorParams? rewardsParams = null;
				bool multiple = false;
				slate.Set<Action<QuestNode, Slate>>("testRunCallback", delegate(QuestNode node, Slate curSlate)
				{
					QuestNode_GiveRewards questNode_GiveRewards = node as QuestNode_GiveRewards;
					if (questNode_GiveRewards != null)
					{
						if (rewardsParams != null)
						{
							multiple = true;
							return;
						}
						rewardsParams = new RewardsGeneratorParams?(questNode_GiveRewards.parms.GetValue(curSlate));
					}
				}, false);
				bool flag = d.CanRun(slate);
				slate.Remove("testRunCallback", false);
				if (multiple)
				{
					return "complex";
				}
				if (rewardsParams != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (rewardsParams.Value.allowGoodwill)
					{
						stringBuilder.AppendWithComma("goodwill");
					}
					if (rewardsParams.Value.allowRoyalFavor)
					{
						stringBuilder.AppendWithComma("favor");
					}
					return stringBuilder.ToString();
				}
				if (!flag)
				{
					return "unknown";
				}
				return "";
			});
			array[12] = new TableDataGetter<QuestScriptDef>("weight histogram", delegate(QuestScriptDef d)
			{
				string text = "";
				for (float num = 0f; num < d.rootSelectionWeight; num += 0.05f)
				{
					text += "*";
					if (num > 3f)
					{
						text += "*";
						break;
					}
				}
				return text;
			});
			DebugTables.MakeTablesDialog<QuestScriptDef>(dataSources, array);
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x0010F29C File Offset: 0x0010D49C
		[DebugOutput]
		public static void QuestSelectionWeightsNow()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
			{
				float localPoints = localPoints2;
				list.Add(new DebugMenuOption(localPoints + " points", DebugMenuOptionMode.Action, delegate()
				{
					IIncidentTarget target = Find.CurrentMap;
					string label = string.Concat(new object[]
					{
						"selection weight now\ntarget: ",
						target.ToString(),
						"\npoints: ",
						localPoints.ToString("F0"),
						"\npopIntentQuest: ",
						StorytellerUtilityPopulation.PopulationIntentForQuest
					});
					IEnumerable<QuestScriptDef> dataSources = from x in DefDatabase<QuestScriptDef>.AllDefsListForReading
					where x.IsRootRandomSelected
					select x;
					TableDataGetter<QuestScriptDef>[] array = new TableDataGetter<QuestScriptDef>[5];
					array[0] = new TableDataGetter<QuestScriptDef>("defName", (QuestScriptDef x) => x.defName);
					array[1] = new TableDataGetter<QuestScriptDef>(label, (QuestScriptDef x) => NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(x, localPoints, target.StoryState).ToString("F3"));
					array[2] = new TableDataGetter<QuestScriptDef>("increases\npopulation", (QuestScriptDef x) => x.rootIncreasesPopulation.ToStringCheckBlank());
					array[3] = new TableDataGetter<QuestScriptDef>("recency\nindex", delegate(QuestScriptDef x)
					{
						if (!target.StoryState.RecentRandomQuests.Contains(x))
						{
							return "";
						}
						return target.StoryState.RecentRandomQuests.IndexOf(x).ToString();
					});
					array[4] = new TableDataGetter<QuestScriptDef>("total\nselection\nchance\nnow", (QuestScriptDef x) => NaturalRandomQuestChooser.DebugTotalNaturalRandomSelectionWeight(x, localPoints, target).ToString("F3"));
					DebugTables.MakeTablesDialog<QuestScriptDef>(dataSources, array);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(from op in list
			orderby op.label
			select op));
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0010F358 File Offset: 0x0010D558
		[DebugOutput]
		public static void DecreeSelectionWeightsNow()
		{
			IIncidentTarget target = Find.CurrentMap;
			string label = "selection weight now\ntarget: " + target.ToString();
			IEnumerable<QuestScriptDef> dataSources = from x in DefDatabase<QuestScriptDef>.AllDefsListForReading
			where x.IsRootDecree
			select x;
			TableDataGetter<QuestScriptDef>[] array = new TableDataGetter<QuestScriptDef>[2];
			array[0] = new TableDataGetter<QuestScriptDef>("defName", (QuestScriptDef x) => x.defName);
			array[1] = new TableDataGetter<QuestScriptDef>(label, (QuestScriptDef x) => NaturalRandomQuestChooser.GetNaturalDecreeSelectionWeight(x, target.StoryState).ToString("F3"));
			DebugTables.MakeTablesDialog<QuestScriptDef>(dataSources, array);
		}

		// Token: 0x0400180D RID: 6157
		public static float lastQuestGeneratedRewardValue;

		// Token: 0x0400180E RID: 6158
		private static readonly float[] QuestRewardDebugPointLevels = new float[]
		{
			35f,
			100f,
			200f,
			400f,
			800f,
			1600f,
			3200f,
			6000f
		};
	}
}
