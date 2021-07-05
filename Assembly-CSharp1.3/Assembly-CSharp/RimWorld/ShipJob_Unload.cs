using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008F0 RID: 2288
	public class ShipJob_Unload : ShipJob
	{
		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x0014E398 File Offset: 0x0014C598
		protected override bool ShouldEnd
		{
			get
			{
				return this.dropMode == TransportShipDropMode.None;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06003BF2 RID: 15346 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool ShowGizmos
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06003BF3 RID: 15347 RVA: 0x0014E3A3 File Offset: 0x0014C5A3
		private Thing ShipThing
		{
			get
			{
				return this.transportShip.shipThing;
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x0014E3B0 File Offset: 0x0014C5B0
		public override bool TryStart()
		{
			return this.transportShip.ShipExistsAndIsSpawned && base.TryStart();
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x0014E3C7 File Offset: 0x0014C5C7
		public override void Tick()
		{
			base.Tick();
			if (this.ShipThing.IsHashIntervalTick(60))
			{
				this.Drop();
			}
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x0014E3E4 File Offset: 0x0014C5E4
		private void Drop()
		{
			Thing thingToDrop = null;
			float num = 0f;
			Map map = this.ShipThing.Map;
			for (int i = 0; i < this.transportShip.TransporterComp.innerContainer.Count; i++)
			{
				Thing thing = this.transportShip.TransporterComp.innerContainer[i];
				float dropPriority = this.GetDropPriority(thing);
				if (dropPriority > num)
				{
					thingToDrop = thing;
					num = dropPriority;
				}
			}
			if (thingToDrop != null)
			{
				IntVec3 dropLoc = this.ShipThing.Position + ShipJob_Unload.DropoffSpotOffset;
				Thing thing2;
				if (this.transportShip.TransporterComp.innerContainer.TryDrop(thingToDrop, dropLoc, map, ThingPlaceMode.Near, out thing2, null, delegate(IntVec3 c)
				{
					Pawn pawn2;
					return !c.Fogged(map) && ((pawn2 = (thingToDrop as Pawn)) == null || !pawn2.Downed || c.GetFirstPawn(map) == null);
				}, !(thingToDrop is Pawn)))
				{
					this.transportShip.TransporterComp.Notify_ThingRemoved(thingToDrop);
					this.droppedThings.Add(thingToDrop);
					thingToDrop.SetForbidden(false, false);
					Pawn pawn;
					if ((pawn = (thingToDrop as Pawn)) != null)
					{
						if (pawn.IsColonist && pawn.Spawned && !map.IsPlayerHome)
						{
							pawn.drafter.Drafted = true;
						}
						if (pawn.guest != null && pawn.guest.IsPrisoner)
						{
							pawn.guest.WaitInsteadOfEscapingForDefaultTicks();
							return;
						}
					}
				}
			}
			else
			{
				this.End();
			}
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x0014E56C File Offset: 0x0014C76C
		private float GetDropPriority(Thing t)
		{
			Pawn p;
			if ((p = (t as Pawn)) == null)
			{
				return 0.25f;
			}
			if (this.droppedThings.Contains(t))
			{
				return 0f;
			}
			if (this.dropMode == TransportShipDropMode.NonRequired && this.transportShip.ShuttleComp.IsRequired(t))
			{
				return 0f;
			}
			Lord lord = p.GetLord();
			LordToil_EnterShuttleOrLeave lordToil_EnterShuttleOrLeave;
			if (((lord != null) ? lord.CurLordToil : null) != null && (lordToil_EnterShuttleOrLeave = (lord.CurLordToil as LordToil_EnterShuttleOrLeave)) != null && lordToil_EnterShuttleOrLeave.shuttle == this.ShipThing)
			{
				return 0f;
			}
			if (!p.AnimalOrWildMan())
			{
				return 1f;
			}
			return 0.5f;
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x0014E60D File Offset: 0x0014C80D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.droppedThings, "droppedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<TransportShipDropMode>(ref this.dropMode, "dropMode", TransportShipDropMode.None, false);
		}

		// Token: 0x04002099 RID: 8345
		public TransportShipDropMode dropMode = TransportShipDropMode.All;

		// Token: 0x0400209A RID: 8346
		private List<Thing> droppedThings = new List<Thing>();

		// Token: 0x0400209B RID: 8347
		public static readonly IntVec3 DropoffSpotOffset = IntVec3.South * 2;

		// Token: 0x0400209C RID: 8348
		private const int DropInterval = 60;
	}
}
