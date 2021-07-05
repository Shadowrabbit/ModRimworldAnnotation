using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000480 RID: 1152
	public struct CurveMark
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x000DBF7F File Offset: 0x000DA17F
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x000DBF87 File Offset: 0x000DA187
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x000DBF8F File Offset: 0x000DA18F
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000DBF97 File Offset: 0x000DA197
		public CurveMark(float x, string message, Color color)
		{
			this.x = x;
			this.message = message;
			this.color = color;
		}

		// Token: 0x040015DE RID: 5598
		private float x;

		// Token: 0x040015DF RID: 5599
		private string message;

		// Token: 0x040015E0 RID: 5600
		private Color color;
	}
}
