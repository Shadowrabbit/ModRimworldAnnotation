using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE5 RID: 3813
	public class PreceptComp_UnwillingToDo_Chance : PreceptComp_UnwillingToDo
	{
		// Token: 0x06005A98 RID: 23192 RVA: 0x001F55B9 File Offset: 0x001F37B9
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			return Rand.Value >= this.chance || base.MemberWillingToDo(ev);
		}

		// Token: 0x06005A99 RID: 23193 RVA: 0x00002688 File Offset: 0x00000888
		public override string GetProhibitionText()
		{
			return null;
		}

		// Token: 0x0400350B RID: 13579
		public float chance;
	}
}
