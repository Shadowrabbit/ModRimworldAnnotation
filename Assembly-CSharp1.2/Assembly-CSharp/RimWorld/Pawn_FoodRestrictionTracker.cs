using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D2 RID: 5330
	public class Pawn_FoodRestrictionTracker : IExposable
	{
		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x060072CB RID: 29387 RVA: 0x0004D360 File Offset: 0x0004B560
		// (set) Token: 0x060072CC RID: 29388 RVA: 0x0004D385 File Offset: 0x0004B585
		public FoodRestriction CurrentFoodRestriction
		{
			get
			{
				if (this.curRestriction == null)
				{
					this.curRestriction = Current.Game.foodRestrictionDatabase.DefaultFoodRestriction();
				}
				return this.curRestriction;
			}
			set
			{
				this.curRestriction = value;
			}
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x060072CD RID: 29389 RVA: 0x00230B34 File Offset: 0x0022ED34
		public bool Configurable
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && !this.pawn.Destroyed && (this.pawn.Faction == Faction.OfPlayer || this.pawn.HostFaction == Faction.OfPlayer);
			}
		}

		// Token: 0x060072CE RID: 29390 RVA: 0x0004D38E File Offset: 0x0004B58E
		public Pawn_FoodRestrictionTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060072CF RID: 29391 RVA: 0x00006B8B File Offset: 0x00004D8B
		public Pawn_FoodRestrictionTracker()
		{
		}

		// Token: 0x060072D0 RID: 29392 RVA: 0x00230B88 File Offset: 0x0022ED88
		public FoodRestriction GetCurrentRespectedRestriction(Pawn getter = null)
		{
			if (!this.Configurable)
			{
				return null;
			}
			if (this.pawn.Faction != Faction.OfPlayer && (getter == null || getter.Faction != Faction.OfPlayer))
			{
				return null;
			}
			if (this.pawn.InMentalState)
			{
				return null;
			}
			return this.CurrentFoodRestriction;
		}

		// Token: 0x060072D1 RID: 29393 RVA: 0x0004D39D File Offset: 0x0004B59D
		public void ExposeData()
		{
			Scribe_References.Look<FoodRestriction>(ref this.curRestriction, "curRestriction", false);
		}

		// Token: 0x04004B93 RID: 19347
		public Pawn pawn;

		// Token: 0x04004B94 RID: 19348
		private FoodRestriction curRestriction;
	}
}
