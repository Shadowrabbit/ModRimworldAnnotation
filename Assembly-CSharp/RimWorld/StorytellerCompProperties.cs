using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FED RID: 4077
	public class StorytellerCompProperties
	{
		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x060058E6 RID: 22758 RVA: 0x001D114C File Offset: 0x001CF34C
		public bool Enabled
		{
			get
			{
				if (!this.enableIfAnyModActive.NullOrEmpty<string>())
				{
					for (int i = 0; i < this.enableIfAnyModActive.Count; i++)
					{
						if (ModsConfig.IsActive(this.enableIfAnyModActive[i]))
						{
							return true;
						}
					}
					return false;
				}
				if (!this.disableIfAnyModActive.NullOrEmpty<string>())
				{
					for (int j = 0; j < this.disableIfAnyModActive.Count; j++)
					{
						if (ModsConfig.IsActive(this.disableIfAnyModActive[j]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x060058E7 RID: 22759 RVA: 0x0003DC61 File Offset: 0x0003BE61
		public StorytellerCompProperties()
		{
		}

		// Token: 0x060058E8 RID: 22760 RVA: 0x0003DC74 File Offset: 0x0003BE74
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x060058E9 RID: 22761 RVA: 0x0003DC8E File Offset: 0x0003BE8E
		public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "a StorytellerCompProperties has null compClass.";
			}
			if (!this.enableIfAnyModActive.NullOrEmpty<string>() && !this.disableIfAnyModActive.NullOrEmpty<string>())
			{
				yield return "enableIfAnyModActive and disableIfAnyModActive can't be used simultaneously";
			}
			yield break;
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}

		// Token: 0x04003B5C RID: 15196
		[TranslationHandle]
		public Type compClass;

		// Token: 0x04003B5D RID: 15197
		public float minDaysPassed;

		// Token: 0x04003B5E RID: 15198
		public List<IncidentTargetTagDef> allowedTargetTags;

		// Token: 0x04003B5F RID: 15199
		public List<IncidentTargetTagDef> disallowedTargetTags;

		// Token: 0x04003B60 RID: 15200
		public float minIncChancePopulationIntentFactor = 0.05f;

		// Token: 0x04003B61 RID: 15201
		public List<string> enableIfAnyModActive;

		// Token: 0x04003B62 RID: 15202
		public List<string> disableIfAnyModActive;
	}
}
