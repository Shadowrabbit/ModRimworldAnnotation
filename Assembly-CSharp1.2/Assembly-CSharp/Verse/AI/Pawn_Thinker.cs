using System;

namespace Verse.AI
{
	// Token: 0x02000A68 RID: 2664
	public class Pawn_Thinker
	{
		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06003F9D RID: 16285 RVA: 0x0002FAA8 File Offset: 0x0002DCA8
		public ThinkTreeDef MainThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06003F9E RID: 16286 RVA: 0x0002FABA File Offset: 0x0002DCBA
		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeMain.thinkRoot;
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06003F9F RID: 16287 RVA: 0x0002FAD1 File Offset: 0x0002DCD1
		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06003FA0 RID: 16288 RVA: 0x0002FAE3 File Offset: 0x0002DCE3
		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.pawn.RaceProps.thinkTreeConstant.thinkRoot;
			}
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x0002FAFA File Offset: 0x0002DCFA
		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x001802FC File Offset: 0x0017E4FC
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

		// Token: 0x06003FA3 RID: 16291 RVA: 0x00180368 File Offset: 0x0017E568
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
				}), false);
			}
			return t;
		}

		// Token: 0x04002BFB RID: 11259
		public Pawn pawn;
	}
}
