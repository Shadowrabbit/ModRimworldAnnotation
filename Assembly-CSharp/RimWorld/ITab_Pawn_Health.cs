using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B0F RID: 6927
	public class ITab_Pawn_Health : ITab
	{
		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x06009873 RID: 39027 RVA: 0x002CD378 File Offset: 0x002CB578
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

		// Token: 0x06009874 RID: 39028 RVA: 0x00065919 File Offset: 0x00063B19
		public ITab_Pawn_Health()
		{
			this.size = new Vector2(630f, 430f);
			this.labelKey = "TabHealth";
			this.tutorTag = "Health";
		}

		// Token: 0x06009875 RID: 39029 RVA: 0x002CD3AC File Offset: 0x002CB5AC
		protected override void FillTab()
		{
			Pawn pawnForHealth = this.PawnForHealth;
			if (pawnForHealth == null)
			{
				Log.Error("Health tab found no selected pawn to display.", false);
				return;
			}
			Corpse corpse = base.SelThing as Corpse;
			bool showBloodLoss = corpse == null || corpse.Age < 60000;
			HealthCardUtility.DrawPawnHealthCard(new Rect(0f, 20f, this.size.x, this.size.y - 20f), pawnForHealth, this.ShouldAllowOperations(), showBloodLoss, base.SelThing);
		}

		// Token: 0x06009876 RID: 39030 RVA: 0x002CD430 File Offset: 0x002CB630
		private bool ShouldAllowOperations()
		{
			Pawn pawn = this.PawnForHealth;
			return !pawn.Dead && base.SelThing.def.AllRecipes.Any((RecipeDef x) => x.AvailableNow && x.AvailableOnNow(pawn)) && (pawn.Faction == Faction.OfPlayer || (pawn.IsPrisonerOfColony || (pawn.HostFaction == Faction.OfPlayer && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving))) || ((!pawn.RaceProps.IsFlesh || pawn.Faction == null || !pawn.Faction.HostileTo(Faction.OfPlayer)) && (!pawn.RaceProps.Humanlike && pawn.Downed)));
		}

		// Token: 0x04006168 RID: 24936
		private const int HideBloodLossTicksThreshold = 60000;

		// Token: 0x04006169 RID: 24937
		public const float Width = 630f;
	}
}
