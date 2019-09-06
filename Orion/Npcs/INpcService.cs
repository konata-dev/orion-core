﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Orion.Npcs.Events;

namespace Orion.Npcs {
    /// <summary>
    /// Provides a mechanism for managing NPCs.
    /// </summary>
    public interface INpcService : IReadOnlyList<INpc>, IService {
        /// <summary>
        /// Gets or sets the base NPC spawning rate.
        /// </summary>
        int BaseNpcSpawningRate { get; set; }

        /// <summary>
        /// Gets or sets the base NPC spawning limit.
        /// </summary>
        int BaseNpcSpawningLimit { get; set; }

        /// <summary>
        /// Occurs when an NPC is spawning.
        /// </summary>
        event EventHandler<SpawningNpcEventArgs> SpawningNpc;

        /// <summary>
        /// Occurs when an NPC spawned.
        /// </summary>
        event EventHandler<SpawnedNpcEventArgs> SpawnedNpc;

        /// <summary>
        /// Occurs when an NPC is having its defaults set.
        /// </summary>
        event EventHandler<SettingNpcDefaultsEventArgs> SettingNpcDefaults;

        /// <summary>
        /// Occurs when an NPC had its defaults set.
        /// </summary>
        event EventHandler<SetNpcDefaultsEventArgs> SetNpcDefaults;

        /// <summary>
        /// Occurs when an NPC is being updated.
        /// </summary>
        event EventHandler<UpdatingNpcEventArgs> UpdatingNpc;

        /// <summary>
        /// Occurs when an NPC's AI is being updated.
        /// </summary>
        event EventHandler<UpdatingNpcEventArgs> UpdatingNpcAi;

        /// <summary>
        /// Occurs when an NPC's AI is updated.
        /// </summary>
        event EventHandler<UpdatedNpcEventArgs> UpdatedNpcAi;

        /// <summary>
        /// Occurs when an NPC is updated.
        /// </summary>
        event EventHandler<UpdatedNpcEventArgs> UpdatedNpc;

        /// <summary>
        /// Occurs when an NPC is being damaged.
        /// </summary>
        event EventHandler<DamagingNpcEventArgs> DamagingNpc;

        /// <summary>
        /// Occurs when an NPC is damaged.
        /// </summary>
        event EventHandler<DamagedNpcEventArgs> DamagedNpc;

        /// <summary>
        /// Occurs when an NPC is transforming to another type.
        /// </summary>
        event EventHandler<NpcTransformingEventArgs> NpcTransforming;

        /// <summary>
        /// Occurs when an NPC has transformed to another type.
        /// </summary>
        event EventHandler<NpcTransformedEventArgs> NpcTransformed;

        /// <summary>
        /// Occurs when an NPC is dropping a loot item.
        /// </summary>
        event EventHandler<NpcDroppingLootItemEventArgs> NpcDroppingLootItem;

        /// <summary>
        /// Occurs when an NPC has dropped a loot item.
        /// </summary>
        event EventHandler<NpcDroppedLootItemEventArgs> NpcDroppedLootItem;

        /// <summary>
        /// Occurs when an NPC was killed.
        /// </summary>
        event EventHandler<KilledNpcEventArgs> KilledNpc;

        /// <summary>
        /// Spawns an NPC with the specified type at the position with the AI values.
        /// </summary>
        /// <param name="type">The NPC type.</param>
        /// <param name="position">The position.</param>
        /// <param name="aiValues">
        /// The AI values to use, or <c>null</c> for none. If not <c>null</c>, this should have length 4.
        /// </param>
        /// <returns>The resulting NPC, or <c>null</c> if none was spawned.</returns>
        /// <exception cref="ArgumentException"><paramref name="aiValues"/> does not have length 4.</exception>
        INpc SpawnNpc(NpcType type, Vector2 position, float[] aiValues = null);
    }
}
