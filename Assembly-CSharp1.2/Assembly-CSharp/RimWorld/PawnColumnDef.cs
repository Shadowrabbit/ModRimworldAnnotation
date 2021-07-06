using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBE RID: 4030
	public class PawnColumnDef : Def
	{
		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06005814 RID: 22548 RVA: 0x0003D26A File Offset: 0x0003B46A
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

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06005815 RID: 22549 RVA: 0x0003D29C File Offset: 0x0003B49C
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

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06005816 RID: 22550 RVA: 0x001CF514 File Offset: 0x001CD714
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

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06005817 RID: 22551 RVA: 0x0003D2D1 File Offset: 0x0003B4D1
		public bool HeaderInteractable
		{
			get
			{
				return this.sortable || !this.headerTip.NullOrEmpty() || this.headerAlwaysInteractable;
			}
		}

		// Token: 0x04003A1B RID: 14875
		public Type workerClass = typeof(PawnColumnWorker);

		// Token: 0x04003A1C RID: 14876
		public bool sortable;

		// Token: 0x04003A1D RID: 14877
		public bool ignoreWhenCalculatingOptimalTableSize;

		// Token: 0x04003A1E RID: 14878
		[NoTranslate]
		public string headerIcon;

		// Token: 0x04003A1F RID: 14879
		public Vector2 headerIconSize;

		// Token: 0x04003A20 RID: 14880
		[MustTranslate]
		public string headerTip;

		// Token: 0x04003A21 RID: 14881
		public bool headerAlwaysInteractable;

		// Token: 0x04003A22 RID: 14882
		public bool paintable;

		// Token: 0x04003A23 RID: 14883
		public TrainableDef trainable;

		// Token: 0x04003A24 RID: 14884
		public int gap;

		// Token: 0x04003A25 RID: 14885
		public WorkTypeDef workType;

		// Token: 0x04003A26 RID: 14886
		public bool moveWorkTypeLabelDown;

		// Token: 0x04003A27 RID: 14887
		public int widthPriority;

		// Token: 0x04003A28 RID: 14888
		public int width = -1;

		// Token: 0x04003A29 RID: 14889
		[Unsaved(false)]
		private PawnColumnWorker workerInt;

		// Token: 0x04003A2A RID: 14890
		[Unsaved(false)]
		private Texture2D headerIconTex;

		// Token: 0x04003A2B RID: 14891
		private const int IconWidth = 26;

		// Token: 0x04003A2C RID: 14892
		private static readonly Vector2 IconSize = new Vector2(26f, 26f);
	}
}
