using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000185 RID: 389
	public static class CompressibilityDeciderUtility
	{
		// Token: 0x06000B17 RID: 2839 RVA: 0x0003C50C File Offset: 0x0003A70C
		public static bool IsSaveCompressible(this Thing t)
		{
			if (Scribe.saver.savingForDebug)
			{
				return false;
			}
			if (!t.def.saveCompressible)
			{
				return false;
			}
			if (t.def.useHitPoints && t.HitPoints != t.MaxHitPoints)
			{
				return false;
			}
			if (!t.Spawned)
			{
				return false;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].compressor != null)
				{
					flag = true;
					if (maps[i].compressor.compressibilityDecider.IsReferenced(t))
					{
						return false;
					}
				}
			}
			if (!flag)
			{
				Log.ErrorOnce("Called IsSaveCompressible but there are no maps with compressor != null. This should never happen. It probably means that we're not saving any map at the moment?", 1935111328);
			}
			return true;
		}
	}
}
