using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAB RID: 2987
	public class QuestPart_SpawnThing : QuestPart
	{
		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x060045AE RID: 17838 RVA: 0x00171049 File Offset: 0x0016F249
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

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x060045AF RID: 17839 RVA: 0x0017105C File Offset: 0x0016F25C
		public override bool IncreasesPopulation
		{
			get
			{
				Pawn pawn = this.thing as Pawn;
				return pawn != null && PawnsArriveQuestPartUtility.IncreasesPopulation(Gen.YieldSingle<Pawn>(pawn), false, false);
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x060045B0 RID: 17840 RVA: 0x00171087 File Offset: 0x0016F287
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

		// Token: 0x060045B1 RID: 17841 RVA: 0x001710A8 File Offset: 0x0016F2A8
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
						IntVec3 intVec = DropCellFinder.RandomDropSpot(this.MapParent.Map, true);
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

		// Token: 0x060045B2 RID: 17842 RVA: 0x001712BB File Offset: 0x0016F4BB
		public override bool QuestPartReserves(Pawn p)
		{
			return p == this.thing || (this.thing is Skyfaller && ((Skyfaller)this.thing).innerContainer.Contains(p));
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x001712F0 File Offset: 0x0016F4F0
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

		// Token: 0x060045B4 RID: 17844 RVA: 0x00171414 File Offset: 0x0016F614
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

		// Token: 0x04002A6D RID: 10861
		public string inSignal;

		// Token: 0x04002A6E RID: 10862
		public Thing thing;

		// Token: 0x04002A6F RID: 10863
		public Faction factionForFindingSpot;

		// Token: 0x04002A70 RID: 10864
		public MapParent mapParent;

		// Token: 0x04002A71 RID: 10865
		public IntVec3 cell = IntVec3.Invalid;

		// Token: 0x04002A72 RID: 10866
		public bool questLookTarget = true;

		// Token: 0x04002A73 RID: 10867
		public bool lookForSafeSpot;

		// Token: 0x04002A74 RID: 10868
		public bool tryLandInShipLandingZone;

		// Token: 0x04002A75 RID: 10869
		public Thing tryLandNearThing;

		// Token: 0x04002A76 RID: 10870
		public Pawn mapParentOfPawn;

		// Token: 0x04002A77 RID: 10871
		private Thing innerSkyfallerThing;

		// Token: 0x04002A78 RID: 10872
		private bool spawned;
	}
}
