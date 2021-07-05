using System;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E1A RID: 7706
	public class SketchResolver_MechCluster : SketchResolver
	{
		// Token: 0x0600A6BD RID: 42685 RVA: 0x0006E491 File Offset: 0x0006C691
		protected override void ResolveInt(ResolveParams parms)
		{
			MechClusterGenerator.ResolveSketch(parms);
		}

		// Token: 0x0600A6BE RID: 42686 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}
	}
}
