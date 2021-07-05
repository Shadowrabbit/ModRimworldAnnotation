using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134D RID: 4941
	public class ITab_Pawn_Social : ITab
	{
		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x060077A2 RID: 30626 RVA: 0x002A2498 File Offset: 0x002A0698
		public override bool IsVisible
		{
			get
			{
				return this.SelPawnForSocialInfo.RaceProps.IsFlesh;
			}
		}

		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x060077A3 RID: 30627 RVA: 0x002A24AC File Offset: 0x002A06AC
		private Pawn SelPawnForSocialInfo
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				throw new InvalidOperationException("Social tab on non-pawn non-corpse " + base.SelThing);
			}
		}

		// Token: 0x060077A4 RID: 30628 RVA: 0x002A24F3 File Offset: 0x002A06F3
		public ITab_Pawn_Social()
		{
			this.size = new Vector2(540f, 510f);
			this.labelKey = "TabSocial";
			this.tutorTag = "Social";
		}

		// Token: 0x060077A5 RID: 30629 RVA: 0x002A2526 File Offset: 0x002A0726
		protected override void FillTab()
		{
			SocialCardUtility.DrawSocialCard(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawnForSocialInfo);
		}

		// Token: 0x0400428F RID: 17039
		public const float Width = 540f;
	}
}
