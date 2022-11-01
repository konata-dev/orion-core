using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Npcs
{
    /// <summary>
    /// Equivalent of Terraria.Datastructures.NPCSpawnParams.
    /// </summary>
    public class NpcSpawnParams
    {
        /// <summary>
        /// Gets or sets a value representing the override for the size multiplier of the npc.
        /// </summary>
        public float? SizeMultiplierOverride { get; set; }

        /// <summary>
        /// Gets or sets a value representing the override for how many players this npc spawn will be scaled to.
        /// </summary>
        public int? PlayercountScaleOverride { get; set; }

        /// <summary>
        /// Gets or sets a value representing the override for how strong health and damage will be scaled to.
        /// </summary>
        public float? StrengthMultiplierOverride { get; set; }

        /// <summary>
        /// Creates a new instance with the given parameters.
        /// </summary>
        /// <param name="sizeMultiplier"></param>
        /// <param name="playerCountScale"></param>
        /// <param name="strengthMultiplier"></param>
        public NpcSpawnParams(float? sizeMultiplier, int? playerCountScale, float? strengthMultiplier)
        {
            SizeMultiplierOverride = sizeMultiplier;
            PlayercountScaleOverride = playerCountScale;
            StrengthMultiplierOverride = strengthMultiplier;
        }
    }
}
