using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB9 RID: 3257
	public class GenStep_ManhunterPack : GenStep
	{
		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06004BE2 RID: 19426 RVA: 0x0019443A File Offset: 0x0019263A
		public override int SeedPart
		{
			get
			{
				return 457293335;
			}
		}

		// Token: 0x06004BE3 RID: 19427 RVA: 0x00194444 File Offset: 0x00192644
		public override void Generate(Map map, GenStepParams parms)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false).WithFenceblocked(true);
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReachMapEdge(x, traverseParams) && x.GetRoom(map).CellCount >= this.MinRoomCells, map, out root))
			{
				float points = (parms.sitePart != null) ? parms.sitePart.parms.threatPoints : this.defaultPointsRange.RandomInRange;
				PawnKindDef animalKind;
				if (parms.sitePart != null && parms.sitePart.parms.animalKind != null)
				{
					animalKind = parms.sitePart.parms.animalKind;
				}
				else if (!ManhunterPackGenStepUtility.TryGetAnimalsKind(points, map.Tile, out animalKind))
				{
					return;
				}
				List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals(animalKind, map.Tile, points, 0);
				for (int i = 0; i < list.Count; i++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
					GenSpawn.Spawn(list[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
					list[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
					list[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false, false, false);
				}
			}
		}

		// Token: 0x04002DE7 RID: 11751
		public FloatRange defaultPointsRange = new FloatRange(300f, 500f);

		// Token: 0x04002DE8 RID: 11752
		private int MinRoomCells = 225;
	}
}
