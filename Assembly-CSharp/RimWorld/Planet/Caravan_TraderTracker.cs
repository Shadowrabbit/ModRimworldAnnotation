using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002125 RID: 8485
	public class Caravan_TraderTracker : IExposable
	{
		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x0600B434 RID: 46132 RVA: 0x00344E10 File Offset: 0x00343010
		public TraderKindDef TraderKind
		{
			get
			{
				List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					Pawn pawn = pawnsListForReading[i];
					if (this.caravan.IsOwner(pawn) && pawn.TraderKind != null)
					{
						return pawn.TraderKind;
					}
				}
				return null;
			}
		}

		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x0600B435 RID: 46133 RVA: 0x000750D5 File Offset: 0x000732D5
		public IEnumerable<Thing> Goods
		{
			get
			{
				List<Thing> inv = CaravanInventoryUtility.AllInventoryItems(this.caravan);
				int num;
				for (int i = 0; i < inv.Count; i = num + 1)
				{
					yield return inv[i];
					num = i;
				}
				List<Pawn> pawns = this.caravan.PawnsListForReading;
				for (int i = 0; i < pawns.Count; i = num + 1)
				{
					Pawn pawn = pawns[i];
					if (!this.caravan.IsOwner(pawn) && (!pawn.RaceProps.packAnimal || pawn.inventory == null || pawn.inventory.innerContainer.Count <= 0) && !this.soldPrisoners.Contains(pawn))
					{
						yield return pawn;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x0600B436 RID: 46134 RVA: 0x000750E5 File Offset: 0x000732E5
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.caravan.ID, 1048142365);
			}
		}

		// Token: 0x17001A85 RID: 6789
		// (get) Token: 0x0600B437 RID: 46135 RVA: 0x000750FC File Offset: 0x000732FC
		public string TraderName
		{
			get
			{
				return this.caravan.LabelCap;
			}
		}

		// Token: 0x17001A86 RID: 6790
		// (get) Token: 0x0600B438 RID: 46136 RVA: 0x00344E60 File Offset: 0x00343060
		public bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && !this.caravan.AllOwnersDowned && this.caravan.Faction != Faction.OfPlayer && this.Goods.Any((Thing x) => this.TraderKind.WillTrade(x.def));
			}
		}

		// Token: 0x0600B439 RID: 46137 RVA: 0x00075109 File Offset: 0x00073309
		public Caravan_TraderTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600B43A RID: 46138 RVA: 0x00344EB0 File Offset: 0x003430B0
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600B43B RID: 46139 RVA: 0x00075123 File Offset: 0x00073323
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Caravan playerCaravan = playerNegotiator.GetCaravan();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(playerCaravan))
			{
				yield return thing;
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			List<Pawn> pawns = playerCaravan.PawnsListForReading;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (!playerCaravan.IsOwner(pawns[i]))
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B43C RID: 46140 RVA: 0x00344F08 File Offset: 0x00343108
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + toGive + " to stock (pawn's trader tracker), but it's already here.", false);
				return;
			}
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.caravan);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				this.caravan.AddPawn(pawn, false);
				if (pawn.IsWorldPawn() && !this.caravan.Spawned)
				{
					Find.WorldPawns.RemovePawn(pawn);
				}
				if (pawn.RaceProps.Humanlike)
				{
					this.soldPrisoners.Add(pawn);
					return;
				}
			}
			else
			{
				Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, this.caravan.PawnsListForReading, null, null);
				if (pawn2 == null)
				{
					Log.Error("Could not find pawn to move sold thing to (sold by player). thing=" + thing, false);
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
				if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
				{
					Log.Error("Could not add item to inventory.", false);
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600B43D RID: 46141 RVA: 0x0034500C File Offset: 0x0034320C
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Caravan caravan = playerNegotiator.GetCaravan();
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.caravan);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, this.caravan.PawnsListForReading, null);
				caravan.AddPawn(pawn, true);
				if (!pawn.IsWorldPawn() && caravan.Spawned)
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				}
				this.soldPrisoners.Remove(pawn);
				return;
			}
			Pawn pawn2 = CaravanInventoryUtility.FindPawnToMoveInventoryTo(thing, caravan.PawnsListForReading, null, null);
			if (pawn2 == null)
			{
				Log.Error("Could not find pawn to move bought thing to (bought by player). thing=" + thing, false);
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error("Could not add item to inventory.", false);
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04007BC3 RID: 31683
		private Caravan caravan;

		// Token: 0x04007BC4 RID: 31684
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
