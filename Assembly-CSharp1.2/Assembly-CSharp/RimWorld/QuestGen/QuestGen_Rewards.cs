using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001ECE RID: 7886
	public static class QuestGen_Rewards
	{
		// Token: 0x0600A937 RID: 43319 RVA: 0x00315C4C File Offset: 0x00313E4C
		public static QuestPart_Choice GiveRewards(this Quest quest, RewardsGeneratorParams parms, string inSignal = null, string customLetterLabel = null, string customLetterText = null, RulePack customLetterLabelRules = null, RulePack customLetterTextRules = null, bool? useDifficultyFactor = null, Action runIfChosenPawnSignalUsed = null, int? variants = null, bool addCampLootReward = false, Pawn asker = null, bool addShuttleLootReward = false, bool addPossibleFutureReward = false)
		{
			QuestPart_Choice result;
			try
			{
				Slate slate = QuestGen.slate;
				RewardsGeneratorParams rewardsGeneratorParams = parms;
				rewardsGeneratorParams.rewardValue = ((rewardsGeneratorParams.rewardValue == 0f) ? slate.Get<float>("rewardValue", 0f, false) : rewardsGeneratorParams.rewardValue);
				if (useDifficultyFactor ?? true)
				{
					rewardsGeneratorParams.rewardValue *= Find.Storyteller.difficultyValues.EffectiveQuestRewardValueFactor;
					rewardsGeneratorParams.rewardValue = Math.Max(1f, rewardsGeneratorParams.rewardValue);
				}
				if (slate.Get<bool>("debugDontGenerateRewardThings", false, false))
				{
					DebugActionsQuests.lastQuestGeneratedRewardValue += Mathf.Max(rewardsGeneratorParams.rewardValue, 250f);
					result = null;
				}
				else
				{
					rewardsGeneratorParams.minGeneratedRewardValue = 250f;
					rewardsGeneratorParams.giverFaction = (rewardsGeneratorParams.giverFaction ?? ((asker != null) ? asker.Faction : null));
					rewardsGeneratorParams.populationIntent = QuestTuning.PopIncreasingRewardWeightByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest);
					if (rewardsGeneratorParams.giverFaction == null || rewardsGeneratorParams.giverFaction.def.permanentEnemy)
					{
						rewardsGeneratorParams.allowGoodwill = false;
					}
					if (rewardsGeneratorParams.giverFaction == null || asker.royalty == null || !asker.royalty.HasAnyTitleIn(asker.Faction) || rewardsGeneratorParams.giverFaction.HostileTo(Faction.OfPlayer))
					{
						rewardsGeneratorParams.allowRoyalFavor = false;
					}
					Slate.VarRestoreInfo restoreInfo = slate.GetRestoreInfo("inSignal");
					if (inSignal.NullOrEmpty())
					{
						inSignal = slate.Get<string>("inSignal", null, false);
					}
					else
					{
						slate.Set<string>("inSignal", QuestGenUtility.HardcodedSignalWithQuestID(inSignal), false);
					}
					try
					{
						QuestPart_Choice questPart_Choice = new QuestPart_Choice();
						questPart_Choice.inSignalChoiceUsed = slate.Get<string>("inSignal", null, false);
						bool flag = false;
						int num;
						if (rewardsGeneratorParams.allowGoodwill && rewardsGeneratorParams.giverFaction != null && rewardsGeneratorParams.giverFaction.HostileTo(Faction.OfPlayer))
						{
							num = 1;
						}
						else
						{
							num = (variants ?? (QuestGen.quest.root.autoAccept ? 1 : 3));
						}
						QuestGen_Rewards.generatedRewards.Clear();
						for (int i = 0; i < num; i++)
						{
							QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
							List<Reward> list = QuestGen_Rewards.GenerateRewards(rewardsGeneratorParams, slate, i == 0, ref flag, choice, num, customLetterLabel, customLetterText, customLetterLabelRules, customLetterTextRules);
							if (list != null)
							{
								questPart_Choice.choices.Add(choice);
								QuestGen_Rewards.generatedRewards.Add(list);
							}
						}
						QuestGen_Rewards.generatedRewards.Clear();
						if (addCampLootReward)
						{
							for (int j = 0; j < questPart_Choice.choices.Count; j++)
							{
								questPart_Choice.choices[j].rewards.Add(new Reward_CampLoot());
							}
						}
						if (addShuttleLootReward)
						{
							for (int k = 0; k < questPart_Choice.choices.Count; k++)
							{
								questPart_Choice.choices[k].rewards.Add(new Reward_ShuttleLoot());
							}
						}
						if (addPossibleFutureReward)
						{
							for (int l = 0; l < questPart_Choice.choices.Count; l++)
							{
								questPart_Choice.choices[l].rewards.Add(new Reward_PossibleFutureReward());
							}
						}
						questPart_Choice.choices.SortByDescending(new Func<QuestPart_Choice.Choice, int>(QuestGen_Rewards.GetDisplayPriority));
						QuestGen.quest.AddPart(questPart_Choice);
						if (flag && runIfChosenPawnSignalUsed != null)
						{
							QuestGen_Rewards.tmpPrevQuestParts.Clear();
							QuestGen_Rewards.tmpPrevQuestParts.AddRange(QuestGen.quest.PartsListForReading);
							runIfChosenPawnSignalUsed();
							List<QuestPart> partsListForReading = QuestGen.quest.PartsListForReading;
							for (int m = 0; m < partsListForReading.Count; m++)
							{
								if (!QuestGen_Rewards.tmpPrevQuestParts.Contains(partsListForReading[m]))
								{
									for (int n = 0; n < questPart_Choice.choices.Count; n++)
									{
										bool flag2 = false;
										for (int num2 = 0; num2 < questPart_Choice.choices[n].rewards.Count; num2++)
										{
											if (questPart_Choice.choices[n].rewards[num2].MakesUseOfChosenPawnSignal)
											{
												flag2 = true;
												break;
											}
										}
										if (flag2)
										{
											questPart_Choice.choices[n].questParts.Add(partsListForReading[m]);
										}
									}
								}
							}
							QuestGen_Rewards.tmpPrevQuestParts.Clear();
						}
						result = questPart_Choice;
					}
					finally
					{
						slate.Restore(restoreInfo);
					}
				}
			}
			finally
			{
				QuestGen_Rewards.generatedRewards.Clear();
			}
			return result;
		}

		// Token: 0x0600A938 RID: 43320 RVA: 0x003160F4 File Offset: 0x003142F4
		private static List<Reward> GenerateRewards(RewardsGeneratorParams parmsResolved, Slate slate, bool addDescriptionRules, ref bool chosenPawnSignalUsed, QuestPart_Choice.Choice choice, int variants, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			List<string> list;
			List<string> list2;
			if (addDescriptionRules)
			{
				list = new List<string>();
				list2 = new List<string>();
			}
			else
			{
				list = null;
				list2 = null;
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < QuestGen_Rewards.generatedRewards.Count; i++)
			{
				for (int j = 0; j < QuestGen_Rewards.generatedRewards[i].Count; j++)
				{
					if (QuestGen_Rewards.generatedRewards[i][j] is Reward_Pawn)
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					break;
				}
			}
			if (flag2)
			{
				parmsResolved.thingRewardItemsOnly = true;
			}
			List<Reward> list3 = null;
			if (variants >= 2 && !QuestGen_Rewards.generatedRewards.Any<List<Reward>>() && parmsResolved.allowGoodwill && !parmsResolved.thingRewardRequired)
			{
				list3 = QuestGen_Rewards.TryGenerateRewards_SocialOnly(parmsResolved, variants >= 3);
				if (list3.NullOrEmpty<Reward>() && variants >= 3)
				{
					list3 = QuestGen_Rewards.TryGenerateRewards_ThingsOnly(parmsResolved);
				}
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = QuestGen_Rewards.TryGenerateNonRepeatingRewards(parmsResolved);
				}
			}
			else if (variants >= 3 && QuestGen_Rewards.generatedRewards.Count == 1 && parmsResolved.allowRoyalFavor && !parmsResolved.thingRewardRequired)
			{
				list3 = QuestGen_Rewards.TryGenerateRewards_RoyalFavorOnly(parmsResolved);
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = QuestGen_Rewards.TryGenerateRewards_ThingsOnly(parmsResolved);
				}
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = QuestGen_Rewards.TryGenerateNonRepeatingRewards(parmsResolved);
				}
			}
			else if (variants >= 2 && QuestGen_Rewards.generatedRewards.Any<List<Reward>>() && !parmsResolved.thingRewardDisallowed)
			{
				list3 = QuestGen_Rewards.TryGenerateRewards_ThingsOnly(parmsResolved);
				if (list3.NullOrEmpty<Reward>())
				{
					list3 = QuestGen_Rewards.TryGenerateNonRepeatingRewards(parmsResolved);
				}
			}
			else
			{
				list3 = QuestGen_Rewards.TryGenerateNonRepeatingRewards(parmsResolved);
			}
			if (list3.NullOrEmpty<Reward>())
			{
				return null;
			}
			Reward_Items reward_Items = (Reward_Items)list3.Find((Reward x) => x is Reward_Items);
			if (reward_Items == null)
			{
				List<Type> b = (from x in list3
				select x.GetType()).ToList<Type>();
				for (int k = 0; k < QuestGen_Rewards.generatedRewards.Count; k++)
				{
					if ((from x in QuestGen_Rewards.generatedRewards[k]
					select x.GetType()).ToList<Type>().ListsEqualIgnoreOrder(b))
					{
						return null;
					}
				}
			}
			else if (list3.Count == 1)
			{
				List<ThingDef> b2 = (from x in reward_Items.ItemsListForReading
				select x.def).ToList<ThingDef>();
				for (int l = 0; l < QuestGen_Rewards.generatedRewards.Count; l++)
				{
					Reward_Items reward_Items2 = (Reward_Items)QuestGen_Rewards.generatedRewards[l].Find((Reward x) => x is Reward_Items);
					if (reward_Items2 != null)
					{
						if ((from x in reward_Items2.ItemsListForReading
						select x.def).ToList<ThingDef>().ListsEqualIgnoreOrder(b2))
						{
							return null;
						}
					}
				}
			}
			list3.SortBy((Reward x) => x is Reward_Items);
			choice.rewards.AddRange(list3);
			for (int m = 0; m < list3.Count; m++)
			{
				if (addDescriptionRules)
				{
					list.Add(list3[m].GetDescription(parmsResolved));
					if (!(list3[m] is Reward_Items))
					{
						list2.Add(list3[m].GetDescription(parmsResolved));
					}
					else if (m == list3.Count - 1)
					{
						flag = true;
					}
				}
				foreach (QuestPart questPart in list3[m].GenerateQuestParts(m, parmsResolved, customLetterLabel, customLetterText, customLetterLabelRules, customLetterTextRules))
				{
					QuestGen.quest.AddPart(questPart);
					choice.questParts.Add(questPart);
				}
				if (!parmsResolved.chosenPawnSignal.NullOrEmpty() && list3[m].MakesUseOfChosenPawnSignal)
				{
					chosenPawnSignalUsed = true;
				}
			}
			if (addDescriptionRules)
			{
				string text = list.AsEnumerable<string>().ToList<string>().ToClauseSequence().Resolve().UncapitalizeFirst();
				if (flag)
				{
					text = text.TrimEnd(new char[]
					{
						'.'
					});
				}
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("allRewardsDescriptions", text.UncapitalizeFirst()),
					new Rule_String("allRewardsDescriptionsExceptItems", list2.Any<string>() ? list2.AsEnumerable<string>().ToList<string>().ToClauseSequence().Resolve().UncapitalizeFirst() : "")
				});
			}
			return list3;
		}

		// Token: 0x0600A939 RID: 43321 RVA: 0x0006F3BC File Offset: 0x0006D5BC
		private static List<Reward> TryGenerateRewards_SocialOnly(RewardsGeneratorParams parms, bool disallowRoyalFavor)
		{
			parms.thingRewardDisallowed = true;
			if (disallowRoyalFavor)
			{
				parms.allowRoyalFavor = false;
			}
			return QuestGen_Rewards.TryGenerateNonRepeatingRewards(parms);
		}

		// Token: 0x0600A93A RID: 43322 RVA: 0x0006F3D7 File Offset: 0x0006D5D7
		private static List<Reward> TryGenerateRewards_RoyalFavorOnly(RewardsGeneratorParams parms)
		{
			parms.allowGoodwill = false;
			parms.thingRewardDisallowed = true;
			return QuestGen_Rewards.TryGenerateNonRepeatingRewards(parms);
		}

		// Token: 0x0600A93B RID: 43323 RVA: 0x0006F3EF File Offset: 0x0006D5EF
		private static List<Reward> TryGenerateRewards_ThingsOnly(RewardsGeneratorParams parms)
		{
			if (parms.thingRewardDisallowed)
			{
				return null;
			}
			parms.allowGoodwill = false;
			parms.allowRoyalFavor = false;
			return QuestGen_Rewards.TryGenerateNonRepeatingRewards(parms);
		}

		// Token: 0x0600A93C RID: 43324 RVA: 0x003165D0 File Offset: 0x003147D0
		private static List<Reward> TryGenerateNonRepeatingRewards(RewardsGeneratorParams parms)
		{
			List<Reward> list = null;
			int i = 0;
			while (i < 10)
			{
				list = RewardsGenerator.Generate(parms);
				if (list.Any((Reward x) => x is Reward_Pawn))
				{
					return list;
				}
				Reward_Items reward_Items = (Reward_Items)list.FirstOrDefault((Reward x) => x is Reward_Items);
				if (reward_Items != null)
				{
					bool flag = false;
					for (int j = 0; j < QuestGen_Rewards.generatedRewards.Count; j++)
					{
						Reward_Items otherGeneratedItems = null;
						for (int l = 0; l < QuestGen_Rewards.generatedRewards[j].Count; l++)
						{
							otherGeneratedItems = (QuestGen_Rewards.generatedRewards[j][l] as Reward_Items);
							if (otherGeneratedItems != null)
							{
								break;
							}
						}
						if (otherGeneratedItems != null)
						{
							int k2;
							int k;
							for (k = 0; k < otherGeneratedItems.items.Count; k = k2 + 1)
							{
								if (reward_Items.items.Any((Thing x) => x.GetInnerIfMinified().def == otherGeneratedItems.items[k].GetInnerIfMinified().def))
								{
									flag = true;
									break;
								}
								k2 = k;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						i++;
						continue;
					}
				}
				return list;
			}
			return list;
		}

		// Token: 0x0600A93D RID: 43325 RVA: 0x00316748 File Offset: 0x00314948
		private static int GetDisplayPriority(QuestPart_Choice.Choice choice)
		{
			for (int i = 0; i < choice.rewards.Count; i++)
			{
				if (choice.rewards[i] is Reward_RoyalFavor)
				{
					return 1;
				}
			}
			for (int j = 0; j < choice.rewards.Count; j++)
			{
				if (choice.rewards[j] is Reward_Goodwill)
				{
					return -1;
				}
			}
			return 0;
		}

		// Token: 0x040072AC RID: 29356
		private const float MinRewardValue = 250f;

		// Token: 0x040072AD RID: 29357
		private const int DefaultVariants = 3;

		// Token: 0x040072AE RID: 29358
		private static List<List<Reward>> generatedRewards = new List<List<Reward>>();

		// Token: 0x040072AF RID: 29359
		private static List<QuestPart> tmpPrevQuestParts = new List<QuestPart>();
	}
}
