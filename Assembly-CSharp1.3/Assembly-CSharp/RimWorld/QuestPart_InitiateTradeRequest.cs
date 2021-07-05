using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7E RID: 2942
	public class QuestPart_InitiateTradeRequest : QuestPart
	{
		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x060044C4 RID: 17604 RVA: 0x0016C665 File Offset: 0x0016A865
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.settlement != null)
				{
					yield return this.settlement;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x060044C5 RID: 17605 RVA: 0x0016C675 File Offset: 0x0016A875
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.settlement.Faction != null)
				{
					yield return this.settlement.Faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x060044C6 RID: 17606 RVA: 0x0016C685 File Offset: 0x0016A885
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.requestedThingDef, -1);
				yield break;
				yield break;
			}
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x0016C698 File Offset: 0x0016A898
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null)
				{
					if (component.ActiveRequest)
					{
						Log.Error("Settlement " + this.settlement.Label + " already has an active trade request.");
						return;
					}
					component.requestThingDef = this.requestedThingDef;
					component.requestCount = this.requestedCount;
					component.expiration = Find.TickManager.TicksGame + this.requestDuration;
				}
			}
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x0016C728 File Offset: 0x0016A928
		public override void Cleanup()
		{
			base.Cleanup();
			if (!this.keepAfterQuestEnds)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null && component.ActiveRequest)
				{
					component.Disable();
				}
			}
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x0016C760 File Offset: 0x0016A960
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<ThingDef>(ref this.requestedThingDef, "requestedThingDef");
			Scribe_Values.Look<int>(ref this.requestedCount, "requestedCount", 0, false);
			Scribe_Values.Look<int>(ref this.requestDuration, "requestDuration", 0, false);
			Scribe_Values.Look<bool>(ref this.keepAfterQuestEnds, "keepAfterQuestEnds", false, false);
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x0016C7DC File Offset: 0x0016A9DC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.settlement = Find.WorldObjects.Settlements.Where(delegate(Settlement x)
			{
				TradeRequestComp component = x.GetComponent<TradeRequestComp>();
				return component != null && !component.ActiveRequest && x.Faction != Faction.OfPlayer;
			}).RandomElementWithFallback(null);
			if (this.settlement == null)
			{
				this.settlement = Find.WorldObjects.Settlements.RandomElementWithFallback(null);
			}
			this.requestedThingDef = ThingDefOf.Silver;
			this.requestedCount = 100;
			this.requestDuration = 60000;
		}

		// Token: 0x040029BA RID: 10682
		public string inSignal;

		// Token: 0x040029BB RID: 10683
		public Settlement settlement;

		// Token: 0x040029BC RID: 10684
		public ThingDef requestedThingDef;

		// Token: 0x040029BD RID: 10685
		public int requestedCount;

		// Token: 0x040029BE RID: 10686
		public int requestDuration;

		// Token: 0x040029BF RID: 10687
		public bool keepAfterQuestEnds;
	}
}
