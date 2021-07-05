using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200133D RID: 4925
	public class ITab_Art : ITab
	{
		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x06007737 RID: 30519 RVA: 0x0029D8C4 File Offset: 0x0029BAC4
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

		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x06007738 RID: 30520 RVA: 0x0029D8F8 File Offset: 0x0029BAF8
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompArt != null && this.SelectedCompArt.Active;
			}
		}

		// Token: 0x06007739 RID: 30521 RVA: 0x0029D90F File Offset: 0x0029BB0F
		public ITab_Art()
		{
			this.size = ITab_Art.WinSize;
			this.labelKey = "TabArt";
			this.tutorTag = "Art";
		}

		// Token: 0x0600773A RID: 30522 RVA: 0x0029D938 File Offset: 0x0029BB38
		protected override void FillTab()
		{
			Rect rect2;
			Rect rect = rect2 = new Rect(0f, 0f, ITab_Art.WinSize.x, ITab_Art.WinSize.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, this.SelectedCompArt.Title.Truncate(rect2.width, null));
			if (ITab_Art.cachedImageSource != this.SelectedCompArt || ITab_Art.cachedTaleRef != this.SelectedCompArt.TaleRef)
			{
				ITab_Art.cachedImageDescription = this.SelectedCompArt.GenerateImageDescription();
				ITab_Art.cachedImageSource = this.SelectedCompArt;
				ITab_Art.cachedTaleRef = this.SelectedCompArt.TaleRef;
			}
			Rect rect3 = rect;
			rect3.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, ITab_Art.cachedImageDescription);
		}

		// Token: 0x0400423C RID: 16956
		private static string cachedImageDescription;

		// Token: 0x0400423D RID: 16957
		private static CompArt cachedImageSource;

		// Token: 0x0400423E RID: 16958
		private static TaleReference cachedTaleRef;

		// Token: 0x0400423F RID: 16959
		private static readonly Vector2 WinSize = new Vector2(400f, 300f);
	}
}
