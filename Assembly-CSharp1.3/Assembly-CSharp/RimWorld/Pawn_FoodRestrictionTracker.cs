using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E30 RID: 3632
	public class Pawn_FoodRestrictionTracker : IExposable
	{
		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06005402 RID: 21506 RVA: 0x001C6DC7 File Offset: 0x001C4FC7
		// (set) Token: 0x06005403 RID: 21507 RVA: 0x001C6DEC File Offset: 0x001C4FEC
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

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06005404 RID: 21508 RVA: 0x001C6DF8 File Offset: 0x001C4FF8
		public bool Configurable
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && !this.pawn.Destroyed && (this.pawn.Faction == Faction.OfPlayer || this.pawn.HostFaction == Faction.OfPlayer);
			}
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x001C6E4C File Offset: 0x001C504C
		public Pawn_FoodRestrictionTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x000033AC File Offset: 0x000015AC
		public Pawn_FoodRestrictionTracker()
		{
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x001C6E5C File Offset: 0x001C505C
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

		// Token: 0x06005408 RID: 21512 RVA: 0x001C6EAC File Offset: 0x001C50AC
		public void ExposeData()
		{
			Scribe_References.Look<FoodRestriction>(ref this.curRestriction, "curRestriction", false);
		}

		// Token: 0x0400316F RID: 12655
		public Pawn pawn;

		// Token: 0x04003170 RID: 12656
		private FoodRestriction curRestriction;
	}
}
