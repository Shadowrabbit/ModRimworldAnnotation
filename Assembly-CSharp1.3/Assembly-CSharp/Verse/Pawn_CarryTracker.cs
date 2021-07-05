using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020002EE RID: 750
	public class Pawn_CarryTracker : IThingHolder, IExposable
	{
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x0007C81C File Offset: 0x0007AA1C
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

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001575 RID: 5493 RVA: 0x0007C839 File Offset: 0x0007AA39
		public bool Full
		{
			get
			{
				return this.AvailableStackSpace(this.CarriedThing.def) <= 0;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x0007C852 File Offset: 0x0007AA52
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0007C85A File Offset: 0x0007AA5A
		public Pawn_CarryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0007C877 File Offset: 0x0007AA77
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0007C893 File Offset: 0x0007AA93
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0007C89B File Offset: 0x0007AA9B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0007C8AC File Offset: 0x0007AAAC
		public int AvailableStackSpace(ThingDef td)
		{
			int num = this.MaxStackSpaceEver(td);
			if (this.CarriedThing != null)
			{
				num -= this.CarriedThing.stackCount;
			}
			return num;
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x0007C8D8 File Offset: 0x0007AAD8
		public int MaxStackSpaceEver(ThingDef td)
		{
			int b = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.CarryingCapacity, true) / td.VolumePerUnit);
			return Mathf.Min(td.stackLimit, b);
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0007C910 File Offset: 0x0007AB10
		public bool TryStartCarry(Thing item)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				Log.Error("Dead/downed pawn " + this.pawn + " tried to start carry item.");
				return false;
			}
			if (this.innerContainer.TryAdd(item, true))
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				return true;
			}
			return false;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0007C994 File Offset: 0x0007AB94
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
				}));
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

		// Token: 0x0600157F RID: 5503 RVA: 0x0007CA80 File Offset: 0x0007AC80
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

		// Token: 0x06001580 RID: 5504 RVA: 0x0007CAD8 File Offset: 0x0007ACD8
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

		// Token: 0x06001581 RID: 5505 RVA: 0x0007CB34 File Offset: 0x0007AD34
		public int CarriedCount(ThingDef def)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.def == def)
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0007CB98 File Offset: 0x0007AD98
		public void DestroyCarriedThing()
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0007CBA6 File Offset: 0x0007ADA6
		public void CarryHandsTick()
		{
			this.innerContainer.ThingOwnerTick(true);
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0007CBB4 File Offset: 0x0007ADB4
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.Drafted)
			{
				Pawn pawn = this.CarriedThing as Pawn;
				if (pawn != null)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandDropPawn".Translate(pawn),
						defaultDesc = "CommandDropPawnDesc".Translate(),
						action = delegate()
						{
							Thing thing;
							this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
						},
						icon = TexCommand.DropCarriedPawn
					};
				}
			}
			yield break;
		}

		// Token: 0x04000F21 RID: 3873
		public Pawn pawn;

		// Token: 0x04000F22 RID: 3874
		public ThingOwner<Thing> innerContainer;
	}
}
