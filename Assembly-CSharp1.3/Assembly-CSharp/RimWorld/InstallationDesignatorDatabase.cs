using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103D RID: 4157
	public static class InstallationDesignatorDatabase
	{
		// Token: 0x06006239 RID: 25145 RVA: 0x00215344 File Offset: 0x00213544
		public static Designator_Install DesignatorFor(ThingDef artDef)
		{
			Designator_Install designator_Install;
			if (InstallationDesignatorDatabase.designators.TryGetValue(artDef, out designator_Install))
			{
				return designator_Install;
			}
			designator_Install = InstallationDesignatorDatabase.NewDesignatorFor(artDef);
			InstallationDesignatorDatabase.designators.Add(artDef, designator_Install);
			return designator_Install;
		}

		// Token: 0x0600623A RID: 25146 RVA: 0x00215376 File Offset: 0x00213576
		private static Designator_Install NewDesignatorFor(ThingDef artDef)
		{
			return new Designator_Install
			{
				hotKey = KeyBindingDefOf.Misc1
			};
		}

		// Token: 0x040037DB RID: 14299
		private static Dictionary<ThingDef, Designator_Install> designators = new Dictionary<ThingDef, Designator_Install>();
	}
}
