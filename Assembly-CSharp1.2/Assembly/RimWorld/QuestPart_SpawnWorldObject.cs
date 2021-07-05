using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111B RID: 4379
	public class QuestPart_SpawnWorldObject : QuestPart
	{
		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06005FB6 RID: 24502 RVA: 0x0004235B File Offset: 0x0004055B
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

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06005FB7 RID: 24503 RVA: 0x001E2E20 File Offset: 0x001E1020
		public override bool IncreasesPopulation
		{
			get
			{
				Site site = this.worldObject as Site;
				return site != null && site.IncreasesPopulation;
			}
		}

		// Token: 0x06005FB8 RID: 24504 RVA: 0x001E2E48 File Offset: 0x001E1048
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && !this.spawned)
			{
				int num = this.worldObject.Tile;
				if (num == -1)
				{
					if (!TileFinder.TryFindNewSiteTile(out num, 7, 27, false, true, -1))
					{
						num = -1;
					}
				}
				else if (Find.WorldObjects.AnyWorldObjectAt(num))
				{
					if (!TileFinder.TryFindPassableTileWithTraversalDistance(num, 1, 50, out num, (int x) => !Find.WorldObjects.AnyWorldObjectAt(x), false, true, false))
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

		// Token: 0x06005FB9 RID: 24505 RVA: 0x001E2F08 File Offset: 0x001E1108
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

		// Token: 0x06005FBA RID: 24506 RVA: 0x001E2FA4 File Offset: 0x001E11A4
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

		// Token: 0x06005FBB RID: 24507 RVA: 0x001E3020 File Offset: 0x001E1220
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			int tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, -1))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null);
			}
		}

		// Token: 0x04003FFE RID: 16382
		public string inSignal;

		// Token: 0x04003FFF RID: 16383
		public WorldObject worldObject;

		// Token: 0x04004000 RID: 16384
		public List<ThingDef> defsToExcludeFromHyperlinks;

		// Token: 0x04004001 RID: 16385
		private bool spawned;
	}
}
