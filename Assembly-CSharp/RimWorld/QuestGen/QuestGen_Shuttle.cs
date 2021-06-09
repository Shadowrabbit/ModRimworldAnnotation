using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001ED2 RID: 7890
	public static class QuestGen_Shuttle
	{
		// Token: 0x0600A94D RID: 43341 RVA: 0x003167AC File Offset: 0x003149AC
		public static Thing GenerateShuttle(Faction owningFaction = null, IEnumerable<Pawn> requiredPawns = null, IEnumerable<ThingDefCount> requiredItems = null, bool acceptColonists = false, bool onlyAcceptColonists = false, bool onlyAcceptHealthy = false, int requireColonistCount = -1, bool dropEverythingIfUnsatisfied = false, bool leaveImmediatelyWhenSatisfied = false, bool dropEverythingOnArrival = false, bool stayAfterDroppedEverythingOnArrival = false, WorldObject missionShuttleTarget = null, WorldObject missionShuttleHome = null, int maxColonistCount = -1, ThingDef shuttleDef = null, bool permitShuttle = false, bool hideControls = true, List<Thing> sendAwayIfAllDespawned = null)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 8811221, false);
				return null;
			}
			Slate slate = QuestGen.slate;
			Thing thing = ThingMaker.MakeThing(shuttleDef ?? ThingDefOf.Shuttle, null);
			if (owningFaction != null)
			{
				thing.SetFaction(owningFaction, null);
			}
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			if (requiredPawns != null)
			{
				compShuttle.requiredPawns.AddRange(requiredPawns);
			}
			if (requiredItems != null)
			{
				compShuttle.requiredItems.AddRange(requiredItems);
			}
			compShuttle.acceptColonists = acceptColonists;
			compShuttle.onlyAcceptColonists = onlyAcceptColonists;
			compShuttle.onlyAcceptHealthy = onlyAcceptHealthy;
			compShuttle.requiredColonistCount = requireColonistCount;
			compShuttle.dropEverythingIfUnsatisfied = dropEverythingIfUnsatisfied;
			compShuttle.leaveImmediatelyWhenSatisfied = leaveImmediatelyWhenSatisfied;
			compShuttle.dropEverythingOnArrival = dropEverythingOnArrival;
			compShuttle.stayAfterDroppedEverythingOnArrival = stayAfterDroppedEverythingOnArrival;
			compShuttle.missionShuttleHome = missionShuttleHome;
			compShuttle.missionShuttleTarget = missionShuttleTarget;
			compShuttle.maxColonistCount = maxColonistCount;
			compShuttle.permitShuttle = permitShuttle;
			compShuttle.hideControls = hideControls;
			if (sendAwayIfAllDespawned != null)
			{
				compShuttle.sendAwayIfAllDespawned = sendAwayIfAllDespawned;
			}
			return thing;
		}

		// Token: 0x0600A94E RID: 43342 RVA: 0x00316890 File Offset: 0x00314A90
		public static QuestPart_SendShuttleAway SendShuttleAway(this Quest quest, Thing shuttle, bool dropEverything = false, string inSignal = null)
		{
			if (shuttle == null)
			{
				return null;
			}
			QuestPart_SendShuttleAway questPart_SendShuttleAway = new QuestPart_SendShuttleAway();
			questPart_SendShuttleAway.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SendShuttleAway.shuttle = shuttle;
			questPart_SendShuttleAway.dropEverything = dropEverything;
			quest.AddPart(questPart_SendShuttleAway);
			return questPart_SendShuttleAway;
		}

		// Token: 0x0600A94F RID: 43343 RVA: 0x003168E0 File Offset: 0x00314AE0
		public static QuestPart_SendShuttleAwayOnCleanup SendShuttleAwayOnCleanup(this Quest quest, Thing shuttle, bool dropEverything = false)
		{
			QuestPart_SendShuttleAwayOnCleanup questPart_SendShuttleAwayOnCleanup = new QuestPart_SendShuttleAwayOnCleanup();
			questPart_SendShuttleAwayOnCleanup.shuttle = shuttle;
			questPart_SendShuttleAwayOnCleanup.dropEverything = dropEverything;
			quest.AddPart(questPart_SendShuttleAwayOnCleanup);
			return questPart_SendShuttleAwayOnCleanup;
		}

		// Token: 0x0600A950 RID: 43344 RVA: 0x0031690C File Offset: 0x00314B0C
		public static QuestPart_AddContentsToShuttle AddContentsToShuttle(this Quest quest, Thing shuttle, IEnumerable<Thing> contents, string inSignal = null)
		{
			if (contents == null)
			{
				return null;
			}
			QuestPart_AddContentsToShuttle questPart_AddContentsToShuttle = new QuestPart_AddContentsToShuttle();
			questPart_AddContentsToShuttle.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_AddContentsToShuttle.shuttle = shuttle;
			questPart_AddContentsToShuttle.Things = contents;
			quest.AddPart(questPart_AddContentsToShuttle);
			return questPart_AddContentsToShuttle;
		}

		// Token: 0x0600A951 RID: 43345 RVA: 0x0031695C File Offset: 0x00314B5C
		public static QuestPart_ShuttleLeaveDelay ShuttleLeaveDelay(this Quest quest, Thing shuttle, int delayTicks, string inSignalEnable = null, IEnumerable<string> inSignalsDisable = null, string outSignalComplete = null, Action complete = null)
		{
			QuestPart_ShuttleLeaveDelay questPart_ShuttleLeaveDelay = new QuestPart_ShuttleLeaveDelay();
			questPart_ShuttleLeaveDelay.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleLeaveDelay.delayTicks = delayTicks;
			questPart_ShuttleLeaveDelay.shuttle = shuttle;
			questPart_ShuttleLeaveDelay.expiryInfoPart = "ShuttleDepartsIn".Translate();
			questPart_ShuttleLeaveDelay.expiryInfoPartTip = "ShuttleDepartsOn".Translate();
			if (inSignalsDisable != null)
			{
				foreach (string item in inSignalsDisable)
				{
					questPart_ShuttleLeaveDelay.inSignalsDisable.Add(item);
				}
			}
			if (!outSignalComplete.NullOrEmpty())
			{
				questPart_ShuttleLeaveDelay.outSignalsCompleted.Add(outSignalComplete);
			}
			if (complete != null)
			{
				string text = QuestGen.GenerateNewSignal("ShuttleLeaveDelay", true);
				QuestGenUtility.RunInner(complete, text);
				questPart_ShuttleLeaveDelay.outSignalsCompleted.Add(text);
			}
			quest.AddPart(questPart_ShuttleLeaveDelay);
			return questPart_ShuttleLeaveDelay;
		}

		// Token: 0x0600A952 RID: 43346 RVA: 0x00316A4C File Offset: 0x00314C4C
		public static QuestPart_ShuttleDelay ShuttleDelay(this Quest quest, int delayTicks, IEnumerable<Pawn> lodgers, Action complete = null, string inSignalEnable = null, IEnumerable<string> inSignalsDisable = null, bool alert = false)
		{
			QuestPart_ShuttleDelay questPart_ShuttleDelay = new QuestPart_ShuttleDelay();
			questPart_ShuttleDelay.inSignalEnable = (inSignalEnable ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ShuttleDelay.delayTicks = delayTicks;
			questPart_ShuttleDelay.alert = alert;
			if (lodgers != null)
			{
				questPart_ShuttleDelay.lodgers.AddRange(lodgers);
			}
			questPart_ShuttleDelay.expiryInfoPart = "ShuttleArrivesIn".Translate();
			questPart_ShuttleDelay.expiryInfoPartTip = "ShuttleArrivesOn".Translate();
			if (complete != null)
			{
				string text = QuestGen.GenerateNewSignal("ShuttleDelay", true);
				QuestGenUtility.RunInner(complete, text);
				questPart_ShuttleDelay.outSignalsCompleted.Add(text);
			}
			quest.AddPart(questPart_ShuttleDelay);
			return questPart_ShuttleDelay;
		}
	}
}
