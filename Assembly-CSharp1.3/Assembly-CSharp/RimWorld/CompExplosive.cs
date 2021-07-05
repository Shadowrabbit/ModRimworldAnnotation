using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200112A RID: 4394
	public class CompExplosive : ThingComp
	{
		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06006988 RID: 27016 RVA: 0x00239086 File Offset: 0x00237286
		public CompProperties_Explosive Props
		{
			get
			{
				return (CompProperties_Explosive)this.props;
			}
		}

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06006989 RID: 27017 RVA: 0x00239093 File Offset: 0x00237293
		protected int StartWickThreshold
		{
			get
			{
				return Mathf.RoundToInt(this.Props.startWickHitPointsPercent * (float)this.parent.MaxHitPoints);
			}
		}

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x0600698A RID: 27018 RVA: 0x002390B4 File Offset: 0x002372B4
		private bool CanEverExplodeFromDamage
		{
			get
			{
				if (this.Props.chanceNeverExplodeFromDamage < 1E-05f)
				{
					return true;
				}
				Rand.PushState();
				Rand.Seed = this.parent.thingIDNumber.GetHashCode();
				bool result = Rand.Value > this.Props.chanceNeverExplodeFromDamage;
				Rand.PopState();
				return result;
			}
		}

		// Token: 0x0600698B RID: 27019 RVA: 0x00239106 File Offset: 0x00237306
		public void AddThingsIgnoredByExplosion(List<Thing> things)
		{
			if (this.thingsIgnoredByExplosion == null)
			{
				this.thingsIgnoredByExplosion = new List<Thing>();
			}
			this.thingsIgnoredByExplosion.AddRange(things);
		}

		// Token: 0x0600698C RID: 27020 RVA: 0x00239128 File Offset: 0x00237328
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Collections.Look<Thing>(ref this.thingsIgnoredByExplosion, "thingsIgnoredByExplosion", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.wickStarted, "wickStarted", false, false);
			Scribe_Values.Look<int>(ref this.wickTicksLeft, "wickTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.destroyedThroughDetonation, "destroyedThroughDetonation", false, false);
			Scribe_Values.Look<int>(ref this.countdownTicksLeft, "countdownTicksLeft", 0, false);
			Scribe_Values.Look<float?>(ref this.customExplosiveRadius, "explosiveRadius", null, false);
		}

		// Token: 0x0600698D RID: 27021 RVA: 0x002391C4 File Offset: 0x002373C4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.Props.countdownTicks != null)
			{
				this.countdownTicksLeft = this.Props.countdownTicks.Value.RandomInRange;
			}
			this.UpdateOverlays();
		}

		// Token: 0x0600698E RID: 27022 RVA: 0x00239208 File Offset: 0x00237408
		public override void CompTick()
		{
			if (this.countdownTicksLeft > 0)
			{
				this.countdownTicksLeft--;
				if (this.countdownTicksLeft == 0)
				{
					this.StartWick(null);
					this.countdownTicksLeft = -1;
				}
			}
			if (this.wickStarted)
			{
				if (this.wickSoundSustainer == null)
				{
					this.StartWickSustainer();
				}
				else
				{
					this.wickSoundSustainer.Maintain();
				}
				if (this.Props.wickMessages != null)
				{
					foreach (WickMessage wickMessage in this.Props.wickMessages)
					{
						if (wickMessage.ticksLeft == this.wickTicksLeft && wickMessage.wickMessagekey != null)
						{
							Messages.Message(wickMessage.wickMessagekey.Translate(this.parent, this.wickTicksLeft.ToStringSecondsFromTicks()), this.parent, wickMessage.messageType ?? MessageTypeDefOf.NeutralEvent, false);
						}
					}
				}
				this.wickTicksLeft--;
				if (this.wickTicksLeft <= 0)
				{
					this.Detonate(this.parent.MapHeld, false);
				}
			}
		}

		// Token: 0x0600698F RID: 27023 RVA: 0x00239348 File Offset: 0x00237548
		private void StartWickSustainer()
		{
			SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.PerTick);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x06006990 RID: 27024 RVA: 0x002393A3 File Offset: 0x002375A3
		private void EndWickSustainer()
		{
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
		}

		// Token: 0x06006991 RID: 27025 RVA: 0x002393C0 File Offset: 0x002375C0
		private void UpdateOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayBurningWick);
			if (this.wickStarted)
			{
				this.overlayBurningWick = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.BurningWick));
			}
		}

		// Token: 0x06006992 RID: 27026 RVA: 0x0023942B File Offset: 0x0023762B
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.KillFinalize && this.Props.explodeOnKilled)
			{
				this.Detonate(previousMap, true);
			}
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x00239448 File Offset: 0x00237648
		public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			if (this.CanEverExplodeFromDamage)
			{
				if (dinfo.Def.ExternalViolenceFor(this.parent) && dinfo.Amount >= (float)this.parent.HitPoints && this.CanExplodeFromDamageType(dinfo.Def))
				{
					if (this.parent.MapHeld != null)
					{
						this.instigator = dinfo.Instigator;
						this.Detonate(this.parent.MapHeld, false);
						if (this.parent.Destroyed)
						{
							absorbed = true;
							return;
						}
					}
				}
				else if (!this.wickStarted && this.Props.startWickOnDamageTaken != null && this.Props.startWickOnDamageTaken.Contains(dinfo.Def))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		// Token: 0x06006994 RID: 27028 RVA: 0x00239514 File Offset: 0x00237714
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!this.CanEverExplodeFromDamage)
			{
				return;
			}
			if (!this.CanExplodeFromDamageType(dinfo.Def))
			{
				return;
			}
			if (!this.parent.Destroyed)
			{
				if (this.wickStarted && dinfo.Def == DamageDefOf.Stun)
				{
					this.StopWick();
					return;
				}
				if (!this.wickStarted && this.parent.HitPoints <= this.StartWickThreshold && dinfo.Def.ExternalViolenceFor(this.parent))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		// Token: 0x06006995 RID: 27029 RVA: 0x002395A0 File Offset: 0x002377A0
		public void StartWick(Thing instigator = null)
		{
			if (this.wickStarted)
			{
				return;
			}
			if (this.ExplosiveRadius() <= 0f)
			{
				return;
			}
			this.instigator = instigator;
			this.wickStarted = true;
			this.wickTicksLeft = this.Props.wickTicks.RandomInRange;
			this.StartWickSustainer();
			GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this.parent, this.Props.explosiveDamageType, null, instigator);
			this.UpdateOverlays();
		}

		// Token: 0x06006996 RID: 27030 RVA: 0x0023960C File Offset: 0x0023780C
		public void StopWick()
		{
			this.wickStarted = false;
			this.instigator = null;
			this.UpdateOverlays();
		}

		// Token: 0x06006997 RID: 27031 RVA: 0x00239624 File Offset: 0x00237824
		public float ExplosiveRadius()
		{
			CompProperties_Explosive props = this.Props;
			float num = this.customExplosiveRadius ?? this.Props.explosiveRadius;
			if (this.parent.stackCount > 1 && props.explosiveExpandPerStackcount > 0f)
			{
				num += Mathf.Sqrt((float)(this.parent.stackCount - 1) * props.explosiveExpandPerStackcount);
			}
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				num += Mathf.Sqrt(this.parent.GetComp<CompRefuelable>().Fuel * props.explosiveExpandPerFuel);
			}
			return num;
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x002396D0 File Offset: 0x002378D0
		protected void Detonate(Map map, bool ignoreUnspawned = false)
		{
			if (!ignoreUnspawned && !this.parent.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			CompProperties_Explosive props = this.Props;
			float num = this.ExplosiveRadius();
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				this.parent.GetComp<CompRefuelable>().ConsumeFuel(this.parent.GetComp<CompRefuelable>().Fuel);
			}
			if (props.destroyThingOnExplosionSize <= num && !this.parent.Destroyed)
			{
				this.destroyedThroughDetonation = true;
				this.parent.Kill(null, null);
			}
			this.EndWickSustainer();
			this.wickStarted = false;
			if (map == null)
			{
				Log.Warning("Tried to detonate CompExplosive in a null map.");
				return;
			}
			if (props.explosionEffect != null)
			{
				Effecter effecter = props.explosionEffect.Spawn();
				effecter.Trigger(new TargetInfo(this.parent.PositionHeld, map, false), new TargetInfo(this.parent.PositionHeld, map, false));
				effecter.Cleanup();
			}
			Thing parent;
			if (this.instigator != null && (!this.instigator.HostileTo(this.parent.Faction) || this.parent.Faction == Faction.OfPlayer))
			{
				parent = this.instigator;
			}
			else
			{
				parent = this.parent;
			}
			GenExplosion.DoExplosion(this.parent.PositionHeld, map, num, props.explosiveDamageType, parent, props.damageAmountBase, props.armorPenetrationBase, props.explosionSound, null, null, null, props.postExplosionSpawnThingDef, props.postExplosionSpawnChance, props.postExplosionSpawnThingCount, props.applyDamageToExplosionCellsNeighbors, props.preExplosionSpawnThingDef, props.preExplosionSpawnChance, props.preExplosionSpawnThingCount, props.chanceToStartFire, props.damageFalloff, null, this.thingsIgnoredByExplosion);
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x0023987B File Offset: 0x00237A7B
		private bool CanExplodeFromDamageType(DamageDef damage)
		{
			return this.Props.requiredDamageTypeToExplode == null || this.Props.requiredDamageTypeToExplode == damage;
		}

		// Token: 0x0600699A RID: 27034 RVA: 0x0023989C File Offset: 0x00237A9C
		public override string CompInspectStringExtra()
		{
			string text = "";
			if (this.countdownTicksLeft != -1)
			{
				text += "DetonationCountdown".Translate(this.countdownTicksLeft.TicksToDays().ToString("0.0"));
			}
			if (this.Props.extraInspectStringKey != null)
			{
				text += ((text != "") ? "\n" : "") + this.Props.extraInspectStringKey.Translate();
			}
			return text;
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x00239933 File Offset: 0x00237B33
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.countdownTicksLeft > 0)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Trigger countdown",
					action = delegate()
					{
						this.countdownTicksLeft = 1;
					}
				};
			}
			yield break;
		}

		// Token: 0x04003AFB RID: 15099
		public bool wickStarted;

		// Token: 0x04003AFC RID: 15100
		protected int wickTicksLeft;

		// Token: 0x04003AFD RID: 15101
		private Thing instigator;

		// Token: 0x04003AFE RID: 15102
		private int countdownTicksLeft = -1;

		// Token: 0x04003AFF RID: 15103
		public bool destroyedThroughDetonation;

		// Token: 0x04003B00 RID: 15104
		private List<Thing> thingsIgnoredByExplosion;

		// Token: 0x04003B01 RID: 15105
		public float? customExplosiveRadius;

		// Token: 0x04003B02 RID: 15106
		protected Sustainer wickSoundSustainer;

		// Token: 0x04003B03 RID: 15107
		private OverlayHandle? overlayBurningWick;
	}
}
