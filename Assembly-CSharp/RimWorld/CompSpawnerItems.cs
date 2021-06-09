using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001859 RID: 6233
	public class CompSpawnerItems : ThingComp
	{
		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x06008A46 RID: 35398 RVA: 0x0005CCC1 File Offset: 0x0005AEC1
		public CompProperties_SpawnerItems Props
		{
			get
			{
				return (CompProperties_SpawnerItems)this.props;
			}
		}

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x06008A47 RID: 35399 RVA: 0x00286580 File Offset: 0x00284780
		public bool Active
		{
			get
			{
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				return comp == null || comp.Awake;
			}
		}

		// Token: 0x06008A48 RID: 35400 RVA: 0x0005CCCE File Offset: 0x0005AECE
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

		// Token: 0x06008A49 RID: 35401 RVA: 0x002865A4 File Offset: 0x002847A4
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

		// Token: 0x06008A4A RID: 35402 RVA: 0x00286614 File Offset: 0x00284814
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

		// Token: 0x06008A4B RID: 35403 RVA: 0x0005CCDE File Offset: 0x0005AEDE
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x06008A4C RID: 35404 RVA: 0x00286670 File Offset: 0x00284870
		public override string CompInspectStringExtra()
		{
			if (this.Active)
			{
				return "NextSpawnedResourceIn".Translate() + ": " + (this.Props.spawnInterval - this.ticksPassed).ToStringTicksToPeriod(true, false, true, true);
			}
			return null;
		}

		// Token: 0x040058B0 RID: 22704
		private int ticksPassed;
	}
}
