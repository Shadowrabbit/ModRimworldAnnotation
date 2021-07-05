using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200157D RID: 5501
	public static class SketchGen
	{
		// Token: 0x06008214 RID: 33300 RVA: 0x002DF984 File Offset: 0x002DDB84
		public static Sketch Generate(SketchResolverDef root, ResolveParams parms)
		{
			if (SketchGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.");
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
				Log.Error("Error in SketchGen: " + arg);
				sketch = parms.sketch;
			}
			finally
			{
				SketchGen.working = false;
			}
			return sketch;
		}

		// Token: 0x04005108 RID: 20744
		private static bool working;
	}
}
