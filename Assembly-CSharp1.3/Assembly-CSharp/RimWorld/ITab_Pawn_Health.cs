using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001349 RID: 4937
	public class ITab_Pawn_Health : ITab
	{
		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x06007790 RID: 30608 RVA: 0x002A1C50 File Offset: 0x0029FE50
		private Pawn PawnForHealth
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return null;
			}
		}

		// Token: 0x06007791 RID: 30609 RVA: 0x002A1C83 File Offset: 0x0029FE83
		public ITab_Pawn_Health()
		{
			this.size = new Vector2(630f, 430f);
			this.labelKey = "TabHealth";
			this.tutorTag = "Health";
		}

		// Token: 0x06007792 RID: 30610 RVA: 0x002A1CB8 File Offset: 0x0029FEB8
		protected override void FillTab()
		{
			Pawn pawnForHealth = this.PawnForHealth;
			if (pawnForHealth == null)
			{
				Log.Error("Health tab found no selected pawn to display.");
				return;
			}
			Corpse corpse = base.SelThing as Corpse;
			bool showBloodLoss = corpse == null || corpse.Age < 60000;
			HealthCardUtility.DrawPawnHealthCard(new Rect(0f, 20f, this.size.x, this.size.y - 20f), pawnForHealth, this.ShouldAllowOperations(), showBloodLoss, base.SelThing);
		}

		// Token: 0x06007793 RID: 30611 RVA: 0x002A1D38 File Offset: 0x0029FF38
		private bool ShouldAllowOperations()
		{
			Pawn pawn = this.PawnForHealth;
			return !pawn.Dead && base.SelThing.def.AllRecipes.Any((RecipeDef x) => x.AvailableNow && x.AvailableOnNow(pawn, null)) && (pawn.Faction == Faction.OfPlayer || (pawn.IsPrisonerOfColony || (pawn.HostFaction == Faction.OfPlayer && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving))) || ((!pawn.RaceProps.IsFlesh || pawn.Faction == null || !pawn.Faction.HostileTo(Faction.OfPlayer)) && (!pawn.RaceProps.Humanlike && pawn.Downed)));
		}

		// Token: 0x04004275 RID: 17013
		private const int HideBloodLossTicksThreshold = 60000;

		// Token: 0x04004276 RID: 17014
		public const float Width = 630f;
	}
}
