using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020008ED RID: 2285
	public class ShipJob_Arrive : ShipJob
	{
		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06003BE6 RID: 15334 RVA: 0x0014DEFA File Offset: 0x0014C0FA
		protected override bool ShouldEnd
		{
			get
			{
				return this.transportShip.shipThing.Spawned;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06003BE7 RID: 15335 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool Interruptible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0014DF0C File Offset: 0x0014C10C
		public override bool TryStart()
		{
			if (!base.TryStart())
			{
				return false;
			}
			if (!this.mapParent.HasMap)
			{
				Log.Error("Trying to start ShipJob_Arrive with a null map.");
				return false;
			}
			if (!this.cell.IsValid)
			{
				this.cell = DropCellFinder.GetBestShuttleLandingSpot(this.mapParent.Map, this.factionForArrival ?? Faction.OfPlayer);
			}
			ThingOwner innerContainer = this.transportShip.TransporterComp.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn p;
				if ((p = (innerContainer[i] as Pawn)) != null && p.IsWorldPawn())
				{
					Find.WorldPawns.RemovePawn(p);
				}
			}
			GenSpawn.Spawn(SkyfallerMaker.MakeSkyfaller(this.transportShip.def.arrivingSkyfaller, this.transportShip.shipThing), this.cell, this.mapParent.Map, WipeMode.Vanish);
			QuestUtility.SendQuestTargetSignals(this.transportShip.questTags, "Arrived", this.transportShip.Named("SUBJECT"));
			return true;
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x0014E014 File Offset: 0x0014C214
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Faction>(ref this.factionForArrival, "factionForArrival", false);
		}

		// Token: 0x0400208E RID: 8334
		public IntVec3 cell = IntVec3.Invalid;

		// Token: 0x0400208F RID: 8335
		public MapParent mapParent;

		// Token: 0x04002090 RID: 8336
		public Faction factionForArrival;
	}
}
