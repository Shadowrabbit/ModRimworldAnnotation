using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013EA RID: 5098
	public static class BuildRelatedCommandUtility
	{
		// Token: 0x06007BFC RID: 31740 RVA: 0x002BC843 File Offset: 0x002BAA43
		public static IEnumerable<Command> RelatedBuildCommands(BuildableDef building)
		{
			foreach (Command command in BuildFacilityCommandUtility.BuildFacilityCommands(building))
			{
				yield return command;
			}
			IEnumerator<Command> enumerator = null;
			ThingDef thingDef = building as ThingDef;
			List<ThingDef> list;
			if (thingDef == null)
			{
				list = null;
			}
			else
			{
				BuildingProperties building2 = thingDef.building;
				list = ((building2 != null) ? building2.relatedBuildCommands : null);
			}
			List<ThingDef> list2 = list;
			if (list2 != null)
			{
				foreach (ThingDef buildable in list2)
				{
					Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(buildable, true);
					if (designator_Build != null)
					{
						yield return designator_Build;
					}
				}
				List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
			}
			yield break;
			yield break;
		}
	}
}
