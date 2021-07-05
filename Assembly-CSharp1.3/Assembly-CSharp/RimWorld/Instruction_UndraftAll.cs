using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D2 RID: 5074
	public class Instruction_UndraftAll : Lesson_Instruction
	{
		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x06007B59 RID: 31577 RVA: 0x002B8383 File Offset: 0x002B6583
		protected override float ProgressPercent
		{
			get
			{
				return 1f - (float)this.DraftedPawns().Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount;
			}
		}

		// Token: 0x06007B5A RID: 31578 RVA: 0x002B83A9 File Offset: 0x002B65A9
		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		// Token: 0x06007B5B RID: 31579 RVA: 0x002B83E0 File Offset: 0x002B65E0
		public override void LessonUpdate()
		{
			foreach (Pawn pawn in this.DraftedPawns())
			{
				GenDraw.DrawArrowPointingAt(pawn.DrawPos, false);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
