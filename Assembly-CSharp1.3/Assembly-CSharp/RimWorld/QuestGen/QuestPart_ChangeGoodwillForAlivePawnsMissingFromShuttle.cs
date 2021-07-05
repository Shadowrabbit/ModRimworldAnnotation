using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016B4 RID: 5812
	public class QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle : QuestPart
	{
		// Token: 0x060086CE RID: 34510 RVA: 0x003048FC File Offset: 0x00302AFC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			IEnumerable<Thing> sentThings;
			if (signal.tag == this.inSignal && signal.args.TryGetArg<IEnumerable<Thing>>("SENT", out sentThings))
			{
				this.DoWork(sentThings);
			}
		}

		// Token: 0x060086CF RID: 34511 RVA: 0x00304938 File Offset: 0x00302B38
		private void DoWork(IEnumerable<Thing> sentThings)
		{
			int num = 0;
			using (IEnumerator<Thing> enumerator = sentThings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Pawn)
					{
						num++;
					}
				}
			}
			int num2 = 0;
			using (List<Pawn>.Enumerator enumerator2 = this.pawns.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (!enumerator2.Current.Dead)
					{
						num2++;
					}
				}
			}
			if (num < num2)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, this.goodwillChange * (num - num2), true, true, this.historyEvent, null);
			}
		}

		// Token: 0x060086D0 RID: 34512 RVA: 0x00304A00 File Offset: 0x00302C00
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.goodwillChange, "goodwillChange", 0, false);
			Scribe_Deep.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04005499 RID: 21657
		public string inSignal;

		// Token: 0x0400549A RID: 21658
		public List<Pawn> pawns;

		// Token: 0x0400549B RID: 21659
		public Faction faction;

		// Token: 0x0400549C RID: 21660
		public int goodwillChange;

		// Token: 0x0400549D RID: 21661
		public HistoryEventDef historyEvent;
	}
}
