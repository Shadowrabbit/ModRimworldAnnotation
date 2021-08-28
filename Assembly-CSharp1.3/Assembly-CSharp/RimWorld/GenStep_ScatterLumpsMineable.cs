using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
    // Token: 0x02000CA6 RID: 3238
    public class GenStep_ScatterLumpsMineable : GenStep_Scatterer
    {
        // Token: 0x17000D07 RID: 3335
        // (get) Token: 0x06004B83 RID: 19331 RVA: 0x00191544 File Offset: 0x0018F744
        public override int SeedPart
        {
            get
            {
                return 920906419;
            }
        }

        // Token: 0x06004B84 RID: 19332 RVA: 0x0019154C File Offset: 0x0018F74C
        public override void Generate(Map map, GenStepParams parms)
        {
            this.minSpacing = 5f;
            this.warnOnFail = false;
            int num = base.CalculateFinalCount(map);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec;
                if (!this.TryFindScatterCell(map, out intVec))
                {
                    return;
                }
                this.ScatterAt(intVec, map, parms, 1);
                this.usedSpots.Add(intVec);
            }
            this.usedSpots.Clear();
        }

        // Token: 0x06004B85 RID: 19333 RVA: 0x001915AC File Offset: 0x0018F7AC
        protected ThingDef ChooseThingDef()
        {
            if (this.forcedDefToScatter != null)
            {
                return this.forcedDefToScatter;
            }
            return DefDatabase<ThingDef>.AllDefs.RandomElementByWeightWithFallback(delegate(ThingDef d)
            {
                if (d.building == null)
                {
                    return 0f;
                }
                if (d.building.mineableThing != null && d.building.mineableThing.BaseMarketValue > this.maxValue)
                {
                    return 0f;
                }
                return d.building.mineableScatterCommonality;
            }, null);
        }

        // Token: 0x06004B86 RID: 19334 RVA: 0x001915D4 File Offset: 0x0018F7D4
        protected override bool CanScatterAt(IntVec3 c, Map map)
        {
            if (base.NearUsedSpot(c, this.minSpacing))
            {
                return false;
            }
            Building edifice = c.GetEdifice(map);
            return edifice != null && edifice.def.building.isNaturalRock;
        }

        // Token: 0x06004B87 RID: 19335 RVA: 0x00191614 File Offset: 0x0018F814
        protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
        {
            ThingDef thingDef = this.ChooseThingDef();
            if (thingDef == null)
            {
                return;
            }
            int numCells = (this.forcedLumpSize > 0) ? this.forcedLumpSize : thingDef.building.mineableScatterLumpSizeRange.RandomInRange;
            this.recentLumpCells.Clear();
            foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(c, map, numCells))
            {
                GenSpawn.Spawn(thingDef, intVec, map, WipeMode.Vanish);
                this.recentLumpCells.Add(intVec);
            }
        }

        // Token: 0x04002DBA RID: 11706
        public ThingDef forcedDefToScatter;

        // Token: 0x04002DBB RID: 11707
        public int forcedLumpSize;

        // Token: 0x04002DBC RID: 11708
        public float maxValue = float.MaxValue;

        // Token: 0x04002DBD RID: 11709
        [Unsaved(false)] protected List<IntVec3> recentLumpCells = new List<IntVec3>();
    }
}
