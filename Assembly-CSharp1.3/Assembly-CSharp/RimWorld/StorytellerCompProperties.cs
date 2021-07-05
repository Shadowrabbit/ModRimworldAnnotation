using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC8 RID: 2760
	public class StorytellerCompProperties
	{
		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06004140 RID: 16704 RVA: 0x0015F19C File Offset: 0x0015D39C
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

		// Token: 0x06004141 RID: 16705 RVA: 0x0015F21C File Offset: 0x0015D41C
		public StorytellerCompProperties()
		{
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x0015F22F File Offset: 0x0015D42F
		public StorytellerCompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x0015F249 File Offset: 0x0015D449
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

		// Token: 0x06004144 RID: 16708 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ResolveReferences(StorytellerDef parentDef)
		{
		}

		// Token: 0x04002706 RID: 9990
		[TranslationHandle]
		public Type compClass;

		// Token: 0x04002707 RID: 9991
		public float minDaysPassed;

		// Token: 0x04002708 RID: 9992
		public List<IncidentTargetTagDef> allowedTargetTags;

		// Token: 0x04002709 RID: 9993
		public List<IncidentTargetTagDef> disallowedTargetTags;

		// Token: 0x0400270A RID: 9994
		public float minIncChancePopulationIntentFactor = 0.05f;

		// Token: 0x0400270B RID: 9995
		public List<string> enableIfAnyModActive;

		// Token: 0x0400270C RID: 9996
		public List<string> disableIfAnyModActive;
	}
}
