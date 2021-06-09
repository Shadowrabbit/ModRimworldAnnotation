using System;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000189 RID: 393
	public static class SoundDefHelper
	{
		// Token: 0x060009D3 RID: 2515 RVA: 0x0000DA73 File Offset: 0x0000BC73
		public static bool NullOrUndefined(this SoundDef def)
		{
			return def == null || def.isUndefined;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0009A668 File Offset: 0x00098868
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
