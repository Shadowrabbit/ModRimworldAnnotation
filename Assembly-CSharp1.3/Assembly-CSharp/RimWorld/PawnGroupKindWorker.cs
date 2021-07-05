using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A9E RID: 2718
	public abstract class PawnGroupKindWorker
	{
		// Token: 0x060040A8 RID: 16552
		public abstract float MinPointsToGenerateAnything(PawnGroupMaker groupMaker, PawnGroupMakerParms parms = null);

		// Token: 0x060040A9 RID: 16553 RVA: 0x0015D98C File Offset: 0x0015BB8C
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
				Log.Error("Exception while generating pawn group: " + arg);
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

		// Token: 0x060040AA RID: 16554
		protected abstract void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true);

		// Token: 0x060040AB RID: 16555 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return true;
		}

		// Token: 0x060040AC RID: 16556
		public abstract IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker);

		// Token: 0x0400258D RID: 9613
		public PawnGroupKindDef def;

		// Token: 0x0400258E RID: 9614
		public static List<List<Pawn>> pawnsBeingGeneratedNow = new List<List<Pawn>>();
	}
}
