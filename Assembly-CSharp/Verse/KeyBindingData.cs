using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000869 RID: 2153
	public class KeyBindingData
	{
		// Token: 0x060035AD RID: 13741 RVA: 0x00006B8B File Offset: 0x00004D8B
		public KeyBindingData()
		{
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x00029A87 File Offset: 0x00027C87
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x00159F94 File Offset: 0x00158194
		public override string ToString()
		{
			string str = "[";
			if (this.keyBindingA != KeyCode.None)
			{
				str += this.keyBindingA.ToString();
			}
			if (this.keyBindingB != KeyCode.None)
			{
				str = str + ", " + this.keyBindingB.ToString();
			}
			return str + "]";
		}

		// Token: 0x0400255A RID: 9562
		public KeyCode keyBindingA;

		// Token: 0x0400255B RID: 9563
		public KeyCode keyBindingB;
	}
}
