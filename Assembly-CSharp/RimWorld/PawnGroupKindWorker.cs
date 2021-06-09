using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC1 RID: 4033
	public abstract class PawnGroupKindWorker
	{
		// Token: 0x0600582B RID: 22571
		public abstract float MinPointsToGenerateAnything(PawnGroupMaker groupMaker);

		// Token: 0x0600582C RID: 22572 RVA: 0x001CF9CC File Offset: 0x001CDBCC
		public List<Pawn> GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, bool errorOnZeroResults = true)
		{
			List<Pawn> list = new List<Pawn>();
			PawnGroupKindWorker.pawnsBeingGeneratedNow.Add(list);
			try
			{
				this.GeneratePawns(parms, groupMaker, list, errorOnZeroResults);
			}
			catch (Exception arg)
			{
				Log.Error("Exception while generating pawn group: " + arg, false);
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Destroy(DestroyMode.Vanish);
				}
				list.Clear();
			}
			finally
			{
				PawnGroupKindWorker.pawnsBeingGeneratedNow.Remove(list);
			}
			return list;
		}

		// Token: 0x0600582D RID: 22573
		protected abstract void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true);

		// Token: 0x0600582E RID: 22574 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return true;
		}

		// Token: 0x0600582F RID: 22575
		public abstract IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker);

		// Token: 0x04003A34 RID: 14900
		public PawnGroupKindDef def;

		// Token: 0x04003A35 RID: 14901
		public static List<List<Pawn>> pawnsBeingGeneratedNow = new List<List<Pawn>>();
	}
}
