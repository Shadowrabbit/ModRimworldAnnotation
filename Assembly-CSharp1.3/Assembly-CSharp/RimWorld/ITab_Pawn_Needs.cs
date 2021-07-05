using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x0200134C RID: 4940
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x0600779D RID: 30621 RVA: 0x002A23CC File Offset: 0x002A05CC
		public override bool IsVisible
		{
			get
			{
				return (!base.SelPawn.RaceProps.Animal || base.SelPawn.Faction != null) && base.SelPawn.needs != null && base.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x0600779E RID: 30622 RVA: 0x002A2421 File Offset: 0x002A0621
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x0600779F RID: 30623 RVA: 0x002A243F File Offset: 0x002A063F
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x060077A0 RID: 30624 RVA: 0x002A244D File Offset: 0x002A064D
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), base.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x060077A1 RID: 30625 RVA: 0x002A2485 File Offset: 0x002A0685
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(base.SelPawn);
		}

		// Token: 0x0400428E RID: 17038
		private Vector2 thoughtScrollPosition;
	}
}
