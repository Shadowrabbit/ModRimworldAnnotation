using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB3 RID: 3251
	public class GenStep_AncientComplex : GenStep_ScattererBestFit
	{
		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06004BC6 RID: 19398 RVA: 0x00193B25 File Offset: 0x00191D25
		protected override IntVec2 Size
		{
			get
			{
				return new IntVec2(this.sketch.layout.container.Width + 10, this.sketch.layout.container.Height + 10);
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06004BC7 RID: 19399 RVA: 0x00193B5C File Offset: 0x00191D5C
		public override int SeedPart
		{
			get
			{
				return 235635649;
			}
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x00193B64 File Offset: 0x00191D64
		public override bool CollisionAt(IntVec3 cell, Map map)
		{
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.IsBuildingArtificial)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x00193BA0 File Offset: 0x00191DA0
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.nearMapCenter = true;
			this.sketch = parms.sitePart.parms.ancientComplexSketch;
			if (this.sketch == null)
			{
				this.sketch = ComplexDefOf.AncientComplex.Worker.GenerateSketch(GenStep_AncientComplex.DefaultComplexSize, null);
				Log.Warning("No ancient complex found in sitepart parms, generating default ancient complex.");
			}
			base.Generate(map, parms);
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x00193C08 File Offset: 0x00191E08
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.ancientComplexSketch = this.sketch;
			resolveParams.threatPoints = new float?(parms.sitePart.parms.threatPoints);
			resolveParams.rect = CellRect.CenteredOn(c, this.sketch.layout.container.Width, this.sketch.layout.container.Height);
			resolveParams.thingSetMakerDef = parms.sitePart.parms.ancientComplexRewardMaker;
			FormCaravanComp component = parms.sitePart.site.GetComponent<FormCaravanComp>();
			if (component != null)
			{
				component.foggedRoomsCheckRect = new CellRect?(resolveParams.rect);
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientComplex", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04002DDF RID: 11743
		private ComplexSketch sketch;

		// Token: 0x04002DE0 RID: 11744
		private static readonly IntVec2 DefaultComplexSize = new IntVec2(80, 80);
	}
}
