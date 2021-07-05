using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D10 RID: 3344
	public class Zone_Growing : Zone, IPlantToGrowSettable
	{
		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06004E39 RID: 20025 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06004E3A RID: 20026 RVA: 0x001A4139 File Offset: 0x001A2339
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextGrowingZoneColor();
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06004E3B RID: 20027 RVA: 0x001A4140 File Offset: 0x001A2340
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return base.Cells;
			}
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x001A4148 File Offset: 0x001A2348
		public Zone_Growing()
		{
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x001A4169 File Offset: 0x001A2369
		public Zone_Growing(ZoneManager zoneManager) : base("GrowingZone".Translate(), zoneManager)
		{
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x001A419A File Offset: 0x001A239A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", true, false);
			Scribe_Values.Look<bool>(ref this.allowCut, "allowCut", true, false);
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x001A41D8 File Offset: 0x001A23D8
		public override string GetInspectString()
		{
			string text = "";
			if (!base.Cells.NullOrEmpty<IntVec3>())
			{
				IntVec3 c = base.Cells.First<IntVec3>();
				if (c.UsesOutdoorTemperature(base.Map))
				{
					text += "OutdoorGrowingPeriod".Translate() + ": " + Zone_Growing.GrowingQuadrumsDescription(base.Map.Tile) + "\n";
				}
				if (PlantUtility.GrowthSeasonNow(c, base.Map, true))
				{
					text += "GrowSeasonHereNow".Translate();
				}
				else
				{
					text += "CannotGrowBadSeasonTemperature".Translate();
				}
			}
			return text;
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x001A4290 File Offset: 0x001A2490
		public static string GrowingQuadrumsDescription(int tile)
		{
			List<Twelfth> list = GenTemperature.TwelfthsInAverageTemperatureRange(tile, 6f, 42f);
			if (list.NullOrEmpty<Twelfth>())
			{
				return "NoGrowingPeriod".Translate();
			}
			if (list.Count == 12)
			{
				return "GrowYearRound".Translate();
			}
			return "PeriodDays".Translate(list.Count * 5 + "/" + 60) + " (" + QuadrumUtility.QuadrumsRangeLabel(list) + ")";
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x001A4331 File Offset: 0x001A2531
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield return new Command_Toggle
			{
				defaultLabel = "CommandAllowSow".Translate(),
				defaultDesc = "CommandAllowSowDesc".Translate(),
				hotKey = KeyBindingDefOf.Command_ItemForbid,
				icon = TexCommand.ForbidOff,
				isActive = (() => this.allowSow),
				toggleAction = delegate()
				{
					this.allowSow = !this.allowSow;
				}
			};
			yield return new Command_Toggle
			{
				defaultLabel = "CommandAllowCut".Translate(),
				defaultDesc = "CommandAllowCutDesc".Translate(),
				icon = Designator_PlantsCut.IconTex,
				isActive = (() => this.allowCut),
				toggleAction = delegate()
				{
					this.allowCut = !this.allowCut;
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x001A4341 File Offset: 0x001A2541
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing_Expand>();
			yield break;
		}

		// Token: 0x06004E43 RID: 20035 RVA: 0x001A434A File Offset: 0x001A254A
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x001A4352 File Offset: 0x001A2552
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool CanAcceptSowNow()
		{
			return true;
		}

		// Token: 0x04002F3E RID: 12094
		private ThingDef plantDefToGrow = ThingDefOf.Plant_Potato;

		// Token: 0x04002F3F RID: 12095
		public bool allowSow = true;

		// Token: 0x04002F40 RID: 12096
		public bool allowCut = true;
	}
}
