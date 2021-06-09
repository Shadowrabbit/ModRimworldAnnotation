using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F9 RID: 5881
	public class SignalAction_Letter : SignalAction
	{
		// Token: 0x0600813C RID: 33084 RVA: 0x00056CD4 File Offset: 0x00054ED4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", Array.Empty<object>());
		}

		// Token: 0x0600813D RID: 33085 RVA: 0x00265450 File Offset: 0x00263650
		protected override void DoAction(SignalArgs args)
		{
			Pawn pawn;
			if (args.TryGetArg<Pawn>("SUBJECT", out pawn))
			{
				ChoiceLetter choiceLetter = this.letter as ChoiceLetter;
				if (choiceLetter != null)
				{
					choiceLetter.text = choiceLetter.text.Resolve().Formatted(pawn.LabelShort, pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
				}
				if (!this.letter.lookTargets.IsValid())
				{
					this.letter.lookTargets = pawn;
				}
			}
			Find.LetterStack.ReceiveLetter(this.letter, null);
		}

		// Token: 0x040053C0 RID: 21440
		public Letter letter;
	}
}
