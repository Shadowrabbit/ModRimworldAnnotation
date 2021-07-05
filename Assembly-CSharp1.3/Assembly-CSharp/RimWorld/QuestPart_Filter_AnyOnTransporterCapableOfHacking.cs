using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B30 RID: 2864
	public class QuestPart_Filter_AnyOnTransporterCapableOfHacking : QuestPart_Filter
	{
		// Token: 0x06004324 RID: 17188 RVA: 0x00166B10 File Offset: 0x00164D10
		protected override bool Pass(SignalArgs args)
		{
			using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)this.transporter.TryGetComp<CompTransporter>().GetDirectlyHeldThings()).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn pawn;
					if ((pawn = (enumerator.Current as Pawn)) != null && HackUtility.IsCapableOfHacking(pawn))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x00166B78 File Offset: 0x00164D78
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.transporter, "transporter", false);
		}

		// Token: 0x040028D7 RID: 10455
		public Thing transporter;
	}
}
