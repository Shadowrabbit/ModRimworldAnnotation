using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000381 RID: 897
	public class CompColorable : ThingComp
	{
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x0009945C File Offset: 0x0009765C
		// (set) Token: 0x06001A4F RID: 6735 RVA: 0x00099464 File Offset: 0x00097664
		public Color? DesiredColor
		{
			get
			{
				return this.desiredColor;
			}
			set
			{
				this.desiredColor = value;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x0009946D File Offset: 0x0009766D
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
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x00099493 File Offset: 0x00097693
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0009949C File Offset: 0x0009769C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.SetColor(this.parent.def.colorGenerator.NewRandomizedColor());
			}
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x00099504 File Offset: 0x00097704
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && !this.active)
			{
				return;
			}
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
			Scribe_Values.Look<Color?>(ref this.desiredColor, "desiredColor", null, false);
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x0009956E File Offset: 0x0009776E
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x0009958C File Offset: 0x0009778C
		public void Recolor()
		{
			if (this.desiredColor == null)
			{
				Log.Error("Tried recoloring apparel which does not have a desired color set!");
				return;
			}
			this.SetColor(this.DesiredColor.Value);
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000995C5 File Offset: 0x000977C5
		public void Disable()
		{
			this.active = false;
			this.color = Color.white;
			this.desiredColor = null;
			this.parent.Notify_ColorChanged();
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000995F0 File Offset: 0x000977F0
		public void SetColor(Color value)
		{
			if (value == this.color)
			{
				return;
			}
			this.active = true;
			this.color = value;
			this.desiredColor = null;
			this.parent.Notify_ColorChanged();
		}

		// Token: 0x0400112B RID: 4395
		private Color? desiredColor;

		// Token: 0x0400112C RID: 4396
		private Color color = Color.white;

		// Token: 0x0400112D RID: 4397
		private bool active;
	}
}
