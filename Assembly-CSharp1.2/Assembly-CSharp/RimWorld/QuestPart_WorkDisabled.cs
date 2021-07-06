using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001085 RID: 4229
	public class QuestPart_WorkDisabled : QuestPartActivable
	{
		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x06005C2B RID: 23595 RVA: 0x0003FED3 File Offset: 0x0003E0D3
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

		// Token: 0x06005C2C RID: 23596 RVA: 0x0003FEE3 File Offset: 0x0003E0E3
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x06005C2D RID: 23597 RVA: 0x0003FEF2 File Offset: 0x0003E0F2
		public override void Cleanup()
		{
			base.Cleanup();
			this.ClearPawnWorkTypesAndSkillsCache();
		}

		// Token: 0x06005C2E RID: 23598 RVA: 0x001D9D3C File Offset: 0x001D7F3C
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

		// Token: 0x06005C2F RID: 23599 RVA: 0x001D9DA8 File Offset: 0x001D7FA8
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

		// Token: 0x04003DBC RID: 15804
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003DBD RID: 15805
		public WorkTags disabledWorkTags;
	}
}
