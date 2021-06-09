using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B25 RID: 2853
	public class Inspiration : IExposable
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x060042E6 RID: 17126 RVA: 0x00031AE3 File Offset: 0x0002FCE3
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x060042E7 RID: 17127 RVA: 0x00031AEB File Offset: 0x0002FCEB
		public float AgeDays
		{
			get
			{
				return (float)this.age / 60000f;
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x060042E8 RID: 17128 RVA: 0x0018CBD0 File Offset: 0x0018ADD0
		public virtual string InspectLine
		{
			get
			{
				int numTicks = (int)((this.def.baseDurationDays - this.AgeDays) * 60000f);
				return this.def.baseInspectLine + " (" + "ExpiresIn".Translate() + ": " + numTicks.ToStringTicksToPeriod(true, false, true, true) + ")";
			}
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x00031AFA File Offset: 0x0002FCFA
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<InspirationDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x00031B30 File Offset: 0x0002FD30
		public virtual void InspirationTick()
		{
			this.age++;
			if (this.AgeDays >= this.def.baseDurationDays)
			{
				this.End();
			}
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x00031B59 File Offset: 0x0002FD59
		public virtual void PostStart()
		{
			this.SendBeginLetter();
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x00031B61 File Offset: 0x0002FD61
		public virtual void PostEnd()
		{
			this.AddEndMessage();
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x00031B69 File Offset: 0x0002FD69
		protected void End()
		{
			this.pawn.mindState.inspirationHandler.EndInspiration(this);
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x0018CC44 File Offset: 0x0018AE44
		protected virtual void SendBeginLetter()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return;
			}
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				return;
			}
			TaggedString taggedString = this.def.beginLetter.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
			if (!string.IsNullOrWhiteSpace(this.reason))
			{
				taggedString = this.reason.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true) + "\n\n" + taggedString;
			}
			string str = (this.def.beginLetterLabel ?? this.def.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
			Find.LetterStack.ReceiveLetter(str, taggedString, this.def.beginLetterDef, this.pawn, null, null, null, null);
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x0018CD7C File Offset: 0x0018AF7C
		protected virtual void AddEndMessage()
		{
			if (this.def.endMessage.NullOrEmpty())
			{
				return;
			}
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				return;
			}
			Messages.Message(this.def.endMessage.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x04002DCD RID: 11725
		public Pawn pawn;

		// Token: 0x04002DCE RID: 11726
		public InspirationDef def;

		// Token: 0x04002DCF RID: 11727
		private int age;

		// Token: 0x04002DD0 RID: 11728
		public string reason;
	}
}
