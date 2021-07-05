using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001119 RID: 4377
	public class QuestPart_SpawnThing : QuestPart
	{
		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06005FA4 RID: 24484 RVA: 0x0004228B File Offset: 0x0004048B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.questLookTarget)
				{
					yield return this.innerSkyfallerThing ?? this.thing;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06005FA5 RID: 24485 RVA: 0x001E2900 File Offset: 0x001E0B00
		public override bool IncreasesPopulation
		{
			get
			{
				Pawn pawn = this.thing as Pawn;
				return pawn != null && PawnsArriveQuestPartUtility.IncreasesPopulation(Gen.YieldSingle<Pawn>(pawn), false, false);
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06005FA6 RID: 24486 RVA: 0x0004229B File Offset: 0x0004049B
		public MapParent MapParent
		{
			get
			{
				if (this.mapParentOfPawn != null)
				{
					return this.mapParentOfPawn.MapHeld.Parent;
				}
				return this.mapParent;
			}
		}

		// Token: 0x06005FA7 RID: 24487 RVA: 0x001E292C File Offset: 0x001E0B2C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.MapParent.HasMap)
			{
				IntVec3 center = IntVec3.Invalid;
				if (this.cell.IsValid)
				{
					center = this.cell;
				}
				else
				{
					Thing thing;
					if (this.tryLandInShipLandingZone && !DropCellFinder.TryFindShipLandingArea(this.MapParent.Map, out center, out thing))
					{
						if (thing != null)
						{
							Messages.Message("ShuttleBlocked".Translate("BlockedBy".Translate(thing).CapitalizeFirst()), thing, MessageTypeDefOf.NeutralEvent, true);
						}
						center = DropCellFinder.TryFindSafeLandingSpotCloseToColony(this.MapParent.Map, this.thing.def.Size, this.factionForFindingSpot, 2);
					}
					if (!center.IsValid && this.tryLandNearThing != null)
					{
						DropCellFinder.FindSafeLandingSpotNearAvoidingHostiles(this.tryLandNearThing, this.MapParent.Map, out center, 35, 15, 25, new IntVec2?(this.thing.def.size));
					}
					if (!center.IsValid && (!this.lookForSafeSpot || !DropCellFinder.FindSafeLandingSpot(out center, this.factionForFindingSpot, this.MapParent.Map, 35, 15, 25, new IntVec2?(this.thing.def.size))))
					{
						IntVec3 intVec = DropCellFinder.RandomDropSpot(this.MapParent.Map);
						if (!DropCellFinder.TryFindDropSpotNear(intVec, this.MapParent.Map, out center, false, false, false, new IntVec2?(this.thing.def.size)))
						{
							center = intVec;
						}
					}
				}
				GenPlace.TryPlaceThing(this.thing, center, this.MapParent.Map, ThingPlaceMode.Near, null, null, default(Rot4));
				this.spawned = true;
				Skyfaller skyfaller = this.thing as Skyfaller;
				if (skyfaller != null && skyfaller.innerContainer.Count == 1)
				{
					this.innerSkyfallerThing = skyfaller.innerContainer.First<Thing>();
					return;
				}
				this.innerSkyfallerThing = null;
			}
		}

		// Token: 0x06005FA8 RID: 24488 RVA: 0x000422BC File Offset: 0x000404BC
		public override bool QuestPartReserves(Pawn p)
		{
			return p == this.thing || (this.thing is Skyfaller && ((Skyfaller)this.thing).innerContainer.Contains(p));
		}

		// Token: 0x06005FA9 RID: 24489 RVA: 0x001E2B40 File Offset: 0x001E0D40
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			if (!this.spawned && (this.thing == null || !(this.thing is Pawn)))
			{
				Scribe_Deep.Look<Thing>(ref this.thing, "thing", Array.Empty<object>());
			}
			else
			{
				Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			}
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.lookForSafeSpot, "lookForSafeSpot", false, false);
			Scribe_References.Look<Faction>(ref this.factionForFindingSpot, "factionForFindingSpot", false);
			Scribe_Values.Look<bool>(ref this.questLookTarget, "questLookTarget", true, false);
			Scribe_References.Look<Thing>(ref this.innerSkyfallerThing, "innerSkyfallerThing", false);
			Scribe_Values.Look<bool>(ref this.tryLandInShipLandingZone, "tryLandInShipLandingZone", false, false);
			Scribe_References.Look<Thing>(ref this.tryLandNearThing, "tryLandNearThing", false);
			Scribe_References.Look<Pawn>(ref this.mapParentOfPawn, "mapParentOfPawn", false);
		}

		// Token: 0x06005FAA RID: 24490 RVA: 0x001E2C64 File Offset: 0x001E0E64
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
			}
		}

		// Token: 0x04003FED RID: 16365
		public string inSignal;

		// Token: 0x04003FEE RID: 16366
		public Thing thing;

		// Token: 0x04003FEF RID: 16367
		public Faction factionForFindingSpot;

		// Token: 0x04003FF0 RID: 16368
		public MapParent mapParent;

		// Token: 0x04003FF1 RID: 16369
		public IntVec3 cell = IntVec3.Invalid;

		// Token: 0x04003FF2 RID: 16370
		public bool questLookTarget = true;

		// Token: 0x04003FF3 RID: 16371
		public bool lookForSafeSpot;

		// Token: 0x04003FF4 RID: 16372
		public bool tryLandInShipLandingZone;

		// Token: 0x04003FF5 RID: 16373
		public Thing tryLandNearThing;

		// Token: 0x04003FF6 RID: 16374
		public Pawn mapParentOfPawn;

		// Token: 0x04003FF7 RID: 16375
		private Thing innerSkyfallerThing;

		// Token: 0x04003FF8 RID: 16376
		private bool spawned;
	}
}
