using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B29 RID: 2857
	public class QuestPart_Filter_AllRequiredThingsLoaded : QuestPart_Filter
	{
		// Token: 0x06004310 RID: 17168 RVA: 0x001667AC File Offset: 0x001649AC
		protected override bool Pass(SignalArgs args)
		{
			if (this.shuttle == null)
			{
				return false;
			}
			CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
			return compShuttle != null && compShuttle.AllRequiredThingsLoaded;
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x001667DA File Offset: 0x001649DA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
		}

		// Token: 0x040028CF RID: 10447
		public Thing shuttle;
	}
}
