using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EBA RID: 7866
	public static class QuestGen_Factions
	{
		// Token: 0x0600A8E1 RID: 43233 RVA: 0x00313D24 File Offset: 0x00311F24
		public static QuestPart_AssaultColony AssaultColony(this Quest quest, Faction faction, MapParent mapParent, IEnumerable<Pawn> pawns, string inSignal = null, string inSignalRemovePawn = null)
		{
			QuestPart_AssaultColony questPart_AssaultColony = new QuestPart_AssaultColony();
			questPart_AssaultColony.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_AssaultColony.faction = faction;
			questPart_AssaultColony.mapParent = mapParent;
			questPart_AssaultColony.pawns.AddRange(pawns);
			questPart_AssaultColony.inSignalRemovePawn = inSignalRemovePawn;
			quest.AddPart(questPart_AssaultColony);
			return questPart_AssaultColony;
		}

		// Token: 0x0600A8E2 RID: 43234 RVA: 0x00313D84 File Offset: 0x00311F84
		public static QuestPart_ExtraFaction ExtraFaction(this Quest quest, Faction faction, IEnumerable<Pawn> pawns, ExtraFactionType factionType, bool areHelpers = false, string inSignalRemovePawn = null)
		{
			QuestPart_ExtraFaction questPart_ExtraFaction = new QuestPart_ExtraFaction
			{
				affectedPawns = pawns.ToList<Pawn>(),
				extraFaction = new ExtraFaction(faction, factionType),
				areHelpers = areHelpers,
				inSignalRemovePawn = inSignalRemovePawn
			};
			quest.AddPart(questPart_ExtraFaction);
			return questPart_ExtraFaction;
		}

		// Token: 0x0600A8E3 RID: 43235 RVA: 0x00313DC8 File Offset: 0x00311FC8
		public static QuestPart_SetFactionRelations SetFactionRelations(this Quest quest, Faction faction, FactionRelationKind relationKind, string inSignal = null, bool? canSendLetter = null)
		{
			QuestPart_SetFactionRelations questPart_SetFactionRelations = new QuestPart_SetFactionRelations();
			questPart_SetFactionRelations.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetFactionRelations.faction = faction;
			questPart_SetFactionRelations.relationKind = relationKind;
			questPart_SetFactionRelations.canSendLetter = (canSendLetter ?? true);
			quest.AddPart(questPart_SetFactionRelations);
			return questPart_SetFactionRelations;
		}

		// Token: 0x0600A8E4 RID: 43236 RVA: 0x00313E30 File Offset: 0x00312030
		public static QuestPart_ReserveFaction ReserveFaction(this Quest quest, Faction faction)
		{
			QuestPart_ReserveFaction questPart_ReserveFaction = new QuestPart_ReserveFaction();
			questPart_ReserveFaction.faction = faction;
			quest.AddPart(questPart_ReserveFaction);
			return questPart_ReserveFaction;
		}

		// Token: 0x0600A8E5 RID: 43237 RVA: 0x00313E54 File Offset: 0x00312054
		public static QuestPart_FactionGoodwillChange FactionGoodwillChange(this Quest quest, Faction faction, int change = 0, string inSignal = null, bool canSendMessage = true, bool canSendHostilityLetter = true, string reason = null, bool getLookTargetFromSignal = true, QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly)
		{
			QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
			questPart_FactionGoodwillChange.faction = faction;
			questPart_FactionGoodwillChange.change = change;
			questPart_FactionGoodwillChange.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_FactionGoodwillChange.canSendMessage = canSendMessage;
			questPart_FactionGoodwillChange.canSendHostilityLetter = canSendHostilityLetter;
			questPart_FactionGoodwillChange.reason = reason;
			questPart_FactionGoodwillChange.getLookTargetFromSignal = getLookTargetFromSignal;
			questPart_FactionGoodwillChange.signalListenMode = signalListenMode;
			quest.AddPart(questPart_FactionGoodwillChange);
			return questPart_FactionGoodwillChange;
		}

		// Token: 0x0600A8E6 RID: 43238 RVA: 0x00313EC4 File Offset: 0x003120C4
		public static QuestPart_SetFactionHidden SetFactionHidden(this Quest quest, Faction faction, bool hidden = false, string inSignal = null)
		{
			QuestPart_SetFactionHidden questPart_SetFactionHidden = new QuestPart_SetFactionHidden();
			questPart_SetFactionHidden.faction = faction;
			questPart_SetFactionHidden.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetFactionHidden.hidden = hidden;
			quest.AddPart(questPart_SetFactionHidden);
			return questPart_SetFactionHidden;
		}
	}
}
