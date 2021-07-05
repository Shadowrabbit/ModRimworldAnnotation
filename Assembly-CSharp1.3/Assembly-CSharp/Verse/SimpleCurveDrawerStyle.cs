using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000483 RID: 1155
	public class SimpleCurveDrawerStyle
	{
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x000DD14D File Offset: 0x000DB34D
		// (set) Token: 0x06002319 RID: 8985 RVA: 0x000DD155 File Offset: 0x000DB355
		public bool DrawBackground { get; set; }

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x000DD15E File Offset: 0x000DB35E
		// (set) Token: 0x0600231B RID: 8987 RVA: 0x000DD166 File Offset: 0x000DB366
		public bool DrawBackgroundLines { get; set; }

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x000DD16F File Offset: 0x000DB36F
		// (set) Token: 0x0600231D RID: 8989 RVA: 0x000DD177 File Offset: 0x000DB377
		public bool DrawMeasures { get; set; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600231E RID: 8990 RVA: 0x000DD180 File Offset: 0x000DB380
		// (set) Token: 0x0600231F RID: 8991 RVA: 0x000DD188 File Offset: 0x000DB388
		public bool DrawPoints { get; set; }

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x000DD191 File Offset: 0x000DB391
		// (set) Token: 0x06002321 RID: 8993 RVA: 0x000DD199 File Offset: 0x000DB399
		public bool DrawLegend { get; set; }

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x000DD1A2 File Offset: 0x000DB3A2
		// (set) Token: 0x06002323 RID: 8995 RVA: 0x000DD1AA File Offset: 0x000DB3AA
		public bool DrawCurveMousePoint { get; set; }

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x000DD1B3 File Offset: 0x000DB3B3
		// (set) Token: 0x06002325 RID: 8997 RVA: 0x000DD1BB File Offset: 0x000DB3BB
		public bool OnlyPositiveValues { get; set; }

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002326 RID: 8998 RVA: 0x000DD1C4 File Offset: 0x000DB3C4
		// (set) Token: 0x06002327 RID: 8999 RVA: 0x000DD1CC File Offset: 0x000DB3CC
		public bool UseFixedSection { get; set; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002328 RID: 9000 RVA: 0x000DD1D5 File Offset: 0x000DB3D5
		// (set) Token: 0x06002329 RID: 9001 RVA: 0x000DD1DD File Offset: 0x000DB3DD
		public bool UseFixedScale { get; set; }

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600232A RID: 9002 RVA: 0x000DD1E6 File Offset: 0x000DB3E6
		// (set) Token: 0x0600232B RID: 9003 RVA: 0x000DD1EE File Offset: 0x000DB3EE
		public bool UseAntiAliasedLines { get; set; }

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600232C RID: 9004 RVA: 0x000DD1F7 File Offset: 0x000DB3F7
		// (set) Token: 0x0600232D RID: 9005 RVA: 0x000DD1FF File Offset: 0x000DB3FF
		public bool PointsRemoveOptimization { get; set; }

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600232E RID: 9006 RVA: 0x000DD208 File Offset: 0x000DB408
		// (set) Token: 0x0600232F RID: 9007 RVA: 0x000DD210 File Offset: 0x000DB410
		public int MeasureLabelsXCount { get; set; }

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002330 RID: 9008 RVA: 0x000DD219 File Offset: 0x000DB419
		// (set) Token: 0x06002331 RID: 9009 RVA: 0x000DD221 File Offset: 0x000DB421
		public int MeasureLabelsYCount { get; set; }

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002332 RID: 9010 RVA: 0x000DD22A File Offset: 0x000DB42A
		// (set) Token: 0x06002333 RID: 9011 RVA: 0x000DD232 File Offset: 0x000DB432
		public bool XIntegersOnly { get; set; }

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002334 RID: 9012 RVA: 0x000DD23B File Offset: 0x000DB43B
		// (set) Token: 0x06002335 RID: 9013 RVA: 0x000DD243 File Offset: 0x000DB443
		public bool YIntegersOnly { get; set; }

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002336 RID: 9014 RVA: 0x000DD24C File Offset: 0x000DB44C
		// (set) Token: 0x06002337 RID: 9015 RVA: 0x000DD254 File Offset: 0x000DB454
		public string LabelX { get; set; }

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06002338 RID: 9016 RVA: 0x000DD25D File Offset: 0x000DB45D
		// (set) Token: 0x06002339 RID: 9017 RVA: 0x000DD265 File Offset: 0x000DB465
		public FloatRange FixedSection { get; set; }

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x000DD26E File Offset: 0x000DB46E
		// (set) Token: 0x0600233B RID: 9019 RVA: 0x000DD276 File Offset: 0x000DB476
		public Vector2 FixedScale { get; set; }

		// Token: 0x0600233C RID: 9020 RVA: 0x000DD280 File Offset: 0x000DB480
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
