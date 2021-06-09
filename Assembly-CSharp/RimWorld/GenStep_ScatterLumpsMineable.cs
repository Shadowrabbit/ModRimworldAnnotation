using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012A3 RID: 4771
	public class GenStep_ScatterLumpsMineable : GenStep_Scatterer
	{
		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x060067B3 RID: 26547 RVA: 0x00046B3C File Offset: 0x00044D3C
		public override int SeedPart
		{
			get
			{
				return 920906419;
			}
		}

		// Token: 0x060067B4 RID: 26548 RVA: 0x001FF770 File Offset: 0x001FD970
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

		// Token: 0x060067B5 RID: 26549 RVA: 0x00046B43 File Offset: 0x00044D43
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

		// Token: 0x060067B6 RID: 26550 RVA: 0x001FF7D0 File Offset: 0x001FD9D0
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (base.NearUsedSpot(c, this.minSpacing))
			{
				return false;
			}
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice.def.building.isNaturalRock;
		}

		// Token: 0x060067B7 RID: 26551 RVA: 0x001FF810 File Offset: 0x001FDA10
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

		// Token: 0x0400450C RID: 17676
		public ThingDef forcedDefToScatter;

		// Token: 0x0400450D RID: 17677
		public int forcedLumpSize;

		// Token: 0x0400450E RID: 17678
		public float maxValue = float.MaxValue;

		// Token: 0x0400450F RID: 17679
		[Unsaved(false)]
		protected List<IntVec3> recentLumpCells = new List<IntVec3>();
	}
}
