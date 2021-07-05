using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;

namespace Verse
{
	// Token: 0x0200008B RID: 139
	public class CostListForDifficulty
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00019F06 File Offset: 0x00018106
		public bool Applies
		{
			get
			{
				if (Find.Storyteller == null)
				{
					return false;
				}
				if (this.cachedDifficulty != Find.Storyteller.difficulty)
				{
					this.RecacheApplies();
				}
				return this.cachedApplies;
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00019F30 File Offset: 0x00018130
		public void RecacheApplies()
		{
			this.cachedDifficulty = Find.Storyteller.difficulty;
			if (this.difficultyVar.NullOrEmpty())
			{
				this.cachedApplies = false;
				return;
			}
			FieldInfo field = typeof(Difficulty).GetField(this.difficultyVar, BindingFlags.Instance | BindingFlags.Public);
			this.cachedApplies = (bool)field.GetValue(this.cachedDifficulty);
			if (this.invert)
			{
				this.cachedApplies = !this.cachedApplies;
			}
		}

		// Token: 0x040001E4 RID: 484
		public string difficultyVar;

		// Token: 0x040001E5 RID: 485
		public List<ThingDefCountClass> costList;

		// Token: 0x040001E6 RID: 486
		public int costStuffCount;

		// Token: 0x040001E7 RID: 487
		public bool invert;

		// Token: 0x040001E8 RID: 488
		private bool cachedApplies;

		// Token: 0x040001E9 RID: 489
		private Difficulty cachedDifficulty;
	}
}
