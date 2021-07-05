using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E60 RID: 3680
	public class Pawn_OutfitTracker : IExposable
	{
		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06005528 RID: 21800 RVA: 0x001CD72D File Offset: 0x001CB92D
		// (set) Token: 0x06005529 RID: 21801 RVA: 0x001CD752 File Offset: 0x001CB952
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

		// Token: 0x0600552A RID: 21802 RVA: 0x001CD782 File Offset: 0x001CB982
		public Pawn_OutfitTracker()
		{
		}

		// Token: 0x0600552B RID: 21803 RVA: 0x001CD795 File Offset: 0x001CB995
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600552C RID: 21804 RVA: 0x001CD7AF File Offset: 0x001CB9AF
		public void ExposeData()
		{
			Scribe_References.Look<Outfit>(ref this.curOutfit, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", Array.Empty<object>());
		}

		// Token: 0x0400327A RID: 12922
		public Pawn pawn;

		// Token: 0x0400327B RID: 12923
		private Outfit curOutfit;

		// Token: 0x0400327C RID: 12924
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();
	}
}
