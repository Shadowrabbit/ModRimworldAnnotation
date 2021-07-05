using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE4 RID: 3812
	public class PreceptComp_UnwillingToDo_Gendered : PreceptComp_UnwillingToDo
	{
		// Token: 0x06005A95 RID: 23189 RVA: 0x001F5544 File Offset: 0x001F3744
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			Pawn pawn;
			return !ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn) || pawn.gender != this.gender || base.MemberWillingToDo(ev);
		}

		// Token: 0x06005A96 RID: 23190 RVA: 0x001F557D File Offset: 0x001F377D
		public override string GetProhibitionText()
		{
			if (!this.displayDescription)
			{
				return null;
			}
			return base.GetProhibitionText() + " (" + this.gender.GetLabel(false) + ")";
		}

		// Token: 0x04003509 RID: 13577
		public Gender gender;

		// Token: 0x0400350A RID: 13578
		public bool displayDescription = true;
	}
}
