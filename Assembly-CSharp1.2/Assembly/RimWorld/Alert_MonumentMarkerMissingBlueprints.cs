using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196E RID: 6510
	public class Alert_MonumentMarkerMissingBlueprints : Alert
	{
		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x06008FF5 RID: 36853 RVA: 0x00296F88 File Offset: 0x00295188
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (!monumentMarker.complete)
						{
							SketchEntity firstEntityWithMissingBlueprint = monumentMarker.FirstEntityWithMissingBlueprint;
							if (firstEntityWithMissingBlueprint != null)
							{
								this.targets.Add(new GlobalTargetInfo(firstEntityWithMissingBlueprint.pos + monumentMarker.Position, maps[i], false));
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x06008FF6 RID: 36854 RVA: 0x0006090E File Offset: 0x0005EB0E
		public Alert_MonumentMarkerMissingBlueprints()
		{
			this.defaultLabel = "MonumentMarkerMissingBlueprints".Translate();
			this.defaultExplanation = "MonumentMarkerMissingBlueprintsDesc".Translate();
		}

		// Token: 0x06008FF7 RID: 36855 RVA: 0x0006094B File Offset: 0x0005EB4B
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04005B91 RID: 23441
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
