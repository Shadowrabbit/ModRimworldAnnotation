using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B45 RID: 2885
	public class QuestPart_GiveDiedOrDownedThoughts : QuestPart
	{
		// Token: 0x0600437D RID: 17277 RVA: 0x00167DCC File Offset: 0x00165FCC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this.aboutPawn, null, this.thoughtsKind);
			}
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x00167E06 File Offset: 0x00166006
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.aboutPawn, "aboutPawn", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<PawnDiedOrDownedThoughtsKind>(ref this.thoughtsKind, "thoughtsKind", PawnDiedOrDownedThoughtsKind.Died, false);
		}

		// Token: 0x04002905 RID: 10501
		public Pawn aboutPawn;

		// Token: 0x04002906 RID: 10502
		public string inSignal;

		// Token: 0x04002907 RID: 10503
		public PawnDiedOrDownedThoughtsKind thoughtsKind;
	}
}
