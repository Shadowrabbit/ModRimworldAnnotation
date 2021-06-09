using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B1B RID: 6939
	public class ITab_Pawn_Social : ITab
	{
		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x060098A9 RID: 39081 RVA: 0x00065BCD File Offset: 0x00063DCD
		public override bool IsVisible
		{
			get
			{
				return this.SelPawnForSocialInfo.RaceProps.IsFlesh;
			}
		}

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x060098AA RID: 39082 RVA: 0x002CD530 File Offset: 0x002CB730
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

		// Token: 0x060098AB RID: 39083 RVA: 0x00065BDF File Offset: 0x00063DDF
		public ITab_Pawn_Social()
		{
			this.size = new Vector2(540f, 510f);
			this.labelKey = "TabSocial";
			this.tutorTag = "Social";
		}

		// Token: 0x060098AC RID: 39084 RVA: 0x00065C12 File Offset: 0x00063E12
		protected override void FillTab()
		{
			SocialCardUtility.DrawSocialCard(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawnForSocialInfo);
		}

		// Token: 0x040061A0 RID: 24992
		public const float Width = 540f;
	}
}
