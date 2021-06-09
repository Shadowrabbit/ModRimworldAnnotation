using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200166D RID: 5741
	public interface IConstructible
	{
		// Token: 0x06007D2C RID: 32044
		List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x06007D2D RID: 32045
		ThingDef EntityToBuildStuff();
	}
}
