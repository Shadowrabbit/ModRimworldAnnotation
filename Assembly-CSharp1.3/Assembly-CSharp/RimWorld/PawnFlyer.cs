using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020010B8 RID: 4280
	public class PawnFlyer : Thing, IThingHolder
	{
		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x06006642 RID: 26178 RVA: 0x0022876D File Offset: 0x0022696D
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

		// Token: 0x06006643 RID: 26179 RVA: 0x0022879A File Offset: 0x0022699A
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06006644 RID: 26180 RVA: 0x002287A2 File Offset: 0x002269A2
		public PawnFlyer()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06006645 RID: 26181 RVA: 0x002287BE File Offset: 0x002269BE
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x06006646 RID: 26182 RVA: 0x002287CC File Offset: 0x002269CC
		public Vector3 DestinationPos
		{
			get
			{
				Pawn flyingPawn = this.FlyingPawn;
				return GenThing.TrueCenter(base.Position, flyingPawn.Rotation, flyingPawn.def.size, flyingPawn.def.Altitude);
			}
		}

		// Token: 0x06006647 RID: 26183 RVA: 0x00228808 File Offset: 0x00226A08
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

		// Token: 0x06006648 RID: 26184 RVA: 0x0022886C File Offset: 0x00226A6C
		protected virtual void RespawnPawn()
		{
			Pawn flyingPawn = this.FlyingPawn;
			Thing thing;
			this.innerContainer.TryDrop(flyingPawn, base.Position, flyingPawn.MapHeld, ThingPlaceMode.Direct, out thing, null, null, false);
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

		// Token: 0x06006649 RID: 26185 RVA: 0x002288F4 File Offset: 0x00226AF4
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

		// Token: 0x0600664A RID: 26186 RVA: 0x00228948 File Offset: 0x00226B48
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

		// Token: 0x0600664B RID: 26187 RVA: 0x002289AC File Offset: 0x00226BAC
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
				Log.Error("Could not add " + pawn.ToStringSafe<Pawn>() + " to a flyer.");
				pawn.Destroy(DestroyMode.Vanish);
			}
			return pawnFlyer;
		}

		// Token: 0x0600664C RID: 26188 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool ValidateFlyer()
		{
			return true;
		}

		// Token: 0x0600664D RID: 26189 RVA: 0x00228A68 File Offset: 0x00226C68
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

		// Token: 0x040039B9 RID: 14777
		private ThingOwner<Thing> innerContainer;

		// Token: 0x040039BA RID: 14778
		protected Vector3 startVec;

		// Token: 0x040039BB RID: 14779
		private float flightDistance;

		// Token: 0x040039BC RID: 14780
		private bool pawnWasDrafted;

		// Token: 0x040039BD RID: 14781
		private bool pawnWasSelected;

		// Token: 0x040039BE RID: 14782
		protected int ticksFlightTime = 120;

		// Token: 0x040039BF RID: 14783
		protected int ticksFlying;

		// Token: 0x040039C0 RID: 14784
		private JobQueue jobQueue;
	}
}
