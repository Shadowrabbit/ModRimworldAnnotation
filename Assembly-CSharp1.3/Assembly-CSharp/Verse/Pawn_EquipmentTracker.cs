using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020002EF RID: 751
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x0007CBF4 File Offset: 0x0007ADF4
		// (set) Token: 0x06001587 RID: 5511 RVA: 0x0007CC40 File Offset: 0x0007AE40
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
					Log.Error("Tried to set non-primary equipment as primary.");
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

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x0007CCBB File Offset: 0x0007AEBB
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

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001589 RID: 5513 RVA: 0x0007CCD2 File Offset: 0x0007AED2
		public List<ThingWithComps> AllEquipmentListForReading
		{
			get
			{
				return this.equipment.InnerListForReading;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x0007CCDF File Offset: 0x0007AEDF
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

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x0600158B RID: 5515 RVA: 0x0007CCEF File Offset: 0x0007AEEF
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0007CCF7 File Offset: 0x0007AEF7
		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0007CD14 File Offset: 0x0007AF14
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

		// Token: 0x0600158E RID: 5518 RVA: 0x0007CDC0 File Offset: 0x0007AFC0
		public void EquipmentTrackerTick()
		{
			List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				allEquipmentListForReading[i].GetComp<CompEquippable>().verbTracker.VerbsTick();
			}
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0007CDFB File Offset: 0x0007AFFB
		public void EquipmentTrackerTickRare()
		{
			this.equipment.ThingOwnerTickRare(true);
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x0007CE09 File Offset: 0x0007B009
		public bool HasAnything()
		{
			return this.equipment.Any;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0007CE18 File Offset: 0x0007B018
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
					Log.Error(this.pawn + " couldn't make room for equipment " + eq);
				}
			}
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0007CE79 File Offset: 0x0007B079
		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0007CE88 File Offset: 0x0007B088
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
				}));
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

		// Token: 0x06001594 RID: 5524 RVA: 0x0007CF00 File Offset: 0x0007B100
		public void DropAllEquipment(IntVec3 pos, bool forbid = true)
		{
			for (int i = this.equipment.Count - 1; i >= 0; i--)
			{
				ThingWithComps thingWithComps;
				this.TryDropEquipment(this.equipment[i], out thingWithComps, pos, forbid);
			}
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0007CF3C File Offset: 0x0007B13C
		public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
		{
			return this.equipment.TryTransferToContainer(eq, container, true);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0007CF4C File Offset: 0x0007B14C
		public void DestroyEquipment(ThingWithComps eq)
		{
			if (!this.equipment.Contains(eq))
			{
				Log.Warning("Tried to destroy equipment " + eq + " but it's not here.");
				return;
			}
			this.Remove(eq);
			eq.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0007CF80 File Offset: 0x0007B180
		public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
		{
			this.equipment.ClearAndDestroyContents(mode);
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x0007CF8E File Offset: 0x0007B18E
		public bool Contains(Thing eq)
		{
			return this.equipment.Contains(eq);
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0007CF9C File Offset: 0x0007B19C
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

		// Token: 0x0600159A RID: 5530 RVA: 0x0007CFD0 File Offset: 0x0007B1D0
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
				}));
				return;
			}
			this.equipment.TryAdd(newEq, true);
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0007D045 File Offset: 0x0007B245
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (PawnAttackGizmoUtility.CanShowEquipmentGizmos())
			{
				try
				{
					Pawn_EquipmentTracker.tmpKeybindings.Add(KeyBindingDefOf.Misc1);
					Pawn_EquipmentTracker.tmpKeybindings.Add(KeyBindingDefOf.Misc2);
					Pawn_EquipmentTracker.tmpKeybindings.Add(KeyBindingDefOf.Misc3);
					List<ThingWithComps> list = this.AllEquipmentListForReading;
					ThingWithComps primaryMelee = list.FirstOrDefault((ThingWithComps w) => w.def.IsMeleeWeapon);
					ThingWithComps primaryRanged = list.FirstOrDefault((ThingWithComps w) => w.def.IsRangedWeapon);
					if (primaryMelee != null)
					{
						KeyBindingDef misc = KeyBindingDefOf.Misc2;
						foreach (Gizmo gizmo in Pawn_EquipmentTracker.<GetGizmos>g__YieldGizmos|30_0(primaryMelee, misc))
						{
							yield return gizmo;
						}
						IEnumerator<Gizmo> enumerator = null;
					}
					if (primaryRanged != null)
					{
						KeyBindingDef misc2 = KeyBindingDefOf.Misc1;
						foreach (Gizmo gizmo2 in Pawn_EquipmentTracker.<GetGizmos>g__YieldGizmos|30_0(primaryRanged, misc2))
						{
							yield return gizmo2;
						}
						IEnumerator<Gizmo> enumerator = null;
					}
					int num;
					for (int i = 0; i < list.Count; i = num + 1)
					{
						ThingWithComps thingWithComps = list[i];
						if (thingWithComps != primaryMelee && thingWithComps != primaryRanged)
						{
							foreach (Gizmo gizmo3 in Pawn_EquipmentTracker.<GetGizmos>g__YieldGizmos|30_0(thingWithComps, null))
							{
								yield return gizmo3;
							}
							IEnumerator<Gizmo> enumerator = null;
						}
						num = i;
					}
					list = null;
					primaryMelee = null;
					primaryRanged = null;
				}
				finally
				{
					Pawn_EquipmentTracker.tmpKeybindings.Clear();
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0007D058 File Offset: 0x0007B258
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

		// Token: 0x0600159D RID: 5533 RVA: 0x0007D104 File Offset: 0x0007B304
		public void Notify_EquipmentRemoved(ThingWithComps eq)
		{
			eq.Notify_Unequipped(this.pawn);
			if (ModsConfig.RoyaltyActive)
			{
				CompBladelinkWeapon compBladelinkWeapon = eq.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.Notify_EquipmentLost(this.pawn);
				}
			}
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0007D13C File Offset: 0x0007B33C
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

		// Token: 0x0600159F RID: 5535 RVA: 0x0007D198 File Offset: 0x0007B398
		public void Notify_PawnDied()
		{
			if (ModsConfig.RoyaltyActive && this.bondedWeapon != null)
			{
				CompBladelinkWeapon compBladelinkWeapon = this.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.UnCode();
				}
			}
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x0007D1CC File Offset: 0x0007B3CC
		public void Notify_KilledPawn()
		{
			foreach (ThingWithComps thingWithComps in this.equipment)
			{
				thingWithComps.Notify_KilledPawn(this.pawn);
			}
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0007D224 File Offset: 0x0007B424
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.equipment;
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x0007D22C File Offset: 0x0007B42C
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0007D246 File Offset: 0x0007B446
		[CompilerGenerated]
		internal static IEnumerable<Gizmo> <GetGizmos>g__YieldGizmos|30_0(ThingWithComps eq, KeyBindingDef preferredHotKey)
		{
			foreach (Command command in eq.GetComp<CompEquippable>().GetVerbsCommands())
			{
				if (Pawn_EquipmentTracker.tmpKeybindings.Count > 0)
				{
					if (preferredHotKey != null && Pawn_EquipmentTracker.tmpKeybindings.Contains(preferredHotKey))
					{
						command.hotKey = preferredHotKey;
						Pawn_EquipmentTracker.tmpKeybindings.Remove(preferredHotKey);
					}
					else
					{
						command.hotKey = Pawn_EquipmentTracker.tmpKeybindings.Pop<KeyBindingDef>();
					}
				}
				yield return command;
			}
			IEnumerator<Command> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000F23 RID: 3875
		public Pawn pawn;

		// Token: 0x04000F24 RID: 3876
		private ThingOwner<ThingWithComps> equipment;

		// Token: 0x04000F25 RID: 3877
		public Thing bondedWeapon;

		// Token: 0x04000F26 RID: 3878
		private static List<KeyBindingDef> tmpKeybindings = new List<KeyBindingDef>();
	}
}
