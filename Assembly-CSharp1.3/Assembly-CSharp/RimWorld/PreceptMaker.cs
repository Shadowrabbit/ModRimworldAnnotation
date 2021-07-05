using System;

namespace RimWorld
{
	// Token: 0x02000EEA RID: 3818
	public static class PreceptMaker
	{
		// Token: 0x06005AA4 RID: 23204 RVA: 0x001F5896 File Offset: 0x001F3A96
		public static Precept MakePrecept(PreceptDef def)
		{
			Precept precept = (Precept)Activator.CreateInstance(def.preceptClass);
			precept.def = def;
			precept.PostMake();
			return precept;
		}
	}
}
