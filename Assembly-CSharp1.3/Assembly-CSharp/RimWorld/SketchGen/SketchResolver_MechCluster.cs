using System;

namespace RimWorld.SketchGen
{
	// Token: 0x02001590 RID: 5520
	public class SketchResolver_MechCluster : SketchResolver
	{
		// Token: 0x06008261 RID: 33377 RVA: 0x002E33F1 File Offset: 0x002E15F1
		protected override void ResolveInt(ResolveParams parms)
		{
			MechClusterGenerator.ResolveSketch(parms);
		}

		// Token: 0x06008262 RID: 33378 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}
	}
}
