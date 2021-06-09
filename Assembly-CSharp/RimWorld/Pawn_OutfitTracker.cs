using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014FD RID: 5373
	public class Pawn_OutfitTracker : IExposable
	{
		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x060073B2 RID: 29618 RVA: 0x0004DF55 File Offset: 0x0004C155
		// (set) Token: 0x060073B3 RID: 29619 RVA: 0x0004DF7A File Offset: 0x0004C17A
		public Outfit CurrentOutfit
		{
			get
			{
				if (this.curOutfit == null)
				{
					this.curOutfit = Current.Game.outfitDatabase.DefaultOutfit();
				}
				return this.curOutfit;
			}
			set
			{
				if (this.curOutfit == value)
				{
					return;
				}
				this.curOutfit = value;
				if (this.pawn.mindState != null)
				{
					this.pawn.mindState.Notify_OutfitChanged();
				}
			}
		}

		// Token: 0x060073B4 RID: 29620 RVA: 0x0004DFAA File Offset: 0x0004C1AA
		public Pawn_OutfitTracker()
		{
		}

		// Token: 0x060073B5 RID: 29621 RVA: 0x0004DFBD File Offset: 0x0004C1BD
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060073B6 RID: 29622 RVA: 0x0004DFD7 File Offset: 0x0004C1D7
		public void ExposeData()
		{
			Scribe_References.Look<Outfit>(ref this.curOutfit, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", Array.Empty<object>());
		}

		// Token: 0x04004C73 RID: 19571
		public Pawn pawn;

		// Token: 0x04004C74 RID: 19572
		private Outfit curOutfit;

		// Token: 0x04004C75 RID: 19573
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();
	}
}
