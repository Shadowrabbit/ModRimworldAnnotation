using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000453 RID: 1107
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x000EDE5C File Offset: 0x000EC05C
		// (set) Token: 0x06001BE5 RID: 7141 RVA: 0x000EDEA8 File Offset: 0x000EC0A8
		public ThingWithComps Primary
		{
			get
			{
				for (int i = 0; i < this.equipment.Count; i++)
				{
					if (this.equipment[i].def.equipmentType == EquipmentType.Primary)
					{
						return this.equipment[i];
					}
				}
				return null;
			}
			private set
			{
				if (this.Primary == value)
				{
					return;
				}
				if (value != null && value.def.equipmentType != EquipmentType.Primary)
				{
					Log.Error("Tried to set non-primary equipment as primary.", false);
					return;
				}
				if (this.Primary != null)
				{
					this.equipment.Remove(this.Primary);
				}
				if (value != null)
				{
					this.equipment.TryAdd(value, true);
				}
				if (this.pawn.drafter != null)
				{
					this.pawn.drafter.Notify_PrimaryWeaponChanged();
				}
			}
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001BE6 RID: 7142 RVA: 0x0001960C File Offset: 0x0001780C
		public CompEquippable PrimaryEq
		{
			get
			{
				if (this.Primary == null)
				{
					return null;
				}
				return this.Primary.GetComp<CompEquippable>();
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x00019623 File Offset: 0x00017823
		public List<ThingWithComps> AllEquipmentListForReading
		{
			get
			{
				return this.equipment.InnerListForReading;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001BE8 RID: 7144 RVA: 0x00019630 File Offset: 0x00017830
		public IEnumerable<Verb> AllEquipmentVerbs
		{
			get
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					ThingWithComps thingWithComps = list[i];
					List<Verb> verbs = thingWithComps.GetComp<CompEquippable>().AllVerbs;
					for (int j = 0; j < verbs.Count; j = num + 1)
					{
						yield return verbs[j];
						num = j;
					}
					verbs = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x00019640 File Offset: 0x00017840
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x00019648 File Offset: 0x00017848
		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x000EDF24 File Offset: 0x000EC124
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<ThingWithComps>>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_References.Look<Thing>(ref this.bondedWeapon, "bondedWeapon", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					foreach (Verb verb in allEquipmentListForReading[i].GetComp<CompEquippable>().AllVerbs)
					{
						verb.caster = this.pawn;
					}
				}
			}
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x000EDFD0 File Offset: 0x000EC1D0
		public void EquipmentTrackerTick()
		{
			List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				allEquipmentListForReading[i].GetComp<CompEquippable>().verbTracker.VerbsTick();
			}
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x00019663 File Offset: 0x00017863
		public void EquipmentTrackerTickRare()
		{
			this.equipment.ThingOwnerTickRare(true);
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x00019671 File Offset: 0x00017871
		public bool HasAnything()
		{
			return this.equipment.Any;
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000EE00C File Offset: 0x000EC20C
		public void MakeRoomFor(ThingWithComps eq)
		{
			if (eq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				ThingWithComps thingWithComps;
				if (this.TryDropEquipment(this.Primary, out thingWithComps, this.pawn.Position, true))
				{
					if (thingWithComps != null)
					{
						thingWithComps.SetForbidden(false, true);
						return;
					}
				}
				else
				{
					Log.Error(this.pawn + " couldn't make room for equipment " + eq, false);
				}
			}
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0001967E File Offset: 0x0001787E
		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000EE070 File Offset: 0x000EC270
		public bool TryDropEquipment(ThingWithComps eq, out ThingWithComps resultingEq, IntVec3 pos, bool forbid = true)
		{
			if (!pos.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					this.pawn,
					" tried to drop ",
					eq,
					" at invalid cell."
				}), false);
				resultingEq = null;
				return false;
			}
			if (this.equipment.TryDrop(eq, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingEq, null, null))
			{
				if (resultingEq != null)
				{
					resultingEq.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000EE0E8 File Offset: 0x000EC2E8
		public void DropAllEquipment(IntVec3 pos, bool forbid = true)
		{
			for (int i = this.equipment.Count - 1; i >= 0; i--)
			{
				ThingWithComps thingWithComps;
				this.TryDropEquipment(this.equipment[i], out thingWithComps, pos, forbid);
			}
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x0001968D File Offset: 0x0001788D
		public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
		{
			return this.equipment.TryTransferToContainer(eq, container, true);
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x0001969D File Offset: 0x0001789D
		public void DestroyEquipment(ThingWithComps eq)
		{
			if (!this.equipment.Contains(eq))
			{
				Log.Warning("Tried to destroy equipment " + eq + " but it's not here.", false);
				return;
			}
			this.Remove(eq);
			eq.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000196D2 File Offset: 0x000178D2
		public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
		{
			this.equipment.ClearAndDestroyContents(mode);
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000196E0 File Offset: 0x000178E0
		public bool Contains(Thing eq)
		{
			return this.equipment.Contains(eq);
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000196EE File Offset: 0x000178EE
		internal void Notify_PrimaryDestroyed()
		{
			if (this.Primary != null)
			{
				this.Remove(this.Primary);
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.CancelBusyStanceSoft();
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000EE124 File Offset: 0x000EC324
		public void AddEquipment(ThingWithComps newEq)
		{
			if (newEq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Pawn ",
					this.pawn.LabelCap,
					" got primaryInt equipment ",
					newEq,
					" while already having primaryInt equipment ",
					this.Primary
				}), false);
				return;
			}
			this.equipment.TryAdd(newEq, true);
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x00019721 File Offset: 0x00017921
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (PawnAttackGizmoUtility.CanShowEquipmentGizmos())
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					ThingWithComps thingWithComps = list[i];
					foreach (Command command in thingWithComps.GetComp<CompEquippable>().GetVerbsCommands())
					{
						switch (i)
						{
						case 0:
							command.hotKey = KeyBindingDefOf.Misc1;
							break;
						case 1:
							command.hotKey = KeyBindingDefOf.Misc2;
							break;
						case 2:
							command.hotKey = KeyBindingDefOf.Misc3;
							break;
						}
						yield return command;
					}
					IEnumerator<Command> enumerator = null;
					num = i;
				}
				list = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x000EE19C File Offset: 0x000EC39C
		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			foreach (Verb verb in eq.GetComp<CompEquippable>().AllVerbs)
			{
				verb.caster = this.pawn;
				verb.Notify_PickedUp();
			}
			eq.Notify_Equipped(this.pawn);
			if (ModsConfig.RoyaltyActive && eq.def.equipmentType == EquipmentType.Primary && this.bondedWeapon != null && !this.bondedWeapon.Destroyed)
			{
				CompBladelinkWeapon compBladelinkWeapon = this.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.Notify_WieldedOtherWeapon();
				}
			}
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x000EE248 File Offset: 0x000EC448
		public void Notify_EquipmentRemoved(ThingWithComps eq)
		{
			CompEquippable comp = eq.GetComp<CompEquippable>();
			if (comp != null)
			{
				comp.Notify_EquipmentLost();
			}
			if (ModsConfig.RoyaltyActive)
			{
				CompBladelinkWeapon compBladelinkWeapon = eq.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.Notify_EquipmentLost(this.pawn);
				}
			}
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x000EE284 File Offset: 0x000EC484
		public void Notify_PawnSpawned()
		{
			if (this.HasAnything() && this.pawn.Downed && this.pawn.GetPosture() != PawnPosture.LayingInBed)
			{
				if (this.pawn.kindDef.destroyGearOnDrop)
				{
					this.DestroyAllEquipment(DestroyMode.Vanish);
					return;
				}
				this.DropAllEquipment(this.pawn.Position, true);
			}
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x000EE2E0 File Offset: 0x000EC4E0
		public void Notify_PawnDied()
		{
			if (ModsConfig.RoyaltyActive && this.bondedWeapon != null)
			{
				CompBladelinkWeapon compBladelinkWeapon = this.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.UnBond();
				}
			}
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x000EE314 File Offset: 0x000EC514
		public void Notify_KilledPawn()
		{
			foreach (ThingWithComps thingWithComps in this.equipment)
			{
				thingWithComps.Notify_KilledPawn(this.pawn);
			}
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x00019731 File Offset: 0x00017931
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.equipment;
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x00019739 File Offset: 0x00017939
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04001422 RID: 5154
		public Pawn pawn;

		// Token: 0x04001423 RID: 5155
		private ThingOwner<ThingWithComps> equipment;

		// Token: 0x04001424 RID: 5156
		public Thing bondedWeapon;
	}
}
