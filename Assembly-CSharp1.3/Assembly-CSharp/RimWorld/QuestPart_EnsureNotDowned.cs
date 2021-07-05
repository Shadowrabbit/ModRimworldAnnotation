using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B75 RID: 2933
	public class QuestPart_EnsureNotDowned : QuestPart
	{
		// Token: 0x0600449C RID: 17564 RVA: 0x0016BEEC File Offset: 0x0016A0EC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				foreach (Pawn p in this.pawns)
				{
					this.EnsureNotDowned(p);
				}
			}
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x0016BF5C File Offset: 0x0016A15C
		protected void EnsureNotDowned(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			int num = 0;
			while (p.Downed && num++ < 15)
			{
				Hediff hediff = hediffs.FirstOrDefault((Hediff h) => h.def.isBad && h.def.everCurableByItem && !(h is Hediff_MissingPart));
				if (hediff == null)
				{
					break;
				}
				p.health.RemoveHediff(hediff);
			}
			if (p.Downed)
			{
				num = 0;
				while (p.Downed && num++ < 15)
				{
					Hediff hediff2 = hediffs.FirstOrDefault((Hediff h) => h.def.isBad && !(h is Hediff_MissingPart));
					if (hediff2 == null)
					{
						break;
					}
					p.health.RemoveHediff(hediff2);
				}
			}
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x0016C018 File Offset: 0x0016A218
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn p) => p == null);
			}
		}

		// Token: 0x040029A1 RID: 10657
		public string inSignal;

		// Token: 0x040029A2 RID: 10658
		public List<Pawn> pawns;
	}
}
