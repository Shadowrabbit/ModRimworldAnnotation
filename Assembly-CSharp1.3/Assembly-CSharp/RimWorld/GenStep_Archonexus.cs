using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB4 RID: 3252
	public class GenStep_Archonexus : GenStep_Scatterer
	{
		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06004BCD RID: 19405 RVA: 0x00193CEA File Offset: 0x00191EEA
		public override int SeedPart
		{
			get
			{
				return 473957495;
			}
		}

		// Token: 0x06004BCE RID: 19406 RVA: 0x00193CF1 File Offset: 0x00191EF1
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.nearMapCenter = true;
			base.Generate(map, parms);
		}

		// Token: 0x06004BCF RID: 19407 RVA: 0x00193D0C File Offset: 0x00191F0C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			ThingDef archonexusCore = ThingDefOf.ArchonexusCore;
			IntVec3 c2 = ThingUtility.InteractionCellWhenAt(archonexusCore, c, archonexusCore.defaultPlacingRot, map);
			return map.reachability.CanReachMapEdge(c2, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false));
		}

		// Token: 0x06004BD0 RID: 19408 RVA: 0x00193D6C File Offset: 0x00191F6C
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			SitePartParams parms2 = parms.sitePart.parms;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.threatPoints = new float?(parms2.threatPoints);
			resolveParams.rect = CellRect.CenteredOn(c, 60, 60);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("archonexus", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04002DE1 RID: 11745
		private const int Size = 60;
	}
}
