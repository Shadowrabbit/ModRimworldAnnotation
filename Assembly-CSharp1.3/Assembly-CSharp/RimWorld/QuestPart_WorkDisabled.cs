using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B51 RID: 2897
	public class QuestPart_WorkDisabled : QuestPartActivable
	{
		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060043D0 RID: 17360 RVA: 0x00168DA3 File Offset: 0x00166FA3
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (base.State == QuestPartState.Enabled)
				{
					List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
					int num;
					for (int i = 0; i < list.Count; i = num + 1)
					{
						if ((this.disabledWorkTags & list[i].workTags) != WorkTags.None)
						{
							yield return list[i];
						}
						num = i;
					}
					list = null;
				}
				yield break;
			}
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x00168DB3 File Offset: 0x00166FB3
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x00168DC2 File Offset: 0x00166FC2
		public override void Cleanup()
		{
			base.Cleanup();
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x00168DD0 File Offset: 0x00166FD0
		private void ClearPawnWorkTypesAndSkillsCache()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i] != null)
				{
					this.pawns[i].Notify_DisabledWorkTypesChanged();
					if (this.pawns[i].skills != null)
					{
						this.pawns[i].skills.Notify_SkillDisablesChanged();
					}
				}
			}
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x00168E3C File Offset: 0x0016703C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<WorkTags>(ref this.disabledWorkTags, "disabledWorkTags", WorkTags.None, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04002924 RID: 10532
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04002925 RID: 10533
		public WorkTags disabledWorkTags;
	}
}
