using System;

namespace Verse
{
	// Token: 0x0200088D RID: 2189
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x0600365D RID: 13917 RVA: 0x0015BA88 File Offset: 0x00159C88
		public static string ToStringHuman(this AnimalNameDisplayMode mode)
		{
			switch (mode)
			{
			case AnimalNameDisplayMode.None:
				return "None".Translate();
			case AnimalNameDisplayMode.TameNamed:
				return "AnimalNameDisplayMode_TameNamed".Translate();
			case AnimalNameDisplayMode.TameAll:
				return "AnimalNameDisplayMode_TameAll".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
