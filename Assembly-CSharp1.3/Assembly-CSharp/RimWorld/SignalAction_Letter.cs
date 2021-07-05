using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200109E RID: 4254
	public class SignalAction_Letter : SignalAction
	{
		// Token: 0x0600656A RID: 25962 RVA: 0x00224092 File Offset: 0x00222292
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", Array.Empty<object>());
		}

		// Token: 0x0600656B RID: 25963 RVA: 0x002240B0 File Offset: 0x002222B0
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

		// Token: 0x04003920 RID: 14624
		public Letter letter;
	}
}
