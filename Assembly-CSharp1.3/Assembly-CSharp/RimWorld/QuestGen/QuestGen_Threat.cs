using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200161D RID: 5661
	public static class QuestGen_Threat
	{
		// Token: 0x060084B9 RID: 33977 RVA: 0x002FB234 File Offset: 0x002F9434
		public static void Raid(this Quest quest, Map map, float points, Faction faction, string inSignalLeave = null, string customLetterLabel = null, string customLetterText = null, RulePack customLetterLabelRules = null, RulePack customLetterTextRules = null, IntVec3? walkInSpot = null, string tag = null, string inSignal = null, string rootSymbol = "root", PawnsArrivalModeDef raidArrivalMode = null, RaidStrategyDef raidStrategy = null)
		{
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.debugLabel = "raid";
			questPart_Incident.incident = IncidentDefOf.RaidEnemy;
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.target = map;
			incidentParms.points = Mathf.Max(points, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
			incidentParms.faction = faction;
			incidentParms.pawnGroupMakerSeed = new int?(Rand.Int);
			incidentParms.inSignalEnd = inSignalLeave;
			incidentParms.questTag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(tag);
			incidentParms.raidArrivalMode = raidArrivalMode;
			incidentParms.raidStrategy = raidStrategy;
			if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
			{
				QuestGen.AddTextRequest(rootSymbol, delegate(string x)
				{
					incidentParms.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, rootSymbol));
			}
			if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
			{
				QuestGen.AddTextRequest(rootSymbol, delegate(string x)
				{
					incidentParms.customLetterText = x;
				}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, rootSymbol));
			}
			IncidentWorker_Raid incidentWorker_Raid = (IncidentWorker_Raid)questPart_Incident.incident.Worker;
			incidentWorker_Raid.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
			incidentWorker_Raid.ResolveRaidArriveMode(incidentParms);
			if (incidentParms.raidArrivalMode.walkIn)
			{
				incidentParms.spawnCenter = (walkInSpot ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, incidentParms, false);
			defaultPawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(defaultPawnGroupMakerParms.points, incidentParms.raidArrivalMode, incidentParms.raidStrategy, defaultPawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat);
			IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(defaultPawnGroupMakerParms);
			questPart_Incident.SetIncidentParmsAndRemoveTarget(incidentParms);
			questPart_Incident.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_Incident);
			QuestGen.AddQuestDescriptionRules(new List<Rule>
			{
				new Rule_String("raidPawnKinds", PawnUtility.PawnKindsToLineList(pawnKinds, "  - ", ColoredText.ThreatColor)),
				new Rule_String("raidArrivalModeInfo", incidentParms.raidArrivalMode.textWillArrive.Formatted(faction))
			});
		}

		// Token: 0x060084BA RID: 33978 RVA: 0x002FB4C4 File Offset: 0x002F96C4
		public static void RandomRaid(this Quest quest, MapParent mapParent, FloatRange pointsRange, Faction faction = null, string inSignal = null, PawnsArrivalModeDef arrivalMode = null, RaidStrategyDef raidStrategy = null, string customLetterLabel = null, string customLetterText = null)
		{
			quest.AddPart(new QuestPart_RandomRaid
			{
				mapParent = mapParent,
				faction = faction,
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				pointsRange = pointsRange,
				arrivalMode = arrivalMode,
				raidStrategy = raidStrategy,
				customLetterLabel = customLetterLabel,
				customLetterText = customLetterText
			});
		}
	}
}
