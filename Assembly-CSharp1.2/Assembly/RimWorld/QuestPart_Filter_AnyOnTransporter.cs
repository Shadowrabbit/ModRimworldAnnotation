using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105B RID: 4187
	public class QuestPart_Filter_AnyOnTransporter : QuestPart_Filter
	{
		// Token: 0x06005B2C RID: 23340 RVA: 0x001D78B0 File Offset: 0x001D5AB0
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			ThingOwner directlyHeldThings = this.transporter.TryGetComp<CompTransporter>().GetDirectlyHeldThings();
			foreach (Pawn item in this.pawns)
			{
				if (directlyHeldThings.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x001D792C File Offset: 0x001D5B2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.transporter, "transporter", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04003D38 RID: 15672
		public Thing transporter;

		// Token: 0x04003D39 RID: 15673
		public List<Pawn> pawns;
	}
}
