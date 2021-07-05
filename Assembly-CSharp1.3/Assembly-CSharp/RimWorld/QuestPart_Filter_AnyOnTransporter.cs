using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B2F RID: 2863
	public class QuestPart_Filter_AnyOnTransporter : QuestPart_Filter
	{
		// Token: 0x06004321 RID: 17185 RVA: 0x00166A24 File Offset: 0x00164C24
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

		// Token: 0x06004322 RID: 17186 RVA: 0x00166AA0 File Offset: 0x00164CA0
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

		// Token: 0x040028D5 RID: 10453
		public Thing transporter;

		// Token: 0x040028D6 RID: 10454
		public List<Pawn> pawns;
	}
}
