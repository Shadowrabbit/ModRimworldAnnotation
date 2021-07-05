using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F12 RID: 3858
	public class CompProperties_OrbitalBeam : CompProperties
	{
		// Token: 0x06005546 RID: 21830 RVA: 0x0003B280 File Offset: 0x00039480
		public CompProperties_OrbitalBeam()
		{
			this.compClass = typeof(CompOrbitalBeam);
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x0003B2AE File Offset: 0x000394AE
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.drawerType != DrawerType.RealtimeOnly && parentDef.drawerType != DrawerType.MapMeshAndRealTime)
			{
				yield return "orbital beam requires realtime drawer";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003668 RID: 13928
		public float width = 8f;

		// Token: 0x04003669 RID: 13929
		public Color color = Color.white;

		// Token: 0x0400366A RID: 13930
		public SoundDef sound;
	}
}
