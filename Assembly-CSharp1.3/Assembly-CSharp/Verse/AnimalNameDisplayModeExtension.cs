using System;

namespace Verse
{
	// Token: 0x020004E1 RID: 1249
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x060025CD RID: 9677 RVA: 0x000EA5B4 File Offset: 0x000E87B4
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
