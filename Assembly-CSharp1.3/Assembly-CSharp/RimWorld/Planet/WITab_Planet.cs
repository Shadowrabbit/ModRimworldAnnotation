using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001813 RID: 6163
	public class WITab_Planet : WITab
	{
		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x0600905E RID: 36958 RVA: 0x0033C4FC File Offset: 0x0033A6FC
		public override bool IsVisible
		{
			get
			{
				return base.SelTileID >= 0;
			}
		}

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x0600905F RID: 36959 RVA: 0x0033C50C File Offset: 0x0033A70C
		private string Desc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PlanetSeed".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.seedString);
				stringBuilder.Append("PlanetCoverageShort".Translate());
				stringBuilder.Append(": ");
				stringBuilder.AppendLine(Find.World.info.planetCoverage.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06009060 RID: 36960 RVA: 0x0033C598 File Offset: 0x0033A798
		public WITab_Planet()
		{
			this.size = WITab_Planet.WinSize;
			this.labelKey = "TabPlanet";
		}

		// Token: 0x06009061 RID: 36961 RVA: 0x0033C5B8 File Offset: 0x0033A7B8
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, WITab_Planet.WinSize.x, WITab_Planet.WinSize.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, Find.World.info.name);
			Rect rect2 = rect;
			rect2.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, this.Desc);
		}

		// Token: 0x04005AD2 RID: 23250
		private static readonly Vector2 WinSize = new Vector2(400f, 150f);
	}
}
