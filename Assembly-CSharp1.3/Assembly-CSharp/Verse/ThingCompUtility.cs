using System;

namespace Verse
{
	// Token: 0x0200038C RID: 908
	public static class ThingCompUtility
	{
		// Token: 0x06001AB1 RID: 6833 RVA: 0x00099E14 File Offset: 0x00098014
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
