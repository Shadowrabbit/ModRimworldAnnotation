using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020017ED RID: 6125
	public class FocusStrengthOffset_ArtificialBuildings : FocusStrengthOffset_Curve
	{
		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x06008795 RID: 34709 RVA: 0x0005AFB4 File Offset: 0x000591B4
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_NearbyArtificialStructures";
			}
		}

		// Token: 0x06008796 RID: 34710 RVA: 0x00030B07 File Offset: 0x0002ED07
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return parent.Spawned;
		}

		// Token: 0x06008797 RID: 34711 RVA: 0x0005AFBB File Offset: 0x000591BB
		protected override float SourceValue(Thing parent)
		{
			return (float)(parent.Spawned ? parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius).Count : 0);
		}

		// Token: 0x06008798 RID: 34712 RVA: 0x0005AFEA File Offset: 0x000591EA
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			MeditationUtility.DrawArtificialBuildingOverlay(parent.Position, parent.def, parent.Map, this.radius);
		}

		// Token: 0x06008799 RID: 34713 RVA: 0x0027BEC0 File Offset: 0x0027A0C0
		public override string InspectStringExtra(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				List<Thing> forCell = parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius);
				if (forCell.Count > 0)
				{
					TaggedString taggedString = "MeditationFocusImpacted".Translate() + ": " + (from c in forCell.Take(3)
					select c.LabelShort).ToCommaList(false).CapitalizeFirst();
					if (forCell.Count > 3)
					{
						taggedString += " " + "Etc".Translate();
					}
					return taggedString;
				}
			}
			return "";
		}

		// Token: 0x040056FF RID: 22271
		public float radius = 10f;
	}
}
