using System;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000107 RID: 263
	public static class SoundDefHelper
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x0002194D File Offset: 0x0001FB4D
		public static bool NullOrUndefined(this SoundDef def)
		{
			return def == null || def.isUndefined;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002195C File Offset: 0x0001FB5C
		public static bool CorrectContextNow(SoundDef def, Map sourceMap)
		{
			if (sourceMap != null && (Find.CurrentMap != sourceMap || WorldRendererUtility.WorldRenderedNow))
			{
				return false;
			}
			switch (def.context)
			{
			case SoundContext.Any:
				return true;
			case SoundContext.MapOnly:
				return Current.ProgramState == ProgramState.Playing && !WorldRendererUtility.WorldRenderedNow;
			case SoundContext.WorldOnly:
				return WorldRendererUtility.WorldRenderedNow;
			default:
				throw new NotImplementedException();
			}
		}
	}
}
