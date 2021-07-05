using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200117B RID: 4475
	public class CompProperties_Refuelable : CompProperties
	{
		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06006B88 RID: 27528 RVA: 0x00241A86 File Offset: 0x0023FC86
		public string FuelLabel
		{
			get
			{
				if (this.fuelLabel.NullOrEmpty())
				{
					return "Fuel".TranslateSimple();
				}
				return this.fuelLabel;
			}
		}

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x06006B89 RID: 27529 RVA: 0x00241AA6 File Offset: 0x0023FCA6
		public string FuelGizmoLabel
		{
			get
			{
				if (this.fuelGizmoLabel.NullOrEmpty())
				{
					return "Fuel".TranslateSimple();
				}
				return this.fuelGizmoLabel;
			}
		}

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x06006B8A RID: 27530 RVA: 0x00241AC8 File Offset: 0x0023FCC8
		public Texture2D FuelIcon
		{
			get
			{
				if (this.fuelIcon == null)
				{
					if (!this.fuelIconPath.NullOrEmpty())
					{
						this.fuelIcon = ContentFinder<Texture2D>.Get(this.fuelIconPath, true);
					}
					else
					{
						ThingDef thingDef;
						if (this.fuelFilter.AnyAllowedDef != null)
						{
							thingDef = this.fuelFilter.AnyAllowedDef;
						}
						else
						{
							thingDef = ThingDefOf.Chemfuel;
						}
						this.fuelIcon = thingDef.uiIcon;
					}
				}
				return this.fuelIcon;
			}
		}

		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06006B8B RID: 27531 RVA: 0x00241B37 File Offset: 0x0023FD37
		public float FuelMultiplierCurrentDifficulty
		{
			get
			{
				if (this.factorByDifficulty)
				{
					Storyteller storyteller = Find.Storyteller;
					if (((storyteller != null) ? storyteller.difficulty : null) != null)
					{
						return this.fuelMultiplier / Find.Storyteller.difficulty.maintenanceCostFactor;
					}
				}
				return this.fuelMultiplier;
			}
		}

		// Token: 0x06006B8C RID: 27532 RVA: 0x00241B74 File Offset: 0x0023FD74
		public CompProperties_Refuelable()
		{
			this.compClass = typeof(CompRefuelable);
		}

		// Token: 0x06006B8D RID: 27533 RVA: 0x00241BD8 File Offset: 0x0023FDD8
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			this.fuelFilter.ResolveReferences();
		}

		// Token: 0x06006B8E RID: 27534 RVA: 0x00241BEC File Offset: 0x0023FDEC
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.destroyOnNoFuel && this.initialFuelPercent <= 0f)
			{
				yield return "Refuelable component has destroyOnNoFuel, but initialFuelPercent <= 0";
			}
			if ((!this.consumeFuelOnlyWhenUsed || this.fuelConsumptionPerTickInRain > 0f) && parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Format("Refuelable component set to consume fuel per tick, but parent tickertype is {0} instead of {1}", parentDef.tickerType, TickerType.Normal);
			}
			yield break;
			yield break;
		}

		// Token: 0x06006B8F RID: 27535 RVA: 0x00241C03 File Offset: 0x0023FE03
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (((ThingDef)req.Def).building.IsTurret)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "RearmCost".Translate(), GenLabel.ThingLabel(this.fuelFilter.AnyAllowedDef, null, (int)(this.fuelCapacity / this.FuelMultiplierCurrentDifficulty)).CapitalizeFirst(), "RearmCostExplanation".Translate(), 3171, null, null, false);
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "ShotsBeforeRearm".Translate(), ((int)this.fuelCapacity).ToString(), "ShotsBeforeRearmExplanation".Translate(), 3171, null, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x04003BD0 RID: 15312
		public float fuelConsumptionRate = 1f;

		// Token: 0x04003BD1 RID: 15313
		public float fuelCapacity = 2f;

		// Token: 0x04003BD2 RID: 15314
		public float initialFuelPercent;

		// Token: 0x04003BD3 RID: 15315
		public float autoRefuelPercent = 0.3f;

		// Token: 0x04003BD4 RID: 15316
		public float fuelConsumptionPerTickInRain;

		// Token: 0x04003BD5 RID: 15317
		public ThingFilter fuelFilter;

		// Token: 0x04003BD6 RID: 15318
		public bool destroyOnNoFuel;

		// Token: 0x04003BD7 RID: 15319
		public bool consumeFuelOnlyWhenUsed;

		// Token: 0x04003BD8 RID: 15320
		public bool consumeFuelOnlyWhenPowered;

		// Token: 0x04003BD9 RID: 15321
		public bool showFuelGizmo;

		// Token: 0x04003BDA RID: 15322
		public bool initialAllowAutoRefuel = true;

		// Token: 0x04003BDB RID: 15323
		public bool showAllowAutoRefuelToggle;

		// Token: 0x04003BDC RID: 15324
		public bool allowRefuelIfNotEmpty = true;

		// Token: 0x04003BDD RID: 15325
		public bool fuelIsMortarBarrel;

		// Token: 0x04003BDE RID: 15326
		public bool targetFuelLevelConfigurable;

		// Token: 0x04003BDF RID: 15327
		public float initialConfigurableTargetFuelLevel;

		// Token: 0x04003BE0 RID: 15328
		public bool drawOutOfFuelOverlay = true;

		// Token: 0x04003BE1 RID: 15329
		public float minimumFueledThreshold;

		// Token: 0x04003BE2 RID: 15330
		public bool drawFuelGaugeInMap;

		// Token: 0x04003BE3 RID: 15331
		public bool atomicFueling;

		// Token: 0x04003BE4 RID: 15332
		private float fuelMultiplier = 1f;

		// Token: 0x04003BE5 RID: 15333
		public bool factorByDifficulty;

		// Token: 0x04003BE6 RID: 15334
		public string fuelLabel;

		// Token: 0x04003BE7 RID: 15335
		public string fuelGizmoLabel;

		// Token: 0x04003BE8 RID: 15336
		public string outOfFuelMessage;

		// Token: 0x04003BE9 RID: 15337
		public string fuelIconPath;

		// Token: 0x04003BEA RID: 15338
		private Texture2D fuelIcon;
	}
}
