using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001B1A RID: 6938
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x060098A4 RID: 39076 RVA: 0x002CE10C File Offset: 0x002CC30C
		public override bool IsVisible
		{
			get
			{
				return (!base.SelPawn.RaceProps.Animal || base.SelPawn.Faction != null) && base.SelPawn.needs != null && base.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x060098A5 RID: 39077 RVA: 0x00065B56 File Offset: 0x00063D56
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x060098A6 RID: 39078 RVA: 0x00065B74 File Offset: 0x00063D74
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x060098A7 RID: 39079 RVA: 0x00065B82 File Offset: 0x00063D82
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), base.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x060098A8 RID: 39080 RVA: 0x00065BBA File Offset: 0x00063DBA
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(base.SelPawn);
		}

		// Token: 0x0400619F RID: 24991
		private Vector2 thoughtScrollPosition;
	}
}
