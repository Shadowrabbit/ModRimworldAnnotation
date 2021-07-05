using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A9B RID: 2715
	public class PawnColumnDef : Def
	{
		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004091 RID: 16529 RVA: 0x0015D397 File Offset: 0x0015B597
		public PawnColumnWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnColumnWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06004092 RID: 16530 RVA: 0x0015D3C9 File Offset: 0x0015B5C9
		public Texture2D HeaderIcon
		{
			get
			{
				if (this.headerIconTex == null && !this.headerIcon.NullOrEmpty())
				{
					this.headerIconTex = ContentFinder<Texture2D>.Get(this.headerIcon, true);
				}
				return this.headerIconTex;
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06004093 RID: 16531 RVA: 0x0015D400 File Offset: 0x0015B600
		public Vector2 HeaderIconSize
		{
			get
			{
				if (this.headerIconSize != default(Vector2))
				{
					return this.headerIconSize;
				}
				if (this.HeaderIcon != null)
				{
					return PawnColumnDef.IconSize;
				}
				return Vector2.zero;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06004094 RID: 16532 RVA: 0x0015D443 File Offset: 0x0015B643
		public bool HeaderInteractable
		{
			get
			{
				return this.sortable || !this.headerTip.NullOrEmpty() || this.headerAlwaysInteractable;
			}
		}

		// Token: 0x04002574 RID: 9588
		public Type workerClass = typeof(PawnColumnWorker);

		// Token: 0x04002575 RID: 9589
		public bool sortable;

		// Token: 0x04002576 RID: 9590
		public bool ignoreWhenCalculatingOptimalTableSize;

		// Token: 0x04002577 RID: 9591
		[NoTranslate]
		public string headerIcon;

		// Token: 0x04002578 RID: 9592
		public Vector2 headerIconSize;

		// Token: 0x04002579 RID: 9593
		[MustTranslate]
		public string headerTip;

		// Token: 0x0400257A RID: 9594
		public bool headerAlwaysInteractable;

		// Token: 0x0400257B RID: 9595
		public bool paintable;

		// Token: 0x0400257C RID: 9596
		public TrainableDef trainable;

		// Token: 0x0400257D RID: 9597
		public int gap;

		// Token: 0x0400257E RID: 9598
		public WorkTypeDef workType;

		// Token: 0x0400257F RID: 9599
		public bool moveWorkTypeLabelDown;

		// Token: 0x04002580 RID: 9600
		public int widthPriority;

		// Token: 0x04002581 RID: 9601
		public int width = -1;

		// Token: 0x04002582 RID: 9602
		[Unsaved(false)]
		private PawnColumnWorker workerInt;

		// Token: 0x04002583 RID: 9603
		[Unsaved(false)]
		private Texture2D headerIconTex;

		// Token: 0x04002584 RID: 9604
		private const int IconWidth = 26;

		// Token: 0x04002585 RID: 9605
		private static readonly Vector2 IconSize = new Vector2(26f, 26f);
	}
}
