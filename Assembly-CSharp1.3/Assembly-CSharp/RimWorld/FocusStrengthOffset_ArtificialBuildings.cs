using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001159 RID: 4441
	public class FocusStrengthOffset_ArtificialBuildings : FocusStrengthOffset_Curve
	{
		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06006AB8 RID: 27320 RVA: 0x0023D7AB File Offset: 0x0023B9AB
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_NearbyArtificialStructures";
			}
		}

		// Token: 0x06006AB9 RID: 27321 RVA: 0x0023D7B2 File Offset: 0x0023B9B2
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return parent.Spawned;
		}

		// Token: 0x06006ABA RID: 27322 RVA: 0x0023D7BA File Offset: 0x0023B9BA
		protected override float SourceValue(Thing parent)
		{
			return (float)(parent.Spawned ? parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius).Count : 0);
		}

		// Token: 0x06006ABB RID: 27323 RVA: 0x0023D7E9 File Offset: 0x0023B9E9
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			MeditationUtility.DrawArtificialBuildingOverlay(parent.Position, parent.def, parent.Map, this.radius);
		}

		// Token: 0x06006ABC RID: 27324 RVA: 0x0023D810 File Offset: 0x0023BA10
		public override string InspectStringExtra(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				List<Thing> forCell = parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius);
				if (forCell.Count > 0)
				{
					IEnumerable<string> source = (from c in forCell
					select GenLabel.ThingLabel(c, 1, false)).Distinct<string>();
					TaggedString taggedString = "MeditationFocusImpacted".Translate() + ": " + source.Take(3).ToCommaList(false, false).CapitalizeFirst();
					if (source.Count<string>() > 3)
					{
						taggedString += " " + "Etc".Translate();
					}
					return taggedString;
				}
			}
			return "";
		}

		// Token: 0x04003B59 RID: 15193
		public float radius = 10f;
	}
}
