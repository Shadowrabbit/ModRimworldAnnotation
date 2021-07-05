using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127D RID: 4733
	public class Alert_MonumentMarkerMissingBlueprints : Alert
	{
		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06007124 RID: 28964 RVA: 0x0025B34C File Offset: 0x0025954C
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

		// Token: 0x06007125 RID: 28965 RVA: 0x0025B3FB File Offset: 0x002595FB
		public Alert_MonumentMarkerMissingBlueprints()
		{
			this.defaultLabel = "MonumentMarkerMissingBlueprints".Translate();
			this.defaultExplanation = "MonumentMarkerMissingBlueprintsDesc".Translate();
		}

		// Token: 0x06007126 RID: 28966 RVA: 0x0025B438 File Offset: 0x00259638
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E3C RID: 15932
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
