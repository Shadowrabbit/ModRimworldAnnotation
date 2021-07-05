using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BB RID: 1723
	public class Inspiration : IExposable
	{
		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002FFB RID: 12283 RVA: 0x0011C8CF File Offset: 0x0011AACF
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x0011C8D7 File Offset: 0x0011AAD7
		public float AgeDays
		{
			get
			{
				return (float)this.age / 60000f;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002FFD RID: 12285 RVA: 0x0011C8E8 File Offset: 0x0011AAE8
		public virtual string InspectLine
		{
			get
			{
				int numTicks = (int)((this.def.baseDurationDays - this.AgeDays) * 60000f);
				return this.def.baseInspectLine + " (" + "ExpiresIn".Translate() + ": " + numTicks.ToStringTicksToPeriod(true, false, true, true) + ")";
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x0011C95C File Offset: 0x0011AB5C
		public virtual string LetterText
		{
			get
			{
				TaggedString taggedString = this.def.beginLetter.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true);
				if (!string.IsNullOrWhiteSpace(this.reason))
				{
					taggedString = this.reason.Formatted(this.pawn.LabelCap, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true) + "\n\n" + taggedString;
				}
				return taggedString;
			}
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x0011CA11 File Offset: 0x0011AC11
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<InspirationDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<string>(ref this.reason, "reason", null, false);
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x0011CA47 File Offset: 0x0011AC47
		public virtual void InspirationTick()
		{
			this.age++;
			if (this.AgeDays >= this.def.baseDurationDays)
			{
				this.End();
			}
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x0011CA70 File Offset: 0x0011AC70
		public virtual void PostStart(bool sendLetter = true)
		{
			if (sendLetter)
			{
				this.SendBeginLetter();
			}
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x0011CA7B File Offset: 0x0011AC7B
		public virtual void PostEnd()
		{
			this.AddEndMessage();
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x0011CA83 File Offset: 0x0011AC83
		protected void End()
		{
			this.pawn.mindState.inspirationHandler.EndInspiration(this);
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x0011CA9C File Offset: 0x0011AC9C
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
			string str = (this.def.beginLetterLabel ?? this.def.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
			Find.LetterStack.ReceiveLetter(str, this.LetterText, this.def.beginLetterDef, this.pawn, null, null, null, null);
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x0011CB3C File Offset: 0x0011AD3C
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

		// Token: 0x04001D20 RID: 7456
		public Pawn pawn;

		// Token: 0x04001D21 RID: 7457
		public InspirationDef def;

		// Token: 0x04001D22 RID: 7458
		private int age;

		// Token: 0x04001D23 RID: 7459
		public string reason;
	}
}
