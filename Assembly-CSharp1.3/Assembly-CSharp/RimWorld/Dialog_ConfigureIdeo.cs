using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E1 RID: 4833
	public class Dialog_ConfigureIdeo : Window
	{
		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x060073B4 RID: 29620 RVA: 0x0026F6CE File Offset: 0x0026D8CE
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1100f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x060073B5 RID: 29621 RVA: 0x0026F6EC File Offset: 0x0026D8EC
		public Dialog_ConfigureIdeo(Ideo ideo, IEnumerable<Pawn> pawns, Action nextAction)
		{
			if (!ModLister.CheckIdeology("Configure ideo dialog"))
			{
				return;
			}
			this.forcePause = true;
			this.doCloseX = false;
			this.doCloseButton = false;
			this.closeOnClickedOutside = false;
			this.absorbInputAroundWindow = true;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
			this.openMenuOnCancel = true;
			this.preventSave = true;
			this.nextAction = nextAction;
			this.ideo = ideo;
			this.startingIdeo = ideo;
			this.pawns.AddRange(pawns);
		}

		// Token: 0x060073B6 RID: 29622 RVA: 0x0026F782 File Offset: 0x0026D982
		public override void PostClose()
		{
			base.PostClose();
			IdeoUIUtility.UnselectCurrent();
		}

		// Token: 0x060073B7 RID: 29623 RVA: 0x0026F790 File Offset: 0x0026D990
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 45f), "ConfigureIdeoligion".Translate());
			Text.Font = GameFont.Small;
			IdeoUIUtility.DoIdeoListAndDetails(new Rect(inRect.x, inRect.y + 45f, inRect.width, inRect.height - 55f - 45f), ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, this.lastNewIdeo != null, false, this.pawns, this.lastNewIdeo);
			if (Widgets.ButtonText(new Rect(inRect.xMax - Window.CloseButSize.x - 18f, inRect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y), "Next".Translate(), true, true, true))
			{
				this.Close(true);
				this.nextAction();
			}
			if (Widgets.ButtonText(new Rect(inRect.center.x - (Window.CloseButSize.x + 18f) / 2f, inRect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y), "AssignColonists".Translate(), true, true, true))
			{
				IEnumerable<Pawn> enumerable = from p in this.pawns
				where p.Ideo != Faction.OfPlayer.ideos.PrimaryIdeo && p.RaceProps.Humanlike
				select p;
				Find.WindowStack.Add(new Dialog_ChooseColonistsForIdeo(this.ideo, enumerable, new Action<List<Pawn>>(this.ChosenForIdeo)));
			}
			if (Widgets.ButtonText(new Rect(inRect.center.x + (Window.CloseButSize.x + 18f) / 2f, inRect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y), "CreateNew".Translate(), true, true, true))
			{
				Ideo newIdeo = IdeoUtility.MakeEmptyIdeo();
				Find.WindowStack.Add(new Dialog_ChooseMemes(newIdeo, MemeCategory.Structure, false, delegate()
				{
					Find.IdeoManager.Add(newIdeo);
					this.MakeIdeoPrimary(newIdeo);
					if (this.lastNewIdeo != null)
					{
						Find.IdeoManager.Remove(this.lastNewIdeo);
					}
					this.lastNewIdeo = newIdeo;
				}));
			}
			Rect rect = new Rect(inRect.x + 18f, inRect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y);
			if ((this.lastNewIdeo != null || this.previousIdeo.Count > 0) && Widgets.ButtonText(rect, "ClearChanges".Translate(), true, true, true))
			{
				if (this.lastNewIdeo != null)
				{
					this.MakeIdeoPrimary(this.startingIdeo);
					Find.IdeoManager.Remove(this.lastNewIdeo);
					this.lastNewIdeo = null;
				}
				if (this.previousIdeo.Count > 0)
				{
					foreach (KeyValuePair<Pawn, Ideo> keyValuePair in this.previousIdeo)
					{
						keyValuePair.Key.ideo.SetIdeo(keyValuePair.Value);
					}
					this.previousIdeo.Clear();
				}
			}
		}

		// Token: 0x060073B8 RID: 29624 RVA: 0x0026FAFC File Offset: 0x0026DCFC
		private void ChosenForIdeo(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				if (!this.previousIdeo.ContainsKey(pawns[i]))
				{
					this.previousIdeo[pawns[i]] = pawns[i].Ideo;
				}
				pawns[i].ideo.SetIdeo(this.ideo);
			}
		}

		// Token: 0x060073B9 RID: 29625 RVA: 0x0026FB64 File Offset: 0x0026DD64
		private void MakeIdeoPrimary(Ideo primaryIdeo)
		{
			if (!Faction.OfPlayer.ideos.IsPrimary(primaryIdeo))
			{
				Faction.OfPlayer.ideos.SetPrimary(primaryIdeo);
			}
			IdeoUIUtility.SetSelected(primaryIdeo);
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i].Ideo != null && this.pawns[i].Ideo != primaryIdeo)
				{
					this.pawns[i].ideo.SetIdeo(primaryIdeo);
				}
			}
			this.ideo = primaryIdeo;
		}

		// Token: 0x04003F6B RID: 16235
		public const float TitleAreaHeight = 45f;

		// Token: 0x04003F6C RID: 16236
		private Ideo ideo;

		// Token: 0x04003F6D RID: 16237
		private Action nextAction;

		// Token: 0x04003F6E RID: 16238
		private Vector2 scrollPosition_ideoList;

		// Token: 0x04003F6F RID: 16239
		private float scrollViewHeight_ideoList;

		// Token: 0x04003F70 RID: 16240
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x04003F71 RID: 16241
		private float scrollViewHeight_ideoDetails;

		// Token: 0x04003F72 RID: 16242
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003F73 RID: 16243
		private Ideo startingIdeo;

		// Token: 0x04003F74 RID: 16244
		private Ideo lastNewIdeo;

		// Token: 0x04003F75 RID: 16245
		private Dictionary<Pawn, Ideo> previousIdeo = new Dictionary<Pawn, Ideo>();
	}
}
