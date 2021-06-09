using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000524 RID: 1316
	public class CompColorable : ThingComp
	{
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x0001D328 File Offset: 0x0001B528
		// (set) Token: 0x060021C0 RID: 8640 RVA: 0x0001D34E File Offset: 0x0001B54E
		public Color Color
		{
			get
			{
				if (!this.active)
				{
					return this.parent.def.graphicData.color;
				}
				return this.color;
			}
			set
			{
				if (value == this.color)
				{
					return;
				}
				this.active = true;
				this.color = value;
				this.parent.Notify_ColorChanged();
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x060021C1 RID: 8641 RVA: 0x0001D378 File Offset: 0x0001B578
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x00107934 File Offset: 0x00105B34
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.Color = this.parent.def.colorGenerator.NewRandomizedColor();
			}
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x0010799C File Offset: 0x00105B9C
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && !this.active)
			{
				return;
			}
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x0001D380 File Offset: 0x0001B580
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}

		// Token: 0x040016F8 RID: 5880
		private Color color = Color.white;

		// Token: 0x040016F9 RID: 5881
		private bool active;
	}
}
