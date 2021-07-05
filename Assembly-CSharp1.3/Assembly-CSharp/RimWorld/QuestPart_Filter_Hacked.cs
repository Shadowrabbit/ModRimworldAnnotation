using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B3B RID: 2875
	public class QuestPart_Filter_Hacked : QuestPart_Filter
	{
		// Token: 0x06004349 RID: 17225 RVA: 0x001671E8 File Offset: 0x001653E8
		protected override bool Pass(SignalArgs args)
		{
			Thing thing;
			if (args.TryGetArg<Thing>("SUBJECT", out thing))
			{
				CompHackable compHackable = thing.TryGetComp<CompHackable>();
				if (compHackable != null)
				{
					return compHackable.IsHacked;
				}
			}
			return false;
		}
	}
}
