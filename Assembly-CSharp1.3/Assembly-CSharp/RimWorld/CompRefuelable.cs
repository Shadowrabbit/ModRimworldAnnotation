using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117C RID: 4476
	[StaticConstructorOnStartup]
	public class CompRefuelable : ThingComp
	{
		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06006B92 RID: 27538 RVA: 0x00241C1A File Offset: 0x0023FE1A
		// (set) Token: 0x06006B93 RID: 27539 RVA: 0x00241C54 File Offset: 0x0023FE54
		public float TargetFuelLevel
		{
			get
			{
				if (this.configuredTargetFuelLevel >= 0f)
				{
					return this.configuredTargetFuelLevel;
				}
				if (this.Props.targetFuelLevelConfigurable)
				{
					return this.Props.initialConfigurableTargetFuelLevel;
				}
				return this.Props.fuelCapacity;
			}
			set
			{
				this.configuredTargetFuelLevel = Mathf.Clamp(value, 0f, this.Props.fuelCapacity);
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06006B94 RID: 27540 RVA: 0x00241C72 File Offset: 0x0023FE72
		public CompProperties_Refuelable Props
		{
			get
			{
				return (CompProperties_Refuelable)this.props;
			}
		}

		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06006B95 RID: 27541 RVA: 0x00241C7F File Offset: 0x0023FE7F
		public float Fuel
		{
			get
			{
				return this.fuel;
			}
		}

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06006B96 RID: 27542 RVA: 0x00241C87 File Offset: 0x0023FE87
		public float FuelPercentOfTarget
		{
			get
			{
				return this.fuel / this.TargetFuelLevel;
			}
		}

		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x06006B97 RID: 27543 RVA: 0x00241C96 File Offset: 0x0023FE96
		public float FuelPercentOfMax
		{
			get
			{
				return this.fuel / this.Props.fuelCapacity;
			}
		}

		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06006B98 RID: 27544 RVA: 0x00241CAA File Offset: 0x0023FEAA
		public bool IsFull
		{
			get
			{
				return this.TargetFuelLevel - this.fuel < 1f;
			}
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06006B99 RID: 27545 RVA: 0x00241CC0 File Offset: 0x0023FEC0
		public bool HasFuel
		{
			get
			{
				return this.fuel > 0f && this.fuel >= this.Props.minimumFueledThreshold;
			}
		}

		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06006B9A RID: 27546 RVA: 0x00241CE7 File Offset: 0x0023FEE7
		private float ConsumptionRatePerTick
		{
			get
			{
				return this.Props.fuelConsumptionRate / 60000f;
			}
		}

		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06006B9B RID: 27547 RVA: 0x00241CFA File Offset: 0x0023FEFA
		public bool ShouldAutoRefuelNow
		{
			get
			{
				return this.FuelPercentOfTarget <= this.Props.autoRefuelPercent && !this.IsFull && this.TargetFuelLevel > 0f && this.ShouldAutoRefuelNowIgnoringFuelPct;
			}
		}

		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06006B9C RID: 27548 RVA: 0x00241D2C File Offset: 0x0023FF2C
		public bool ShouldAutoRefuelNowIgnoringFuelPct
		{
			get
			{
				return !this.parent.IsBurning() && (this.flickComp == null || this.flickComp.SwitchIsOn) && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Flick) == null && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Deconstruct) == null;
			}
		}

		// Token: 0x06006B9D RID: 27549 RVA: 0x00241DA4 File Offset: 0x0023FFA4
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowAutoRefuel = this.Props.initialAllowAutoRefuel;
			this.fuel = this.Props.fuelCapacity * this.Props.initialFuelPercent;
			this.flickComp = this.parent.GetComp<CompFlickable>();
		}

		// Token: 0x06006B9E RID: 27550 RVA: 0x00241DF8 File Offset: 0x0023FFF8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fuel, "fuel", 0f, false);
			Scribe_Values.Look<float>(ref this.configuredTargetFuelLevel, "configuredTargetFuelLevel", -1f, false);
			Scribe_Values.Look<bool>(ref this.allowAutoRefuel, "allowAutoRefuel", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.Props.showAllowAutoRefuelToggle)
			{
				this.allowAutoRefuel = this.Props.initialAllowAutoRefuel;
			}
		}

		// Token: 0x06006B9F RID: 27551 RVA: 0x00241E70 File Offset: 0x00240070
		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.allowAutoRefuel)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenRefuel);
			}
			else if (!this.HasFuel && this.Props.drawOutOfFuelOverlay)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.OutOfFuel);
			}
			if (this.Props.drawFuelGaugeInMap)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = this.parent.DrawPos + Vector3.up * 0.1f;
				r.size = CompRefuelable.FuelBarSize;
				r.fillPercent = this.FuelPercentOfMax;
				r.filledMat = CompRefuelable.FuelBarFilledMat;
				r.unfilledMat = CompRefuelable.FuelBarUnfilledMat;
				r.margin = 0.15f;
				Rot4 rotation = this.parent.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				r.rotation = rotation;
				GenDraw.DrawFillableBar(r);
			}
		}

		// Token: 0x06006BA0 RID: 27552 RVA: 0x00241F80 File Offset: 0x00240180
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Props.fuelIsMortarBarrel && !Find.Storyteller.difficulty.classicMortars)
			{
				return;
			}
			if (previousMap != null && this.Props.fuelFilter.AllowedDefCount == 1 && this.Props.initialFuelPercent == 0f)
			{
				ThingDef thingDef = this.Props.fuelFilter.AllowedThingDefs.First<ThingDef>();
				int i = GenMath.RoundRandom(1f * this.fuel);
				while (i > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = Mathf.Min(i, thingDef.stackLimit);
					i -= thing.stackCount;
					GenPlace.TryPlaceThing(thing, this.parent.Position, previousMap, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
		}

		// Token: 0x06006BA1 RID: 27553 RVA: 0x00242054 File Offset: 0x00240254
		public override string CompInspectStringExtra()
		{
			if (this.Props.fuelIsMortarBarrel && !Find.Storyteller.difficulty.classicMortars)
			{
				return string.Empty;
			}
			string text = string.Concat(new string[]
			{
				this.Props.FuelLabel,
				": ",
				this.fuel.ToStringDecimalIfSmall(),
				" / ",
				this.Props.fuelCapacity.ToStringDecimalIfSmall()
			});
			if (!this.Props.consumeFuelOnlyWhenUsed && this.HasFuel)
			{
				int numTicks = (int)(this.fuel / this.Props.fuelConsumptionRate * 60000f);
				text = text + " (" + numTicks.ToStringTicksToPeriod(true, false, true, true) + ")";
			}
			if (!this.HasFuel && !this.Props.outOfFuelMessage.NullOrEmpty())
			{
				text += string.Format("\n{0} ({1}x {2})", this.Props.outOfFuelMessage, this.GetFuelCountToFullyRefuel(), this.Props.fuelFilter.AnyAllowedDef.label);
			}
			if (this.Props.targetFuelLevelConfigurable)
			{
				text += "\n" + "ConfiguredTargetFuelLevel".Translate(this.TargetFuelLevel.ToStringDecimalIfSmall());
			}
			return text;
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x002421B0 File Offset: 0x002403B0
		public override void CompTick()
		{
			base.CompTick();
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (!this.Props.consumeFuelOnlyWhenUsed && (this.flickComp == null || this.flickComp.SwitchIsOn) && (!this.Props.consumeFuelOnlyWhenPowered || (comp != null && comp.PowerOn)))
			{
				this.ConsumeFuel(this.ConsumptionRatePerTick);
			}
			if (this.Props.fuelConsumptionPerTickInRain > 0f && this.parent.Spawned && this.parent.Map.weatherManager.RainRate > 0.4f && !this.parent.Map.roofGrid.Roofed(this.parent.Position))
			{
				this.ConsumeFuel(this.Props.fuelConsumptionPerTickInRain);
			}
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x00242284 File Offset: 0x00240484
		public void ConsumeFuel(float amount)
		{
			if (this.Props.fuelIsMortarBarrel && !Find.Storyteller.difficulty.classicMortars)
			{
				return;
			}
			if (this.fuel <= 0f)
			{
				return;
			}
			this.fuel -= amount;
			if (this.fuel <= 0f)
			{
				this.fuel = 0f;
				if (this.Props.destroyOnNoFuel)
				{
					this.parent.Destroy(DestroyMode.Vanish);
				}
				this.parent.BroadcastCompSignal("RanOutOfFuel");
			}
		}

		// Token: 0x06006BA4 RID: 27556 RVA: 0x00242310 File Offset: 0x00240510
		public void Refuel(List<Thing> fuelThings)
		{
			if (this.Props.atomicFueling)
			{
				if (fuelThings.Sum((Thing t) => t.stackCount) < this.GetFuelCountToFullyRefuel())
				{
					Log.ErrorOnce("Error refueling; not enough fuel available for proper atomic refuel", 19586442);
					return;
				}
			}
			int num = this.GetFuelCountToFullyRefuel();
			while (num > 0 && fuelThings.Count > 0)
			{
				Thing thing = fuelThings.Pop<Thing>();
				int num2 = Mathf.Min(num, thing.stackCount);
				this.Refuel((float)num2);
				thing.SplitOff(num2).Destroy(DestroyMode.Vanish);
				num -= num2;
			}
		}

		// Token: 0x06006BA5 RID: 27557 RVA: 0x002423AC File Offset: 0x002405AC
		public void Refuel(float amount)
		{
			this.fuel += amount * this.Props.FuelMultiplierCurrentDifficulty;
			if (this.fuel > this.Props.fuelCapacity)
			{
				this.fuel = this.Props.fuelCapacity;
			}
			this.parent.BroadcastCompSignal("Refueled");
		}

		// Token: 0x06006BA6 RID: 27558 RVA: 0x00242407 File Offset: 0x00240607
		public void Notify_UsedThisTick()
		{
			this.ConsumeFuel(this.ConsumptionRatePerTick);
		}

		// Token: 0x06006BA7 RID: 27559 RVA: 0x00242418 File Offset: 0x00240618
		public int GetFuelCountToFullyRefuel()
		{
			if (this.Props.atomicFueling)
			{
				return Mathf.CeilToInt(this.Props.fuelCapacity / this.Props.FuelMultiplierCurrentDifficulty);
			}
			return Mathf.Max(Mathf.CeilToInt((this.TargetFuelLevel - this.fuel) / this.Props.FuelMultiplierCurrentDifficulty), 1);
		}

		// Token: 0x06006BA8 RID: 27560 RVA: 0x00242473 File Offset: 0x00240673
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.Props.fuelIsMortarBarrel && !Find.Storyteller.difficulty.classicMortars)
			{
				yield break;
			}
			if (this.Props.targetFuelLevelConfigurable)
			{
				yield return new Command_SetTargetFuelLevel
				{
					refuelable = this,
					defaultLabel = "CommandSetTargetFuelLevel".Translate(),
					defaultDesc = "CommandSetTargetFuelLevelDesc".Translate(),
					icon = CompRefuelable.SetTargetFuelLevelCommand
				};
			}
			if (this.Props.showFuelGizmo && Find.Selector.SingleSelectedThing == this.parent)
			{
				yield return new Gizmo_RefuelableFuelStatus
				{
					refuelable = this
				};
			}
			if (this.Props.showAllowAutoRefuelToggle)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandToggleAllowAutoRefuel".Translate(),
					defaultDesc = "CommandToggleAllowAutoRefuelDesc".Translate(),
					hotKey = KeyBindingDefOf.Command_ItemForbid,
					icon = (this.allowAutoRefuel ? TexCommand.ForbidOff : TexCommand.ForbidOn),
					isActive = (() => this.allowAutoRefuel),
					toggleAction = delegate()
					{
						this.allowAutoRefuel = !this.allowAutoRefuel;
					}
				};
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to 0",
					action = delegate()
					{
						this.fuel = 0f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to 0.1",
					action = delegate()
					{
						this.fuel = 0.1f;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set fuel to max",
					action = delegate()
					{
						this.fuel = this.Props.fuelCapacity;
						this.parent.BroadcastCompSignal("Refueled");
					}
				};
			}
			yield break;
		}

		// Token: 0x04003BEB RID: 15339
		private float fuel;

		// Token: 0x04003BEC RID: 15340
		private float configuredTargetFuelLevel = -1f;

		// Token: 0x04003BED RID: 15341
		public bool allowAutoRefuel = true;

		// Token: 0x04003BEE RID: 15342
		private CompFlickable flickComp;

		// Token: 0x04003BEF RID: 15343
		public const string RefueledSignal = "Refueled";

		// Token: 0x04003BF0 RID: 15344
		public const string RanOutOfFuelSignal = "RanOutOfFuel";

		// Token: 0x04003BF1 RID: 15345
		private static readonly Texture2D SetTargetFuelLevelCommand = ContentFinder<Texture2D>.Get("UI/Commands/SetTargetFuelLevel", true);

		// Token: 0x04003BF2 RID: 15346
		private static readonly Vector2 FuelBarSize = new Vector2(1f, 0.2f);

		// Token: 0x04003BF3 RID: 15347
		private static readonly Material FuelBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.56f, 0.13f), false);

		// Token: 0x04003BF4 RID: 15348
		private static readonly Material FuelBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
