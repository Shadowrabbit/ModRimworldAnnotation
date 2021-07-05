using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02001616 RID: 5654
	public static class QuestGen_Misc
	{
		// Token: 0x0600846F RID: 33903 RVA: 0x002F7CA0 File Offset: 0x002F5EA0
		public static QuestPart_InvolvedFactions AddInvolvedFaction(this Quest quest, Faction faction)
		{
			QuestPart_InvolvedFactions questPart_InvolvedFactions = ((QuestPart_InvolvedFactions)quest.PartsListForReading.First((QuestPart p) => p is QuestPart_InvolvedFactions)) ?? new QuestPart_InvolvedFactions();
			questPart_InvolvedFactions.factions.Add(faction);
			if (!quest.PartsListForReading.Contains(questPart_InvolvedFactions))
			{
				quest.AddPart(questPart_InvolvedFactions);
			}
			return questPart_InvolvedFactions;
		}

		// Token: 0x06008470 RID: 33904 RVA: 0x002F7D08 File Offset: 0x002F5F08
		public static QuestPart_SpawnThing SpawnSkyfaller(this Quest quest, Map map, ThingDef skyfallerDef, IEnumerable<Thing> innerThings, Faction factionForSafeSpot = null, IntVec3? cell = null, string inSignal = null, bool lookForSafeSpot = false, bool tryLandInShipLandingZone = false, Thing tryLandNearThing = null, Pawn mapParentOfPawn = null)
		{
			Skyfaller thing = SkyfallerMaker.MakeSkyfaller(skyfallerDef, innerThings);
			QuestPart_SpawnThing questPart_SpawnThing = new QuestPart_SpawnThing();
			questPart_SpawnThing.thing = thing;
			questPart_SpawnThing.mapParent = ((map == null) ? null : map.Parent);
			questPart_SpawnThing.mapParentOfPawn = mapParentOfPawn;
			questPart_SpawnThing.factionForFindingSpot = factionForSafeSpot;
			if (cell != null)
			{
				questPart_SpawnThing.cell = cell.Value;
			}
			questPart_SpawnThing.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SpawnThing.lookForSafeSpot = lookForSafeSpot;
			questPart_SpawnThing.tryLandInShipLandingZone = tryLandInShipLandingZone;
			questPart_SpawnThing.tryLandNearThing = tryLandNearThing;
			quest.AddPart(questPart_SpawnThing);
			return questPart_SpawnThing;
		}

		// Token: 0x06008471 RID: 33905 RVA: 0x002F7DA0 File Offset: 0x002F5FA0
		public static QuestPart_SpawnThing SpawnThing(this Quest quest, Map map, Thing thing, Faction factionForSafeSpot = null, IntVec3? cell = null, string inSignal = null, bool lookForSafeSpot = false, bool tryLandInShipLandingZone = false, Thing tryLandNearThing = null, Pawn mapParentOfPawn = null)
		{
			QuestPart_SpawnThing questPart_SpawnThing = new QuestPart_SpawnThing();
			questPart_SpawnThing.thing = thing;
			questPart_SpawnThing.mapParent = ((map == null) ? null : map.Parent);
			questPart_SpawnThing.mapParentOfPawn = mapParentOfPawn;
			questPart_SpawnThing.factionForFindingSpot = factionForSafeSpot;
			if (cell != null)
			{
				questPart_SpawnThing.cell = cell.Value;
			}
			questPart_SpawnThing.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SpawnThing.lookForSafeSpot = lookForSafeSpot;
			questPart_SpawnThing.tryLandInShipLandingZone = tryLandInShipLandingZone;
			questPart_SpawnThing.tryLandNearThing = tryLandNearThing;
			quest.AddPart(questPart_SpawnThing);
			return questPart_SpawnThing;
		}

		// Token: 0x06008472 RID: 33906 RVA: 0x002F7E30 File Offset: 0x002F6030
		public static QuestPart_SetFaction SetFaction(this Quest quest, IEnumerable<Thing> things, Faction faction, string inSignal = null)
		{
			QuestPart_SetFaction questPart_SetFaction = new QuestPart_SetFaction();
			questPart_SetFaction.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetFaction.faction = faction;
			questPart_SetFaction.things.AddRange(things);
			QuestGen.quest.AddPart(questPart_SetFaction);
			return questPart_SetFaction;
		}

		// Token: 0x06008473 RID: 33907 RVA: 0x002F7E80 File Offset: 0x002F6080
		public static QuestPart_JoinPlayer JoinPlayer(this Quest quest, MapParent mapParent, IEnumerable<Pawn> pawns, bool joinPlayer = false, bool makePrisoners = false, string inSignal = null)
		{
			QuestPart_JoinPlayer questPart_JoinPlayer = new QuestPart_JoinPlayer();
			questPart_JoinPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_JoinPlayer.joinPlayer = joinPlayer;
			questPart_JoinPlayer.makePrisoners = makePrisoners;
			questPart_JoinPlayer.mapParent = mapParent;
			questPart_JoinPlayer.pawns.AddRange(pawns);
			quest.AddPart(questPart_JoinPlayer);
			return questPart_JoinPlayer;
		}

		// Token: 0x06008474 RID: 33908 RVA: 0x002F7EE0 File Offset: 0x002F60E0
		public static QuestPart_LeavePlayer LeavePlayer(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null, Faction replacementFaction = null, string inSignalRemovePawn = null)
		{
			QuestPart_LeavePlayer questPart_LeavePlayer = new QuestPart_LeavePlayer();
			questPart_LeavePlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_LeavePlayer.pawns.AddRange(pawns);
			questPart_LeavePlayer.replacementFaction = replacementFaction;
			questPart_LeavePlayer.inSignalRemovePawn = inSignalRemovePawn;
			quest.AddPart(questPart_LeavePlayer);
			return questPart_LeavePlayer;
		}

		// Token: 0x06008475 RID: 33909 RVA: 0x002F7F38 File Offset: 0x002F6138
		public static QuestPart_SurpriseReinforcement SurpriseReinforcements(this Quest quest, string inSignalEnabled, MapParent mapParent, Faction faction, float reinforcementChance)
		{
			QuestPart_SurpriseReinforcement questPart_SurpriseReinforcement = new QuestPart_SurpriseReinforcement();
			questPart_SurpriseReinforcement.inSignalEnable = inSignalEnabled;
			questPart_SurpriseReinforcement.mapParent = mapParent;
			questPart_SurpriseReinforcement.faction = faction;
			questPart_SurpriseReinforcement.reinforcementChance = reinforcementChance;
			quest.AddPart(questPart_SurpriseReinforcement);
			return questPart_SurpriseReinforcement;
		}

		// Token: 0x06008476 RID: 33910 RVA: 0x002F7F70 File Offset: 0x002F6170
		public static QuestPart_DropPods DropPods(this Quest quest, MapParent mapParent, IEnumerable<Thing> contents, string customLetterLabel = null, RulePack customLetterLabelRules = null, string customLetterText = null, RulePack customLetterTextRules = null, bool? sendStandardLetter = true, bool useTradeDropSpot = false, bool joinPlayer = false, bool makePrisoners = false, string inSignal = null, IEnumerable<Thing> thingsToExcludeFromHyperlinks = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly, IntVec3? dropSpot = null, bool destroyItemsOnCleanup = true)
		{
			QuestPart_DropPods dropPods = new QuestPart_DropPods();
			dropPods.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			dropPods.signalListenMode = signalListenMode;
			if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
			}
			if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					dropPods.customLetterText = x;
				}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
			}
			dropPods.sendStandardLetter = (sendStandardLetter ?? dropPods.sendStandardLetter);
			dropPods.useTradeDropSpot = useTradeDropSpot;
			dropPods.joinPlayer = joinPlayer;
			dropPods.makePrisoners = makePrisoners;
			dropPods.mapParent = mapParent;
			dropPods.Things = contents;
			dropPods.destroyItemsOnCleanup = destroyItemsOnCleanup;
			if (dropSpot != null)
			{
				dropPods.dropSpot = dropSpot.Value;
			}
			if (thingsToExcludeFromHyperlinks != null)
			{
				dropPods.thingsToExcludeFromHyperlinks.AddRange(from t in thingsToExcludeFromHyperlinks
				select t.GetInnerIfMinified().def);
			}
			QuestGen.quest.AddPart(dropPods);
			return dropPods;
		}

		// Token: 0x06008477 RID: 33911 RVA: 0x002F8104 File Offset: 0x002F6304
		public static void AddMemoryThought(this Quest quest, IEnumerable<Pawn> pawns, ThoughtDef def, string inSignal = null, Pawn otherPawn = null, bool? addToLookTargets = null)
		{
			foreach (Pawn pawn in pawns)
			{
				QuestPart_AddMemoryThought questPart_AddMemoryThought = new QuestPart_AddMemoryThought();
				questPart_AddMemoryThought.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
				questPart_AddMemoryThought.def = def;
				questPart_AddMemoryThought.pawn = pawn;
				questPart_AddMemoryThought.otherPawn = otherPawn;
				questPart_AddMemoryThought.addToLookTargets = (addToLookTargets ?? true);
				QuestGen.quest.AddPart(questPart_AddMemoryThought);
			}
		}

		// Token: 0x06008478 RID: 33912 RVA: 0x002F81AC File Offset: 0x002F63AC
		public static QuestPart_Letter Letter(this Quest quest, LetterDef letterDef, string inSignal = null, string chosenPawnSignal = null, Faction relatedFaction = null, MapParent useColonistsOnMap = null, bool useColonistsFromCaravanArg = false, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly, IEnumerable<object> lookTargets = null, bool filterDeadPawnsFromLookTargets = false, string text = null, RulePack textRules = null, string label = null, RulePack labelRules = null, string getColonistsFromSignal = null)
		{
			Slate slate = QuestGen.slate;
			QuestPart_Letter questPart_Letter = new QuestPart_Letter();
			questPart_Letter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? slate.Get<string>("inSignal", null, false));
			LetterDef letterDef2 = letterDef ?? LetterDefOf.NeutralEvent;
			if (typeof(ChoiceLetter).IsAssignableFrom(letterDef2.letterClass))
			{
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter("error", "error", letterDef2, QuestGenUtility.ToLookTargets(lookTargets), relatedFaction, QuestGen.quest, null);
				questPart_Letter.letter = choiceLetter;
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.label = x;
				}, QuestGenUtility.MergeRules(labelRules, label, "root"));
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.text = x;
				}, QuestGenUtility.MergeRules(textRules, text, "root"));
			}
			else
			{
				questPart_Letter.letter = LetterMaker.MakeLetter(letterDef2);
				questPart_Letter.letter.lookTargets = QuestGenUtility.ToLookTargets(lookTargets);
				questPart_Letter.letter.relatedFaction = relatedFaction;
			}
			questPart_Letter.chosenPawnSignal = QuestGenUtility.HardcodedSignalWithQuestID(chosenPawnSignal);
			questPart_Letter.useColonistsOnMap = useColonistsOnMap;
			questPart_Letter.useColonistsFromCaravanArg = useColonistsFromCaravanArg;
			questPart_Letter.signalListenMode = signalListenMode;
			questPart_Letter.filterDeadPawnsFromLookTargets = filterDeadPawnsFromLookTargets;
			questPart_Letter.getColonistsFromSignal = getColonistsFromSignal;
			QuestGen.quest.AddPart(questPart_Letter);
			return questPart_Letter;
		}

		// Token: 0x06008479 RID: 33913 RVA: 0x002F8300 File Offset: 0x002F6500
		public static void PawnsArrive(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null, MapParent mapParent = null, PawnsArrivalModeDef arrivalMode = null, bool joinPlayer = false, IntVec3? walkInSpot = null, string customLetterLabel = null, string customLetterText = null, RulePack customLetterLabelRules = null, RulePack customLetterTextRules = null, bool isSingleReward = false, bool rewardDetailsHidden = false, bool sendStandardLetter = true)
		{
			Slate slate = QuestGen.slate;
			PawnsArrivalModeDef pawnsArrivalModeDef = arrivalMode ?? PawnsArrivalModeDefOf.EdgeWalkIn;
			QuestPart_PawnsArrive pawnsArrive = new QuestPart_PawnsArrive();
			pawnsArrive.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			pawnsArrive.pawns.AddRange(pawns);
			pawnsArrive.arrivalMode = pawnsArrivalModeDef;
			pawnsArrive.joinPlayer = joinPlayer;
			pawnsArrive.mapParent = (mapParent ?? QuestGen.slate.Get<Map>("map", null, false).Parent);
			pawnsArrive.sendStandardLetter = sendStandardLetter;
			if (pawnsArrivalModeDef.walkIn)
			{
				pawnsArrive.spawnNear = (walkInSpot ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
			}
			if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterText = x;
				}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
			}
			QuestGen.quest.AddPart(pawnsArrive);
			if (isSingleReward)
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = pawnsArrive.inSignal;
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.questParts.Add(pawnsArrive);
				foreach (Pawn pawn in pawnsArrive.pawns)
				{
					choice.rewards.Add(new Reward_Pawn
					{
						pawn = pawn,
						detailsHidden = rewardDetailsHidden
					});
				}
				questPart_Choice.choices.Add(choice);
				QuestGen.quest.AddPart(questPart_Choice);
			}
		}

		// Token: 0x0600847A RID: 33914 RVA: 0x002F853C File Offset: 0x002F673C
		public static QuestPart_AddQuestRefugeeDelayedReward AddQuestRefugeeDelayedReward(this Quest quest, Pawn acceptee, Faction faction, IEnumerable<Pawn> pawns, FloatRange marketValueRange, string inSignalRemovePawn = null)
		{
			QuestPart_AddQuestRefugeeDelayedReward questPart_AddQuestRefugeeDelayedReward = new QuestPart_AddQuestRefugeeDelayedReward();
			questPart_AddQuestRefugeeDelayedReward.acceptee = quest.AccepterPawn;
			questPart_AddQuestRefugeeDelayedReward.inSignal = QuestGen.slate.Get<string>("inSignal", null, false);
			questPart_AddQuestRefugeeDelayedReward.inSignalRemovePawn = inSignalRemovePawn;
			questPart_AddQuestRefugeeDelayedReward.faction = faction;
			questPart_AddQuestRefugeeDelayedReward.lodgers.AddRange(pawns);
			questPart_AddQuestRefugeeDelayedReward.marketValueRange = marketValueRange;
			quest.AddPart(questPart_AddQuestRefugeeDelayedReward);
			return questPart_AddQuestRefugeeDelayedReward;
		}

		// Token: 0x0600847B RID: 33915 RVA: 0x002F85A0 File Offset: 0x002F67A0
		public static QuestPart_PawnJoinOffer PawnJoinOffer(this Quest quest, Pawn pawn, string letterLabel, string letterTitle, string letterText, Action accepted = null, Action rejected = null, string inSignal = null, string outSignalPawnAccepted = null, string outSignalPawnRejected = null, bool charity = false)
		{
			QuestPart_PawnJoinOffer questPart_PawnJoinOffer = new QuestPart_PawnJoinOffer();
			questPart_PawnJoinOffer.pawn = pawn;
			questPart_PawnJoinOffer.inSignalEnable = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PawnJoinOffer.outSignalPawnAccepted = (outSignalPawnAccepted ?? QuestGen.GenerateNewSignal("Accepted", true));
			questPart_PawnJoinOffer.outSignalPawnRejected = (outSignalPawnRejected ?? QuestGen.GenerateNewSignal("Rejected", true));
			questPart_PawnJoinOffer.letterLabel = letterLabel;
			questPart_PawnJoinOffer.letterText = letterText;
			questPart_PawnJoinOffer.letterTitle = letterTitle;
			questPart_PawnJoinOffer.charity = charity;
			if (accepted != null)
			{
				QuestGenUtility.RunInner(accepted, questPart_PawnJoinOffer.outSignalPawnAccepted);
			}
			if (rejected != null)
			{
				QuestGenUtility.RunInner(rejected, questPart_PawnJoinOffer.outSignalPawnRejected);
			}
			quest.AddPart(questPart_PawnJoinOffer);
			return questPart_PawnJoinOffer;
		}

		// Token: 0x0600847C RID: 33916 RVA: 0x002F8650 File Offset: 0x002F6850
		public static QuestPart_Choice RewardChoice(this Quest quest, IEnumerable<QuestPart_Choice.Choice> choices = null, string inSignalChoiceUsed = null)
		{
			QuestPart_Choice questPart_Choice = new QuestPart_Choice();
			questPart_Choice.inSignalChoiceUsed = inSignalChoiceUsed;
			if (choices != null)
			{
				questPart_Choice.choices.AddRange(choices);
			}
			quest.AddPart(questPart_Choice);
			return questPart_Choice;
		}

		// Token: 0x0600847D RID: 33917 RVA: 0x002F8684 File Offset: 0x002F6884
		public static QuestPart_BetrayalOffer BetrayalOffer(this Quest quest, IEnumerable<Pawn> pawns, ExtraFaction extraFaction, Pawn asker, Action success = null, Action failure = null, Action enabled = null, IEnumerable<string> inSignals = null, string inSignalEnable = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_BetrayalOffer questPart_BetrayalOffer = new QuestPart_BetrayalOffer();
			questPart_BetrayalOffer.pawns.AddRange(pawns);
			questPart_BetrayalOffer.asker = asker;
			questPart_BetrayalOffer.extraFaction = extraFaction;
			questPart_BetrayalOffer.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_BetrayalOffer.signalListenMode = signalListenMode;
			if (inSignals != null)
			{
				questPart_BetrayalOffer.inSignals.AddRange(inSignals);
			}
			if (success != null)
			{
				string text = QuestGen.GenerateNewSignal("BetrayalOfferSuccess", true);
				QuestGenUtility.RunInner(success, text);
				questPart_BetrayalOffer.outSignalSuccess = text;
			}
			if (failure != null)
			{
				string text2 = QuestGen.GenerateNewSignal("BetrayalOfferFailure", true);
				QuestGenUtility.RunInner(failure, text2);
				questPart_BetrayalOffer.outSignalFailure = text2;
			}
			if (enabled != null)
			{
				string text3 = QuestGen.GenerateNewSignal("BetrayalOfferEnabled", true);
				QuestGenUtility.RunInner(enabled, text3);
				questPart_BetrayalOffer.outSignalEnabled = text3;
			}
			quest.AddPart(questPart_BetrayalOffer);
			return questPart_BetrayalOffer;
		}

		// Token: 0x0600847E RID: 33918 RVA: 0x002F874C File Offset: 0x002F694C
		public static void Leave(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null, bool sendStandardLetter = true, bool leaveOnCleanup = true, string inSignalRemovePawn = null, bool wakeUp = false)
		{
			QuestPart_Leave questPart_Leave = new QuestPart_Leave();
			questPart_Leave.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Leave.pawns.AddRange(pawns);
			questPart_Leave.sendStandardLetter = sendStandardLetter;
			questPart_Leave.leaveOnCleanup = leaveOnCleanup;
			questPart_Leave.inSignalRemovePawn = inSignalRemovePawn;
			questPart_Leave.wakeUp = wakeUp;
			quest.AddPart(questPart_Leave);
		}

		// Token: 0x0600847F RID: 33919 RVA: 0x002F87B0 File Offset: 0x002F69B0
		public static QuestPart_Alert Alert(this Quest quest, string label, string explanation, LookTargets lookTargets = null, bool critical = false, bool getLookTargetsFromSignal = false, string inSignalEnable = null, string inSignalDisable = null)
		{
			QuestPart_Alert questPart_Alert = new QuestPart_Alert();
			questPart_Alert.label = label;
			questPart_Alert.explanation = explanation;
			questPart_Alert.critical = critical;
			questPart_Alert.getLookTargetsFromSignal = getLookTargetsFromSignal;
			questPart_Alert.lookTargets = lookTargets;
			questPart_Alert.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Alert.inSignalDisable = inSignalDisable;
			quest.AddPart(questPart_Alert);
			return questPart_Alert;
		}

		// Token: 0x06008480 RID: 33920 RVA: 0x002F8818 File Offset: 0x002F6A18
		public static QuestPart_Message Message(this Quest quest, string message, MessageTypeDef messageType = null, bool getLookTargetsFromSignal = false, RulePack rules = null, LookTargets lookTargets = null, string inSignal = null)
		{
			QuestPart_Message questPart = new QuestPart_Message();
			questPart.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart.messageType = (messageType ?? MessageTypeDefOf.NeutralEvent);
			questPart.lookTargets = lookTargets;
			questPart.getLookTargetsFromSignal = getLookTargetsFromSignal;
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				questPart.message = x;
			}, QuestGenUtility.MergeRules(rules, message, "root"));
			QuestGen.quest.AddPart(questPart);
			return questPart;
		}

		// Token: 0x06008481 RID: 33921 RVA: 0x002F88C4 File Offset: 0x002F6AC4
		public static QuestPart_Notify_PlayerRaidedSomeone Notify_PlayerRaidedSomeone(this Quest quest, Map getRaidersFromMap = null, MapParent getRaidersFromMapParent = null, string inSignal = null)
		{
			QuestPart_Notify_PlayerRaidedSomeone questPart_Notify_PlayerRaidedSomeone = new QuestPart_Notify_PlayerRaidedSomeone();
			questPart_Notify_PlayerRaidedSomeone.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Notify_PlayerRaidedSomeone.getRaidersFromMap = getRaidersFromMap;
			questPart_Notify_PlayerRaidedSomeone.getRaidersFromMapParent = getRaidersFromMapParent;
			quest.AddPart(questPart_Notify_PlayerRaidedSomeone);
			return questPart_Notify_PlayerRaidedSomeone;
		}

		// Token: 0x06008482 RID: 33922 RVA: 0x002F8910 File Offset: 0x002F6B10
		public static QuestPart_FactionGoodwillChange_ShuttleSentThings GoodwillChangeShuttleSentThings(this Quest quest, Faction faction, IEnumerable<Pawn> pawns, int changeNotOnShuttle, string inSignalEnable = null, IEnumerable<string> inSignalsShuttleSent = null, string inSignalShuttleDestroyed = null, HistoryEventDef reason = null, bool canSendMessage = true, bool canSendHostilityLetter = false, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_FactionGoodwillChange_ShuttleSentThings questPart_FactionGoodwillChange_ShuttleSentThings = new QuestPart_FactionGoodwillChange_ShuttleSentThings();
			questPart_FactionGoodwillChange_ShuttleSentThings.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_FactionGoodwillChange_ShuttleSentThings.inSignalsShuttleSent.AddRange(inSignalsShuttleSent);
			questPart_FactionGoodwillChange_ShuttleSentThings.inSignalShuttleDestroyed = inSignalShuttleDestroyed;
			questPart_FactionGoodwillChange_ShuttleSentThings.changeNotOnShuttle = changeNotOnShuttle;
			questPart_FactionGoodwillChange_ShuttleSentThings.things.AddRange(pawns);
			questPart_FactionGoodwillChange_ShuttleSentThings.faction = faction;
			questPart_FactionGoodwillChange_ShuttleSentThings.historyEvent = reason;
			questPart_FactionGoodwillChange_ShuttleSentThings.canSendMessage = canSendMessage;
			questPart_FactionGoodwillChange_ShuttleSentThings.canSendHostilityLetter = canSendHostilityLetter;
			questPart_FactionGoodwillChange_ShuttleSentThings.signalListenMode = signalListenMode;
			quest.AddPart(questPart_FactionGoodwillChange_ShuttleSentThings);
			return questPart_FactionGoodwillChange_ShuttleSentThings;
		}

		// Token: 0x06008483 RID: 33923 RVA: 0x002F8998 File Offset: 0x002F6B98
		public static QuestPart_BiocodeWeapons BiocodeWeapons(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null)
		{
			QuestPart_BiocodeWeapons questPart_BiocodeWeapons = new QuestPart_BiocodeWeapons();
			questPart_BiocodeWeapons.pawns.AddRange(pawns);
			questPart_BiocodeWeapons.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_BiocodeWeapons);
			return questPart_BiocodeWeapons;
		}

		// Token: 0x06008484 RID: 33924 RVA: 0x002F89DB File Offset: 0x002F6BDB
		public static QuestPart_DestroyThingsOrPassToWorld DestroyThingsOrPassToWorld(this Quest quest, IEnumerable<Thing> things, string inSignal = null, bool questLookTargets = true, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_DestroyThingsOrPassToWorld questPart_DestroyThingsOrPassToWorld = new QuestPart_DestroyThingsOrPassToWorld();
			questPart_DestroyThingsOrPassToWorld.things.AddRange(things);
			questPart_DestroyThingsOrPassToWorld.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DestroyThingsOrPassToWorld.questLookTargets = true;
			questPart_DestroyThingsOrPassToWorld.signalListenMode = signalListenMode;
			return questPart_DestroyThingsOrPassToWorld;
		}

		// Token: 0x06008485 RID: 33925 RVA: 0x002F8A1C File Offset: 0x002F6C1C
		public static QuestPart_DescriptionPart DescriptionPart(this Quest quest, string descriptionPart, string inSignalEnable = null, string inSignalDisable = null, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly, RulePack rulePack = null)
		{
			QuestPart_DescriptionPart questPart = new QuestPart_DescriptionPart();
			questPart.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart.inSignalDisable = inSignalDisable;
			questPart.descriptionPart = descriptionPart;
			questPart.signalListenMode = signalListenMode;
			quest.AddPart(questPart);
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				questPart.descriptionPart = x;
			}, QuestGenUtility.MergeRules(rulePack, descriptionPart, "root"));
			return questPart;
		}

		// Token: 0x06008486 RID: 33926 RVA: 0x002F8AB8 File Offset: 0x002F6CB8
		public static QuestPart_Dialog Dialog(this Quest quest, string text, string title = null, string inSignal = null, RulePack titleRules = null, RulePack textRules = null, QuestPart.SignalListenMode signalListMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_Dialog questPart = new QuestPart_Dialog();
			questPart.title = title;
			questPart.text = text;
			questPart.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart.signalListenMode = signalListMode;
			quest.AddPart(questPart);
			if (!title.NullOrEmpty())
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					questPart.title = x;
				}, QuestGenUtility.MergeRules(titleRules, title, "root"));
			}
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				questPart.text = x;
			}, QuestGenUtility.MergeRules(titleRules, text, "root"));
			return questPart;
		}

		// Token: 0x06008487 RID: 33927 RVA: 0x002F8B7C File Offset: 0x002F6D7C
		public static QuestPart_SetQuestNotYetAccepted SetQuestNotYetAccepted(this Quest quest, string inSignal = null, bool revertOngoingQuestAfterLoad = false)
		{
			QuestPart_SetQuestNotYetAccepted questPart_SetQuestNotYetAccepted = new QuestPart_SetQuestNotYetAccepted();
			questPart_SetQuestNotYetAccepted.inSignal = inSignal;
			questPart_SetQuestNotYetAccepted.revertOngoingQuestAfterLoad = revertOngoingQuestAfterLoad;
			quest.AddPart(questPart_SetQuestNotYetAccepted);
			return questPart_SetQuestNotYetAccepted;
		}

		// Token: 0x06008488 RID: 33928 RVA: 0x002F8BA8 File Offset: 0x002F6DA8
		public static QuestPart_StartWick StartWick(this Quest quest, Thing thing, string inSignal = null)
		{
			QuestPart_StartWick questPart_StartWick = new QuestPart_StartWick();
			questPart_StartWick.explosiveThing = thing;
			questPart_StartWick.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_StartWick);
			return questPart_StartWick;
		}

		// Token: 0x06008489 RID: 33929 RVA: 0x002F8BE8 File Offset: 0x002F6DE8
		public static QuestPart_GiveDiedOrDownedThoughts GiveDiedOrDownedThoughts(this Quest quest, Pawn aboutPawn, PawnDiedOrDownedThoughtsKind thoughtsKind, string inSignal = null)
		{
			QuestPart_GiveDiedOrDownedThoughts questPart_GiveDiedOrDownedThoughts = new QuestPart_GiveDiedOrDownedThoughts();
			questPart_GiveDiedOrDownedThoughts.aboutPawn = aboutPawn;
			questPart_GiveDiedOrDownedThoughts.thoughtsKind = thoughtsKind;
			questPart_GiveDiedOrDownedThoughts.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_GiveDiedOrDownedThoughts);
			return questPart_GiveDiedOrDownedThoughts;
		}

		// Token: 0x0400526E RID: 21102
		private const string RootSymbol = "root";
	}
}
