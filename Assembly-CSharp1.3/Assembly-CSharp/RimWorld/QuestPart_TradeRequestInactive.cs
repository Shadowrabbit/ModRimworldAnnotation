using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4F RID: 2895
	public class QuestPart_TradeRequestInactive : QuestPartActivable
	{
		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060043C1 RID: 17345 RVA: 0x00168BE2 File Offset: 0x00166DE2
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

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060043C2 RID: 17346 RVA: 0x00168BF2 File Offset: 0x00166DF2
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

		// Token: 0x060043C3 RID: 17347 RVA: 0x00168C04 File Offset: 0x00166E04
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

		// Token: 0x060043C4 RID: 17348 RVA: 0x00168C60 File Offset: 0x00166E60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x00168C79 File Offset: 0x00166E79
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.settlement = Find.WorldObjects.Settlements.FirstOrDefault<Settlement>();
		}

		// Token: 0x04002922 RID: 10530
		public Settlement settlement;
	}
}
