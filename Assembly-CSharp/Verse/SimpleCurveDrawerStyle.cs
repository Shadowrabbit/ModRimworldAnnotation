using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007E4 RID: 2020
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060032E3 RID: 13027 RVA: 0x00027EA0 File Offset: 0x000260A0
		// (set) Token: 0x060032E4 RID: 13028 RVA: 0x00027EA8 File Offset: 0x000260A8
		public bool DrawBackground { get; set; }

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x060032E5 RID: 13029 RVA: 0x00027EB1 File Offset: 0x000260B1
		// (set) Token: 0x060032E6 RID: 13030 RVA: 0x00027EB9 File Offset: 0x000260B9
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x00027EC2 File Offset: 0x000260C2
		// (set) Token: 0x060032E8 RID: 13032 RVA: 0x00027ECA File Offset: 0x000260CA
		public bool DrawMeasures { get; set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x00027ED3 File Offset: 0x000260D3
		// (set) Token: 0x060032EA RID: 13034 RVA: 0x00027EDB File Offset: 0x000260DB
		public bool DrawPoints { get; set; }

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x060032EB RID: 13035 RVA: 0x00027EE4 File Offset: 0x000260E4
		// (set) Token: 0x060032EC RID: 13036 RVA: 0x00027EEC File Offset: 0x000260EC
		public bool DrawLegend { get; set; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x00027EF5 File Offset: 0x000260F5
		// (set) Token: 0x060032EE RID: 13038 RVA: 0x00027EFD File Offset: 0x000260FD
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060032EF RID: 13039 RVA: 0x00027F06 File Offset: 0x00026106
		// (set) Token: 0x060032F0 RID: 13040 RVA: 0x00027F0E File Offset: 0x0002610E
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x060032F1 RID: 13041 RVA: 0x00027F17 File Offset: 0x00026117
		// (set) Token: 0x060032F2 RID: 13042 RVA: 0x00027F1F File Offset: 0x0002611F
		public bool UseFixedSection { get; set; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060032F3 RID: 13043 RVA: 0x00027F28 File Offset: 0x00026128
		// (set) Token: 0x060032F4 RID: 13044 RVA: 0x00027F30 File Offset: 0x00026130
		public bool UseFixedScale { get; set; }

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060032F5 RID: 13045 RVA: 0x00027F39 File Offset: 0x00026139
		// (set) Token: 0x060032F6 RID: 13046 RVA: 0x00027F41 File Offset: 0x00026141
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060032F7 RID: 13047 RVA: 0x00027F4A File Offset: 0x0002614A
		// (set) Token: 0x060032F8 RID: 13048 RVA: 0x00027F52 File Offset: 0x00026152
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060032F9 RID: 13049 RVA: 0x00027F5B File Offset: 0x0002615B
		// (set) Token: 0x060032FA RID: 13050 RVA: 0x00027F63 File Offset: 0x00026163
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060032FB RID: 13051 RVA: 0x00027F6C File Offset: 0x0002616C
		// (set) Token: 0x060032FC RID: 13052 RVA: 0x00027F74 File Offset: 0x00026174
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x060032FD RID: 13053 RVA: 0x00027F7D File Offset: 0x0002617D
		// (set) Token: 0x060032FE RID: 13054 RVA: 0x00027F85 File Offset: 0x00026185
		public bool XIntegersOnly { get; set; }

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x00027F8E File Offset: 0x0002618E
		// (set) Token: 0x06003300 RID: 13056 RVA: 0x00027F96 File Offset: 0x00026196
		public bool YIntegersOnly { get; set; }

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06003301 RID: 13057 RVA: 0x00027F9F File Offset: 0x0002619F
		// (set) Token: 0x06003302 RID: 13058 RVA: 0x00027FA7 File Offset: 0x000261A7
		public string LabelX { get; set; }

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x00027FB0 File Offset: 0x000261B0
		// (set) Token: 0x06003304 RID: 13060 RVA: 0x00027FB8 File Offset: 0x000261B8
		public FloatRange FixedSection { get; set; }

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06003305 RID: 13061 RVA: 0x00027FC1 File Offset: 0x000261C1
		// (set) Token: 0x06003306 RID: 13062 RVA: 0x00027FC9 File Offset: 0x000261C9
		public Vector2 FixedScale { get; set; }

		// Token: 0x06003307 RID: 13063 RVA: 0x0014EF78 File Offset: 0x0014D178
		public SimpleCurveDrawerStyle()
		{
			this.DrawBackground = false;
			this.DrawBackgroundLines = true;
			this.DrawMeasures = false;
			this.DrawPoints = true;
			this.DrawLegend = false;
			this.DrawCurveMousePoint = false;
			this.OnlyPositiveValues = false;
			this.UseFixedSection = false;
			this.UseFixedScale = false;
			this.UseAntiAliasedLines = false;
			this.PointsRemoveOptimization = false;
			this.MeasureLabelsXCount = 5;
			this.MeasureLabelsYCount = 5;
			this.XIntegersOnly = false;
			this.YIntegersOnly = false;
			this.LabelX = "x";
		}
	}
}
