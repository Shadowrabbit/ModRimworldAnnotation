using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E63 RID: 3683
	public class Pawn_ApparelTracker : IThingHolder, IExposable
	{
		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x0600553E RID: 21822 RVA: 0x001CDC54 File Offset: 0x001CBE54
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x0600553F RID: 21823 RVA: 0x001CDC5C File Offset: 0x001CBE5C
		public List<Apparel> WornApparel
		{
			get
			{
				return this.wornApparel.InnerListForReading;
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06005540 RID: 21824 RVA: 0x001CDC6C File Offset: 0x001CBE6C
		public List<ApparelRequirement> ActiveRequirementsForReading
		{
			get
			{
				if (this.activeRequirementsDirty)
				{
					this.activeRequirements.Clear();
					foreach (ApparelRequirementWithSource apparelRequirementWithSource in this.AllRequirements)
					{
						string text;
						if (ApparelUtility.IsRequirementActive(apparelRequirementWithSource.requirement, apparelRequirementWithSource.Source, this.pawn, out text))
						{
							this.activeRequirements.Add(apparelRequirementWithSource.requirement);
						}
					}
					this.activeRequirementsDirty = false;
				}
				return this.activeRequirements;
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06005541 RID: 21825 RVA: 0x001CDD08 File Offset: 0x001CBF08
		public List<ApparelRequirementWithSource> AllRequirements
		{
			get
			{
				if (this.allRequirementsDirty)
				{
					this.allRequirements.Clear();
					if (ModsConfig.RoyaltyActive && this.pawn.royalty != null)
					{
						foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesForReading)
						{
							if (!royalTitle.def.requiredApparel.NullOrEmpty<ApparelRequirement>())
							{
								foreach (ApparelRequirement apparelRequirement in royalTitle.def.requiredApparel)
								{
									if (apparelRequirement.IsValid)
									{
										this.allRequirements.Add(new ApparelRequirementWithSource(apparelRequirement, royalTitle));
									}
								}
							}
						}
					}
					if (ModsConfig.IdeologyActive && this.pawn.Ideo != null)
					{
						Precept_Role role = this.pawn.Ideo.GetRole(this.pawn);
						if (role != null && !role.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
						{
							foreach (PreceptApparelRequirement preceptApparelRequirement in role.apparelRequirements)
							{
								if (preceptApparelRequirement.requirement.IsValid)
								{
									this.allRequirements.Add(new ApparelRequirementWithSource(preceptApparelRequirement.requirement, role));
								}
							}
						}
					}
					this.allRequirementsDirty = false;
				}
				return this.allRequirements;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06005542 RID: 21826 RVA: 0x001CDEB0 File Offset: 0x001CC0B0
		public int WornApparelCount
		{
			get
			{
				return this.wornApparel.Count;
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06005543 RID: 21827 RVA: 0x001CDEBD File Offset: 0x001CC0BD
		public bool AnyApparel
		{
			get
			{
				return this.wornApparel.Count != 0;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06005544 RID: 21828 RVA: 0x001CDED0 File Offset: 0x001CC0D0
		public bool AnyClothing
		{
			get
			{
				using (List<Apparel>.Enumerator enumerator = this.wornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.apparel.countsAsClothingForNudity)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06005545 RID: 21829 RVA: 0x001CDF34 File Offset: 0x001CC134
		public bool AnyApparelLocked
		{
			get
			{
				return !this.lockedApparel.NullOrEmpty<Apparel>();
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06005546 RID: 21830 RVA: 0x001CDF44 File Offset: 0x001CC144
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

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06005547 RID: 21831 RVA: 0x001CDF90 File Offset: 0x001CC190
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

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06005548 RID: 21832 RVA: 0x001CDFCA File Offset: 0x001CC1CA
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

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06005549 RID: 21833 RVA: 0x001CDFE0 File Offset: 0x001CC1E0
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

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x0600554A RID: 21834 RVA: 0x001CE018 File Offset: 0x001CC218
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

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x0600554B RID: 21835 RVA: 0x001CE0E0 File Offset: 0x001CC2E0
		public bool AnyApparelNeedsRecoloring
		{
			get
			{
				using (List<Apparel>.Enumerator enumerator = this.WornApparel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.DesiredColor != null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x0600554C RID: 21836 RVA: 0x001CE144 File Offset: 0x001CC344
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

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x0600554D RID: 21837 RVA: 0x001CE154 File Offset: 0x001CC354
		public Verb FirstApparelVerb
		{
			get
			{
				List<Apparel> list = this.WornApparel;
				for (int i = 0; i < list.Count; i++)
				{
					CompReloadable comp = list[i].GetComp<CompReloadable>();
					List<Verb> list2 = (comp != null) ? comp.AllVerbs : null;
					if (list2 != null && list2.Count != 0)
					{
						return list2[0];
					}
				}
				return null;
			}
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x001CE1A8 File Offset: 0x001CC3A8
		public Pawn_ApparelTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.wornApparel = new ThingOwner<Apparel>(this);
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x001CE1FC File Offset: 0x001CC3FC
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

		// Token: 0x06005550 RID: 21840 RVA: 0x001CE324 File Offset: 0x001CC524
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

		// Token: 0x06005551 RID: 21841 RVA: 0x001CE39C File Offset: 0x001CC59C
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

		// Token: 0x06005552 RID: 21842 RVA: 0x001CE486 File Offset: 0x001CC686
		public bool IsLocked(Apparel apparel)
		{
			return this.lockedApparel != null && this.lockedApparel.Contains(apparel);
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x001CE49E File Offset: 0x001CC69E
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

		// Token: 0x06005554 RID: 21844 RVA: 0x001CE4C9 File Offset: 0x001CC6C9
		public void Unlock(Apparel apparel)
		{
			if (!this.IsLocked(apparel))
			{
				return;
			}
			this.lockedApparel.Remove(apparel);
		}

		// Token: 0x06005555 RID: 21845 RVA: 0x001CE4E4 File Offset: 0x001CC6E4
		public void LockAll()
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.Lock(this.wornApparel[i]);
			}
		}

		// Token: 0x06005556 RID: 21846 RVA: 0x001CE51C File Offset: 0x001CC71C
		private void TakeWearoutDamageForDay(Thing ap)
		{
			int num = GenMath.RoundRandom(ap.def.apparel.wearPerDay);
			if (num > 0)
			{
				ap.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
			if (ap.Destroyed && PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.Dead)
			{
				Messages.Message("MessageWornApparelDeterioratedAway".Translate(GenLabel.ThingLabel(ap.def, ap.Stuff, 1), this.pawn).CapitalizeFirst(), this.pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06005557 RID: 21847 RVA: 0x001CE5D8 File Offset: 0x001CC7D8
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

		// Token: 0x06005558 RID: 21848 RVA: 0x001CE628 File Offset: 0x001CC828
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
				}));
				return;
			}
			if (CompBiocodable.IsBiocoded(newApparel) && !CompBiocodable.IsBiocodedFor(newApparel, this.pawn))
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
				}));
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
							Log.Error(this.pawn + " could not drop " + apparel);
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
				}));
			}
			this.wornApparel.TryAdd(newApparel, false);
			if (locked)
			{
				this.Lock(newApparel);
			}
		}

		// Token: 0x06005559 RID: 21849 RVA: 0x001CE7EB File Offset: 0x001CC9EB
		public void Remove(Apparel ap)
		{
			this.wornApparel.Remove(ap);
		}

		// Token: 0x0600555A RID: 21850 RVA: 0x001CE7FC File Offset: 0x001CC9FC
		public bool TryDrop(Apparel ap)
		{
			Apparel apparel;
			return this.TryDrop(ap, out apparel);
		}

		// Token: 0x0600555B RID: 21851 RVA: 0x001CE812 File Offset: 0x001CCA12
		public bool TryDrop(Apparel ap, out Apparel resultingAp)
		{
			return this.TryDrop(ap, out resultingAp, this.pawn.PositionHeld, true);
		}

		// Token: 0x0600555C RID: 21852 RVA: 0x001CE828 File Offset: 0x001CCA28
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

		// Token: 0x0600555D RID: 21853 RVA: 0x001CE85C File Offset: 0x001CCA5C
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

		// Token: 0x0600555E RID: 21854 RVA: 0x001CE8E1 File Offset: 0x001CCAE1
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.wornApparel.ClearAndDestroyContents(mode);
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x001CE8EF File Offset: 0x001CCAEF
		public bool Contains(Thing apparel)
		{
			return this.wornApparel.Contains(apparel);
		}

		// Token: 0x06005560 RID: 21856 RVA: 0x001CE900 File Offset: 0x001CCB00
		public bool Wearing(Thing apparel)
		{
			using (List<Apparel>.Enumerator enumerator = this.wornApparel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == apparel)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005561 RID: 21857 RVA: 0x001CE958 File Offset: 0x001CCB58
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

		// Token: 0x06005562 RID: 21858 RVA: 0x001CE9B8 File Offset: 0x001CCBB8
		public void Notify_PawnKilled(DamageInfo? dinfo)
		{
			if (dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this.pawn))
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (this.wornApparel[i].def.useHitPoints)
					{
						int num = Mathf.RoundToInt((float)this.wornApparel[i].HitPoints * Rand.Range(0.15f, 0.4f));
						this.wornApparel[i].TakeDamage(new DamageInfo(dinfo.Value.Def, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
					}
				}
			}
			for (int j = 0; j < this.wornApparel.Count; j++)
			{
				this.wornApparel[j].Notify_PawnKilled();
			}
		}

		// Token: 0x06005563 RID: 21859 RVA: 0x001CEAAC File Offset: 0x001CCCAC
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

		// Token: 0x06005564 RID: 21860 RVA: 0x001CEB2A File Offset: 0x001CCD2A
		public void Notify_TitleChanged()
		{
			this.activeRequirementsDirty = true;
			this.allRequirementsDirty = true;
		}

		// Token: 0x06005565 RID: 21861 RVA: 0x001CEB2A File Offset: 0x001CCD2A
		public void Notify_RoleChanged()
		{
			this.activeRequirementsDirty = true;
			this.allRequirementsDirty = true;
		}

		// Token: 0x06005566 RID: 21862 RVA: 0x001CEB2A File Offset: 0x001CCD2A
		public void Notify_IdeoChanged()
		{
			this.activeRequirementsDirty = true;
			this.allRequirementsDirty = true;
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x001CEB3A File Offset: 0x001CCD3A
		private void SortWornApparelIntoDrawOrder()
		{
			this.wornApparel.InnerListForReading.Sort((Apparel a, Apparel b) => a.def.apparel.LastLayer.drawOrder.CompareTo(b.def.apparel.LastLayer.drawOrder));
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x001CEB6C File Offset: 0x001CCD6C
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

		// Token: 0x06005569 RID: 21865 RVA: 0x001CEC10 File Offset: 0x001CCE10
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

		// Token: 0x0600556A RID: 21866 RVA: 0x001CEC80 File Offset: 0x001CCE80
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

		// Token: 0x0600556B RID: 21867 RVA: 0x001CECEC File Offset: 0x001CCEEC
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

		// Token: 0x0600556C RID: 21868 RVA: 0x001CECFC File Offset: 0x001CCEFC
		public void Notify_ApparelChanged()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.pawn.Drawer.renderer.graphics.SetApparelGraphicsDirty();
				PortraitsCache.SetDirty(this.pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			});
		}

		// Token: 0x0600556D RID: 21869 RVA: 0x001CED10 File Offset: 0x001CCF10
		public void Notify_ApparelAdded(Apparel apparel)
		{
			this.SortWornApparelIntoDrawOrder();
			this.Notify_ApparelChanged();
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
			apparel.Notify_Equipped(this.pawn);
		}

		// Token: 0x0600556E RID: 21870 RVA: 0x001CEDBC File Offset: 0x001CCFBC
		public void Notify_ApparelRemoved(Apparel apparel)
		{
			this.Notify_ApparelChanged();
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
			apparel.Notify_Unequipped(this.pawn);
		}

		// Token: 0x0600556F RID: 21871 RVA: 0x001CEE48 File Offset: 0x001CD048
		public void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				this.wornApparel[i].Notify_BulletImpactNearby(impactData);
			}
		}

		// Token: 0x06005570 RID: 21872 RVA: 0x001CEE7D File Offset: 0x001CD07D
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.wornApparel;
		}

		// Token: 0x06005571 RID: 21873 RVA: 0x001CEE85 File Offset: 0x001CD085
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04003282 RID: 12930
		public Pawn pawn;

		// Token: 0x04003283 RID: 12931
		private ThingOwner<Apparel> wornApparel;

		// Token: 0x04003284 RID: 12932
		private List<Apparel> lockedApparel;

		// Token: 0x04003285 RID: 12933
		private int lastApparelWearoutTick = -1;

		// Token: 0x04003286 RID: 12934
		private List<ApparelRequirementWithSource> allRequirements = new List<ApparelRequirementWithSource>();

		// Token: 0x04003287 RID: 12935
		private List<ApparelRequirement> activeRequirements = new List<ApparelRequirement>();

		// Token: 0x04003288 RID: 12936
		private bool activeRequirementsDirty = true;

		// Token: 0x04003289 RID: 12937
		private bool allRequirementsDirty = true;

		// Token: 0x0400328A RID: 12938
		private const int RecordWalkedNakedTaleIntervalTicks = 60000;

		// Token: 0x0400328B RID: 12939
		private const float AutoUnlockHealthPctThreshold = 0.5f;

		// Token: 0x0400328C RID: 12940
		private static readonly List<Apparel> EmptyApparel = new List<Apparel>();

		// Token: 0x0400328D RID: 12941
		private static List<Apparel> tmpApparelList = new List<Apparel>();

		// Token: 0x0400328E RID: 12942
		private static List<Apparel> tmpApparel = new List<Apparel>();
	}
}
