using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B72 RID: 2930
	public class QuestPart_DisableTradeRequest : QuestPart
	{
		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06004489 RID: 17545 RVA: 0x0016BA4F File Offset: 0x00169C4F
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

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x0600448A RID: 17546 RVA: 0x0016BA5F File Offset: 0x00169C5F
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

		// Token: 0x0600448B RID: 17547 RVA: 0x0016BA70 File Offset: 0x00169C70
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
				if (component != null && component.ActiveRequest)
				{
					component.Disable();
				}
			}
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0016BAB4 File Offset: 0x00169CB4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0016BAE0 File Offset: 0x00169CE0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.settlement = Find.WorldObjects.Settlements.Where(delegate(Settlement x)
			{
				TradeRequestComp component = x.GetComponent<TradeRequestComp>();
				return component != null && component.ActiveRequest;
			}).RandomElementWithFallback(null);
			if (this.settlement == null)
			{
				this.settlement = Find.WorldObjects.Settlements.RandomElementWithFallback(null);
			}
		}

		// Token: 0x04002997 RID: 10647
		public string inSignal;

		// Token: 0x04002998 RID: 10648
		public Settlement settlement;
	}
}
