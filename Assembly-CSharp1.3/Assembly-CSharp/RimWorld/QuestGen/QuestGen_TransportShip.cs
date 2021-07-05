using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200161E RID: 5662
	public static class QuestGen_TransportShip
	{
		// Token: 0x060084BB RID: 33979 RVA: 0x002FB530 File Offset: 0x002F9730
		public static QuestPart_SetupTransportShip GenerateTransportShip(this Quest quest, TransportShipDef def, IEnumerable<Thing> contents, Thing shipThing, string inSignal = null)
		{
			QuestPart_SetupTransportShip questPart_SetupTransportShip = new QuestPart_SetupTransportShip();
			questPart_SetupTransportShip.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetupTransportShip.transportShip = TransportShipMaker.MakeTransportShip(def, null, shipThing);
			List<Thing> items;
			if (contents == null)
			{
				items = null;
			}
			else
			{
				items = (from c in contents
				where !(c is Pawn)
				select c).ToList<Thing>();
			}
			questPart_SetupTransportShip.items = items;
			questPart_SetupTransportShip.pawns = ((contents != null) ? contents.OfType<Pawn>().ToList<Pawn>() : null);
			QuestPart_SetupTransportShip questPart_SetupTransportShip2 = questPart_SetupTransportShip;
			quest.AddPart(questPart_SetupTransportShip2);
			return questPart_SetupTransportShip2;
		}

		// Token: 0x060084BC RID: 33980 RVA: 0x002FB5C4 File Offset: 0x002F97C4
		public static QuestPart_SendTransportShipAwayOnCleanup SendTransportShipAwayOnCleanup(this Quest quest, TransportShip transportShip, bool unloadContents = false, TransportShipDropMode unsatisfiedDropMode = TransportShipDropMode.NonRequired)
		{
			QuestPart_SendTransportShipAwayOnCleanup questPart_SendTransportShipAwayOnCleanup = new QuestPart_SendTransportShipAwayOnCleanup
			{
				transportShip = transportShip,
				unloadContents = unloadContents,
				unsatisfiedDropMode = unsatisfiedDropMode
			};
			quest.AddPart(questPart_SendTransportShipAwayOnCleanup);
			return questPart_SendTransportShipAwayOnCleanup;
		}

		// Token: 0x060084BD RID: 33981 RVA: 0x002FB5F4 File Offset: 0x002F97F4
		public static QuestPart_AddShipJob AddShipJob(this Quest quest, TransportShip transportShip, ShipJobDef def, ShipJobStartMode startMode = ShipJobStartMode.Queue, string inSignal = null)
		{
			QuestPart_AddShipJob questPart_AddShipJob = new QuestPart_AddShipJob
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shipJobStartMode = startMode,
				transportShip = transportShip,
				shipJobDef = def
			};
			quest.AddPart(questPart_AddShipJob);
			return questPart_AddShipJob;
		}

		// Token: 0x060084BE RID: 33982 RVA: 0x002FB644 File Offset: 0x002F9844
		public static QuestPart_AddShipJob_Arrive AddShipJob_Arrive(this Quest quest, TransportShip transportShip, MapParent mapParent, IntVec3? cell = null, ShipJobStartMode startMode = ShipJobStartMode.Queue, Faction factionForArrival = null, string inSignal = null)
		{
			QuestPart_AddShipJob_Arrive questPart_AddShipJob_Arrive = new QuestPart_AddShipJob_Arrive
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shipJobStartMode = startMode,
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.Arrive,
				cell = (cell ?? IntVec3.Invalid),
				mapParent = mapParent,
				factionForArrival = factionForArrival
			};
			quest.AddPart(questPart_AddShipJob_Arrive);
			return questPart_AddShipJob_Arrive;
		}

		// Token: 0x060084BF RID: 33983 RVA: 0x002FB6C4 File Offset: 0x002F98C4
		public static QuestPart_AddShipJob_WaitTime AddShipJob_WaitTime(this Quest quest, TransportShip transportShip, int duration, bool leaveImmediatelyWhenSatisfied, List<Thing> sendAwayIfAllDespawned = null, string inSignal = null)
		{
			QuestPart_AddShipJob_WaitTime questPart_AddShipJob_WaitTime = new QuestPart_AddShipJob_WaitTime
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.WaitTime,
				leaveImmediatelyWhenSatisfied = leaveImmediatelyWhenSatisfied,
				sendAwayIfAllDespawned = sendAwayIfAllDespawned,
				duration = duration
			};
			quest.AddPart(questPart_AddShipJob_WaitTime);
			return questPart_AddShipJob_WaitTime;
		}

		// Token: 0x060084C0 RID: 33984 RVA: 0x002FB724 File Offset: 0x002F9924
		public static QuestPart_AddShipJob_WaitForever AddShipJob_WaitForever(this Quest quest, TransportShip transportShip, bool leaveImmediatelyWhenSatisfied, bool showGizmos, List<Thing> sendAwayIfAllDespawned = null, string inSignal = null)
		{
			QuestPart_AddShipJob_WaitForever questPart_AddShipJob_WaitForever = new QuestPart_AddShipJob_WaitForever
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.WaitForever,
				leaveImmediatelyWhenSatisfied = leaveImmediatelyWhenSatisfied,
				sendAwayIfAllDespawned = sendAwayIfAllDespawned
			};
			quest.AddPart(questPart_AddShipJob_WaitForever);
			return questPart_AddShipJob_WaitForever;
		}

		// Token: 0x060084C1 RID: 33985 RVA: 0x002FB780 File Offset: 0x002F9980
		public static QuestPart_AddShipJob_WaitSendable AddShipJob_WaitSendable(this Quest quest, TransportShip transportShip, MapParent destination, bool leaveImmeiatelyWhenSatisfied = false, string inSignal = null)
		{
			QuestPart_AddShipJob_WaitSendable questPart_AddShipJob_WaitSendable = new QuestPart_AddShipJob_WaitSendable
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.WaitSendable,
				destination = destination,
				leaveImmediatelyWhenSatisfied = leaveImmeiatelyWhenSatisfied
			};
			quest.AddPart(questPart_AddShipJob_WaitSendable);
			return questPart_AddShipJob_WaitSendable;
		}

		// Token: 0x060084C2 RID: 33986 RVA: 0x002FB7D8 File Offset: 0x002F99D8
		public static QuestPart_AddShipJob_FlyAway AddShipJob_FlyAway(this Quest quest, TransportShip transportShip, int destinationTile = -1, TransportPodsArrivalAction arrivalAction = null, TransportShipDropMode dropMode = TransportShipDropMode.NonRequired, string inSignal = null)
		{
			QuestPart_AddShipJob_FlyAway questPart_AddShipJob_FlyAway = new QuestPart_AddShipJob_FlyAway
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.FlyAway,
				destinationTile = destinationTile,
				arrivalAction = arrivalAction,
				dropMode = dropMode
			};
			quest.AddPart(questPart_AddShipJob_FlyAway);
			return questPart_AddShipJob_FlyAway;
		}
	}
}
