using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
    // Token: 0x020012C6 RID: 4806
    public class GenStep_ManhunterPack : GenStep
    {
        // Token: 0x1700100C RID: 4108
        // (get) Token: 0x0600682E RID: 26670 RVA: 0x00046EEF File Offset: 0x000450EF
        public override int SeedPart
        {
            get { return 457293335; }
        }

        /// <summary>
        /// 生成方法
        /// </summary>
        /// <param name="map"></param>
        /// <param name="parms"></param>
        public override void Generate(Map map, GenStepParams parms)
        {
            TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors);
            IntVec3 root;
            if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(
                (IntVec3 x) => x.Standable(map) && !x.Fogged(map) &&
                               map.reachability.CanReachMapEdge(x, traverseParams) &&
                               x.GetRoom(map).CellCount >= MinRoomCells, map, out root))
            {
                //点数
                float points = (parms.sitePart != null)
                    ? parms.sitePart.parms.threatPoints
                    : this.defaultPointsRange.RandomInRange;
                //动物种类
                PawnKindDef animalKind;
                if (parms.sitePart != null && parms.sitePart.parms.animalKind != null)
                {
                    animalKind = parms.sitePart.parms.animalKind;
                }
                else if (!ManhunterPackGenStepUtility.TryGetAnimalsKind(points, map.Tile, out animalKind))
                {
                    return;
                }
                //生成动物
                List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals_NewTmp(animalKind, map.Tile, points, 0);
                for (int i = 0; i < list.Count; i++)
                {
                    //随机位置
                    IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
                    //生成动物
                    GenSpawn.Spawn(list[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
                    list[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
                    list[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null,
                        false, false, null, false);
                }
            }
        }

        // Token: 0x0400455A RID: 17754
        public FloatRange defaultPointsRange = new FloatRange(300f, 500f);

        // Token: 0x0400455B RID: 17755
        private int MinRoomCells = 225;
    }
}