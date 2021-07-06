using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001816 RID: 6166
	public class CompProperties_Refuelable : CompProperties
	{
		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x06008872 RID: 34930 RVA: 0x0005B9B8 File Offset: 0x00059BB8
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

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x06008873 RID: 34931 RVA: 0x0005B9D8 File Offset: 0x00059BD8
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

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x06008874 RID: 34932 RVA: 0x0027F044 File Offset: 0x0027D244
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

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x06008875 RID: 34933 RVA: 0x0005B9F8 File Offset: 0x00059BF8
		public float FuelMultiplierCurrentDifficulty
		{
			get
			{
				if (this.factorByDifficulty)
				{
					return this.fuelMultiplier / Find.Storyteller.difficultyValues.maintenanceCostFactor;
				}
				return this.fuelMultiplier;
			}
		}

		// Token: 0x06008876 RID: 34934 RVA: 0x0027F0B4 File Offset: 0x0027D2B4
		public CompProperties_Refuelable()
		{
			this.compClass = typeof(CompRefuelable);
		}

		// Token: 0x06008877 RID: 34935 RVA: 0x0005BA1F File Offset: 0x00059C1F
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			this.fuelFilter.ResolveReferences();
		}

		// Token: 0x06008878 RID: 34936 RVA: 0x0005BA33 File Offset: 0x00059C33
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

		// Token: 0x06008879 RID: 34937 RVA: 0x0005BA4A File Offset: 0x00059C4A
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

		// Token: 0x0400578E RID: 22414
		public float fuelConsumptionRate = 1f;

		// Token: 0x0400578F RID: 22415
		public float fuelCapacity = 2f;

		// Token: 0x04005790 RID: 22416
		public float initialFuelPercent;

		// Token: 0x04005791 RID: 22417
		public float autoRefuelPercent = 0.3f;

		// Token: 0x04005792 RID: 22418
		public float fuelConsumptionPerTickInRain;

		// Token: 0x04005793 RID: 22419
		public ThingFilter fuelFilter;

		// Token: 0x04005794 RID: 22420
		public bool destroyOnNoFuel;

		// Token: 0x04005795 RID: 22421
		public bool consumeFuelOnlyWhenUsed;

		// Token: 0x04005796 RID: 22422
		public bool showFuelGizmo;

		// Token: 0x04005797 RID: 22423
		public bool initialAllowAutoRefuel = true;

		// Token: 0x04005798 RID: 22424
		public bool showAllowAutoRefuelToggle;

		// Token: 0x04005799 RID: 22425
		public bool targetFuelLevelConfigurable;

		// Token: 0x0400579A RID: 22426
		public float initialConfigurableTargetFuelLevel;

		// Token: 0x0400579B RID: 22427
		public bool drawOutOfFuelOverlay = true;

		// Token: 0x0400579C RID: 22428
		public float minimumFueledThreshold;

		// Token: 0x0400579D RID: 22429
		public bool drawFuelGaugeInMap;

		// Token: 0x0400579E RID: 22430
		public bool atomicFueling;

		// Token: 0x0400579F RID: 22431
		private float fuelMultiplier = 1f;

		// Token: 0x040057A0 RID: 22432
		public bool factorByDifficulty;

		// Token: 0x040057A1 RID: 22433
		public string fuelLabel;

		// Token: 0x040057A2 RID: 22434
		public string fuelGizmoLabel;

		// Token: 0x040057A3 RID: 22435
		public string outOfFuelMessage;

		// Token: 0x040057A4 RID: 22436
		public string fuelIconPath;

		// Token: 0x040057A5 RID: 22437
		private Texture2D fuelIcon;
	}
}
