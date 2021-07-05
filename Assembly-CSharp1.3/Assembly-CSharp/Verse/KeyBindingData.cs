using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C7 RID: 1223
	public class KeyBindingData
	{
		// Token: 0x06002541 RID: 9537 RVA: 0x000033AC File Offset: 0x000015AC
		public KeyBindingData()
		{
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000E87F4 File Offset: 0x000E69F4
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x000E880C File Offset: 0x000E6A0C
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

		// Token: 0x04001738 RID: 5944
		public KeyCode keyBindingA;

		// Token: 0x04001739 RID: 5945
		public KeyCode keyBindingB;
	}
}
