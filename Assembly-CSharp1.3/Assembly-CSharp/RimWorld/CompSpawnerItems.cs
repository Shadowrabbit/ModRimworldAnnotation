using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A5 RID: 4517
	public class CompSpawnerItems : ThingComp
	{
		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x06006CC6 RID: 27846 RVA: 0x002484C2 File Offset: 0x002466C2
		public CompProperties_SpawnerItems Props
		{
			get
			{
				return (CompProperties_SpawnerItems)this.props;
			}
		}

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x06006CC7 RID: 27847 RVA: 0x002484D0 File Offset: 0x002466D0
		public bool Active
		{
			get
			{
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				return comp == null || comp.Awake;
			}
		}

		// Token: 0x06006CC8 RID: 27848 RVA: 0x002484F4 File Offset: 0x002466F4
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield return new Command_Action
			{
				defaultLabel = "DEV: Spawn items",
				action = delegate()
				{
					this.SpawnItems();
				}
			};
			yield break;
		}

		// Token: 0x06006CC9 RID: 27849 RVA: 0x00248504 File Offset: 0x00246704
		private void SpawnItems()
		{
			ThingDef thingDef;
			if (this.Props.MatchingItems.TryRandomElement(out thingDef))
			{
				int stackCount = Mathf.CeilToInt(this.Props.approxMarketValuePerDay / thingDef.BaseMarketValue);
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = stackCount;
				GenPlace.TryPlaceThing(thing, this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, null, default(Rot4));
			}
		}

		// Token: 0x06006CCA RID: 27850 RVA: 0x00248574 File Offset: 0x00246774
		public override void CompTickRare()
		{
			if (!this.Active)
			{
				return;
			}
			this.ticksPassed += 250;
			if (this.ticksPassed >= this.Props.spawnInterval)
			{
				this.SpawnItems();
				this.ticksPassed -= this.Props.spawnInterval;
			}
		}

		// Token: 0x06006CCB RID: 27851 RVA: 0x002485CD File Offset: 0x002467CD
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x06006CCC RID: 27852 RVA: 0x002485E4 File Offset: 0x002467E4
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "NextSpawnedResourceIn".Translate() + ": " + (this.Props.spawnInterval - this.ticksPassed).ToStringTicksToPeriod(true, false, true, true);
			}
			return null;
		}

		// Token: 0x04003C7C RID: 15484
		private int ticksPassed;
	}
}
