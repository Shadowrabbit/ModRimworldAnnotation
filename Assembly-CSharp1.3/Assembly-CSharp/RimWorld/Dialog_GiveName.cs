using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E9 RID: 4841
	public abstract class Dialog_GiveName : Window
	{
		// Token: 0x1700145C RID: 5212
		// (get) Token: 0x0600742D RID: 29741 RVA: 0x00275CD1 File Offset: 0x00273ED1
		private float Height
		{
			get
			{
				if (!this.useSecondName)
				{
					return 200f;
				}
				return 300f;
			}
		}

		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x0600742E RID: 29742 RVA: 0x00275CE6 File Offset: 0x00273EE6
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, this.Height);
			}
		}

		// Token: 0x0600742F RID: 29743 RVA: 0x00275CF8 File Offset: 0x00273EF8
		public Dialog_GiveName()
		{
			if (Find.AnyPlayerHomeMap != null && Find.AnyPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
			{
				if (Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
				}
				else
				{
					this.suggestingPawn = Find.AnyPlayerHomeMap.mapPawns.FreeColonists.RandomElement<Pawn>();
				}
			}
			else
			{
				this.suggestingPawn = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep.RandomElement<Pawn>();
			}
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06007430 RID: 29744 RVA: 0x00275D98 File Offset: 0x00273F98
		public override void DoWindowContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			bool flag = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
			{
				flag = true;
				Event.current.Use();
			}
			Rect rect2;
			if (!this.useSecondName)
			{
				Widgets.Label(new Rect(0f, 0f, rect.width, rect.height), this.nameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn).CapitalizeFirst());
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, 80f, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, 80f, rect.width / 2f + 70f, 35f), this.curName);
				rect2 = new Rect(rect.width / 2f + 90f, rect.height - 35f, rect.width / 2f - 90f, 35f);
			}
			else
			{
				float num = 0f;
				string text = this.nameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn).CapitalizeFirst();
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.nameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, num, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true))
				{
					this.curName = this.nameGenerator();
				}
				this.curName = Widgets.TextField(new Rect(0f, num, rect.width / 2f + 70f, 35f), this.curName);
				num += 60f;
				text = this.secondNameMessageKey.Translate(this.suggestingPawn.LabelShort, this.suggestingPawn);
				Widgets.Label(new Rect(0f, num, rect.width, rect.height), text);
				num += Text.CalcHeight(text, rect.width) + 10f;
				if (this.secondNameGenerator != null && Widgets.ButtonText(new Rect(rect.width / 2f + 90f, num, rect.width / 2f - 90f, 35f), "Randomize".Translate(), true, true, true))
				{
					this.curSecondName = this.secondNameGenerator();
				}
				this.curSecondName = Widgets.TextField(new Rect(0f, num, rect.width / 2f + 70f, 35f), this.curSecondName);
				num += 45f;
				float num2 = rect.width / 2f - 90f;
				rect2 = new Rect(rect.width / 2f - num2 / 2f, rect.height - 35f, num2, 35f);
			}
			if (Widgets.ButtonText(rect2, "OK".Translate(), true, true, true) || flag)
			{
				if (this.IsValidName(this.curName) && (!this.useSecondName || this.IsValidSecondName(this.curSecondName)))
				{
					if (this.useSecondName)
					{
						this.Named(this.curName);
						this.NamedSecond(this.curSecondName);
						Messages.Message(this.gainedNameMessageKey.Translate(this.curName, this.curSecondName), MessageTypeDefOf.TaskCompletion, false);
					}
					else
					{
						this.Named(this.curName);
						Messages.Message(this.gainedNameMessageKey.Translate(this.curName), MessageTypeDefOf.TaskCompletion, false);
					}
					Find.WindowStack.TryRemove(this, true);
				}
				else
				{
					Messages.Message(this.invalidNameMessageKey.Translate(), MessageTypeDefOf.RejectInput, false);
				}
				Event.current.Use();
			}
		}

		// Token: 0x06007431 RID: 29745
		protected abstract bool IsValidName(string s);

		// Token: 0x06007432 RID: 29746
		protected abstract void Named(string s);

		// Token: 0x06007433 RID: 29747 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool IsValidSecondName(string s)
		{
			return true;
		}

		// Token: 0x06007434 RID: 29748 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void NamedSecond(string s)
		{
		}

		// Token: 0x04003FEB RID: 16363
		protected Pawn suggestingPawn;

		// Token: 0x04003FEC RID: 16364
		protected string curName;

		// Token: 0x04003FED RID: 16365
		protected Func<string> nameGenerator;

		// Token: 0x04003FEE RID: 16366
		protected string nameMessageKey;

		// Token: 0x04003FEF RID: 16367
		protected string gainedNameMessageKey;

		// Token: 0x04003FF0 RID: 16368
		protected string invalidNameMessageKey;

		// Token: 0x04003FF1 RID: 16369
		protected bool useSecondName;

		// Token: 0x04003FF2 RID: 16370
		protected string curSecondName;

		// Token: 0x04003FF3 RID: 16371
		protected Func<string> secondNameGenerator;

		// Token: 0x04003FF4 RID: 16372
		protected string secondNameMessageKey;

		// Token: 0x04003FF5 RID: 16373
		protected string invalidSecondNameMessageKey;
	}
}
