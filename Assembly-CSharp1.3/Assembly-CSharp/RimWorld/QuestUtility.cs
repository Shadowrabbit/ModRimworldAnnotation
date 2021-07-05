using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000BCE RID: 3022
	public static class QuestUtility
	{
		// Token: 0x060046C1 RID: 18113 RVA: 0x00176534 File Offset: 0x00174734
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return QuestUtility.GenerateQuestAndMakeAvailable(root, slate);
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x0017655C File Offset: 0x0017475C
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, Slate vars)
		{
			Quest quest = QuestGen.Generate(root, vars);
			Find.QuestManager.Add(quest);
			return quest;
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x00176580 File Offset: 0x00174780
		public static void SendLetterQuestAvailable(Quest quest)
		{
			TaggedString label = IncidentDefOf.GiveQuest_Random.letterLabel + ": " + quest.name;
			TaggedString taggedString;
			if (quest.initiallyAccepted)
			{
				label = "LetterQuestAutomaticallyAcceptedTitle".Translate(quest.name);
				taggedString = "LetterQuestBecameActive".Translate(quest.name);
				int questTicksRemaining = QuestUtility.GetQuestTicksRemaining(quest);
				if (questTicksRemaining > 0)
				{
					taggedString += " " + "LetterQuestActiveNowTime".Translate(questTicksRemaining.ToStringTicksToPeriod(false, false, false, true));
				}
			}
			else
			{
				taggedString = "LetterQuestBecameAvailable".Translate(quest.name);
				if (quest.ticksUntilAcceptanceExpiry >= 0)
				{
					taggedString += "\n\n" + "LetterQuestRequiresAcceptance".Translate(quest.ticksUntilAcceptanceExpiry.ToStringTicksToPeriod(false, false, false, true));
				}
			}
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, taggedString, (quest.root != null && quest.root.questAvailableLetterDef != null) ? quest.root.questAvailableLetterDef : IncidentDefOf.GiveQuest_Random.letterDef, LookTargets.Invalid, null, quest, null);
			choiceLetter.title = quest.name;
			Find.LetterStack.ReceiveLetter(choiceLetter, null);
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x001766BC File Offset: 0x001748BC
		public static int GetQuestTicksRemaining(Quest quest)
		{
			foreach (QuestPart questPart in quest.PartsListForReading)
			{
				QuestPart_Delay questPart_Delay = questPart as QuestPart_Delay;
				if (questPart_Delay != null && questPart_Delay.State == QuestPartState.Enabled && questPart_Delay.isBad && !questPart_Delay.expiryInfoPart.NullOrEmpty())
				{
					return questPart_Delay.TicksLeft;
				}
			}
			return 0;
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x0017673C File Offset: 0x0017493C
		public static void GenerateBackCompatibilityNameFor(Quest quest)
		{
			quest.name = NameGenerator.GenerateName(RulePackDefOf.NamerQuestDefault, from x in Find.QuestManager.QuestsListForReading
			select x.name, false, "defaultQuestName");
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x00176790 File Offset: 0x00174990
		public static bool CanPawnAcceptQuest(Pawn p, Quest quest)
		{
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = quest.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null && !questPart_RequirementsToAccept.CanPawnAccept(p))
				{
					return false;
				}
			}
			return !p.Destroyed && p.IsFreeColonist && !p.Downed && !p.Suspended && !p.IsQuestLodger();
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x00176800 File Offset: 0x00174A00
		public static bool CanAcceptQuest(Quest quest)
		{
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = quest.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null && !questPart_RequirementsToAccept.CanAccept().Accepted)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x0017684C File Offset: 0x00174A4C
		public static Vector2 GetLocForDates()
		{
			if (Find.AnyPlayerHomeMap != null)
			{
				return Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
			}
			return default(Vector2);
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x00176880 File Offset: 0x00174A80
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, default(SignalArgs));
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x0017689D File Offset: 0x00174A9D
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1));
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x001768AC File Offset: 0x00174AAC
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2));
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x001768BC File Offset: 0x00174ABC
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3));
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x001768CE File Offset: 0x00174ACE
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3, arg4));
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x001768E2 File Offset: 0x00174AE2
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument[] args)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(args));
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x001768F4 File Offset: 0x00174AF4
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, SignalArgs args)
		{
			if (questTags == null)
			{
				return;
			}
			for (int i = 0; i < questTags.Count; i++)
			{
				Find.SignalManager.SendSignal(new Signal(questTags[i] + "." + signalPart, args));
			}
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x00176938 File Offset: 0x00174B38
		public static void AddQuestTag(ref List<string> questTags, string questTagToAdd)
		{
			if (questTags == null)
			{
				questTags = new List<string>();
			}
			if (questTags.Contains(questTagToAdd))
			{
				return;
			}
			questTags.Add(questTagToAdd);
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x00176958 File Offset: 0x00174B58
		public static void AddQuestTag(object obj, string questTagToAdd)
		{
			if (questTagToAdd.NullOrEmpty())
			{
				return;
			}
			if (obj is Thing)
			{
				QuestUtility.AddQuestTag(ref ((Thing)obj).questTags, questTagToAdd);
				return;
			}
			if (obj is WorldObject)
			{
				QuestUtility.AddQuestTag(ref ((WorldObject)obj).questTags, questTagToAdd);
				return;
			}
			if (obj is Map)
			{
				QuestUtility.AddQuestTag(ref ((Map)obj).Parent.questTags, questTagToAdd);
				return;
			}
			if (obj is Lord)
			{
				QuestUtility.AddQuestTag(ref ((Lord)obj).questTags, questTagToAdd);
				return;
			}
			if (obj is Faction)
			{
				QuestUtility.AddQuestTag(ref ((Faction)obj).questTags, questTagToAdd);
				return;
			}
			if (obj is TransportShip)
			{
				QuestUtility.AddQuestTag(ref ((TransportShip)obj).questTags, questTagToAdd);
				return;
			}
			if (obj is IEnumerable)
			{
				foreach (object obj2 in ((IEnumerable)obj))
				{
					if (obj2 is Thing)
					{
						QuestUtility.AddQuestTag(ref ((Thing)obj2).questTags, questTagToAdd);
					}
					else if (obj2 is WorldObject)
					{
						QuestUtility.AddQuestTag(ref ((WorldObject)obj2).questTags, questTagToAdd);
					}
					else if (obj2 is Map)
					{
						QuestUtility.AddQuestTag(ref ((Map)obj2).Parent.questTags, questTagToAdd);
					}
					else if (obj2 is Faction)
					{
						QuestUtility.AddQuestTag(ref ((Faction)obj2).questTags, questTagToAdd);
					}
					else if (obj is TransportShip)
					{
						QuestUtility.AddQuestTag(ref ((TransportShip)obj).questTags, questTagToAdd);
					}
				}
			}
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x00176AEC File Offset: 0x00174CEC
		public static bool AnyMatchingTags(List<string> first, List<string> second)
		{
			if (first.NullOrEmpty<string>() || second.NullOrEmpty<string>())
			{
				return false;
			}
			for (int i = 0; i < first.Count; i++)
			{
				for (int j = 0; j < second.Count; j++)
				{
					if (first[i] == second[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x00176B45 File Offset: 0x00174D45
		public static bool IsReservedByQuestOrQuestBeingGenerated(Pawn pawn)
		{
			return Find.QuestManager.IsReservedByAnyQuest(pawn) || (QuestGen.quest != null && QuestGen.quest.QuestReserves(pawn)) || QuestGen.WasGeneratedForQuestBeingGenerated(pawn);
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x00176B70 File Offset: 0x00174D70
		public static IEnumerable<QuestPart_WorkDisabled> GetWorkDisabledQuestPart(Pawn p)
		{
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			int num;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (quests[i].State == QuestState.Ongoing)
				{
					Quest quest = quests[i];
					List<QuestPart> partList = quest.PartsListForReading;
					for (int j = 0; j < partList.Count; j = num + 1)
					{
						QuestPart_WorkDisabled questPart_WorkDisabled;
						if ((questPart_WorkDisabled = (partList[j] as QuestPart_WorkDisabled)) != null && questPart_WorkDisabled.pawns.Contains(p))
						{
							yield return questPart_WorkDisabled;
						}
						num = j;
					}
					partList = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x00176B80 File Offset: 0x00174D80
		public static bool IsQuestLodger(this Pawn p)
		{
			return p.HasExtraHomeFaction(null) || p.HasExtraMiniFaction(null);
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x00176B94 File Offset: 0x00174D94
		public static bool IsQuestHelper(this Pawn p)
		{
			if (!p.IsQuestLodger())
			{
				return false;
			}
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_ExtraFaction questPart_ExtraFaction;
						if ((questPart_ExtraFaction = (partsListForReading[j] as QuestPart_ExtraFaction)) != null && questPart_ExtraFaction.affectedPawns.Contains(p) && questPart_ExtraFaction.areHelpers)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x00176C20 File Offset: 0x00174E20
		public static bool IsQuestReward(this Pawn pawn)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if ((questsListForReading[i].State == QuestState.NotYetAccepted || questsListForReading[i].State == QuestState.Ongoing) && pawn.IsQuestReward(questsListForReading[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x00176C78 File Offset: 0x00174E78
		public static bool IsQuestReward(this Pawn pawn, Quest quest)
		{
			List<QuestPart> partsListForReading = quest.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				QuestPart_Choice questPart_Choice;
				if ((questPart_Choice = (partsListForReading[i] as QuestPart_Choice)) != null)
				{
					for (int j = 0; j < questPart_Choice.choices.Count; j++)
					{
						QuestPart_Choice.Choice choice = questPart_Choice.choices[j];
						for (int k = 0; k < choice.rewards.Count; k++)
						{
							Reward_Pawn reward_Pawn;
							if ((reward_Pawn = (choice.rewards[k] as Reward_Pawn)) != null && reward_Pawn.pawn == pawn)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x00176D14 File Offset: 0x00174F14
		public static bool LodgerAllowedDecrees(this Pawn p)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_AllowDecreesForLodger questPart_AllowDecreesForLodger;
						if ((questPart_AllowDecreesForLodger = (partsListForReading[j] as QuestPart_AllowDecreesForLodger)) != null && questPart_AllowDecreesForLodger.lodger == p)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x00176D88 File Offset: 0x00174F88
		public static bool HasExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraHomeFaction(forQuest) != null;
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x00176D94 File Offset: 0x00174F94
		public static bool HasExtraMiniFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraMiniFaction(forQuest) != null;
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x00176DA0 File Offset: 0x00174FA0
		public static bool HasExtraHomeFaction(this Pawn p, Faction faction)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, null);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == ExtraFactionType.HomeFaction && QuestUtility.tmpExtraFactions[i].faction == faction)
				{
					QuestUtility.tmpExtraFactions.Clear();
					return true;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return false;
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x00176E14 File Offset: 0x00175014
		public static bool HasExtraMiniFaction(this Pawn p, Faction faction)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, null);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == ExtraFactionType.MiniFaction && QuestUtility.tmpExtraFactions[i].faction == faction)
				{
					QuestUtility.tmpExtraFactions.Clear();
					return true;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return false;
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x00176E89 File Offset: 0x00175089
		public static Faction GetExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HomeFaction, forQuest);
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x00176E93 File Offset: 0x00175093
		public static Faction GetExtraHostFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HostFaction, forQuest);
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x00176E9D File Offset: 0x0017509D
		public static Faction GetExtraMiniFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.MiniFaction, forQuest);
		}

		// Token: 0x060046E1 RID: 18145 RVA: 0x00176EA7 File Offset: 0x001750A7
		public static bool InSameExtraFaction(this Pawn p, Pawn target, ExtraFactionType type, Quest forQuest = null)
		{
			return p.GetSharedExtraFaction(target, type, forQuest) != null;
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x00176EB8 File Offset: 0x001750B8
		public static Faction GetSharedExtraFaction(this Pawn p, Pawn target, ExtraFactionType type, Quest forQuest = null)
		{
			Faction extraFaction = p.GetExtraFaction(type, forQuest);
			Faction extraFaction2 = target.GetExtraFaction(type, forQuest);
			if (extraFaction != null && extraFaction == extraFaction2)
			{
				return extraFaction;
			}
			return null;
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x00176EE4 File Offset: 0x001750E4
		public static Faction GetExtraFaction(this Pawn p, ExtraFactionType extraFactionType, Quest forQuest = null)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, forQuest);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == extraFactionType)
				{
					Faction faction = QuestUtility.tmpExtraFactions[i].faction;
					QuestUtility.tmpExtraFactions.Clear();
					return faction;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return null;
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x00176F58 File Offset: 0x00175158
		public static void GetExtraFactionsFromQuestParts(Pawn pawn, List<ExtraFaction> outExtraFactions, Quest forQuest = null)
		{
			outExtraFactions.Clear();
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing || questsListForReading[i] == forQuest)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_ExtraFaction questPart_ExtraFaction = partsListForReading[j] as QuestPart_ExtraFaction;
						if (questPart_ExtraFaction != null && questPart_ExtraFaction.affectedPawns.Contains(pawn))
						{
							outExtraFactions.Add(questPart_ExtraFaction.extraFaction);
						}
					}
				}
			}
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x00176FEC File Offset: 0x001751EC
		public static bool IsBorrowedByAnyFaction(this Pawn pawn)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction;
						if ((questPart_LendColonistsToFaction = (partsListForReading[j] as QuestPart_LendColonistsToFaction)) != null && questPart_LendColonistsToFaction.LentColonistsListForReading.Contains(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x00177068 File Offset: 0x00175268
		public static int TotalBorrowedColonistCount()
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			int num = 0;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction;
						if ((questPart_LendColonistsToFaction = (partsListForReading[j] as QuestPart_LendColonistsToFaction)) != null)
						{
							num += questPart_LendColonistsToFaction.LentColonistsListForReading.Count;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x001770E6 File Offset: 0x001752E6
		public static IEnumerable<T> GetAllQuestPartsOfType<T>(bool ongoingOnly = true) where T : class
		{
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			int num;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (!ongoingOnly || quests[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partList = quests[i].PartsListForReading;
					for (int j = 0; j < partList.Count; j = num + 1)
					{
						T t = partList[j] as T;
						if (t != null)
						{
							yield return t;
						}
						num = j;
					}
					partList = null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x001770F8 File Offset: 0x001752F8
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target)
		{
			int num;
			QuestUtility.AppendInspectStringsFromQuestParts(sb, target, out num);
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x0017710E File Offset: 0x0017530E
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target, out int count)
		{
			QuestUtility.AppendInspectStringsFromQuestParts(delegate(string str, Quest quest)
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(str);
			}, target, out count);
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x00177130 File Offset: 0x00175330
		public static void AppendInspectStringsFromQuestParts(Action<string, Quest> func, ISelectable target, out int count)
		{
			count = 0;
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					QuestState state = questsListForReading[i].State;
					QuestUtility.tmpQuestParts.Clear();
					QuestUtility.tmpQuestParts.AddRange(questsListForReading[i].PartsListForReading);
					QuestUtility.tmpQuestParts.SortBy(delegate(QuestPart x)
					{
						if (!(x is QuestPartActivable))
						{
							return 0;
						}
						return ((QuestPartActivable)x).EnableTick;
					});
					for (int j = 0; j < QuestUtility.tmpQuestParts.Count; j++)
					{
						QuestPartActivable questPartActivable = QuestUtility.tmpQuestParts[j] as QuestPartActivable;
						if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
						{
							string str = questPartActivable.ExtraInspectString(target);
							if (!str.NullOrEmpty())
							{
								func(str.Formatted(target.Named("TARGET")), questsListForReading[i]);
								count++;
							}
						}
					}
					QuestUtility.tmpQuestParts.Clear();
				}
			}
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x00177242 File Offset: 0x00175442
		public static IEnumerable<Gizmo> GetQuestRelatedGizmos(Thing thing)
		{
			if (Find.Selector.SelectedObjects.Count == 1)
			{
				QuestUtility.<>c__DisplayClass96_0 CS$<>8__locals1 = new QuestUtility.<>c__DisplayClass96_0();
				CS$<>8__locals1.linkedQuest = null;
				List<Quest> quests = Find.QuestManager.QuestsListForReading;
				int num;
				for (int i = 0; i < quests.Count; i = num + 1)
				{
					if (!quests[i].hidden && !quests[i].Historical && !quests[i].dismissed)
					{
						if (quests[i].QuestLookTargets.Contains(thing) || quests[i].QuestSelectTargets.Contains(thing))
						{
							CS$<>8__locals1.linkedQuest = quests[i];
						}
						List<QuestPart> parts = quests[i].PartsListForReading;
						for (int j = 0; j < parts.Count; j = num + 1)
						{
							QuestPartActivable questPartActivable;
							if ((questPartActivable = (parts[j] as QuestPartActivable)) != null && questPartActivable.State == QuestPartState.Enabled)
							{
								IEnumerable<Gizmo> enumerable = questPartActivable.ExtraGizmos(thing);
								if (enumerable != null)
								{
									foreach (Gizmo gizmo in enumerable)
									{
										yield return gizmo;
									}
									IEnumerator<Gizmo> enumerator = null;
								}
							}
							num = j;
						}
						parts = null;
					}
					num = i;
				}
				if (CS$<>8__locals1.linkedQuest != null)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandOpenLinkedQuest".Translate(CS$<>8__locals1.linkedQuest.name),
						defaultDesc = "CommandOpenLinkedQuestDesc".Translate(),
						icon = TexCommand.OpenLinkedQuestTex,
						action = delegate()
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
							((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(CS$<>8__locals1.linkedQuest);
						}
					};
				}
				CS$<>8__locals1 = null;
				quests = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060046EC RID: 18156 RVA: 0x00177254 File Offset: 0x00175454
		public static Gizmo GetSelectMonumentMarkerGizmo(Thing thing)
		{
			if (!thing.Spawned || !ModsConfig.RoyaltyActive)
			{
				return null;
			}
			List<Thing> list = thing.Map.listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
			for (int i = 0; i < list.Count; i++)
			{
				MonumentMarker m = (MonumentMarker)list[i];
				if (m.IsPart(thing))
				{
					return new Command_Action
					{
						defaultLabel = "CommandSelectMonumentMarker".Translate(),
						defaultDesc = "CommandSelectMonumentMarkerDesc".Translate(),
						icon = ThingDefOf.MonumentMarker.uiIcon,
						iconAngle = ThingDefOf.MonumentMarker.uiIconAngle,
						iconOffset = ThingDefOf.MonumentMarker.uiIconOffset,
						action = delegate()
						{
							CameraJumper.TrySelect(m);
						}
					};
				}
			}
			return null;
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x00177338 File Offset: 0x00175538
		public static bool AnyQuestDisablesRandomMoodCausedMentalBreaksFor(Pawn p)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_DisableRandomMoodCausedMentalBreaks questPart_DisableRandomMoodCausedMentalBreaks;
						if ((questPart_DisableRandomMoodCausedMentalBreaks = (partsListForReading[j] as QuestPart_DisableRandomMoodCausedMentalBreaks)) != null && questPart_DisableRandomMoodCausedMentalBreaks.State == QuestPartState.Enabled && questPart_DisableRandomMoodCausedMentalBreaks.pawns.Contains(p))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x001773BB File Offset: 0x001755BB
		public static IEnumerable<Quest> GetSubquests(this Quest quest, QuestState? state = null)
		{
			List<Quest> allQuests = Find.QuestManager.questsInDisplayOrder;
			int num;
			for (int i = 0; i < allQuests.Count; i = num + 1)
			{
				if (allQuests[i].parent == quest)
				{
					if (state != null)
					{
						QuestState state2 = allQuests[i].State;
						QuestState? questState = state;
						if (!(state2 == questState.GetValueOrDefault() & questState != null))
						{
							goto IL_B8;
						}
					}
					yield return allQuests[i];
				}
				IL_B8:
				num = i;
			}
			yield break;
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x001773D2 File Offset: 0x001755D2
		public static bool IsSubquestOf(this Quest quest, Quest parent)
		{
			return quest.parent != null && quest.parent == parent;
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x001773E8 File Offset: 0x001755E8
		public static bool IsGoodwillLockedByQuest(Faction a, Faction b)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_FactionGoodwillLocked questPart_FactionGoodwillLocked;
						if ((questPart_FactionGoodwillLocked = (partsListForReading[j] as QuestPart_FactionGoodwillLocked)) != null && questPart_FactionGoodwillLocked.State == QuestPartState.Enabled && questPart_FactionGoodwillLocked.AppliesTo(a, b))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04002B47 RID: 11079
		public const string QuestTargetSignalPart_MapGenerated = "MapGenerated";

		// Token: 0x04002B48 RID: 11080
		public const string QuestTargetSignalPart_MapRemoved = "MapRemoved";

		// Token: 0x04002B49 RID: 11081
		public const string QuestTargetSignalPart_Spawned = "Spawned";

		// Token: 0x04002B4A RID: 11082
		public const string QuestTargetSignalPart_Despawned = "Despawned";

		// Token: 0x04002B4B RID: 11083
		public const string QuestTargetSignalPart_Destroyed = "Destroyed";

		// Token: 0x04002B4C RID: 11084
		public const string QuestTargetSignalPart_Killed = "Killed";

		// Token: 0x04002B4D RID: 11085
		public const string QuestTargetSignalPart_ChangedFaction = "ChangedFaction";

		// Token: 0x04002B4E RID: 11086
		public const string QuestTargetSignalPart_ChangedFactionToNonPlayer = "ChangedFactionToNonPlayer";

		// Token: 0x04002B4F RID: 11087
		public const string QuestTargetSignalPart_Hacked = "Hacked";

		// Token: 0x04002B50 RID: 11088
		public const string QuestTargetSignalPart_HackingStarted = "HackingStarted";

		// Token: 0x04002B51 RID: 11089
		public const string QuestTargetSignalPart_StartedExtractingFromContainer = "StartedExtractingFromContainer";

		// Token: 0x04002B52 RID: 11090
		public const string QuestTargetSignalPart_Researched = "Researched";

		// Token: 0x04002B53 RID: 11091
		public const string QuestTargetSignalPart_LeftMap = "LeftMap";

		// Token: 0x04002B54 RID: 11092
		public const string QuestTargetSignalPart_SurgeryViolation = "SurgeryViolation";

		// Token: 0x04002B55 RID: 11093
		public const string QuestTargetSignalPart_Arrested = "Arrested";

		// Token: 0x04002B56 RID: 11094
		public const string QuestTargetSignalPart_Released = "Released";

		// Token: 0x04002B57 RID: 11095
		public const string QuestTargetSignalPart_Recruited = "Recruited";

		// Token: 0x04002B58 RID: 11096
		public const string QuestTargetSignalPart_Kidnapped = "Kidnapped";

		// Token: 0x04002B59 RID: 11097
		public const string QuestTargetSignalPart_ChangedHostFaction = "ChangedHostFaction";

		// Token: 0x04002B5A RID: 11098
		public const string QuestTargetSignalPart_NoLongerFactionLeader = "NoLongerFactionLeader";

		// Token: 0x04002B5B RID: 11099
		public const string QuestTargetSignalPart_TitleChanged = "TitleChanged";

		// Token: 0x04002B5C RID: 11100
		public const string QuestTargetSignalPart_TitleAwardedWhenUpdatingChanged = "TitleAwardedWhenUpdatingChanged";

		// Token: 0x04002B5D RID: 11101
		public const string QuestTargetSignalPart_Banished = "Banished";

		// Token: 0x04002B5E RID: 11102
		public const string QuestTargetSignalPart_Rescued = "Rescued";

		// Token: 0x04002B5F RID: 11103
		public const string QuestTargetSignalPart_RanWild = "RanWild";

		// Token: 0x04002B60 RID: 11104
		public const string QuestTargetSignalPart_Enslaved = "Enslaved";

		// Token: 0x04002B61 RID: 11105
		public const string QuestTargetSignalPart_PlayerTended = "PlayerTended";

		// Token: 0x04002B62 RID: 11106
		public const string QuestTargetSignalPart_ShuttleSentSatisfied = "SentSatisfied";

		// Token: 0x04002B63 RID: 11107
		public const string QuestTargetSignalPart_ShuttleSentUnsatisfied = "SentUnsatisfied";

		// Token: 0x04002B64 RID: 11108
		public const string QuestTargetSignalPart_ShuttleSentWithExtraColonists = "SentWithExtraColonists";

		// Token: 0x04002B65 RID: 11109
		public const string QuestTargetSignalPart_ShuttleUnloaded = "Unloaded";

		// Token: 0x04002B66 RID: 11110
		public const string QuestTargetSignalPart_AllEnemiesDefeated = "AllEnemiesDefeated";

		// Token: 0x04002B67 RID: 11111
		public const string QuestTargetSignalPart_TradeRequestFulfilled = "TradeRequestFulfilled";

		// Token: 0x04002B68 RID: 11112
		public const string QuestTargetSignalPart_PeaceTalksResolved = "Resolved";

		// Token: 0x04002B69 RID: 11113
		public const string QuestTargetSignalPart_LaunchedShip = "LaunchedShip";

		// Token: 0x04002B6A RID: 11114
		public const string QuestTargetSignalPart_ReactorDestroyed = "ReactorDestroyed";

		// Token: 0x04002B6B RID: 11115
		public const string QuestTargetSignalPart_MonumentCompleted = "MonumentCompleted";

		// Token: 0x04002B6C RID: 11116
		public const string QuestTargetSignalPart_MonumentDestroyed = "MonumentDestroyed";

		// Token: 0x04002B6D RID: 11117
		public const string QuestTargetSignalPart_MonumentCancelled = "MonumentCancelled";

		// Token: 0x04002B6E RID: 11118
		public const string QuestTargetSignalPart_AllHivesDestroyed = "AllHivesDestroyed";

		// Token: 0x04002B6F RID: 11119
		public const string QuestTargetSignalPart_ExitMentalState = "ExitMentalState";

		// Token: 0x04002B70 RID: 11120
		public const string QuestTargetSignalPart_FactionBecameHostileToPlayer = "BecameHostileToPlayer";

		// Token: 0x04002B71 RID: 11121
		public const string QuestTargetSignalPart_FactionBuiltBuilding = "BuiltBuilding";

		// Token: 0x04002B72 RID: 11122
		public const string QuestTargetSignalPart_FactionPlacedBlueprint = "PlacedBlueprint";

		// Token: 0x04002B73 RID: 11123
		public const string QuestTargetSignalPart_CeremonyExpired = "CeremonyExpired";

		// Token: 0x04002B74 RID: 11124
		public const string QuestTargetSignalPart_CeremonyFailed = "CeremonyFailed";

		// Token: 0x04002B75 RID: 11125
		public const string QuestTargetSignalPart_CeremonyDone = "CeremonyDone";

		// Token: 0x04002B76 RID: 11126
		public const string QuestTargetSignalPart_QuestEnded = "QuestEnded";

		// Token: 0x04002B77 RID: 11127
		public const string QuestTargetSignalPart_ShipDisposed = "Disposed";

		// Token: 0x04002B78 RID: 11128
		public const string QuestTargetSignalPart_ShipArrived = "Arrived";

		// Token: 0x04002B79 RID: 11129
		public const string QuestTargetSignalPart_ShipFlewAway = "FlewAway";

		// Token: 0x04002B7A RID: 11130
		public const string QuestTargetSignalPart_ShipThingAdded = "ThingAdded";

		// Token: 0x04002B7B RID: 11131
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x04002B7C RID: 11132
		private static List<QuestPart> tmpQuestParts = new List<QuestPart>();
	}
}
