using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A36 RID: 2614
	public class WorldObjectCompProperties
	{
		// Token: 0x06003F59 RID: 16217 RVA: 0x00158BC7 File Offset: 0x00156DC7
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}

		// Token: 0x040022D9 RID: 8921
		[TranslationHandle]
		public Type compClass = typeof(WorldObjectComp);
	}
}
