using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F52 RID: 3922
	public class WorldObjectCompProperties
	{
		// Token: 0x06005642 RID: 22082 RVA: 0x0003BDBB File Offset: 0x00039FBB
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}

		// Token: 0x040037A2 RID: 14242
		[TranslationHandle]
		public Type compClass = typeof(WorldObjectComp);
	}
}
