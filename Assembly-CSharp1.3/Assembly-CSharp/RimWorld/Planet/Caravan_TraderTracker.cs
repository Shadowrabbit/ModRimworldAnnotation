using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017C5 RID: 6085
	public class Caravan_TraderTracker : IExposable
	{
		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x06008D44 RID: 36164 RVA: 0x0032D650 File Offset: 0x0032B850
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

		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x06008D45 RID: 36165 RVA: 0x0032D6A0 File Offset: 0x0032B8A0
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

		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x06008D46 RID: 36166 RVA: 0x0032D6B0 File Offset: 0x0032B8B0
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.caravan.ID, 1048142365);
			}
		}

		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x06008D47 RID: 36167 RVA: 0x0032D6C7 File Offset: 0x0032B8C7
		public string TraderName
		{
			get
			{
				return this.caravan.LabelCap;
			}
		}

		// Token: 0x170016FE RID: 5886
		// (get) Token: 0x06008D48 RID: 36168 RVA: 0x0032D6D4 File Offset: 0x0032B8D4
		public bool CanTradeNow
		{
			get
			{
				return this.TraderKind != null && !this.caravan.AllOwnersDowned && this.caravan.Faction != Faction.OfPlayer && this.Goods.Any((Thing x) => this.TraderKind.WillTrade(x.def));
			}
		}

		// Token: 0x06008D49 RID: 36169 RVA: 0x0032D721 File Offset: 0x0032B921
		public Caravan_TraderTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06008D4A RID: 36170 RVA: 0x0032D73C File Offset: 0x0032B93C
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06008D4B RID: 36171 RVA: 0x0032D792 File Offset: 0x0032B992
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

		// Token: 0x06008D4C RID: 36172 RVA: 0x0032D7A4 File Offset: 0x0032B9A4
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + toGive + " to stock (pawn's trader tracker), but it's already here.");
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
					Log.Error("Could not find pawn to move sold thing to (sold by player). thing=" + thing);
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
				if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
				{
					Log.Error("Could not add item to inventory.");
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06008D4D RID: 36173 RVA: 0x0032D8A4 File Offset: 0x0032BAA4
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
				Log.Error("Could not find pawn to move bought thing to (bought by player). thing=" + thing);
				thing.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!pawn2.inventory.innerContainer.TryAdd(thing, true))
			{
				Log.Error("Could not add item to inventory.");
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04005984 RID: 22916
		private Caravan caravan;

		// Token: 0x04005985 RID: 22917
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
