using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015C7 RID: 5575
	public class SymbolResolver_SinglePawn : SymbolResolver
	{
		// Token: 0x06008343 RID: 33603 RVA: 0x002EBEFC File Offset: 0x002EA0FC
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && ((rp.singlePawnToSpawn != null && rp.singlePawnToSpawn.Spawned) || SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out intVec));
		}

		// Token: 0x06008344 RID: 33604 RVA: 0x002EBF38 File Offset: 0x002EA138
		public override void Resolve(ResolveParams rp)
		{
			if (rp.singlePawnToSpawn != null && rp.singlePawnToSpawn.Spawned)
			{
				return;
			}
			Map map = BaseGen.globalSettings.map;
			IntVec3 loc;
			if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out loc))
			{
				if (rp.singlePawnToSpawn != null)
				{
					Find.WorldPawns.PassToWorld(rp.singlePawnToSpawn, PawnDiscardDecideMode.Decide);
				}
				return;
			}
			Pawn pawn;
			if (rp.singlePawnToSpawn == null)
			{
				PawnGenerationRequest value;
				if (rp.singlePawnGenerationRequest != null)
				{
					value = rp.singlePawnGenerationRequest.Value;
				}
				else
				{
					PawnKindDef pawnKindDef;
					if ((pawnKindDef = rp.singlePawnKindDef) == null)
					{
						pawnKindDef = (from x in DefDatabase<PawnKindDef>.AllDefsListForReading
						where x.defaultFactionType == null || !x.defaultFactionType.isPlayer
						select x).RandomElement<PawnKindDef>();
					}
					PawnKindDef pawnKindDef2 = pawnKindDef;
					Faction faction = rp.faction;
					if (faction == null && pawnKindDef2.RaceProps.Humanlike)
					{
						if (pawnKindDef2.defaultFactionType != null)
						{
							faction = FactionUtility.DefaultFactionFrom(pawnKindDef2.defaultFactionType);
							if (faction == null)
							{
								return;
							}
						}
						else if (!(from x in Find.FactionManager.AllFactions
						where !x.IsPlayer
						select x).TryRandomElement(out faction))
						{
							return;
						}
					}
					value = new PawnGenerationRequest(pawnKindDef2, faction, PawnGenerationContext.NonPlayer, map.Tile, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false);
				}
				pawn = PawnGenerator.GeneratePawn(value);
				if (rp.postThingGenerate != null)
				{
					rp.postThingGenerate(pawn);
				}
			}
			else
			{
				pawn = rp.singlePawnToSpawn;
			}
			if (!pawn.Dead && rp.disableSinglePawn != null && rp.disableSinglePawn.Value)
			{
				pawn.mindState.Active = false;
			}
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			if (rp.singlePawnLord != null)
			{
				rp.singlePawnLord.AddPawn(pawn);
			}
			if (rp.postThingSpawn != null)
			{
				rp.postThingSpawn(pawn);
			}
		}

		// Token: 0x06008345 RID: 33605 RVA: 0x002EC154 File Offset: 0x002EA354
		public static bool TryFindSpawnCell(ResolveParams rp, out IntVec3 cell)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rp.rect, (IntVec3 x) => x.Standable(map) && (rp.singlePawnSpawnCellExtraPredicate == null || rp.singlePawnSpawnCellExtraPredicate(x)), out cell);
		}
	}
}
