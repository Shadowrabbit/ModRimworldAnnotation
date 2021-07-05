using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001663 RID: 5731
	public static class InstallationDesignatorDatabase
	{
		// Token: 0x06007CDC RID: 31964 RVA: 0x002552E0 File Offset: 0x002534E0
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

		// Token: 0x06007CDD RID: 31965 RVA: 0x00053E41 File Offset: 0x00052041
		private static Designator_Install NewDesignatorFor(ThingDef artDef)
		{
			return new Designator_Install
			{
				hotKey = KeyBindingDefOf.Misc1
			};
		}

		// Token: 0x04005194 RID: 20884
		private static Dictionary<ThingDef, Designator_Install> designators = new Dictionary<ThingDef, Designator_Install>();
	}
}
