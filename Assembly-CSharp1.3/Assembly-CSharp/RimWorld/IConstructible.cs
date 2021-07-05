using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001044 RID: 4164
	public interface IConstructible
	{
		// Token: 0x0600626F RID: 25199
		List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x06006270 RID: 25200
		ThingDef EntityToBuildStuff();
	}
}
