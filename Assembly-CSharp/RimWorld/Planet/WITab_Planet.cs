using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021E1 RID: 8673
	public class WITab_Planet : WITab
	{
		// Token: 0x17001B92 RID: 7058
		// (get) Token: 0x0600B9AA RID: 47530 RVA: 0x00078390 File Offset: 0x00076590
		public override bool IsVisible
		{
			get
			{
				return base.SelTileID >= 0;
			}
		}

		// Token: 0x17001B93 RID: 7059
		// (get) Token: 0x0600B9AB RID: 47531 RVA: 0x00356C54 File Offset: 0x00354E54
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

		// Token: 0x0600B9AC RID: 47532 RVA: 0x0007839E File Offset: 0x0007659E
		public WITab_Planet()
		{
			this.size = WITab_Planet.WinSize;
			this.labelKey = "TabPlanet";
		}

		// Token: 0x0600B9AD RID: 47533 RVA: 0x00356CE0 File Offset: 0x00354EE0
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

		// Token: 0x04007EE2 RID: 32482
		private static readonly Vector2 WinSize = new Vector2(400f, 150f);
	}
}
