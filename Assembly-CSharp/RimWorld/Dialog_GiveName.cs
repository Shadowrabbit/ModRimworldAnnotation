using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E4 RID: 6628
	public abstract class Dialog_GiveName : Window
	{
		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x0600929C RID: 37532 RVA: 0x0006239F File Offset: 0x0006059F
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

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x0600929D RID: 37533 RVA: 0x000623B4 File Offset: 0x000605B4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(640f, this.Height);
			}
		}

		// Token: 0x0600929E RID: 37534 RVA: 0x002A2C3C File Offset: 0x002A0E3C
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

		// Token: 0x0600929F RID: 37535 RVA: 0x002A2CDC File Offset: 0x002A0EDC
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

		// Token: 0x060092A0 RID: 37536
		protected abstract bool IsValidName(string s);

		// Token: 0x060092A1 RID: 37537
		protected abstract void Named(string s);

		// Token: 0x060092A2 RID: 37538 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool IsValidSecondName(string s)
		{
			return true;
		}

		// Token: 0x060092A3 RID: 37539 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void NamedSecond(string s)
		{
		}

		// Token: 0x04005CC7 RID: 23751
		protected Pawn suggestingPawn;

		// Token: 0x04005CC8 RID: 23752
		protected string curName;

		// Token: 0x04005CC9 RID: 23753
		protected Func<string> nameGenerator;

		// Token: 0x04005CCA RID: 23754
		protected string nameMessageKey;

		// Token: 0x04005CCB RID: 23755
		protected string gainedNameMessageKey;

		// Token: 0x04005CCC RID: 23756
		protected string invalidNameMessageKey;

		// Token: 0x04005CCD RID: 23757
		protected bool useSecondName;

		// Token: 0x04005CCE RID: 23758
		protected string curSecondName;

		// Token: 0x04005CCF RID: 23759
		protected Func<string> secondNameGenerator;

		// Token: 0x04005CD0 RID: 23760
		protected string secondNameMessageKey;

		// Token: 0x04005CD1 RID: 23761
		protected string invalidSecondNameMessageKey;
	}
}
