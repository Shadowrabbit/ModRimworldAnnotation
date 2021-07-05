using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CBE RID: 3262
	public class GenStep_PrisonerWillingToJoin : GenStep_Scatterer
	{
		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06004BF3 RID: 19443 RVA: 0x00194EF8 File Offset: 0x001930F8
		public override int SeedPart
		{
			get
			{
				return 69356099;
			}
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x00194F00 File Offset: 0x00193100
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false)))
			{
				return false;
			}
			foreach (IntVec3 c2 in CellRect.CenteredOn(c, 8, 8))
			{
				if (!c2.InBounds(map) || c2.GetEdifice(map) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x00194FA0 File Offset: 0x001931A0
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			CellRect cellRect = CellRect.CenteredOn(loc, 8, 8).ClipInsideMap(map);
			Pawn singlePawnToSpawn;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				singlePawnToSpawn = (Pawn)parms.sitePart.things.Take(parms.sitePart.things[0]);
			}
			else
			{
				PrisonerWillingToJoinComp component = map.Parent.GetComponent<PrisonerWillingToJoinComp>();
				if (component != null && component.pawn.Any)
				{
					singlePawnToSpawn = component.pawn.Take(component.pawn[0]);
				}
				else
				{
					singlePawnToSpawn = PrisonerWillingToJoinQuestUtility.GeneratePrisoner(map.Tile, faction);
				}
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("prisonCell", resolveParams, null);
			BaseGen.Generate();
			ResolveParams resolveParams2 = default(ResolveParams);
			resolveParams2.rect = cellRect;
			resolveParams2.faction = faction;
			resolveParams2.singlePawnToSpawn = singlePawnToSpawn;
			resolveParams2.singlePawnSpawnCellExtraPredicate = ((IntVec3 x) => x.GetDoor(map) == null);
			resolveParams2.postThingSpawn = delegate(Thing x)
			{
				MapGenerator.rootsToUnfog.Add(x.Position);
				((Pawn)x).mindState.WillJoinColonyIfRescued = true;
			};
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("pawn", resolveParams2, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
		}

		// Token: 0x04002DF9 RID: 11769
		private const int Size = 8;
	}
}
