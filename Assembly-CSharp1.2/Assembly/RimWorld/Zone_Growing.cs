using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134C RID: 4940
	public class Zone_Growing : Zone, IPlantToGrowSettable
	{
		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x06006B32 RID: 27442 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x06006B33 RID: 27443 RVA: 0x00048F7B File Offset: 0x0004717B
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextGrowingZoneColor();
			}
		}

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x06006B34 RID: 27444 RVA: 0x00048F82 File Offset: 0x00047182
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return base.Cells;
			}
		}

		// Token: 0x06006B35 RID: 27445 RVA: 0x00048F8A File Offset: 0x0004718A
		public Zone_Growing()
		{
		}

		// Token: 0x06006B36 RID: 27446 RVA: 0x00048FA4 File Offset: 0x000471A4
		public Zone_Growing(ZoneManager zoneManager) : base("GrowingZone".Translate(), zoneManager)
		{
		}

		// Token: 0x06006B37 RID: 27447 RVA: 0x00048FCE File Offset: 0x000471CE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", true, false);
		}

		// Token: 0x06006B38 RID: 27448 RVA: 0x0021173C File Offset: 0x0020F93C
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

		// Token: 0x06006B39 RID: 27449 RVA: 0x002117F4 File Offset: 0x0020F9F4
		public static string GrowingQuadrumsDescription(int tile)
		{
			List<Twelfth> list = GenTemperature.TwelfthsInAverageTemperatureRange(tile, 10f, 42f);
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

		// Token: 0x06006B3A RID: 27450 RVA: 0x00048FF8 File Offset: 0x000471F8
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
			yield break;
			yield break;
		}

		// Token: 0x06006B3B RID: 27451 RVA: 0x00049008 File Offset: 0x00047208
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing_Expand>();
			yield break;
		}

		// Token: 0x06006B3C RID: 27452 RVA: 0x00049011 File Offset: 0x00047211
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x06006B3D RID: 27453 RVA: 0x00049019 File Offset: 0x00047219
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x06006B3E RID: 27454 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public bool CanAcceptSowNow()
		{
			return true;
		}

		// Token: 0x04004763 RID: 18275
		private ThingDef plantDefToGrow = ThingDefOf.Plant_Potato;

		// Token: 0x04004764 RID: 18276
		public bool allowSow = true;
	}
}
