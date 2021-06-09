using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E03 RID: 7683
	public static class SketchGen
	{
		// Token: 0x0600A673 RID: 42611 RVA: 0x003041D4 File Offset: 0x003023D4
		public static Sketch Generate(SketchResolverDef root, ResolveParams parms)
		{
			if (SketchGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.", false);
				return parms.sketch;
			}
			SketchGen.working = true;
			Sketch sketch;
			try
			{
				root.Resolve(parms);
				sketch = parms.sketch;
			}
			catch (Exception arg)
			{
				Log.Error("Error in SketchGen: " + arg, false);
				sketch = parms.sketch;
			}
			finally
			{
				SketchGen.working = false;
			}
			return sketch;
		}

		// Token: 0x040070E2 RID: 28898
		private static bool working;
	}
}
