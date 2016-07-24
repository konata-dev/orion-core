﻿using System;
using Microsoft.Xna.Framework;
using Orion.Interfaces;
using System.ComponentModel;

namespace Orion.Events.Npc
{
	/// <summary>
	/// Provides data for the <see cref="INpcService.NpcSpawning"/> event.
	/// </summary>
	public class NpcSpawningEventArgs : HandledEventArgs
	{
		/// <summary>
		/// Gets or sets the position of the <see cref="INpc"/> that is spawning in the NPC array.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Gets the <see cref="INpc"/> that is spawning.
		/// </summary>
		public INpc Npc { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="NpcSpawningEventArgs"/> class.
		/// </summary>
		/// <param name="index">The position of the <see cref="INpc"/> that is spawning in the NPC array.</param>
		/// <param name="npc">The <see cref="INpc"/> that is spawning.</param>
		public NpcSpawningEventArgs(int index, INpc npc)
		{
			Index = index;
			Npc = npc;
		}
	}
}
