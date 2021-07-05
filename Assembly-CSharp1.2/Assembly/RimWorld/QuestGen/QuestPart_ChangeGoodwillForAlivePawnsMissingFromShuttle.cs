using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F86 RID: 8070
	public class QuestPart_ChangeGoodwillForAlivePawnsMissingFromShuttle : QuestPart
	{
		// Token: 0x0600ABCA RID: 43978 RVA: 0x003200BC File Offset: 0x0031E2BC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			IEnumerable<Thing> sentThings;
			if (signal.tag == this.inSignal && signal.args.TryGetArg<IEnumerable<Thing>>("SENT", out sentThings))
			{
				this.DoWork(sentThings);
			}
		}

		// Token: 0x0600ABCB RID: 43979 RVA: 0x003200F8 File Offset: 0x0031E2F8
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
				this.faction.TryAffectGoodwillWith(Faction.OfPlayer, this.goodwillChange * (num2 - num), true, true, this.reason, null);
			}
		}

		// Token: 0x0600ABCC RID: 43980 RVA: 0x003201C0 File Offset: 0x0031E3C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.goodwillChange, "goodwillChange", 0, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04007515 RID: 29973
		public string inSignal;

		// Token: 0x04007516 RID: 29974
		public List<Pawn> pawns;

		// Token: 0x04007517 RID: 29975
		public Faction faction;

		// Token: 0x04007518 RID: 29976
		public int goodwillChange;

		// Token: 0x04007519 RID: 29977
		public string reason;
	}
}
