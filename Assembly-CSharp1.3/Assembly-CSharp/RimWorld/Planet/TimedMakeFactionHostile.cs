using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017FA RID: 6138
	public class TimedMakeFactionHostile : WorldObjectComp
	{
		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x06008F43 RID: 36675 RVA: 0x00335330 File Offset: 0x00333530
		public int? TicksLeft
		{
			get
			{
				if (this.ticksLeftMakeFactionHostile != -1)
				{
					return new int?(this.ticksLeftMakeFactionHostile);
				}
				return null;
			}
		}

		// Token: 0x06008F44 RID: 36676 RVA: 0x0033535B File Offset: 0x0033355B
		public void SetupTimer(int ticks, string message = null, HistoryEventDef reason = null)
		{
			this.timerMakeFactionHostile = ticks;
			this.ticksLeftMakeFactionHostile = -1;
			this.messageBecameHostile = message;
			this.reasonBecameHostile = reason;
		}

		// Token: 0x06008F45 RID: 36677 RVA: 0x00335379 File Offset: 0x00333579
		public override void PostMyMapRemoved()
		{
			this.ticksLeftMakeFactionHostile = -1;
		}

		// Token: 0x06008F46 RID: 36678 RVA: 0x00335382 File Offset: 0x00333582
		public override void PostMapGenerate()
		{
			this.ticksLeftMakeFactionHostile = this.timerMakeFactionHostile;
		}

		// Token: 0x06008F47 RID: 36679 RVA: 0x00335390 File Offset: 0x00333590
		public override void CompTick()
		{
			if (this.ticksLeftMakeFactionHostile != -1 && this.parent.Faction.HostileTo(Faction.OfPlayer))
			{
				this.ticksLeftMakeFactionHostile = -1;
			}
			if (this.ticksLeftMakeFactionHostile != -1)
			{
				this.ticksLeftMakeFactionHostile--;
				if (this.ticksLeftMakeFactionHostile == 0)
				{
					if (this.parent.Faction.temporary)
					{
						this.parent.Faction.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
					}
					else
					{
						this.parent.Faction.TryAffectGoodwillWith(Faction.OfPlayer, this.parent.Faction.GoodwillToMakeHostile(Faction.OfPlayer), true, true, this.reasonBecameHostile, null);
					}
					if (this.messageBecameHostile != null)
					{
						Messages.Message(this.messageBecameHostile, MessageTypeDefOf.NegativeEvent, true);
					}
					this.ticksLeftMakeFactionHostile = -1;
				}
			}
		}

		// Token: 0x06008F48 RID: 36680 RVA: 0x0033547C File Offset: 0x0033367C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timerMakeFactionHostile, "timerMakeFactionHostile", -1, false);
			Scribe_Values.Look<int>(ref this.ticksLeftMakeFactionHostile, "ticksLeftMakeFactionHostile", -1, false);
			Scribe_Values.Look<string>(ref this.messageBecameHostile, "messageBecameHostile", null, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.reasonBecameHostile, "reasonBecameHostile");
		}

		// Token: 0x04005A18 RID: 23064
		private int timerMakeFactionHostile = -1;

		// Token: 0x04005A19 RID: 23065
		private int ticksLeftMakeFactionHostile = -1;

		// Token: 0x04005A1A RID: 23066
		private string messageBecameHostile;

		// Token: 0x04005A1B RID: 23067
		public HistoryEventDef reasonBecameHostile;
	}
}
