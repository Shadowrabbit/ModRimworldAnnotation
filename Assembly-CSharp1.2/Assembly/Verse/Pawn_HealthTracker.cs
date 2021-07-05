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
	// Token: 0x02000457 RID: 1111
	public class Pawn_HealthTracker : IExposable
	{
		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x000197B7 File Offset: 0x000179B7
		public PawnHealthState State
		{
			get
			{
				return this.healthState;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001C13 RID: 7187 RVA: 0x000197BF File Offset: 0x000179BF
		public bool Downed
		{
			get
			{
				return this.healthState == PawnHealthState.Down;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x000197CA File Offset: 0x000179CA
		public bool Dead
		{
			get
			{
				return this.healthState == PawnHealthState.Dead;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001C15 RID: 7189 RVA: 0x000197D5 File Offset: 0x000179D5
		public float LethalDamageThreshold
		{
			get
			{
				return 150f * this.pawn.HealthScale;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x000197E8 File Offset: 0x000179E8
		public bool InPainShock
		{
			get
			{
				return this.hediffSet.PainTotal >= this.pawn.GetStatValue(StatDefOf.PainShockThreshold, true);
			}
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x000EE67C File Offset: 0x000EC87C
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

		// Token: 0x06001C18 RID: 7192 RVA: 0x000EE6EC File Offset: 0x000EC8EC
		public void Reset()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.Clear();
			this.capacities.Clear();
			this.summaryHealth.Notify_HealthChanged();
			this.surgeryBills.Clear();
			this.immunity = new ImmunityHandler(this.pawn);
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x000EE740 File Offset: 0x000EC940
		public void ExposeData()
		{
			Scribe_Values.Look<PawnHealthState>(ref this.healthState, "healthState", PawnHealthState.Mobile, false);
			Scribe_Values.Look<bool>(ref this.forceIncap, "forceIncap", false, false);
			Scribe_Values.Look<bool>(ref this.beCarriedByCaravanIfSick, "beCarriedByCaravanIfSick", true, false);
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

		// Token: 0x06001C1A RID: 7194 RVA: 0x000EE7E0 File Offset: 0x000EC9E0
		public Hediff AddHediff(HediffDef def, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, null);
			this.AddHediff(hediff, part, dinfo, result);
			return hediff;
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x000EE808 File Offset: 0x000ECA08
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

		// Token: 0x06001C1C RID: 7196 RVA: 0x0001980B File Offset: 0x00017A0B
		public void RemoveHediff(Hediff hediff)
		{
			this.hediffSet.hediffs.Remove(hediff);
			hediff.PostRemoved();
			this.Notify_HediffChanged(null);
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000EE8B0 File Offset: 0x000ECAB0
		public void Notify_HediffChanged(Hediff hediff)
		{
			this.hediffSet.DirtyCache();
			this.CheckForStateChange(null, hediff);
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x000EE8D8 File Offset: 0x000ECAD8
		public void Notify_UsedVerb(Verb verb, LocalTargetInfo target)
		{
			foreach (Hediff hediff in this.hediffSet.hediffs)
			{
				hediff.Notify_PawnUsedVerb(verb, target);
			}
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000EE930 File Offset: 0x000ECB30
		public void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			Faction factionOrExtraMiniOrHomeFaction = this.pawn.FactionOrExtraMiniOrHomeFaction;
			if (dinfo.Instigator != null && factionOrExtraMiniOrHomeFaction != null && factionOrExtraMiniOrHomeFaction.IsPlayer && !this.pawn.InAggroMentalState)
			{
				Pawn pawn = dinfo.Instigator as Pawn;
				if (pawn != null && pawn.guilt != null && pawn.mindState != null)
				{
					pawn.guilt.Notify_Guilty();
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
			if (factionOrExtraMiniOrHomeFaction != null)
			{
				factionOrExtraMiniOrHomeFaction.Notify_MemberTookDamage(this.pawn, dinfo);
				if (Current.ProgramState == ProgramState.Playing && factionOrExtraMiniOrHomeFaction == Faction.OfPlayer && dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.SpawnedOrAnyParentSpawned)
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
				this.pawn.stances.stunner.Notify_DamageApplied(dinfo, !this.pawn.RaceProps.IsFlesh);
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
					if (this.pawn.RaceProps.Humanlike && pawn2.RaceProps.Humanlike && this.pawn.needs.mood != null && (!pawn2.HostileTo(this.pawn) || (pawn2.Faction == factionOrExtraMiniOrHomeFaction && pawn2.InMentalState)))
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HarmedMe, pawn2);
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

		// Token: 0x06001C20 RID: 7200 RVA: 0x000EEC20 File Offset: 0x000ECE20
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
				if (dinfo.Def.additionalHediffs != null)
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

		// Token: 0x06001C21 RID: 7201 RVA: 0x000EED6C File Offset: 0x000ECF6C
		public void RestorePart(BodyPartRecord part, Hediff diffException = null, bool checkStateChange = true)
		{
			if (part == null)
			{
				Log.Error("Tried to restore null body part.", false);
				return;
			}
			this.RestorePartRecursiveInt(part, diffException);
			this.hediffSet.DirtyCache();
			if (checkStateChange)
			{
				this.CheckForStateChange(null, null);
			}
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000EEDB0 File Offset: 0x000ECFB0
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

		// Token: 0x06001C23 RID: 7203 RVA: 0x000EEE34 File Offset: 0x000ED034
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
								num = HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent) * Find.Storyteller.difficultyValues.enemyDeathOnDownedChanceFactor;
							}
							if (Rand.Chance(num))
							{
								if (DebugViewSettings.logCauseOfDeath)
								{
									Log.Message("CauseOfDeath: chance on downed " + num.ToStringPercent(), false);
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

		// Token: 0x06001C24 RID: 7204 RVA: 0x0001982C File Offset: 0x00017A2C
		private bool ShouldBeDowned()
		{
			return this.InPainShock || !this.capacities.CanBeAwake || !this.capacities.CapableOf(PawnCapacityDefOf.Moving);
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x000EF158 File Offset: 0x000ED358
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
					Log.Message("CauseOfDeath: zero efficiency of " + this.pawn.RaceProps.body.corePart.Label, false);
				}
				return true;
			}
			return this.ShouldBeDeadFromLethalDamageThreshold();
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x000EF218 File Offset: 0x000ED418
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
						Log.Message("CauseOfDeath: no longer capable of " + pawnCapacityDef.defName, false);
					}
					return pawnCapacityDef;
				}
			}
			return null;
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x000EF294 File Offset: 0x000ED494
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
				}), false);
			}
			return flag;
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x000EF344 File Offset: 0x000ED544
		public bool WouldDieAfterAddingHediff(Hediff hediff)
		{
			if (this.Dead)
			{
				return true;
			}
			bool flag = this.CheckPredicateAfterAddingHediff(hediff, new Func<bool>(this.ShouldBeDead));
			if (flag && DebugViewSettings.logCauseOfDeath)
			{
				Log.Message("CauseOfDeath: WouldDieAfterAddingHediff=true for " + this.pawn.Name, false);
			}
			return flag;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000EF394 File Offset: 0x000ED594
		public bool WouldDieAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldDieAfterAddingHediff(hediff);
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x00019858 File Offset: 0x00017A58
		public bool WouldBeDownedAfterAddingHediff(Hediff hediff)
		{
			return !this.Dead && this.CheckPredicateAfterAddingHediff(hediff, new Func<bool>(this.ShouldBeDowned));
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x000EF3C0 File Offset: 0x000ED5C0
		public bool WouldBeDownedAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldBeDownedAfterAddingHediff(hediff);
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x00019877 File Offset: 0x00017A77
		public void SetDead()
		{
			if (this.Dead)
			{
				Log.Error(this.pawn + " set dead while already dead.", false);
			}
			this.healthState = PawnHealthState.Dead;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000EF3EC File Offset: 0x000ED5EC
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

		// Token: 0x06001C2E RID: 7214 RVA: 0x000EF490 File Offset: 0x000ED690
		private HashSet<Hediff> CalculateMissingPartHediffsFromInjury(Hediff hediff)
		{
			Pawn_HealthTracker.<>c__DisplayClass44_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.hediff = hediff;
			CS$<>8__locals1.missing = null;
			if (CS$<>8__locals1.hediff.Part != null && CS$<>8__locals1.hediff.Part != this.pawn.RaceProps.body.corePart && CS$<>8__locals1.hediff.Severity >= this.hediffSet.GetPartHealth(CS$<>8__locals1.hediff.Part))
			{
				CS$<>8__locals1.missing = new HashSet<Hediff>();
				this.<CalculateMissingPartHediffsFromInjury>g__AddAllParts|44_0(CS$<>8__locals1.hediff.Part, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.missing;
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000EF52C File Offset: 0x000ED72C
		private void MakeDowned(DamageInfo? dinfo, Hediff hediff)
		{
			if (this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeDowned while already downed.", false);
				return;
			}
			if (this.pawn.guilt != null && this.pawn.GetLord() != null && this.pawn.GetLord().LordJob != null && this.pawn.GetLord().LordJob.GuiltyOnDowned)
			{
				this.pawn.guilt.Notify_Guilty();
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

		// Token: 0x06001C30 RID: 7216 RVA: 0x000EF834 File Offset: 0x000EDA34
		private void MakeUndowned()
		{
			if (!this.Downed)
			{
				Log.Error(this.pawn + " tried to do MakeUndowned when already undowned.", false);
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
			if (this.pawn.guest != null)
			{
				this.pawn.guest.Notify_PawnUndowned();
			}
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000EF90C File Offset: 0x000EDB0C
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

		// Token: 0x06001C32 RID: 7218 RVA: 0x000EFBA4 File Offset: 0x000EDDA4
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

		// Token: 0x06001C33 RID: 7219 RVA: 0x000EFCE4 File Offset: 0x000EDEE4
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
					}), false);
					try
					{
						this.RemoveHediff(hediff);
					}
					catch (Exception arg)
					{
						Log.Error("Error while removing hediff: " + arg, false);
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
					select x).RandomElement<Hediff_Injury>().Heal(num * this.pawn.HealthScale * 0.01f);
					flag2 = true;
				}
				if (this.hediffSet.HasTendedAndHealingInjury() && (this.pawn.needs.food == null || !this.pawn.needs.food.Starving))
				{
					Hediff_Injury hediff_Injury = (from x in this.hediffSet.GetHediffs<Hediff_Injury>()
					where x.CanHealFromTending()
					select x).RandomElement<Hediff_Injury>();
					float tendQuality = hediff_Injury.TryGetComp<HediffComp_TendDuration>().tendQuality;
					float num2 = GenMath.LerpDouble(0f, 1f, 0.5f, 1.5f, Mathf.Clamp01(tendQuality));
					hediff_Injury.Heal(8f * num2 * this.pawn.HealthScale * 0.01f);
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

		// Token: 0x06001C34 RID: 7220 RVA: 0x0001989E File Offset: 0x00017A9E
		public bool HasHediffsNeedingTend(bool forAlert = false)
		{
			return this.hediffSet.HasTendableHediff(forAlert);
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000F03C8 File Offset: 0x000EE5C8
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

		// Token: 0x06001C36 RID: 7222 RVA: 0x000F0454 File Offset: 0x000EE654
		public void DropBloodFilth()
		{
			if ((this.pawn.Spawned || this.pawn.ParentHolder is Pawn_CarryTracker) && this.pawn.SpawnedOrAnyParentSpawned && this.pawn.RaceProps.BloodDef != null)
			{
				FilthMaker.TryMakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, this.pawn.RaceProps.BloodDef, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
			}
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000F04D8 File Offset: 0x000EE6D8
		[CompilerGenerated]
		private void <CalculateMissingPartHediffsFromInjury>g__AddAllParts|44_0(BodyPartRecord part, ref Pawn_HealthTracker.<>c__DisplayClass44_0 A_2)
		{
			Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
			hediff_MissingPart.lastInjury = A_2.hediff.def;
			hediff_MissingPart.Part = part;
			A_2.missing.Add(hediff_MissingPart);
			foreach (BodyPartRecord part2 in part.parts)
			{
				this.<CalculateMissingPartHediffsFromInjury>g__AddAllParts|44_0(part2, ref A_2);
			}
		}

		// Token: 0x04001438 RID: 5176
		private Pawn pawn;

		// Token: 0x04001439 RID: 5177
		private PawnHealthState healthState = PawnHealthState.Mobile;

		// Token: 0x0400143A RID: 5178
		[Unsaved(false)]
		public Effecter woundedEffecter;

		// Token: 0x0400143B RID: 5179
		[Unsaved(false)]
		public Effecter deflectionEffecter;

		// Token: 0x0400143C RID: 5180
		public bool forceIncap;

		// Token: 0x0400143D RID: 5181
		public bool beCarriedByCaravanIfSick;

		// Token: 0x0400143E RID: 5182
		public HediffSet hediffSet;

		// Token: 0x0400143F RID: 5183
		public PawnCapacitiesHandler capacities;

		// Token: 0x04001440 RID: 5184
		public BillStack surgeryBills;

		// Token: 0x04001441 RID: 5185
		public SummaryHealthHandler summaryHealth;

		// Token: 0x04001442 RID: 5186
		public ImmunityHandler immunity;
	}
}
