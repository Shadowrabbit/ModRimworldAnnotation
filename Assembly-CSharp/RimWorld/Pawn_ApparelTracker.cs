using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001504 RID: 5380
	public class Pawn_ApparelTracker : IThingHolder, IExposable
	{
		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x060073D9 RID: 29657 RVA: 0x0004E18D File Offset: 0x0004C38D
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x060073DA RID: 29658 RVA: 0x0004E195 File Offset: 0x0004C395
		public List<Apparel> WornApparel
		{
			get
			{
				return this.wornApparel.InnerListForReading;
			}
		}

		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x060073DB RID: 29659 RVA: 0x0004E1A2 File Offset: 0x0004C3A2
		public int WornApparelCount
		{
			get
			{
				return this.wornApparel.Count;
			}
		}

		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x060073DC RID: 29660 RVA: 0x0004E1AF File Offset: 0x0004C3AF
		public bool AnyApparel
		{
			get
			{
				return this.wornApparel.Count != 0;
			}
		}

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x060073DD RID: 29661 RVA: 0x0004E1BF File Offset: 0x0004C3BF
		public bool AnyApparelLocked
		{
			get
			{
				return !this.lockedApparel.NullOrEmpty<Apparel>();
			}
		}

		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x060073DE RID: 29662 RVA: 0x002351C4 File Offset: 0x002333C4
		public bool AnyApparelUnlocked
		{
			get
			{
				if (!this.AnyApparelLocked)
				{
					return this.AnyApparel;
				}
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (!this.IsLocked(this.wornApparel[i]))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x060073DF RID: 29663 RVA: 0x00235210 File Offset: 0x00233410
		public bool AllApparelLocked
		{
			get
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (!this.IsLocked(this.wornApparel[i]))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x060073E0 RID: 29664 RVA: 0x0004E1CF File Offset: 0x0004C3CF
		public List<Apparel> LockedApparel
		{
			get
			{
				if (this.lockedApparel == null)
				{
					return Pawn_ApparelTracker.EmptyApparel;
				}
				return this.lockedApparel;
			}
		}

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x060073E1 RID: 29665 RVA: 0x0023524C File Offset: 0x0023344C
		public IEnumerable<Apparel> UnlockedApparel
		{
			get
			{
				if (!this.AnyApparelLocked)
				{
					return this.WornApparel;
				}
				return from x in this.WornApparel
				where !this.IsLocked(x)
				select x;
			}
		}

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x060073E2 RID: 29666 RVA: 0x00235284 File Offset: 0x00233484
		public bool PsychologicallyNude
		{
			get
			{
				if (this.pawn.gender == Gender.None)
				{
					return false;
				}
				if (this.pawn.IsWildMan())
				{
					return false;
				}
				bool flag;
				bool flag2;
				this.HasBasicApparel(out flag, out flag2);
				if (!flag)
				{
					bool flag3 = false;
					using (IEnumerator<BodyPartRecord> enumerator = this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsInGroup(BodyPartGroupDefOf.Legs))
							{
								flag3 = true;
								break;
							}
						}
					}
					if (!flag3)
					{
						flag = true;
					}
				}
				if (this.pawn.gender == Gender.Male)
				{
					return !flag;
				}
				return this.pawn.gender == Gender.Female && (!flag || !flag2);
			}
		}

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x060073E3 RID: 29667 RVA: 0x0004E1E5 File Offset: 0x0004C3E5
		public IEnumerable<Verb> AllApparelVerbs
		{
			get
			{
				List<Apparel> list = this.WornApparel;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					Apparel apparel = list[i];
					CompReloadable comp = apparel.GetComp<CompReloadable>();
					List<Verb> verbs = (comp != null) ? comp.AllVerbs : null;
					if (verbs != null)
					{
						for (int j = 0; j < verbs.Count; j = num + 1)
						{
							yield return verbs[j];
							num = j;
						}
						verbs = null;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x060073E4 RID: 29668 RVA: 0x0004E1F5 File Offset: 0x0004C3F5
		public Pawn_ApparelTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.wornApparel = new ThingOwner<Apparel>(this);
		}

		// Token: 0x060073E5 RID: 29669 RVA: 0x0023534C File Offset: 0x0023354C
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Apparel>>(ref this.wornApparel, "wornApparel", new object[]
			{
				this
			});
			Scribe_Collections.Look<Apparel>(ref this.lockedApparel, "lockedApparel", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastApparelWearoutTick, "lastApparelWearoutTick", 0, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.SortWornApparelIntoDrawOrder();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.lockedApparel != null)
				{
					this.lockedApparel.RemoveAll((Apparel x) => x == null);
				}
				foreach (Apparel apparel in this.WornApparel)
				{
					CompReloadable comp = apparel.GetComp<CompReloadable>();
					if (comp != null)
					{
						foreach (Verb verb in comp.AllVerbs)
						{
							verb.caster = this.pawn;
						}
					}
				}
			}
		}

		// Token: 0x060073E6 RID: 29670 RVA: 0x00235474 File Offset: 0x00233674
		public void ApparelTrackerTickRare()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.lastApparelWearoutTick < 0)
			{
				this.lastApparelWearoutTick = ticksGame;
			}
			if (ticksGame - this.lastApparelWearoutTick >= 60000)
			{
				if (!this.pawn.IsWorldPawn())
				{
					for (int i = this.wornApparel.Count - 1; i >= 0; i--)
					{
						this.TakeWearoutDamageForDay(this.wornApparel[i]);
					}
				}
				this.lastApparelWearoutTick = ticksGame;
			}
		}

		// Token: 0x060073E7 RID: 29671 RVA: 0x002354EC File Offset: 0x002336EC
		public void ApparelTrackerTick()
		{
			this.wornApparel.ThingOwnerTick(true);
			if (this.pawn.IsColonist && this.pawn.Spawned && !this.pawn.Dead && this.pawn.IsHashIntervalTick(60000) && this.PsychologicallyNude)
			{
				TaleRecorder.RecordTale(TaleDefOf.WalkedNaked, new object[]
				{
					this.pawn
				});
			}
			if (this.lockedApparel != null)
			{
				for (int i = this.lockedApparel.Count - 1; i >= 0; i--)
				{
					if (this.lockedApparel[i].def.useHitPoints && (float)this.lockedApparel[i].HitPoints / (float)this.lockedApparel[i].MaxHitPoints < 0.5f)
					{
						this.Unlock(this.lockedApparel[i]);
					}
				}
			}
		}

		// Token: 0x060073E8 RID: 29672 RVA: 0x0004E217 File Offset: 0x0004C417
		public bool IsLocked(Apparel apparel)
		{
			return this.lockedApparel != null && this.lockedApparel.Contains(apparel);
		}

		// Token: 0x060073E9 RID: 29673 RVA: 0x0004E22F File Offset: 0x0004C42F
		public void Lock(Apparel apparel)
		{
			if (this.IsLocked(apparel))
			{
				return;
			}
			if (this.lockedApparel == null)
			{
				this.lockedApparel = new List<Apparel>();
			}
			this.lockedApparel.Add(apparel);
		}

		// Token: 0x060073EA RID: 29674 RVA: 0x0004E25A File Offset: 0x0004C45A
		public void Unlock(Apparel apparel)
		{
			if (!this.IsLocked(apparel))
			{
				return;
			}
			this.lockedApparel.Remove(apparel);
		}

		// Token: 0x060073EB RID: 29675 RVA: 0x002355D8 File Offset: 0x002337D8
		public void LockAll()
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.Lock(this.wornApparel[i]);
			}
		}

		// Token: 0x060073EC RID: 29676 RVA: 0x00235610 File Offset: 0x00233810
		private void TakeWearoutDamageForDay(Thing ap)
		{
			int num = GenMath.RoundRandom(ap.def.apparel.wearPerDay);
			if (num > 0)
			{
				ap.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			if (ap.Destroyed && PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.Dead)
			{
				Messages.Message("MessageWornApparelDeterioratedAway".Translate(GenLabel.ThingLabel(ap.def, ap.Stuff, 1), this.pawn).CapitalizeFirst(), this.pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x060073ED RID: 29677 RVA: 0x002356C8 File Offset: 0x002338C8
		public bool CanWearWithoutDroppingAnything(ThingDef apDef)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(apDef, this.wornApparel[i].def, this.pawn.RaceProps.body))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060073EE RID: 29678 RVA: 0x00235718 File Offset: 0x00233918
		public void Wear(Apparel newApparel, bool dropReplacedApparel = true, bool locked = false)
		{
			if (newApparel.Spawned)
			{
				newApparel.DeSpawn(DestroyMode.Vanish);
			}
			if (!ApparelUtility.HasPartsToWear(this.pawn, newApparel.def))
			{
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" tried to wear ",
					newApparel,
					" but he has no body parts required to wear it."
				}), false);
				return;
			}
			if (EquipmentUtility.IsBiocoded(newApparel) && !EquipmentUtility.IsBiocodedFor(newApparel, this.pawn))
			{
				CompBiocodable compBiocodable = newApparel.TryGetComp<CompBiocodable>();
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" tried to wear ",
					newApparel,
					" but it is biocoded for ",
					compBiocodable.CodedPawnLabel,
					" ."
				}), false);
				return;
			}
			for (int i = this.wornApparel.Count - 1; i >= 0; i--)
			{
				Apparel apparel = this.wornApparel[i];
				if (!ApparelUtility.CanWearTogether(newApparel.def, apparel.def, this.pawn.RaceProps.body))
				{
					if (dropReplacedApparel)
					{
						bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
						Apparel apparel2;
						if (!this.TryDrop(apparel, out apparel2, this.pawn.PositionHeld, forbid))
						{
							Log.Error(this.pawn + " could not drop " + apparel, false);
							return;
						}
					}
					else
					{
						this.Remove(apparel);
					}
				}
			}
			if (newApparel.Wearer != null)
			{
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" is trying to wear ",
					newApparel,
					" but this apparel already has a wearer (",
					newApparel.Wearer,
					"). This may or may not cause bugs."
				}), false);
			}
			this.wornApparel.TryAdd(newApparel, false);
			if (locked)
			{
				this.Lock(newApparel);
			}
		}

		// Token: 0x060073EF RID: 29679 RVA: 0x0004E273 File Offset: 0x0004C473
		public void Remove(Apparel ap)
		{
			this.wornApparel.Remove(ap);
		}

		// Token: 0x060073F0 RID: 29680 RVA: 0x002358E0 File Offset: 0x00233AE0
		public bool TryDrop(Apparel ap)
		{
			Apparel apparel;
			return this.TryDrop(ap, out apparel);
		}

		// Token: 0x060073F1 RID: 29681 RVA: 0x0004E282 File Offset: 0x0004C482
		public bool TryDrop(Apparel ap, out Apparel resultingAp)
		{
			return this.TryDrop(ap, out resultingAp, this.pawn.PositionHeld, true);
		}

		// Token: 0x060073F2 RID: 29682 RVA: 0x0004E298 File Offset: 0x0004C498
		public bool TryDrop(Apparel ap, out Apparel resultingAp, IntVec3 pos, bool forbid = true)
		{
			if (this.wornApparel.TryDrop(ap, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingAp, null, null))
			{
				if (resultingAp != null)
				{
					resultingAp.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060073F3 RID: 29683 RVA: 0x002358F8 File Offset: 0x00233AF8
		public void DropAll(IntVec3 pos, bool forbid = true, bool dropLocked = true)
		{
			Pawn_ApparelTracker.tmpApparelList.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if (dropLocked || !this.IsLocked(this.wornApparel[i]))
				{
					Pawn_ApparelTracker.tmpApparelList.Add(this.wornApparel[i]);
				}
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparelList.Count; j++)
			{
				Apparel apparel;
				this.TryDrop(Pawn_ApparelTracker.tmpApparelList[j], out apparel, pos, forbid);
			}
		}

		// Token: 0x060073F4 RID: 29684 RVA: 0x0004E2C9 File Offset: 0x0004C4C9
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.wornApparel.ClearAndDestroyContents(mode);
		}

		// Token: 0x060073F5 RID: 29685 RVA: 0x0004E2D7 File Offset: 0x0004C4D7
		public bool Contains(Thing apparel)
		{
			return this.wornApparel.Contains(apparel);
		}

		// Token: 0x060073F6 RID: 29686 RVA: 0x00235980 File Offset: 0x00233B80
		public bool WouldReplaceLockedApparel(Apparel newApparel)
		{
			if (!this.AnyApparelLocked)
			{
				return false;
			}
			for (int i = 0; i < this.lockedApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(newApparel.def, this.lockedApparel[i].def, this.pawn.RaceProps.body))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060073F7 RID: 29687 RVA: 0x002359E0 File Offset: 0x00233BE0
		public void Notify_PawnKilled(DamageInfo? dinfo)
		{
			if (dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this.pawn))
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (this.wornApparel[i].def.useHitPoints)
					{
						int num = Mathf.RoundToInt((float)this.wornApparel[i].HitPoints * Rand.Range(0.15f, 0.4f));
						this.wornApparel[i].TakeDamage(new DamageInfo(dinfo.Value.Def, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					}
				}
			}
			for (int j = 0; j < this.wornApparel.Count; j++)
			{
				this.wornApparel[j].Notify_PawnKilled();
			}
		}

		// Token: 0x060073F8 RID: 29688 RVA: 0x00235AD0 File Offset: 0x00233CD0
		public void Notify_LostBodyPart()
		{
			Pawn_ApparelTracker.tmpApparel.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Pawn_ApparelTracker.tmpApparel.Add(this.wornApparel[i]);
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparel.Count; j++)
			{
				Apparel apparel = Pawn_ApparelTracker.tmpApparel[j];
				if (!ApparelUtility.HasPartsToWear(this.pawn, apparel.def))
				{
					this.Remove(apparel);
				}
			}
		}

		// Token: 0x060073F9 RID: 29689 RVA: 0x0004E2E5 File Offset: 0x0004C4E5
		private void SortWornApparelIntoDrawOrder()
		{
			this.wornApparel.InnerListForReading.Sort((Apparel a, Apparel b) => a.def.apparel.LastLayer.drawOrder.CompareTo(b.def.apparel.LastLayer.drawOrder));
		}

		// Token: 0x060073FA RID: 29690 RVA: 0x00235B50 File Offset: 0x00233D50
		public void HasBasicApparel(out bool hasPants, out bool hasShirt)
		{
			hasShirt = false;
			hasPants = false;
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						hasShirt = true;
					}
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						hasPants = true;
					}
					if (hasShirt & hasPants)
					{
						return;
					}
				}
			}
		}

		// Token: 0x060073FB RID: 29691 RVA: 0x00235BF4 File Offset: 0x00233DF4
		public Apparel FirstApparelOnBodyPartGroup(BodyPartGroupDef g)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						return apparel;
					}
				}
			}
			return null;
		}

		// Token: 0x060073FC RID: 29692 RVA: 0x00235C64 File Offset: 0x00233E64
		public bool BodyPartGroupIsCovered(BodyPartGroupDef bp)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == bp)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060073FD RID: 29693 RVA: 0x0004E316 File Offset: 0x0004C516
		public IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			for (int i = 0; i < this.wornApparel.Count; i = num + 1)
			{
				foreach (Gizmo gizmo in this.wornApparel[i].GetWornGizmos())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x060073FE RID: 29694 RVA: 0x0004E326 File Offset: 0x0004C526
		private void ApparelChanged()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.pawn.Drawer.renderer.graphics.SetApparelGraphicsDirty();
				PortraitsCache.SetDirty(this.pawn);
			});
		}

		// Token: 0x060073FF RID: 29695 RVA: 0x00235CD0 File Offset: 0x00233ED0
		public void Notify_ApparelAdded(Apparel apparel)
		{
			this.SortWornApparelIntoDrawOrder();
			this.ApparelChanged();
			CompReloadable comp = apparel.GetComp<CompReloadable>();
			List<Verb> list = (comp != null) ? comp.AllVerbs : null;
			if (list != null)
			{
				foreach (Verb verb in list)
				{
					verb.caster = this.pawn;
					verb.Notify_PickedUp();
				}
			}
			if (!apparel.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			}
		}

		// Token: 0x06007400 RID: 29696 RVA: 0x00235D70 File Offset: 0x00233F70
		public void Notify_ApparelRemoved(Apparel apparel)
		{
			this.ApparelChanged();
			if (this.pawn.outfits != null && this.pawn.outfits.forcedHandler != null)
			{
				this.pawn.outfits.forcedHandler.SetForced(apparel, false);
			}
			if (this.IsLocked(apparel))
			{
				this.Unlock(apparel);
			}
			if (!apparel.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
			{
				this.pawn.health.capacities.Notify_CapacityLevelsDirty();
			}
		}

		// Token: 0x06007401 RID: 29697 RVA: 0x00235DF0 File Offset: 0x00233FF0
		public void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.wornApparel[i].Notify_BulletImpactNearby(impactData);
			}
		}

		// Token: 0x06007402 RID: 29698 RVA: 0x0004E339 File Offset: 0x0004C539
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.wornApparel;
		}

		// Token: 0x06007403 RID: 29699 RVA: 0x0004E341 File Offset: 0x0004C541
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04004C85 RID: 19589
		public Pawn pawn;

		// Token: 0x04004C86 RID: 19590
		private ThingOwner<Apparel> wornApparel;

		// Token: 0x04004C87 RID: 19591
		private List<Apparel> lockedApparel;

		// Token: 0x04004C88 RID: 19592
		private int lastApparelWearoutTick = -1;

		// Token: 0x04004C89 RID: 19593
		private const int RecordWalkedNakedTaleIntervalTicks = 60000;

		// Token: 0x04004C8A RID: 19594
		private const float AutoUnlockHealthPctThreshold = 0.5f;

		// Token: 0x04004C8B RID: 19595
		private static readonly List<Apparel> EmptyApparel = new List<Apparel>();

		// Token: 0x04004C8C RID: 19596
		private static List<Apparel> tmpApparelList = new List<Apparel>();

		// Token: 0x04004C8D RID: 19597
		private static List<Apparel> tmpApparel = new List<Apparel>();
	}
}
