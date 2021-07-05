using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020002F1 RID: 753
	public class Pawn_HealthTracker : IExposable
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x0007D25D File Offset: 0x0007B45D
		public PawnHealthState State
		{
			get
			{
				return this.healthState;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x0007D265 File Offset: 0x0007B465
		public bool Downed
		{
			get
			{
				return this.healthState == PawnHealthState.Down;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x0007D270 File Offset: 0x0007B470
		public bool Dead
		{
			get
			{
				return this.healthState == PawnHealthState.Dead;
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x0007D27B File Offset: 0x0007B47B
		public float LethalDamageThreshold
		{
			get
			{
				return 150f * this.pawn.HealthScale;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x0007D28E File Offset: 0x0007B48E
		public bool InPainShock
		{
			get
			{
				return this.hediffSet.PainTotal >= this.pawn.GetStatValue(StatDefOf.PainShockThreshold, true);
			}
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0007D2B4 File Offset: 0x0007B4B4
		public Pawn_HealthTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.hediffSet = new HediffSet(pawn);
			this.capacities = new PawnCapacitiesHandler(pawn);
			this.summaryHealth = new SummaryHealthHandler(pawn);
			this.surgeryBills = new BillStack(pawn);
			this.immunity = new ImmunityHandler(pawn);
			this.beCarriedByCaravanIfSick = pawn.RaceProps.Humanlike;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x0007D32C File Offset: 0x0007B52C
		public void Reset()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.Clear();
			this.capacities.Clear();
			this.summaryHealth.Notify_HealthChanged();
			this.surgeryBills.Clear();
			this.immunity = new ImmunityHandler(this.pawn);
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x0007D380 File Offset: 0x0007B580
		public void ExposeData()
		{
			Scribe_Values.Look<PawnHealthState>(ref this.healthState, "healthState", PawnHealthState.Mobile, false);
			Scribe_Values.Look<bool>(ref this.forceIncap, "forceIncap", false, false);
			Scribe_Values.Look<bool>(ref this.beCarriedByCaravanIfSick, "beCarriedByCaravanIfSick", true, false);
			Scribe_Values.Look<bool>(ref this.killedByRitual, "killedByRitual", false, false);
			Scribe_Values.Look<int>(ref this.lastReceivedNeuralSuperchargeTick, "lastReceivedNeuralSuperchargeTick", -1, false);
			Scribe_Deep.Look<HediffSet>(ref this.hediffSet, "hediffSet", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<BillStack>(ref this.surgeryBills, "surgeryBills", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<ImmunityHandler>(ref this.immunity, "immunity", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x0007D444 File Offset: 0x0007B644
		public Hediff AddHediff(HediffDef def, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, null);
			this.AddHediff(hediff, part, dinfo, result);
			return hediff;
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0007D46C File Offset: 0x0007B66C
		public void AddHediff(Hediff hediff, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			if (part != null)
			{
				hediff.Part = part;
			}
			this.hediffSet.AddDirect(hediff, dinfo, result);
			this.CheckForStateChange(dinfo, hediff);
			if (this.pawn.RaceProps.hediffGiverSets != null)
			{
				for (int i = 0; i < this.pawn.RaceProps.hediffGiverSets.Count; i++)
				{
					HediffGiverSetDef hediffGiverSetDef = this.pawn.RaceProps.hediffGiverSets[i];
					for (int j = 0; j < hediffGiverSetDef.hediffGivers.Count; j++)
					{
						hediffGiverSetDef.hediffGivers[j].OnHediffAdded(this.pawn, hediff);
					}
				}
			}
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0007D512 File Offset: 0x0007B712
		public void RemoveHediff(Hediff hediff)
		{
			this.hediffSet.hediffs.Remove(hediff);
			hediff.PostRemoved();
			this.Notify_HediffChanged(null);
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0007D534 File Offset: 0x0007B734
		public void RemoveAllHediffs()
		{
			for (int i = this.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				this.RemoveHediff(this.hediffSet.hediffs[i]);
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0007D578 File Offset: 0x0007B778
		public void Notify_HediffChanged(Hediff hediff)
		{
			this.hediffSet.DirtyCache();
			this.CheckForStateChange(null, hediff);
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0007D5A0 File Offset: 0x0007B7A0
		public void Notify_UsedVerb(Verb verb, LocalTargetInfo target)
		{
			foreach (Hediff hediff in this.hediffSet.hediffs)
			{
				hediff.Notify_PawnUsedVerb(verb, target);
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0007D5F8 File Offset: 0x0007B7F8
		public void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			Faction homeFaction = this.pawn.HomeFaction;
			if (dinfo.Instigator != null && homeFaction != null && homeFaction.IsPlayer && !this.pawn.InAggroMentalState)
			{
				Pawn pawn = dinfo.Instigator as Pawn;
				if (dinfo.InstigatorGuilty && pawn != null && pawn.guilt != null && pawn.mindState != null)
				{
					pawn.guilt.Notify_Guilty(60000);
				}
			}
			if (this.pawn.Spawned)
			{
				if (!this.pawn.Position.Fogged(this.pawn.Map))
				{
					this.pawn.mindState.Active = true;
				}
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnDamaged(this.pawn, dinfo);
				}
				if (dinfo.Def.ExternalViolenceFor(this.pawn))
				{
					GenClamor.DoClamor(this.pawn, 18f, ClamorDefOf.Harm);
				}
				this.pawn.jobs.Notify_DamageTaken(dinfo);
			}
			if (homeFaction != null)
			{
				homeFaction.Notify_MemberTookDamage(this.pawn, dinfo);
				if (Current.ProgramState == ProgramState.Playing && homeFaction == Faction.OfPlayer && dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.SpawnedOrAnyParentSpawned)
				{
					this.pawn.MapHeld.dangerWatcher.Notify_ColonistHarmedExternally();
				}
			}
			if (this.pawn.apparel != null && !dinfo.IgnoreArmor)
			{
				List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].CheckPreAbsorbDamage(dinfo))
					{
						absorbed = true;
						return;
					}
				}
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.Notify_DamageTaken(dinfo);
				this.pawn.stances.stunner.Notify_DamageApplied(dinfo);
			}
			if (this.pawn.RaceProps.IsFlesh && dinfo.Def.ExternalViolenceFor(this.pawn))
			{
				Pawn pawn2 = dinfo.Instigator as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.HostileTo(this.pawn))
					{
						this.pawn.relations.canGetRescuedThought = true;
					}
					if (this.pawn.RaceProps.Humanlike && pawn2.RaceProps.Humanlike && this.pawn.needs.mood != null && (!pawn2.HostileTo(this.pawn) || (pawn2.Faction == homeFaction && pawn2.InMentalState)))
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HarmedMe, pawn2, null);
					}
				}
				TaleRecorder.RecordTale(TaleDefOf.Wounded, new object[]
				{
					this.pawn,
					pawn2,
					dinfo.Weapon
				});
			}
			absorbed = false;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0007D8E4 File Offset: 0x0007BAE4
		public void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.ShouldBeDead())
			{
				if (!this.pawn.Destroyed)
				{
					this.pawn.Kill(new DamageInfo?(dinfo), null);
					return;
				}
			}
			else
			{
				Pawn pawn;
				if (dinfo.Def.additionalHediffs != null && (dinfo.Def.applyAdditionalHediffsIfHuntingForFood || (pawn = (dinfo.Instigator as Pawn)) == null || pawn.CurJob == null || pawn.CurJob.def != JobDefOf.PredatorHunt))
				{
					List<DamageDefAdditionalHediff> additionalHediffs = dinfo.Def.additionalHediffs;
					for (int i = 0; i < additionalHediffs.Count; i++)
					{
						DamageDefAdditionalHediff damageDefAdditionalHediff = additionalHediffs[i];
						if (damageDefAdditionalHediff.hediff != null)
						{
							float num = (damageDefAdditionalHediff.severityFixed <= 0f) ? (totalDamageDealt * damageDefAdditionalHediff.severityPerDamageDealt) : damageDefAdditionalHediff.severityFixed;
							if (damageDefAdditionalHediff.victimSeverityScalingByInvBodySize)
							{
								num *= 1f / this.pawn.BodySize;
							}
							if (damageDefAdditionalHediff.victimSeverityScaling != null)
							{
								num *= this.pawn.GetStatValue(damageDefAdditionalHediff.victimSeverityScaling, true);
							}
							if (num >= 0f)
							{
								Hediff hediff = HediffMaker.MakeHediff(damageDefAdditionalHediff.hediff, this.pawn, null);
								hediff.Severity = num;
								this.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
								if (this.Dead)
								{
									return;
								}
							}
						}
					}
				}
				for (int j = 0; j < this.hediffSet.hediffs.Count; j++)
				{
					this.hediffSet.hediffs[j].Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
				}
			}
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0007DA74 File Offset: 0x0007BC74
		public void RestorePart(BodyPartRecord part, Hediff diffException = null, bool checkStateChange = true)
		{
			if (part == null)
			{
				Log.Error("Tried to restore null body part.");
				return;
			}
			this.RestorePartRecursiveInt(part, diffException);
			this.hediffSet.DirtyCache();
			if (checkStateChange)
			{
				this.CheckForStateChange(null, null);
			}
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0007DAB8 File Offset: 0x0007BCB8
		private void RestorePartRecursiveInt(BodyPartRecord part, Hediff diffException = null)
		{
			List<Hediff> hediffs = this.hediffSet.hediffs;
			for (int i = hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff = hediffs[i];
				if (hediff.Part == part && hediff != diffException && !hediff.def.keepOnBodyPartRestoration)
				{
					hediffs.RemoveAt(i);
					hediff.PostRemoved();
				}
			}
			for (int j = 0; j < part.parts.Count; j++)
			{
				this.RestorePartRecursiveInt(part.parts[j], diffException);
			}
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0007DB3C File Offset: 0x0007BD3C
		public void CheckForStateChange(DamageInfo? dinfo, Hediff hediff)
		{
			if (!this.Dead)
			{
				if (this.ShouldBeDead())
				{
					if (!this.pawn.Destroyed)
					{
						this.pawn.Kill(dinfo, hediff);
					}
					return;
				}
				if (!this.Downed)
				{
					if (this.ShouldBeDowned())
					{
						if (!this.forceIncap && dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this.pawn) && !this.pawn.IsWildMan() && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && (this.pawn.HostFaction == null || !this.pawn.HostFaction.IsPlayer))
						{
							float num;
							if (this.pawn.RaceProps.Animal)
							{
								num = 0.5f;
							}
							else if (this.pawn.RaceProps.IsMechanoid)
							{
								num = 1f;
							}
							else
							{
								num = HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent) * Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor;
							}
							if (Rand.Chance(num))
							{
								if (DebugViewSettings.logCauseOfDeath)
								{
									Log.Message("CauseOfDeath: chance on downed " + num.ToStringPercent());
								}
								this.pawn.Kill(dinfo, null);
								return;
							}
						}
						this.forceIncap = false;
						this.MakeDowned(dinfo, hediff);
						return;
					}
					if (!this.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						if (this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null && this.pawn.jobs != null && this.pawn.CurJob != null)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
						if (this.pawn.equipment != null && this.pawn.equipment.Primary != null)
						{
							if (this.pawn.kindDef.destroyGearOnDrop)
							{
								this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
								return;
							}
							if (this.pawn.InContainerEnclosed)
							{
								this.pawn.equipment.TryTransferEquipmentToContainer(this.pawn.equipment.Primary, this.pawn.holdingOwner);
								return;
							}
							if (this.pawn.SpawnedOrAnyParentSpawned)
							{
								ThingWithComps thingWithComps;
								this.pawn.equipment.TryDropEquipment(this.pawn.equipment.Primary, out thingWithComps, this.pawn.PositionHeld, true);
								return;
							}
							if (!this.pawn.IsCaravanMember())
							{
								this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
								return;
							}
							ThingWithComps primary = this.pawn.equipment.Primary;
							this.pawn.equipment.Remove(primary);
							if (!this.pawn.inventory.innerContainer.TryAdd(primary, true))
							{
								primary.Destroy(DestroyMode.Vanish);
								return;
							}
						}
					}
				}
				else if (!this.ShouldBeDowned())
				{
					this.MakeUndowned();
					return;
				}
			}
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0007DE5D File Offset: 0x0007C05D
		private bool ShouldBeDowned()
		{
			return this.InPainShock || !this.capacities.CanBeAwake || !this.capacities.CapableOf(PawnCapacityDefOf.Moving);
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0007DE8C File Offset: 0x0007C08C
		private bool ShouldBeDead()
		{
			if (this.Dead)
			{
				return true;
			}
			for (int i = 0; i < this.hediffSet.hediffs.Count; i++)
			{
				if (this.hediffSet.hediffs[i].CauseDeathNow())
				{
					return true;
				}
			}
			if (this.ShouldBeDeadFromRequiredCapacity() != null)
			{
				return true;
			}
			if (PawnCapacityUtility.CalculatePartEfficiency(this.hediffSet, this.pawn.RaceProps.body.corePart, false, null) <= 0.0001f)
			{
				if (DebugViewSettings.logCauseOfDeath)
				{
					Log.Message("CauseOfDeath: zero efficiency of " + this.pawn.RaceProps.body.corePart.Label);
				}
				return true;
			}
			return this.ShouldBeDeadFromLethalDamageThreshold();
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0007DF48 File Offset: 0x0007C148
		public PawnCapacityDef ShouldBeDeadFromRequiredCapacity()
		{
			List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				PawnCapacityDef pawnCapacityDef = allDefsListForReading[i];
				if ((this.pawn.RaceProps.IsFlesh ? pawnCapacityDef.lethalFlesh : pawnCapacityDef.lethalMechanoids) && !this.capacities.CapableOf(pawnCapacityDef))
				{
					if (DebugViewSettings.logCauseOfDeath)
					{
						Log.Message("CauseOfDeath: no longer capable of " + pawnCapacityDef.defName);
					}
					return pawnCapacityDef;
				}
			}
			return null;
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x0007DFC4 File Offset: 0x0007C1C4
		public bool ShouldBeDeadFromLethalDamageThreshold()
		{
			float num = 0f;
			for (int i = 0; i < this.hediffSet.hediffs.Count; i++)
			{
				if (this.hediffSet.hediffs[i] is Hediff_Injury)
				{
					num += this.hediffSet.hediffs[i].Severity;
				}
			}
			bool flag = num >= this.LethalDamageThreshold;
			if (flag && DebugViewSettings.logCauseOfDeath)
			{
				Log.Message(string.Concat(new object[]
				{
					"CauseOfDeath: lethal damage ",
					num,
					" >= ",
					this.LethalDamageThreshold
				}));
			}
			return flag;
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0007E074 File Offset: 0x0007C274
		public bool WouldLosePartAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.CheckPredicateAfterAddingHediff(hediff, () => this.hediffSet.PartIsMissing(part));
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0007E0C4 File Offset: 0x0007C2C4
		public bool WouldDieAfterAddingHediff(Hediff hediff)
		{
			if (this.Dead)
			{
				return true;
			}
			bool flag = this.CheckPredicateAfterAddingHediff(hediff, new Func<bool>(this.ShouldBeDead));
			if (flag && DebugViewSettings.logCauseOfDeath)
			{
				Log.Message("CauseOfDeath: WouldDieAfterAddingHediff=true for " + this.pawn.Name);
			}
			return flag;
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0007E114 File Offset: 0x0007C314
		public bool WouldDieAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldDieAfterAddingHediff(hediff);
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0007E13D File Offset: 0x0007C33D
		public bool WouldBeDownedAfterAddingHediff(Hediff hediff)
		{
			return !this.Dead && this.CheckPredicateAfterAddingHediff(hediff, new Func<bool>(this.ShouldBeDowned));
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0007E15C File Offset: 0x0007C35C
		public bool WouldBeDownedAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldBeDownedAfterAddingHediff(hediff);
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0007E185 File Offset: 0x0007C385
		public void SetDead()
		{
			if (this.Dead)
			{
				Log.Error(this.pawn + " set dead while already dead.");
			}
			this.healthState = PawnHealthState.Dead;
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0007E1AC File Offset: 0x0007C3AC
		private bool CheckPredicateAfterAddingHediff(Hediff hediff, Func<bool> pred)
		{
			HashSet<Hediff> missing = this.CalculateMissingPartHediffsFromInjury(hediff);
			this.hediffSet.hediffs.Add(hediff);
			if (missing != null)
			{
				this.hediffSet.hediffs.AddRange(missing);
			}
			this.hediffSet.DirtyCache();
			bool result = pred();
			if (missing != null)
			{
				this.hediffSet.hediffs.RemoveAll((Hediff x) => missing.Contains(x));
			}
			this.hediffSet.hediffs.Remove(hediff);
			this.hediffSet.DirtyCache();
			return result;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0007E250 File Offset: 0x0007C450
		private HashSet<Hediff> CalculateMissingPartHediffsFromInjury(Hediff hediff)
		{
			Pawn_HealthTracker.<>c__DisplayClass48_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.hediff = hediff;
			CS$<>8__locals1.missing = null;
			if (CS$<>8__locals1.hediff.Part != null && CS$<>8__locals1.hediff.Part != this.pawn.RaceProps.body.corePart && CS$<>8__locals1.hediff.Severity >= this.hediffSet.GetPartHealth(CS$<>8__locals1.hediff.Part))
			{
				CS$<>8__locals1.missing = new HashSet<Hediff>();
				this.<CalculateMissingPartHediffsFromInjury>g__AddAllParts|48_0(CS$<>8__locals1.hediff.Part, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.missing;
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0007E2EC File Offset: 0x0007C4EC
		private void MakeDowned(DamageInfo? dinfo, Hediff hediff)
		{
			if (this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeDowned while already downed.");
				return;
			}
			if (this.pawn.guilt != null && this.pawn.GetLord() != null && this.pawn.GetLord().LordJob != null && this.pawn.GetLord().LordJob.GuiltyOnDowned)
			{
				this.pawn.guilt.Notify_Guilty(60000);
			}
			this.healthState = PawnHealthState.Down;
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this.pawn, dinfo, PawnDiedOrDownedThoughtsKind.Downed);
			if (this.pawn.InMentalState && this.pawn.MentalStateDef.recoverFromDowned)
			{
				this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
			if (this.pawn.Spawned)
			{
				this.pawn.DropAndForbidEverything(true);
				this.pawn.stances.CancelBusyStanceSoft();
			}
			this.pawn.ClearMind(true, false, false);
			if (Current.ProgramState == ProgramState.Playing)
			{
				Lord lord = this.pawn.GetLord();
				if (lord != null && (lord.LordJob == null || lord.LordJob.RemoveDownedPawns))
				{
					lord.Notify_PawnLost(this.pawn, PawnLostCondition.IncappedOrKilled, dinfo);
				}
			}
			if (this.pawn.Drafted)
			{
				this.pawn.drafter.Drafted = false;
			}
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			if (this.pawn.SpawnedOrAnyParentSpawned)
			{
				GenHostility.Notify_PawnLostForTutor(this.pawn, this.pawn.MapHeld);
			}
			if (this.pawn.RaceProps.Humanlike && Current.ProgramState == ProgramState.Playing && this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.pawn.HostileTo(Faction.OfPlayer))
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Capturing, this.pawn, OpportunityType.Important);
				}
				if (this.pawn.Faction == Faction.OfPlayer)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Rescuing, this.pawn, OpportunityType.Critical);
				}
			}
			if (dinfo != null && dinfo.Value.Instigator != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null)
				{
					RecordsUtility.Notify_PawnDowned(this.pawn, pawn);
				}
			}
			if (this.pawn.Spawned)
			{
				TaleRecorder.RecordTale(TaleDefOf.Downed, new object[]
				{
					this.pawn,
					(dinfo != null) ? (dinfo.Value.Instigator as Pawn) : null,
					(dinfo != null) ? dinfo.Value.Weapon : null
				});
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this.pawn, RulePackDefOf.Transition_Downed, (dinfo != null) ? (dinfo.Value.Instigator as Pawn) : null, hediff, (dinfo != null) ? dinfo.Value.HitPart : null));
			}
			Find.Storyteller.Notify_PawnEvent(this.pawn, AdaptationEvent.Downed, dinfo);
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0007E604 File Offset: 0x0007C804
		private void MakeUndowned()
		{
			if (!this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeUndowned when already undowned.");
				return;
			}
			this.healthState = PawnHealthState.Mobile;
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerDowned".Translate(this.pawn.LabelCap, this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			if (this.pawn.Spawned && !this.pawn.InBed())
			{
				this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
			}
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			if (this.pawn.guest != null)
			{
				this.pawn.guest.Notify_PawnUndowned();
			}
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0007E6E4 File Offset: 0x0007C8E4
		public void NotifyPlayerOfKilled(DamageInfo? dinfo, Hediff hediff, Caravan caravan)
		{
			TaggedString taggedString = "";
			if (dinfo != null)
			{
				taggedString = dinfo.Value.Def.deathMessage.Formatted(this.pawn.LabelShortCap, this.pawn.Named("PAWN"));
			}
			else if (hediff != null)
			{
				taggedString = "PawnDiedBecauseOf".Translate(this.pawn.LabelShortCap, hediff.def.LabelCap, this.pawn.Named("PAWN"));
			}
			else
			{
				taggedString = "PawnDied".Translate(this.pawn.LabelShortCap, this.pawn.Named("PAWN"));
			}
			Quest quest = null;
			if (this.pawn.IsBorrowedByAnyFaction())
			{
				foreach (QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction in QuestUtility.GetAllQuestPartsOfType<QuestPart_LendColonistsToFaction>(true))
				{
					if (questPart_LendColonistsToFaction.LentColonistsListForReading.Contains(this.pawn))
					{
						taggedString += "\n\n" + "LentColonistDied".Translate(this.pawn.Named("PAWN"), questPart_LendColonistsToFaction.lendColonistsToFaction.Named("FACTION"));
						quest = questPart_LendColonistsToFaction.quest;
						break;
					}
				}
			}
			taggedString = taggedString.AdjustedFor(this.pawn, "PAWN", true);
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				TaggedString taggedString2 = "Death".Translate() + ": " + this.pawn.LabelShortCap;
				if (caravan != null)
				{
					Messages.Message("MessageCaravanDeathCorpseAddedToInventory".Translate(this.pawn.Named("PAWN")), caravan, MessageTypeDefOf.PawnDeath, true);
				}
				if (this.pawn.Ideo != null)
				{
					foreach (Precept precept in this.pawn.Ideo.PreceptsListForReading)
					{
						if (!string.IsNullOrWhiteSpace(precept.def.extraTextPawnDeathLetter))
						{
							taggedString += "\n\n" + precept.def.extraTextPawnDeathLetter.Formatted(this.pawn.Named("PAWN"));
						}
					}
				}
				if (this.pawn.Name != null && !this.pawn.Name.Numerical && this.pawn.RaceProps.Animal)
				{
					taggedString2 += " (" + this.pawn.KindLabel + ")";
				}
				this.pawn.relations.CheckAppendBondedAnimalDiedInfo(ref taggedString, ref taggedString2);
				Find.LetterStack.ReceiveLetter(taggedString2, taggedString, LetterDefOf.Death, this.pawn, null, quest, null, null);
				return;
			}
			Messages.Message(taggedString, this.pawn, MessageTypeDefOf.PawnDeath, true);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0007EA18 File Offset: 0x0007CC18
		public void Notify_Resurrected()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x.TryGetComp<HediffComp_Immunizable>() != null);
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x is Hediff_Injury && !x.IsPermanent());
			this.hediffSet.hediffs.RemoveAll(delegate(Hediff x)
			{
				if (!x.def.everCurableByItem)
				{
					return false;
				}
				if (x.def.lethalSeverity >= 0f)
				{
					return true;
				}
				if (x.def.stages != null)
				{
					return x.def.stages.Any((HediffStage y) => y.lifeThreatening);
				}
				return false;
			});
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x is Hediff_Injury && x.IsPermanent() && this.hediffSet.GetPartHealth(x.Part) <= 0f);
			for (;;)
			{
				Hediff_MissingPart hediff_MissingPart = (from x in this.hediffSet.GetMissingPartsCommonAncestors()
				where !this.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x.Part)
				select x).FirstOrDefault<Hediff_MissingPart>();
				if (hediff_MissingPart == null)
				{
					break;
				}
				this.RestorePart(hediff_MissingPart.Part, null, false);
			}
			this.hediffSet.DirtyCache();
			if (this.ShouldBeDead())
			{
				this.hediffSet.hediffs.RemoveAll((Hediff h) => !h.def.keepOnBodyPartRestoration);
			}
			this.Notify_HediffChanged(null);
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0007EB58 File Offset: 0x0007CD58
		public void HealthTick()
		{
			if (this.Dead)
			{
				return;
			}
			for (int i = this.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff = this.hediffSet.hediffs[i];
				try
				{
					hediff.Tick();
					hediff.PostTick();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception ticking hediff ",
						hediff.ToStringSafe<Hediff>(),
						" for pawn ",
						this.pawn.ToStringSafe<Pawn>(),
						". Removing hediff... Exception: ",
						ex
					}));
					try
					{
						this.RemoveHediff(hediff);
					}
					catch (Exception arg)
					{
						Log.Error("Error while removing hediff: " + arg);
					}
				}
				if (this.Dead)
				{
					return;
				}
			}
			bool flag = false;
			for (int j = this.hediffSet.hediffs.Count - 1; j >= 0; j--)
			{
				Hediff hediff2 = this.hediffSet.hediffs[j];
				if (hediff2.ShouldRemove)
				{
					this.hediffSet.hediffs.RemoveAt(j);
					hediff2.PostRemoved();
					flag = true;
				}
			}
			if (flag)
			{
				this.Notify_HediffChanged(null);
			}
			if (this.Dead)
			{
				return;
			}
			this.immunity.ImmunityHandlerTick();
			if (this.pawn.RaceProps.IsFlesh && this.pawn.IsHashIntervalTick(600) && (this.pawn.needs.food == null || !this.pawn.needs.food.Starving))
			{
				bool flag2 = false;
				if (this.hediffSet.HasNaturallyHealingInjury())
				{
					float num = 8f;
					if (this.pawn.GetPosture() != PawnPosture.Standing)
					{
						num += 4f;
						Building_Bed building_Bed = this.pawn.CurrentBed();
						if (building_Bed != null)
						{
							num += building_Bed.def.building.bed_healPerDay;
						}
					}
					foreach (Hediff hediff3 in this.hediffSet.hediffs)
					{
						HediffStage curStage = hediff3.CurStage;
						if (curStage != null && curStage.naturalHealingFactor != -1f)
						{
							num *= curStage.naturalHealingFactor;
						}
					}
					(from x in this.hediffSet.GetHediffs<Hediff_Injury>()
					where x.CanHealNaturally()
					select x).RandomElement<Hediff_Injury>().Heal(num * this.pawn.HealthScale * 0.01f * this.pawn.GetStatValue(StatDefOf.InjuryHealingFactor, true));
					flag2 = true;
				}
				if (this.hediffSet.HasTendedAndHealingInjury() && (this.pawn.needs.food == null || !this.pawn.needs.food.Starving))
				{
					Hediff_Injury hediff_Injury = (from x in this.hediffSet.GetHediffs<Hediff_Injury>()
					where x.CanHealFromTending()
					select x).RandomElement<Hediff_Injury>();
					float tendQuality = hediff_Injury.TryGetComp<HediffComp_TendDuration>().tendQuality;
					float num2 = GenMath.LerpDouble(0f, 1f, 0.5f, 1.5f, Mathf.Clamp01(tendQuality));
					hediff_Injury.Heal(8f * num2 * this.pawn.HealthScale * 0.01f * this.pawn.GetStatValue(StatDefOf.InjuryHealingFactor, true));
					flag2 = true;
				}
				if (flag2 && !this.HasHediffsNeedingTendByPlayer(false) && !HealthAIUtility.ShouldSeekMedicalRest(this.pawn) && !this.hediffSet.HasTendedAndHealingInjury() && PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageFullyHealed".Translate(this.pawn.LabelCap, this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			if (this.pawn.RaceProps.IsFlesh && this.hediffSet.BleedRateTotal >= 0.1f)
			{
				float num3 = this.hediffSet.BleedRateTotal * this.pawn.BodySize;
				if (this.pawn.GetPosture() == PawnPosture.Standing)
				{
					num3 *= 0.004f;
				}
				else
				{
					num3 *= 0.0004f;
				}
				if (Rand.Value < num3)
				{
					this.DropBloodFilth();
				}
			}
			if (this.pawn.IsHashIntervalTick(60))
			{
				List<HediffGiverSetDef> hediffGiverSets = this.pawn.RaceProps.hediffGiverSets;
				if (hediffGiverSets != null)
				{
					for (int k = 0; k < hediffGiverSets.Count; k++)
					{
						List<HediffGiver> hediffGivers = hediffGiverSets[k].hediffGivers;
						for (int l = 0; l < hediffGivers.Count; l++)
						{
							hediffGivers[l].OnIntervalPassed(this.pawn, null);
							if (this.pawn.Dead)
							{
								return;
							}
						}
					}
				}
				if (this.pawn.story != null)
				{
					List<Trait> allTraits = this.pawn.story.traits.allTraits;
					for (int m = 0; m < allTraits.Count; m++)
					{
						TraitDegreeData currentData = allTraits[m].CurrentData;
						if (currentData.randomDiseaseMtbDays > 0f && Rand.MTBEventOccurs(currentData.randomDiseaseMtbDays, 60000f, 60f))
						{
							BiomeDef biome;
							if (this.pawn.Tile != -1)
							{
								biome = Find.WorldGrid[this.pawn.Tile].biome;
							}
							else
							{
								biome = DefDatabase<BiomeDef>.GetRandom();
							}
							IncidentDef incidentDef = (from d in DefDatabase<IncidentDef>.AllDefs
							where d.category == IncidentCategoryDefOf.DiseaseHuman
							select d).RandomElementByWeightWithFallback((IncidentDef d) => biome.CommonalityOfDisease(d), null);
							if (incidentDef != null)
							{
								string text;
								List<Pawn> list = ((IncidentWorker_Disease)incidentDef.Worker).ApplyToPawns(Gen.YieldSingle<Pawn>(this.pawn), out text);
								if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
								{
									if (list.Contains(this.pawn))
									{
										Find.LetterStack.ReceiveLetter("LetterLabelTraitDisease".Translate(incidentDef.diseaseIncident.label), "LetterTraitDisease".Translate(this.pawn.LabelCap, incidentDef.diseaseIncident.label, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true), LetterDefOf.NegativeEvent, this.pawn, null, null, null, null);
									}
									else if (!text.NullOrEmpty())
									{
										Messages.Message(text, this.pawn, MessageTypeDefOf.NeutralEvent, true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x0007F25C File Offset: 0x0007D45C
		public bool HasHediffsNeedingTend(bool forAlert = false)
		{
			return this.hediffSet.HasTendableHediff(forAlert);
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0007F26C File Offset: 0x0007D46C
		public bool HasHediffsNeedingTendByPlayer(bool forAlert = false)
		{
			if (this.HasHediffsNeedingTend(forAlert))
			{
				if (this.pawn.NonHumanlikeOrWildMan())
				{
					if (this.pawn.Faction == Faction.OfPlayer)
					{
						return true;
					}
					Building_Bed building_Bed = this.pawn.CurrentBed();
					if (building_Bed != null && building_Bed.Faction == Faction.OfPlayer)
					{
						return true;
					}
				}
				else if ((this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null) || this.pawn.HostFaction == Faction.OfPlayer)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x0007F2F8 File Offset: 0x0007D4F8
		public void DropBloodFilth()
		{
			if ((this.pawn.Spawned || this.pawn.ParentHolder is Pawn_CarryTracker) && this.pawn.SpawnedOrAnyParentSpawned && this.pawn.RaceProps.BloodDef != null)
			{
				FilthMaker.TryMakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, this.pawn.RaceProps.BloodDef, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
			}
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0007F37C File Offset: 0x0007D57C
		public IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Hediff hediff in this.hediffSet.hediffs)
			{
				IEnumerable<Gizmo> gizmos = hediff.GetGizmos();
				if (gizmos != null)
				{
					foreach (Gizmo gizmo in gizmos)
					{
						yield return gizmo;
					}
					IEnumerator<Gizmo> enumerator2 = null;
				}
			}
			List<Hediff>.Enumerator enumerator = default(List<Hediff>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0007F38C File Offset: 0x0007D58C
		[CompilerGenerated]
		private void <CalculateMissingPartHediffsFromInjury>g__AddAllParts|48_0(BodyPartRecord part, ref Pawn_HealthTracker.<>c__DisplayClass48_0 A_2)
		{
			Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
			hediff_MissingPart.lastInjury = A_2.hediff.def;
			hediff_MissingPart.Part = part;
			A_2.missing.Add(hediff_MissingPart);
			foreach (BodyPartRecord part2 in part.parts)
			{
				this.<CalculateMissingPartHediffsFromInjury>g__AddAllParts|48_0(part2, ref A_2);
			}
		}

		// Token: 0x04000F2B RID: 3883
		private Pawn pawn;

		// Token: 0x04000F2C RID: 3884
		private PawnHealthState healthState = PawnHealthState.Mobile;

		// Token: 0x04000F2D RID: 3885
		[Unsaved(false)]
		public Effecter woundedEffecter;

		// Token: 0x04000F2E RID: 3886
		[Unsaved(false)]
		public Effecter deflectionEffecter;

		// Token: 0x04000F2F RID: 3887
		public bool forceIncap;

		// Token: 0x04000F30 RID: 3888
		public bool beCarriedByCaravanIfSick;

		// Token: 0x04000F31 RID: 3889
		public bool killedByRitual;

		// Token: 0x04000F32 RID: 3890
		public int lastReceivedNeuralSuperchargeTick = -1;

		// Token: 0x04000F33 RID: 3891
		public HediffSet hediffSet;

		// Token: 0x04000F34 RID: 3892
		public PawnCapacitiesHandler capacities;

		// Token: 0x04000F35 RID: 3893
		public BillStack surgeryBills;

		// Token: 0x04000F36 RID: 3894
		public SummaryHealthHandler summaryHealth;

		// Token: 0x04000F37 RID: 3895
		public ImmunityHandler immunity;
	}
}
