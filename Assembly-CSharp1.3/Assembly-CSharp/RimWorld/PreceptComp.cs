using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EDC RID: 3804
	public abstract class PreceptComp
	{
		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x06005A74 RID: 23156 RVA: 0x001F4E0C File Offset: 0x001F300C
		public virtual IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
		{
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberWitnessedAction(HistoryEvent ev, Precept precept, Pawn member)
		{
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_MemberGenerated(Pawn pawn, Precept precept)
		{
		}

		// Token: 0x06005A78 RID: 23160 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_AddBedThoughts(Pawn pawn, Precept precept)
		{
		}

		// Token: 0x06005A79 RID: 23161 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool MemberWillingToDo(HistoryEvent ev)
		{
			return true;
		}

		// Token: 0x06005A7A RID: 23162 RVA: 0x001F4E15 File Offset: 0x001F3015
		public virtual IEnumerable<string> GetDescriptions()
		{
			yield return this.description;
			yield break;
		}

		// Token: 0x06005A7B RID: 23163 RVA: 0x001F4E25 File Offset: 0x001F3025
		public virtual IEnumerable<string> ConfigErrors(PreceptDef parent)
		{
			yield break;
		}

		// Token: 0x040034FB RID: 13563
		[MustTranslate]
		public string description;
	}
}
