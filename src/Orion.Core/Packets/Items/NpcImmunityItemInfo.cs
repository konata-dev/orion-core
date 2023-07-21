using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Orion.Core.Items;
using Orion.Core.Utils;

namespace Orion.Core.Packets.Items
{
    /// <summary>
    /// A packet sent to set an npc immune item's information.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 26)]
    public struct NpcImmunityItemInfo : IPacket
    {
        [FieldOffset(0)] private byte _bytes;

        /// <summary>
        /// Gets or sets the item index. A value of <c>400</c> in <see cref="PacketContext.Server"/> indicates that the
        /// item is being spawned.
        /// </summary>
        /// <value>The item index.</value>
        [field: FieldOffset(0)] public short ItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the item's position.
        /// </summary>
        /// <value>The item's position.</value>
        [field: FieldOffset(2)] public Vector2f Position { get; set; }

        /// <summary>
        /// Gets or sets the item's velocity.
        /// </summary>
        /// <value>The item's velocity.</value>
        [field: FieldOffset(10)] public Vector2f Velocity { get; set; }

        /// <summary>
        /// Gets or sets the item's stack size.
        /// </summary>
        /// <value>The item's stack size.</value>
        [field: FieldOffset(18)] public short StackSize { get; set; }

        /// <summary>
        /// Gets or sets the item's prefix.
        /// </summary>
        /// <value>The item's prefix.</value>
        [field: FieldOffset(20)] public ItemPrefix Prefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player who spawned the item can pick up the item immediately.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the player who spawned the item can pick up the item immediately; otherwise,
        /// <see langword="false"/>.
        /// </value>
        [field: FieldOffset(21)] public bool AllowSelfPickup { get; set; }

        /// <summary>
        /// Gets or sets the item's ID. A value of <see cref="ItemId.None"/> in <see cref="PacketContext.Server"/>
        /// indicates that the item is being removed.
        /// </summary>
        /// <value>The item's ID.</value>
        [field: FieldOffset(22)] public ItemId Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how long this item is blocked from being picked up by NPCs.
        /// </summary>
        [field: FieldOffset(24)] public byte NpcImmunityTime { get; set; } 

        PacketId IPacket.Id => PacketId.NpcImmunityItemInfo;

        int IPacket.ReadBody(Span<byte> span, PacketContext context) => span.Read(ref _bytes, 25);

        int IPacket.WriteBody(Span<byte> span, PacketContext context) => span.Write(ref _bytes, 25);
    }
}
