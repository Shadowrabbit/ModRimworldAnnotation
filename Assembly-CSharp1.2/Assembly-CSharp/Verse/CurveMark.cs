using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007E0 RID: 2016
	public struct CurveMark
	{
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x00027DC9 File Offset: 0x00025FC9
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x00027DD1 File Offset: 0x00025FD1
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x060032CD RID: 13005 RVA: 0x00027DD9 File Offset: 0x00025FD9
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x00027DE1 File Offset: 0x00025FE1
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x04002307 RID: 8967
		private float x;

		// Token: 0x04002308 RID: 8968
		private string message;

		// Token: 0x04002309 RID: 8969
		private Color color;
	}
}
