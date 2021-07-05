using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000097 RID: 151
	public class CompProperties
	{
		// Token: 0x06000525 RID: 1317 RVA: 0x0001A81F File Offset: 0x00018A1F
		public CompProperties()
		{
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001A837 File Offset: 0x00018A37
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001A856 File Offset: 0x00018A56
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001A86D File Offset: 0x00018A6D
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			yield break;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x0400025A RID: 602
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);
	}
}
