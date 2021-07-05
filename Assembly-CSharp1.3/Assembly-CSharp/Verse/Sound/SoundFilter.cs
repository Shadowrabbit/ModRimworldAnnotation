using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000547 RID: 1351
	public abstract class SoundFilter
	{
		// Token: 0x0600287E RID: 10366
		public abstract void SetupOn(AudioSource source);

		// Token: 0x0600287F RID: 10367 RVA: 0x000F6D44 File Offset: 0x000F4F44
		protected static T GetOrMakeFilterOn<T>(AudioSource source) where T : Behaviour
		{
			T t = source.gameObject.GetComponent<T>();
			if (t != null)
			{
				t.enabled = true;
			}
			else
			{
				t = source.gameObject.AddComponent<T>();
			}
			return t;
		}
	}
}
