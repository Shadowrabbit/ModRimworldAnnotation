using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E7B RID: 3707
	public class Pawn_TraderTracker : IExposable
	{
		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x060056BB RID: 22203 RVA: 0x001D6D3D File Offset: 0x001D4F3D
		public IEnumerable<Thing> Goods
		{
			get
			{
				Lord lord = this.pawn.GetLord();
				if (lord == null || !(lord.LordJob is LordJob_TradeWithColony))
				{
					int num;
					for (int i = 0; i < this.pawn.inventory.innerContainer.Count; i = num + 1)
					{
						Thing thing = this.pawn.inventory.innerContainer[i];
						if (!this.pawn.inventory.NotForSale(thing))
						{
							yield return thing;
						}
						num = i;
					}
				}
				if (lord != null)
				{
					int num;
					for (int i = 0; i < lord.ownedPawns.Count; i = num + 1)
					{
						Pawn p = lord.ownedPawns[i];
						TraderCaravanRole traderCaravanRole = p.GetTraderCaravanRole();
						if (traderCaravanRole == TraderCaravanRole.Carrier)
						{
							for (int j = 0; j < p.inventory.innerContainer.Count; j = num + 1)
							{
								yield return p.inventory.innerContainer[j];
								num = j;
							}
						}
						else if (traderCaravanRole == TraderCaravanRole.Chattel && !this.soldPrisoners.Contains(p))
						{
							yield return p;
						}
						p = null;
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x060056BC RID: 22204 RVA: 0x001D6D4D File Offset: 0x001D4F4D
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.pawn.thingIDNumber, 1149275593);
			}
		}

		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x060056BD RID: 22205 RVA: 0x001D6D64 File Offset: 0x001D4F64
		public string TraderName
		{
			get
			{
				return this.pawn.LabelShort;
			}
		}

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x060056BE RID: 22206 RVA: 0x001D6D74 File Offset: 0x001D4F74
		public bool CanTradeNow
		{
			get
			{
				return !this.pawn.Dead && this.pawn.Spawned && this.pawn.mindState.wantsToTradeWithColony && this.pawn.CanCasuallyInteractNow(false, false) && !this.pawn.Downed && !this.pawn.IsPrisoner && this.pawn.Faction != Faction.OfPlayer && (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer)) && (this.Goods.Any((Thing x) => this.traderKind.WillTrade(x.def)) || this.traderKind.tradeCurrency == TradeCurrency.Favor);
			}
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x001D6E40 File Offset: 0x001D5040
		public Pawn_TraderTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x001D6E5C File Offset: 0x001D505C
		public void ExposeData()
		{
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x001D6EC2 File Offset: 0x001D50C2
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			IEnumerable<Thing> enumerable = from x in this.pawn.Map.listerThings.AllThings
			where x.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(x, this.pawn) && !x.Position.Fogged(x.Map) && (this.pawn.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && this.ReachableForTrade(x)
			select x;
			foreach (Thing thing in enumerable)
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			if (this.pawn.GetLord() != null)
			{
				foreach (Pawn pawn in from x in TradeUtility.AllSellableColonyPawns(this.pawn.Map, true)
				where !x.Downed && this.ReachableForTrade(x)
				select x)
				{
					yield return pawn;
				}
				IEnumerator<Pawn> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060056C2 RID: 22210 RVA: 0x001D6ED4 File Offset: 0x001D50D4
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + toGive + " to stock (pawn's trader tracker), but it's already here.");
				return;
			}
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
				this.AddPawnToStock(pawn);
				return;
			}
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
			Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this.pawn, thing);
			if (thing2 != null)
			{
				if (!thing2.TryAbsorbStack(thing, false))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				this.AddThingToRandomInventory(thing);
			}
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x001D6F64 File Offset: 0x001D5164
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				Lord lord = pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.Undefined, null);
				}
				if (this.soldPrisoners.Contains(pawn))
				{
					this.soldPrisoners.Remove(pawn);
					return;
				}
			}
			else
			{
				IntVec3 positionHeld = toGive.PositionHeld;
				Map mapHeld = toGive.MapHeld;
				Thing thing = toGive.SplitOff(countToGive);
				thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				if (GenPlace.TryPlaceThing(thing, positionHeld, mapHeld, ThingPlaceMode.Near, null, null, default(Rot4)))
				{
					Lord lord2 = this.pawn.GetLord();
					if (lord2 != null)
					{
						lord2.extraForbiddenThings.Add(thing);
						return;
					}
				}
				else
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not place bought thing ",
						thing,
						" at ",
						positionHeld
					}));
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x060056C4 RID: 22212 RVA: 0x001D705C File Offset: 0x001D525C
		private void AddPawnToStock(Pawn newPawn)
		{
			if (!newPawn.Spawned)
			{
				GenSpawn.Spawn(newPawn, this.pawn.Position, this.pawn.Map, WipeMode.Vanish);
			}
			if (newPawn.Faction != this.pawn.Faction)
			{
				newPawn.SetFaction(this.pawn.Faction, null);
			}
			if (newPawn.RaceProps.Humanlike)
			{
				newPawn.kindDef = PawnKindDefOf.Slave;
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				newPawn.Destroy(DestroyMode.Vanish);
				Log.Error(string.Concat(new object[]
				{
					"Tried to sell pawn ",
					newPawn,
					" to ",
					this.pawn,
					", but ",
					this.pawn,
					" has no lord. Traders without lord can't buy pawns."
				}));
				return;
			}
			if (newPawn.RaceProps.Humanlike)
			{
				this.soldPrisoners.Add(newPawn);
			}
			lord.AddPawn(newPawn);
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x001D714C File Offset: 0x001D534C
		private void AddThingToRandomInventory(Thing thing)
		{
			Lord lord = this.pawn.GetLord();
			IEnumerable<Pawn> source = Enumerable.Empty<Pawn>();
			if (lord != null)
			{
				source = from x in lord.ownedPawns
				where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
				select x;
			}
			if (source.Any<Pawn>())
			{
				if (!source.RandomElement<Pawn>().inventory.innerContainer.TryAdd(thing, true))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else if (!this.pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060056C6 RID: 22214 RVA: 0x001D71E2 File Offset: 0x001D53E2
		private bool ReachableForTrade(Thing thing)
		{
			return this.pawn.Map == thing.Map && this.pawn.Map.reachability.CanReach(this.pawn.Position, thing, PathEndMode.Touch, TraverseMode.PassDoors, Danger.Some);
		}

		// Token: 0x04003332 RID: 13106
		private Pawn pawn;

		// Token: 0x04003333 RID: 13107
		public TraderKindDef traderKind;

		// Token: 0x04003334 RID: 13108
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
