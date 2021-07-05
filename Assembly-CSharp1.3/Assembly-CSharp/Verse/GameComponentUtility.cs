using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000131 RID: 305
	public static class GameComponentUtility
	{
		// Token: 0x06000863 RID: 2147 RVA: 0x000274F8 File Offset: 0x000256F8
		public static void GameComponentUpdate()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentUpdate();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0002754C File Offset: 0x0002574C
		public static void GameComponentTick()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentTick();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x000275A0 File Offset: 0x000257A0
		public static void GameComponentOnGUI()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].GameComponentOnGUI();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x000275F4 File Offset: 0x000257F4
		public static void FinalizeInit()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].FinalizeInit();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00027648 File Offset: 0x00025848
		public static void StartedNewGame()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].StartedNewGame();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0002769C File Offset: 0x0002589C
		public static void LoadedGame()
		{
			List<GameComponent> components = Current.Game.components;
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					components[i].LoadedGame();
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}
	}
}
