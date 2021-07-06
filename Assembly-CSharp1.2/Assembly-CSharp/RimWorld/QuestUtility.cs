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
	// Token: 0x0200115B RID: 4443
	public static class QuestUtility
	{
		// Token: 0x06006195 RID: 24981 RVA: 0x001E8680 File Offset: 0x001E6880
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return QuestUtility.GenerateQuestAndMakeAvailable(root, slate);
		}

		// Token: 0x06006196 RID: 24982 RVA: 0x001E86A8 File Offset: 0x001E68A8
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, Slate vars)
		{
			Quest quest = QuestGen.Generate(root, vars);
			Find.QuestManager.Add(quest);
			return quest;
		}

		// Token: 0x06006197 RID: 24983 RVA: 0x001E86CC File Offset: 0x001E68CC
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

		// Token: 0x06006198 RID: 24984 RVA: 0x001E8808 File Offset: 0x001E6A08
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

		// Token: 0x06006199 RID: 24985 RVA: 0x001E8888 File Offset: 0x001E6A88
		public static void GenerateBackCompatibilityNameFor(Quest quest)
		{
			quest.name = NameGenerator.GenerateName(RulePackDefOf.NamerQuestDefault, from x in Find.QuestManager.QuestsListForReading
			select x.name, false, "defaultQuestName");
		}

		// Token: 0x0600619A RID: 24986 RVA: 0x001E88DC File Offset: 0x001E6ADC
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

		// Token: 0x0600619B RID: 24987 RVA: 0x001E894C File Offset: 0x001E6B4C
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

		// Token: 0x0600619C RID: 24988 RVA: 0x001E8998 File Offset: 0x001E6B98
		public static Vector2 GetLocForDates()
		{
			if (Find.AnyPlayerHomeMap != null)
			{
				return Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
			}
			return default(Vector2);
		}

		// Token: 0x0600619D RID: 24989 RVA: 0x001E89CC File Offset: 0x001E6BCC
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, default(SignalArgs));
		}

		// Token: 0x0600619E RID: 24990 RVA: 0x00043312 File Offset: 0x00041512
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1));
		}

		// Token: 0x0600619F RID: 24991 RVA: 0x00043321 File Offset: 0x00041521
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2));
		}

		// Token: 0x060061A0 RID: 24992 RVA: 0x00043331 File Offset: 0x00041531
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3));
		}

		// Token: 0x060061A1 RID: 24993 RVA: 0x00043343 File Offset: 0x00041543
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3, arg4));
		}

		// Token: 0x060061A2 RID: 24994 RVA: 0x00043357 File Offset: 0x00041557
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument[] args)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(args));
		}

		// Token: 0x060061A3 RID: 24995 RVA: 0x001E89EC File Offset: 0x001E6BEC
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

		// Token: 0x060061A4 RID: 24996 RVA: 0x00043366 File Offset: 0x00041566
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

		// Token: 0x060061A5 RID: 24997 RVA: 0x001E8A30 File Offset: 0x001E6C30
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
				}
			}
		}

		// Token: 0x060061A6 RID: 24998 RVA: 0x001E8B88 File Offset: 0x001E6D88
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

		// Token: 0x060061A7 RID: 24999 RVA: 0x00043386 File Offset: 0x00041586
		public static bool IsReservedByQuestOrQuestBeingGenerated(Pawn pawn)
		{
			return Find.QuestManager.IsReservedByAnyQuest(pawn) || (QuestGen.quest != null && QuestGen.quest.QuestReserves(pawn)) || QuestGen.WasGeneratedForQuestBeingGenerated(pawn);
		}

		// Token: 0x060061A8 RID: 25000 RVA: 0x000433B1 File Offset: 0x000415B1
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

		// Token: 0x060061A9 RID: 25001 RVA: 0x000433C1 File Offset: 0x000415C1
		public static bool IsQuestLodger(this Pawn p)
		{
			return p.HasExtraHomeFaction(null) || p.HasExtraMiniFaction(null);
		}

		// Token: 0x060061AA RID: 25002 RVA: 0x001E8BE4 File Offset: 0x001E6DE4
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

		// Token: 0x060061AB RID: 25003 RVA: 0x001E8C70 File Offset: 0x001E6E70
		public static bool IsQuestReward(this Pawn pawn)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.NotYetAccepted || questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_Choice questPart_Choice;
						if ((questPart_Choice = (partsListForReading[j] as QuestPart_Choice)) != null)
						{
							for (int k = 0; k < questPart_Choice.choices.Count; k++)
							{
								QuestPart_Choice.Choice choice = questPart_Choice.choices[k];
								for (int l = 0; l < choice.rewards.Count; l++)
								{
									Reward_Pawn reward_Pawn;
									if ((reward_Pawn = (choice.rewards[l] as Reward_Pawn)) != null && reward_Pawn.pawn == pawn)
									{
										return true;
									}
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060061AC RID: 25004 RVA: 0x001E8D5C File Offset: 0x001E6F5C
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

		// Token: 0x060061AD RID: 25005 RVA: 0x000433D5 File Offset: 0x000415D5
		public static bool HasExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraHomeFaction(forQuest) != null;
		}

		// Token: 0x060061AE RID: 25006 RVA: 0x000433E1 File Offset: 0x000415E1
		public static bool HasExtraMiniFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraMiniFaction(forQuest) != null;
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x001E8DD0 File Offset: 0x001E6FD0
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

		// Token: 0x060061B0 RID: 25008 RVA: 0x001E8E44 File Offset: 0x001E7044
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

		// Token: 0x060061B1 RID: 25009 RVA: 0x000433ED File Offset: 0x000415ED
		public static Faction GetExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HomeFaction, forQuest);
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x000433F7 File Offset: 0x000415F7
		public static Faction GetExtraHostFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HostFaction, forQuest);
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x00043401 File Offset: 0x00041601
		public static Faction GetExtraMiniFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.MiniFaction, forQuest);
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x0004340B File Offset: 0x0004160B
		public static bool InSameExtraFaction(this Pawn p, Pawn target, ExtraFactionType type, Quest forQuest = null)
		{
			return p.GetSharedExtraFaction(target, type, forQuest) != null;
		}

		// Token: 0x060061B5 RID: 25013 RVA: 0x001E8EBC File Offset: 0x001E70BC
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

		// Token: 0x060061B6 RID: 25014 RVA: 0x001E8EE8 File Offset: 0x001E70E8
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

		// Token: 0x060061B7 RID: 25015 RVA: 0x001E8F5C File Offset: 0x001E715C
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

		// Token: 0x060061B8 RID: 25016 RVA: 0x001E8FF0 File Offset: 0x001E71F0
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

		// Token: 0x060061B9 RID: 25017 RVA: 0x001E906C File Offset: 0x001E726C
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

		// Token: 0x060061BA RID: 25018 RVA: 0x00043419 File Offset: 0x00041619
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

		// Token: 0x060061BB RID: 25019 RVA: 0x001E90EC File Offset: 0x001E72EC
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target)
		{
			int num;
			QuestUtility.AppendInspectStringsFromQuestParts(sb, target, out num);
		}

		// Token: 0x060061BC RID: 25020 RVA: 0x00043429 File Offset: 0x00041629
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

		// Token: 0x060061BD RID: 25021 RVA: 0x001E9104 File Offset: 0x001E7304
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

		// Token: 0x060061BE RID: 25022 RVA: 0x00043449 File Offset: 0x00041649
		public static IEnumerable<Gizmo> GetQuestRelatedGizmos(Thing thing)
		{
			if (Find.Selector.SelectedObjects.Count == 1)
			{
				Quest linkedQuest = Find.QuestManager.QuestsListForReading.FirstOrDefault((Quest q) => !q.Historical && !q.dismissed && (q.QuestLookTargets.Contains(thing) || q.QuestSelectTargets.Contains(thing)));
				if (linkedQuest != null && !linkedQuest.hidden)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandOpenLinkedQuest".Translate(linkedQuest.name),
						defaultDesc = "CommandOpenLinkedQuestDesc".Translate(),
						icon = TexCommand.OpenLinkedQuestTex,
						action = delegate()
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
							((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(linkedQuest);
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x001E9218 File Offset: 0x001E7418
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

		// Token: 0x060061C0 RID: 25024 RVA: 0x001E92FC File Offset: 0x001E74FC
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

		// Token: 0x04004145 RID: 16709
		public const string QuestTargetSignalPart_MapGenerated = "MapGenerated";

		// Token: 0x04004146 RID: 16710
		public const string QuestTargetSignalPart_MapRemoved = "MapRemoved";

		// Token: 0x04004147 RID: 16711
		public const string QuestTargetSignalPart_Spawned = "Spawned";

		// Token: 0x04004148 RID: 16712
		public const string QuestTargetSignalPart_Despawned = "Despawned";

		// Token: 0x04004149 RID: 16713
		public const string QuestTargetSignalPart_Destroyed = "Destroyed";

		// Token: 0x0400414A RID: 16714
		public const string QuestTargetSignalPart_Killed = "Killed";

		// Token: 0x0400414B RID: 16715
		public const string QuestTargetSignalPart_ChangedFaction = "ChangedFaction";

		// Token: 0x0400414C RID: 16716
		public const string QuestTargetSignalPart_LeftMap = "LeftMap";

		// Token: 0x0400414D RID: 16717
		public const string QuestTargetSignalPart_SurgeryViolation = "SurgeryViolation";

		// Token: 0x0400414E RID: 16718
		public const string QuestTargetSignalPart_Arrested = "Arrested";

		// Token: 0x0400414F RID: 16719
		public const string QuestTargetSignalPart_Recruited = "Recruited";

		// Token: 0x04004150 RID: 16720
		public const string QuestTargetSignalPart_Kidnapped = "Kidnapped";

		// Token: 0x04004151 RID: 16721
		public const string QuestTargetSignalPart_ChangedHostFaction = "ChangedHostFaction";

		// Token: 0x04004152 RID: 16722
		public const string QuestTargetSignalPart_NoLongerFactionLeader = "NoLongerFactionLeader";

		// Token: 0x04004153 RID: 16723
		public const string QuestTargetSignalPart_TitleChanged = "TitleChanged";

		// Token: 0x04004154 RID: 16724
		public const string QuestTargetSignalPart_TitleAwardedWhenUpdatingChanged = "TitleAwardedWhenUpdatingChanged";

		// Token: 0x04004155 RID: 16725
		public const string QuestTargetSignalPart_Banished = "Banished";

		// Token: 0x04004156 RID: 16726
		public const string QuestTargetSignalPart_RanWild = "RanWild";

		// Token: 0x04004157 RID: 16727
		public const string QuestTargetSignalPart_ShuttleSentSatisfied = "SentSatisfied";

		// Token: 0x04004158 RID: 16728
		public const string QuestTargetSignalPart_ShuttleSentUnsatisfied = "SentUnsatisfied";

		// Token: 0x04004159 RID: 16729
		public const string QuestTargetSignalPart_ShuttleSentWithExtraColonists = "SentWithExtraColonists";

		// Token: 0x0400415A RID: 16730
		public const string QuestTargetSignalPart_ShuttleUnloaded = "Unloaded";

		// Token: 0x0400415B RID: 16731
		public const string QuestTargetSignalPart_AllEnemiesDefeated = "AllEnemiesDefeated";

		// Token: 0x0400415C RID: 16732
		public const string QuestTargetSignalPart_TradeRequestFulfilled = "TradeRequestFulfilled";

		// Token: 0x0400415D RID: 16733
		public const string QuestTargetSignalPart_PeaceTalksResolved = "Resolved";

		// Token: 0x0400415E RID: 16734
		public const string QuestTargetSignalPart_LaunchedShip = "LaunchedShip";

		// Token: 0x0400415F RID: 16735
		public const string QuestTargetSignalPart_ReactorDestroyed = "ReactorDestroyed";

		// Token: 0x04004160 RID: 16736
		public const string QuestTargetSignalPart_MonumentCompleted = "MonumentCompleted";

		// Token: 0x04004161 RID: 16737
		public const string QuestTargetSignalPart_MonumentDestroyed = "MonumentDestroyed";

		// Token: 0x04004162 RID: 16738
		public const string QuestTargetSignalPart_MonumentCancelled = "MonumentCancelled";

		// Token: 0x04004163 RID: 16739
		public const string QuestTargetSignalPart_AllHivesDestroyed = "AllHivesDestroyed";

		// Token: 0x04004164 RID: 16740
		public const string QuestTargetSignalPart_ExitMentalState = "ExitMentalState";

		// Token: 0x04004165 RID: 16741
		public const string QuestTargetSignalPart_FactionBecameHostileToPlayer = "BecameHostileToPlayer";

		// Token: 0x04004166 RID: 16742
		public const string QuestTargetSignalPart_CeremonyExpired = "CeremonyExpired";

		// Token: 0x04004167 RID: 16743
		public const string QuestTargetSignalPart_CeremonyFailed = "CeremonyFailed";

		// Token: 0x04004168 RID: 16744
		public const string QuestTargetSignalPart_CeremonyDone = "CeremonyDone";

		// Token: 0x04004169 RID: 16745
		public const string QuestTargetSignalPart_QuestEnded = "QuestEnded";

		// Token: 0x0400416A RID: 16746
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x0400416B RID: 16747
		private static List<QuestPart> tmpQuestParts = new List<QuestPart>();
	}
}
