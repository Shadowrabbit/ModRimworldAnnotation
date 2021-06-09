using System;

namespace Verse
{
	// Token: 0x02000531 RID: 1329
	public static class ThingCompUtility
	{
		// Token: 0x06002235 RID: 8757 RVA: 0x00107F14 File Offset: 0x00106114
		public static T TryGetComp<T>(this Thing thing) where T : ThingComp
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return default(T);
			}
			return thingWithComps.GetComp<T>();
		}
	}
}
