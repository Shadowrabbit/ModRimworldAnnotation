using System;

namespace Verse
{
	// Token: 0x02000384 RID: 900
	public class CompColorable_Animated : CompColorable
	{
		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001A5C RID: 6748 RVA: 0x000996D4 File Offset: 0x000978D4
		public CompProperties_ColorableAnimated Props
		{
			get
			{
				return (CompProperties_ColorableAnimated)this.props;
			}
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x000996E4 File Offset: 0x000978E4
		public override void Initialize(CompProperties props)
		{
			this.props = props;
			if (this.Props.startWithRandom)
			{
				this.colorOffset = Rand.RangeInclusive(0, this.Props.colors.Count - 1);
			}
			base.SetColor(this.Props.colors[this.colorOffset % this.Props.colors.Count]);
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x00099750 File Offset: 0x00097950
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(this.Props.changeInterval))
			{
				base.SetColor(this.Props.colors[this.colorOffset % this.Props.colors.Count]);
				this.colorOffset++;
			}
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x000997B6 File Offset: 0x000979B6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.colorOffset, "colorOffset", 0, false);
		}

		// Token: 0x04001131 RID: 4401
		public int colorOffset;
	}
}
