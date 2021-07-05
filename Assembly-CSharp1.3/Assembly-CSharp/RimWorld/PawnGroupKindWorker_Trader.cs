using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC3 RID: 3523
	public class PawnGroupKindWorker_Trader : PawnGroupKindWorker
	{
		// Token: 0x06005187 RID: 20871 RVA: 0x000682C5 File Offset: 0x000664C5
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker, PawnGroupMakerParms parms = null)
		{
			return 0f;
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x001B78C0 File Offset: 0x001B5AC0
		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && groupMaker.traders.Any<PawnGenOption>() && (parms.tile == -1 || groupMaker.carriers.Any((PawnGenOption x) => Find.WorldGrid[parms.tile].biome.IsPackAnimalAllowed(x.kind.race)));
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x001B7920 File Offset: 0x001B5B20
		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					Log.Error("Cannot generate trader caravan for " + parms.faction + ".");
				}
				return;
			}
			if (!parms.faction.def.caravanTraderKinds.Any<TraderKindDef>())
			{
				Log.Error("Cannot generate trader caravan for " + parms.faction + " because it has no trader kinds.");
				return;
			}
			PawnGenOption pawnGenOption = groupMaker.traders.FirstOrDefault((PawnGenOption x) => !x.kind.trader);
			if (pawnGenOption != null)
			{
				Log.Error("Cannot generate arriving trader caravan for " + parms.faction + " because there is a pawn kind (" + pawnGenOption.kind.LabelCap + ") who is not a trader but is in a traders list.");
				return;
			}
			PawnGenOption pawnGenOption2 = groupMaker.carriers.FirstOrDefault((PawnGenOption x) => !x.kind.RaceProps.packAnimal);
			if (pawnGenOption2 != null)
			{
				Log.Error("Cannot generate arriving trader caravan for " + parms.faction + " because there is a pawn kind (" + pawnGenOption2.kind.LabelCap + ") who is not a carrier but is in a carriers list.");
				return;
			}
			if (parms.seed != null)
			{
				Log.Warning("Deterministic seed not implemented for this pawn group kind worker. The result will be random anyway.");
			}
			TraderKindDef traderKindDef;
			if (parms.traderKind == null)
			{
				traderKindDef = parms.faction.def.caravanTraderKinds.RandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality);
			}
			else
			{
				traderKindDef = parms.traderKind;
			}
			TraderKindDef traderKindDef2 = traderKindDef;
			Pawn pawn = this.GenerateTrader(parms, groupMaker, traderKindDef2);
			outPawns.Add(pawn);
			ThingSetMakerParams parms2 = default(ThingSetMakerParams);
			parms2.traderDef = traderKindDef2;
			parms2.tile = new int?(parms.tile);
			parms2.makingFaction = parms.faction;
			List<Thing> wares = ThingSetMakerDefOf.TraderStock.root.Generate(parms2).InRandomOrder(null).ToList<Thing>();
			foreach (Pawn item in this.GetSlavesAndAnimalsFromWares(parms, pawn, wares))
			{
				outPawns.Add(item);
			}
			this.GenerateCarriers(parms, groupMaker, pawn, wares, outPawns);
			this.GenerateGuards(parms, groupMaker, pawn, wares, outPawns);
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x001B7B78 File Offset: 0x001B5D78
		private Pawn GenerateTrader(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, TraderKindDef traderKind)
		{
			PawnKindDef kind = groupMaker.traders.RandomElementByWeight((PawnGenOption x) => x.selectionWeight).kind;
			Faction faction = parms.faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			Ideo ideo = parms.ideo;
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, parms.tile, false, false, false, false, true, false, 1f, false, true, true, true, parms.inhabitants, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, ideo, false, false));
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			pawn.trader.traderKind = traderKind;
			parms.points -= pawn.kindDef.combatPower;
			return pawn;
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x001B7C6C File Offset: 0x001B5E6C
		private void GenerateCarriers(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
		{
			List<Thing> list = (from x in wares
			where !(x is Pawn)
			select x).ToList<Thing>();
			int i = 0;
			int num = Mathf.CeilToInt((float)list.Count / 8f);
			PawnKindDef kind = (from x in groupMaker.carriers
			where parms.tile == -1 || Find.WorldGrid[parms.tile].biome.IsPackAnimalAllowed(x.kind.race)
			select x).RandomElementByWeight((PawnGenOption x) => x.selectionWeight).kind;
			List<Pawn> list2 = new List<Pawn>();
			for (int j = 0; j < num; j++)
			{
				PawnKindDef kind2 = kind;
				Faction faction = parms.faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				Ideo ideo = parms.ideo;
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind2, faction, context, parms.tile, false, false, false, false, true, false, 1f, false, true, true, true, parms.inhabitants, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, ideo, false, false));
				if (i < list.Count)
				{
					pawn.inventory.innerContainer.TryAdd(list[i], true);
					i++;
				}
				list2.Add(pawn);
				outPawns.Add(pawn);
			}
			while (i < list.Count)
			{
				list2.RandomElement<Pawn>().inventory.innerContainer.TryAdd(list[i], true);
				i++;
			}
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x001B7E29 File Offset: 0x001B6029
		private IEnumerable<Pawn> GetSlavesAndAnimalsFromWares(PawnGroupMakerParms parms, Pawn trader, List<Thing> wares)
		{
			int num;
			for (int i = 0; i < wares.Count; i = num + 1)
			{
				Pawn pawn = wares[i] as Pawn;
				if (pawn != null)
				{
					if (pawn.Faction != parms.faction)
					{
						pawn.SetFaction(parms.faction, null);
					}
					yield return pawn;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x001B7E40 File Offset: 0x001B6040
		private void GenerateGuards(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
		{
			if (!groupMaker.guards.Any<PawnGenOption>())
			{
				return;
			}
			foreach (PawnGenOption pawnGenOption in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.guards, parms))
			{
				PawnGenerationRequest request = PawnGenerationRequest.MakeDefault();
				request.KindDef = pawnGenOption.kind;
				request.Faction = parms.faction;
				request.Tile = parms.tile;
				request.MustBeCapableOfViolence = true;
				request.Inhabitant = parms.inhabitants;
				request.FixedIdeo = parms.ideo;
				Pawn item = PawnGenerator.GeneratePawn(request);
				outPawns.Add(item);
			}
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x0002974C File Offset: 0x0002794C
		public override IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			throw new NotImplementedException();
		}
	}
}
