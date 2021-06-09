using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000F8 RID: 248
	public class CompProperties
	{
		// Token: 0x0600071A RID: 1818 RVA: 0x0000BC60 File Offset: 0x00009E60
		public CompProperties()
		{
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0000BC78 File Offset: 0x00009E78
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0000BC97 File Offset: 0x00009E97
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0000BCAE File Offset: 0x00009EAE
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			yield break;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x04000425 RID: 1061
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);
	}
}
