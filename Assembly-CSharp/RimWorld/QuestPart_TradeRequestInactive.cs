using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001080 RID: 4224
	public class QuestPart_TradeRequestInactive : QuestPartActivable
	{
		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06005C01 RID: 23553 RVA: 0x0003FD36 File Offset: 0x0003DF36
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

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06005C02 RID: 23554 RVA: 0x0003FD46 File Offset: 0x0003DF46
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

		// Token: 0x06005C03 RID: 23555 RVA: 0x001D9814 File Offset: 0x001D7A14
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.settlement == null || !this.settlement.Spawned)
			{
				base.Complete();
				return;
			}
			TradeRequestComp component = this.settlement.GetComponent<TradeRequestComp>();
			if (component == null || !component.ActiveRequest)
			{
				base.Complete(this.settlement.Named("SUBJECT"));
			}
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x0003FD56 File Offset: 0x0003DF56
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x0003FD6F File Offset: 0x0003DF6F
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.settlement = Find.WorldObjects.Settlements.FirstOrDefault<Settlement>();
		}

		// Token: 0x04003DAB RID: 15787
		public Settlement settlement;
	}
}
