using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x0200091A RID: 2330
	public abstract class SoundFilter
	{
		// Token: 0x060039B6 RID: 14774
		public abstract void SetupOn(AudioSource source);

		// Token: 0x060039B7 RID: 14775 RVA: 0x00168158 File Offset: 0x00166358
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
