using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAC RID: 2988
	public class QuestPart_SpawnWorldObject : QuestPart
	{
		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x060045B7 RID: 17847 RVA: 0x00171483 File Offset: 0x0016F683
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.worldObject != null)
				{
					yield return this.worldObject;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x060045B8 RID: 17848 RVA: 0x00171494 File Offset: 0x0016F694
		public override bool IncreasesPopulation
		{
			get
			{
				Site site = this.worldObject as Site;
				return site != null && site.IncreasesPopulation;
			}
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x001714BC File Offset: 0x0016F6BC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && !this.spawned)
			{
				int num = this.worldObject.Tile;
				if (num == -1)
				{
					if (!TileFinder.TryFindNewSiteTile(out num, 7, 27, false, TileFinderMode.Near, -1, false))
					{
						num = -1;
					}
				}
				else if (Find.WorldObjects.AnyWorldObjectAt(num))
				{
					if (!TileFinder.TryFindPassableTileWithTraversalDistance(num, 1, 50, out num, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x), false, TileFinderMode.Near, false, false))
					{
						num = -1;
					}
				}
				if (num != -1)
				{
					this.worldObject.Tile = num;
					Find.WorldObjects.Add(this.worldObject);
					this.spawned = true;
				}
			}
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x0017157C File Offset: 0x0016F77C
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			Site site;
			if ((site = (this.worldObject as Site)) != null)
			{
				for (int i = 0; i < site.parts.Count; i++)
				{
					if (site.parts[i].things != null)
					{
						for (int j = 0; j < site.parts[i].things.Count; j++)
						{
							if (site.parts[i].things[j].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00171618 File Offset: 0x0016F818
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			Scribe_Collections.Look<ThingDef>(ref this.defsToExcludeFromHyperlinks, "defsToExcludeFromHyperlinks", LookMode.Def, Array.Empty<object>());
			if (this.spawned)
			{
				Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
				return;
			}
			Scribe_Deep.Look<WorldObject>(ref this.worldObject, "worldObject", Array.Empty<object>());
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x00171694 File Offset: 0x0016F894
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, TileFinderMode.Near, -1, false))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null);
			}
		}

		// Token: 0x04002A79 RID: 10873
		public string inSignal;

		// Token: 0x04002A7A RID: 10874
		public WorldObject worldObject;

		// Token: 0x04002A7B RID: 10875
		public List<ThingDef> defsToExcludeFromHyperlinks;

		// Token: 0x04002A7C RID: 10876
		private bool spawned;
	}
}
