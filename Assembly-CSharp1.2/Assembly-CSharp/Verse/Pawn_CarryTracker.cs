using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000452 RID: 1106
	public class Pawn_CarryTracker : IThingHolder, IExposable
	{
		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001BD5 RID: 7125 RVA: 0x00019563 File Offset: 0x00017763
		public Thing CarriedThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001BD6 RID: 7126 RVA: 0x00019580 File Offset: 0x00017780
		public bool Full
		{
			get
			{
				return this.AvailableStackSpace(this.CarriedThing.def) <= 0;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001BD7 RID: 7127 RVA: 0x00019599 File Offset: 0x00017799
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x000195A1 File Offset: 0x000177A1
		public Pawn_CarryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000195BE File Offset: 0x000177BE
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x000195DA File Offset: 0x000177DA
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000195E2 File Offset: 0x000177E2
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000EDBD4 File Offset: 0x000EBDD4
		public int AvailableStackSpace(ThingDef td)
		{
			int num = this.MaxStackSpaceEver(td);
			if (this.CarriedThing != null)
			{
				num -= this.CarriedThing.stackCount;
			}
			return num;
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x000EDC00 File Offset: 0x000EBE00
		public int MaxStackSpaceEver(ThingDef td)
		{
			int b = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / td.VolumePerUnit);
			return Mathf.Min(td.stackLimit, b);
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x000EDC38 File Offset: 0x000EBE38
		public bool TryStartCarry(Thing item)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				Log.Error("Dead/downed pawn " + this.pawn + " tried to start carry item.", false);
				return false;
			}
			if (this.innerContainer.TryAdd(item, true))
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				return true;
			}
			return false;
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x000EDCBC File Offset: 0x000EBEBC
		public int TryStartCarry(Thing item, int count, bool reserve = true)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				Log.Error(string.Concat(new object[]
				{
					"Dead/downed pawn ",
					this.pawn,
					" tried to start carry ",
					item.ToStringSafe<Thing>()
				}), false);
				return 0;
			}
			count = Mathf.Min(count, this.AvailableStackSpace(item.def));
			count = Mathf.Min(count, item.stackCount);
			int num = this.innerContainer.TryAdd(item.SplitOff(count), count, true);
			if (num > 0)
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				if (reserve)
				{
					this.pawn.Reserve(this.CarriedThing, this.pawn.CurJob, 1, -1, null, true);
				}
			}
			return num;
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000EDDA8 File Offset: 0x000EBFA8
		public bool TryDropCarriedThing(IntVec3 dropLoc, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000EDE00 File Offset: 0x000EC000
		public bool TryDropCarriedThing(IntVec3 dropLoc, int count, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, count, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x000195F0 File Offset: 0x000177F0
		public void DestroyCarriedThing()
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000195FE File Offset: 0x000177FE
		public void CarryHandsTick()
		{
			this.innerContainer.ThingOwnerTick(true);
		}

		// Token: 0x04001420 RID: 5152
		public Pawn pawn;

		// Token: 0x04001421 RID: 5153
		public ThingOwner<Thing> innerContainer;
	}
}
