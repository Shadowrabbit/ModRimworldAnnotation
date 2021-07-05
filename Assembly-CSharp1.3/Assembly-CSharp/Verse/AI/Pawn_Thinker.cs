using System;

namespace Verse.AI
{
	// Token: 0x0200060F RID: 1551
	public class Pawn_Thinker
	{
		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002CD6 RID: 11478 RVA: 0x0010D7C2 File Offset: 0x0010B9C2
		public ThinkTreeDef MainThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002CD7 RID: 11479 RVA: 0x0010D7D4 File Offset: 0x0010B9D4
		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain.thinkRoot;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002CD8 RID: 11480 RVA: 0x0010D7EB File Offset: 0x0010B9EB
		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002CD9 RID: 11481 RVA: 0x0010D7FD File Offset: 0x0010B9FD
		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant.thinkRoot;
			}
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x0010D814 File Offset: 0x0010BA14
		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x0010D824 File Offset: 0x0010BA24
		public T TryGetMainTreeThinkNode<T>() where T : ThinkNode
		{
			foreach (ThinkNode thinkNode in this.MainThinkNodeRoot.ChildrenRecursive)
			{
				T t = thinkNode as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x0010D890 File Offset: 0x0010BA90
		public T GetMainTreeThinkNode<T>() where T : ThinkNode
		{
			T t = this.TryGetMainTreeThinkNode<T>();
			if (t == null)
			{
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" looked for ThinkNode of type ",
					typeof(T),
					" and didn't find it."
				}));
			}
			return t;
		}

		// Token: 0x04001BA2 RID: 7074
		public Pawn pawn;
	}
}
