using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AFA RID: 6906
	public class ITab_Art : ITab
	{
		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x0600980E RID: 38926 RVA: 0x002CA0E0 File Offset: 0x002C82E0
		private CompArt SelectedCompArt
		{
			get
			{
				Thing thing = Find.Selector.SingleSelectedThing;
				MinifiedThing minifiedThing = thing as MinifiedThing;
				if (minifiedThing != null)
				{
					thing = minifiedThing.InnerThing;
				}
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompArt>();
			}
		}

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x0600980F RID: 38927 RVA: 0x00065454 File Offset: 0x00063654
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompArt != null && this.SelectedCompArt.Active;
			}
		}

		// Token: 0x06009810 RID: 38928 RVA: 0x0006546B File Offset: 0x0006366B
		public ITab_Art()
		{
			this.size = ITab_Art.WinSize;
			this.labelKey = "TabArt";
			this.tutorTag = "Art";
		}

		// Token: 0x06009811 RID: 38929 RVA: 0x002CA114 File Offset: 0x002C8314
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, ITab_Art.WinSize.x, ITab_Art.WinSize.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.SelectedCompArt.Title);
			if (ITab_Art.cachedImageSource != this.SelectedCompArt || ITab_Art.cachedTaleRef != this.SelectedCompArt.TaleRef)
			{
				ITab_Art.cachedImageDescription = this.SelectedCompArt.GenerateImageDescription();
				ITab_Art.cachedImageSource = this.SelectedCompArt;
				ITab_Art.cachedTaleRef = this.SelectedCompArt.TaleRef;
			}
			Rect rect2 = rect;
			rect2.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, ITab_Art.cachedImageDescription);
		}

		// Token: 0x04006120 RID: 24864
		private static string cachedImageDescription;

		// Token: 0x04006121 RID: 24865
		private static CompArt cachedImageSource;

		// Token: 0x04006122 RID: 24866
		private static TaleReference cachedTaleRef;

		// Token: 0x04006123 RID: 24867
		private static readonly Vector2 WinSize = new Vector2(400f, 300f);
	}
}
