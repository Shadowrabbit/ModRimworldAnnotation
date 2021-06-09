using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B03 RID: 6915
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x06009837 RID: 38967 RVA: 0x002CAEC8 File Offset: 0x002C90C8
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
					Log.Error("Character tab found no selected pawn to display.", false);
					return null;
				}
				return pawn;
			}
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x06009838 RID: 38968 RVA: 0x000656A8 File Offset: 0x000638A8
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x06009839 RID: 38969 RVA: 0x000656B8 File Offset: 0x000638B8
		public ITab_Pawn_Character()
		{
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x0600983A RID: 38970 RVA: 0x000656D6 File Offset: 0x000638D6
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout) + new Vector2(17f, 17f) * 2f;
		}

		// Token: 0x0600983B RID: 38971 RVA: 0x002CAF10 File Offset: 0x002C9110
		protected override void FillTab()
		{
			this.UpdateSize();
			Vector2 vector = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout);
			CharacterCardUtility.DrawCharacterCard(new Rect(17f, 17f, vector.x, vector.y), this.PawnToShowInfoAbout, null, default(Rect));
		}
	}
}
