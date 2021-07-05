using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001481 RID: 5249
	public static class FacilitiesUtility
	{
		// Token: 0x06007D83 RID: 32131 RVA: 0x002C5748 File Offset: 0x002C3948
		public static void NotifyFacilitiesAboutChangedLOSBlockers(List<Region> affectedRegions)
		{
			if (!affectedRegions.Any<Region>())
			{
				return;
			}
			if (FacilitiesUtility.working)
			{
				Log.Warning("Tried to update facilities while already updating.");
				return;
			}
			FacilitiesUtility.working = true;
			try
			{
				FacilitiesUtility.visited.Clear();
				FacilitiesUtility.processed.Clear();
				int facilitiesToProcess = affectedRegions[0].Map.listerThings.ThingsInGroup(ThingRequestGroup.Facility).Count;
				int affectedByFacilitiesToProcess = affectedRegions[0].Map.listerThings.ThingsInGroup(ThingRequestGroup.AffectedByFacilities).Count;
				int facilitiesProcessed = 0;
				int affectedByFacilitiesProcessed = 0;
				if (facilitiesToProcess > 0 && affectedByFacilitiesToProcess > 0)
				{
					RegionProcessor <>9__1;
					for (int i = 0; i < affectedRegions.Count; i++)
					{
						if (!FacilitiesUtility.visited.Contains(affectedRegions[i]))
						{
							Region root = affectedRegions[i];
							RegionEntryPredicate entryCondition = (Region from, Region r) => !FacilitiesUtility.visited.Contains(r);
							RegionProcessor regionProcessor;
							if ((regionProcessor = <>9__1) == null)
							{
								regionProcessor = (<>9__1 = delegate(Region x)
								{
									FacilitiesUtility.visited.Add(x);
									List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
									for (int j = 0; j < list.Count; j++)
									{
										if (!FacilitiesUtility.processed.Contains(list[j]))
										{
											FacilitiesUtility.processed.Add(list[j]);
											CompFacility compFacility = list[j].TryGetComp<CompFacility>();
											CompAffectedByFacilities compAffectedByFacilities = list[j].TryGetComp<CompAffectedByFacilities>();
											if (compFacility != null)
											{
												compFacility.Notify_LOSBlockerSpawnedOrDespawned();
												int num = facilitiesProcessed;
												facilitiesProcessed = num + 1;
											}
											if (compAffectedByFacilities != null)
											{
												compAffectedByFacilities.Notify_LOSBlockerSpawnedOrDespawned();
												int num = affectedByFacilitiesProcessed;
												affectedByFacilitiesProcessed = num + 1;
											}
										}
									}
									return facilitiesProcessed >= facilitiesToProcess && affectedByFacilitiesProcessed >= affectedByFacilitiesToProcess;
								});
							}
							RegionTraverser.BreadthFirstTraverse(root, entryCondition, regionProcessor, FacilitiesUtility.RegionsToSearch, RegionType.Set_Passable);
							if (facilitiesProcessed >= facilitiesToProcess && affectedByFacilitiesProcessed >= affectedByFacilitiesToProcess)
							{
								break;
							}
						}
					}
				}
			}
			finally
			{
				FacilitiesUtility.working = false;
				FacilitiesUtility.visited.Clear();
				FacilitiesUtility.processed.Clear();
			}
		}

		// Token: 0x04004E4B RID: 20043
		private const float MaxDistToLinkToFacilityEver = 10f;

		// Token: 0x04004E4C RID: 20044
		private static int RegionsToSearch = (1 + 2 * Mathf.CeilToInt(0.8333333f)) * (1 + 2 * Mathf.CeilToInt(0.8333333f));

		// Token: 0x04004E4D RID: 20045
		private static HashSet<Region> visited = new HashSet<Region>();

		// Token: 0x04004E4E RID: 20046
		private static HashSet<Thing> processed = new HashSet<Thing>();

		// Token: 0x04004E4F RID: 20047
		private static bool working;
	}
}
