using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001342 RID: 4930
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x0600775A RID: 30554 RVA: 0x0029E8AC File Offset: 0x0029CAAC
		private Pawn PawnToShowInfoAbout
		{
			get
			{
				Pawn pawn = null;
				if (base.SelPawn != null)
				{
					pawn = base.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse != null)
					{
						pawn = corpse.InnerPawn;
					}
				}
				if (pawn == null)
				{
					Log.Error("Character tab found no selected pawn to display.");
					return null;
				}
				return pawn;
			}
		}

		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x0600775B RID: 30555 RVA: 0x0029E8F2 File Offset: 0x0029CAF2
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x0600775C RID: 30556 RVA: 0x0029E902 File Offset: 0x0029CB02
		public ITab_Pawn_Character()
		{
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x0600775D RID: 30557 RVA: 0x0029E920 File Offset: 0x0029CB20
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout) + new Vector2(17f, 17f) * 2f;
		}

		// Token: 0x0600775E RID: 30558 RVA: 0x0029E958 File Offset: 0x0029CB58
		protected override void FillTab()
		{
			this.UpdateSize();
			Vector2 vector = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout);
			CharacterCardUtility.DrawCharacterCard(new Rect(17f, 17f, vector.x, vector.y), this.PawnToShowInfoAbout, null, default(Rect));
		}
	}
}
