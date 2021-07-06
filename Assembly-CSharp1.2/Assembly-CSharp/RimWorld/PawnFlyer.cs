using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001715 RID: 5909
	public class PawnFlyer : Thing, IThingHolder
	{
		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x06008235 RID: 33333 RVA: 0x0005773E File Offset: 0x0005593E
		public Pawn FlyingPawn
		{
			get
			{
				if (this.innerContainer.InnerListForReading.Count <= 0)
				{
					return null;
				}
				return this.innerContainer.InnerListForReading[0] as Pawn;
			}
		}

		// Token: 0x06008236 RID: 33334 RVA: 0x0005776B File Offset: 0x0005596B
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06008237 RID: 33335 RVA: 0x00057773 File Offset: 0x00055973
		public PawnFlyer()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06008238 RID: 33336 RVA: 0x0005778F File Offset: 0x0005598F
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x06008239 RID: 33337 RVA: 0x00269C40 File Offset: 0x00267E40
		public Vector3 DestinationPos
		{
			get
			{
				Pawn flyingPawn = this.FlyingPawn;
				return GenThing.TrueCenter(base.Position, flyingPawn.Rotation, flyingPawn.def.size, flyingPawn.def.Altitude);
			}
		}

		// Token: 0x0600823A RID: 33338 RVA: 0x00269C7C File Offset: 0x00267E7C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				float num = Mathf.Max(this.flightDistance, 1f) / this.def.pawnFlyer.flightSpeed;
				num = Mathf.Max(num, this.def.pawnFlyer.flightDurationMin);
				this.ticksFlightTime = num.SecondsToTicks();
				this.ticksFlying = 0;
			}
		}

		// Token: 0x0600823B RID: 33339 RVA: 0x00269CE0 File Offset: 0x00267EE0
		protected virtual void RespawnPawn()
		{
			Pawn flyingPawn = this.FlyingPawn;
			Thing thing;
			this.innerContainer.TryDrop_NewTmp(flyingPawn, base.Position, flyingPawn.MapHeld, ThingPlaceMode.Direct, out thing, null, null, false);
			if (flyingPawn.drafter != null)
			{
				flyingPawn.drafter.Drafted = this.pawnWasDrafted;
			}
			if (this.pawnWasSelected && Find.CurrentMap == flyingPawn.Map)
			{
				Find.Selector.Unshelve(flyingPawn, false, true);
			}
			if (this.jobQueue != null)
			{
				flyingPawn.jobs.RestoreCapturedJobs(this.jobQueue, true);
			}
		}

		// Token: 0x0600823C RID: 33340 RVA: 0x00269D68 File Offset: 0x00267F68
		public override void Tick()
		{
			if (this.ticksFlying >= this.ticksFlightTime)
			{
				this.RespawnPawn();
				this.Destroy(DestroyMode.Vanish);
			}
			else
			{
				if (this.ticksFlying % 5 == 0)
				{
					this.CheckDestination();
				}
				this.innerContainer.ThingOwnerTick(true);
			}
			this.ticksFlying++;
		}

		// Token: 0x0600823D RID: 33341 RVA: 0x00269DBC File Offset: 0x00267FBC
		private void CheckDestination()
		{
			if (!Verb_Jump.ValidJumpTarget(base.Map, base.Position))
			{
				int num = GenRadial.NumCellsInRadius(3.9f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = base.Position + GenRadial.RadialPattern[i];
					if (Verb_Jump.ValidJumpTarget(base.Map, intVec))
					{
						base.Position = intVec;
						return;
					}
				}
			}
		}

		// Token: 0x0600823E RID: 33342 RVA: 0x00269E20 File Offset: 0x00268020
		public static PawnFlyer MakeFlyer(ThingDef flyingDef, Pawn pawn, IntVec3 destCell)
		{
			PawnFlyer pawnFlyer = (PawnFlyer)ThingMaker.MakeThing(flyingDef, null);
			if (!pawnFlyer.ValidateFlyer())
			{
				return null;
			}
			pawnFlyer.startVec = pawn.TrueCenter();
			pawnFlyer.flightDistance = pawn.Position.DistanceTo(destCell);
			pawnFlyer.pawnWasDrafted = pawn.Drafted;
			pawnFlyer.pawnWasSelected = Find.Selector.IsSelected(pawn);
			if (pawnFlyer.pawnWasDrafted)
			{
				Find.Selector.ShelveSelected(pawn);
			}
			pawnFlyer.jobQueue = pawn.jobs.CaptureAndClearJobQueue();
			pawn.DeSpawn(DestroyMode.Vanish);
			if (!pawnFlyer.innerContainer.TryAdd(pawn, true))
			{
				Log.Error("Could not add " + pawn.ToStringSafe<Pawn>() + " to a flyer.", false);
				pawn.Destroy(DestroyMode.Vanish);
			}
			return pawnFlyer;
		}

		// Token: 0x0600823F RID: 33343 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool ValidateFlyer()
		{
			return true;
		}

		// Token: 0x06008240 RID: 33344 RVA: 0x00269EDC File Offset: 0x002680DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<Vector3>(ref this.startVec, "startVec", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.flightDistance, "flightDistance", 0f, false);
			Scribe_Values.Look<bool>(ref this.pawnWasDrafted, "pawnWasDrafted", false, false);
			Scribe_Values.Look<bool>(ref this.pawnWasSelected, "pawnWasSelected", false, false);
			Scribe_Values.Look<int>(ref this.ticksFlightTime, "ticksFlightTime", 0, false);
			Scribe_Values.Look<int>(ref this.ticksFlying, "ticksFlying", 0, false);
			Scribe_Deep.Look<JobQueue>(ref this.jobQueue, "jobQueue", Array.Empty<object>());
		}

		// Token: 0x0400546A RID: 21610
		private ThingOwner<Thing> innerContainer;

		// Token: 0x0400546B RID: 21611
		protected Vector3 startVec;

		// Token: 0x0400546C RID: 21612
		private float flightDistance;

		// Token: 0x0400546D RID: 21613
		private bool pawnWasDrafted;

		// Token: 0x0400546E RID: 21614
		private bool pawnWasSelected;

		// Token: 0x0400546F RID: 21615
		protected int ticksFlightTime = 120;

		// Token: 0x04005470 RID: 21616
		protected int ticksFlying;

		// Token: 0x04005471 RID: 21617
		private JobQueue jobQueue;
	}
}
