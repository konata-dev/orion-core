﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Orion.World.Chests {
    /// <summary>
    /// Orion's implementation of <see cref="IChestService"/>.
    /// </summary>
    internal sealed class OrionChestService : OrionService, IChestService {
        private readonly IList<Terraria.Chest> _terrariaChests;
        private readonly IList<OrionChest> _chests;
        
        [ExcludeFromCodeCoverage]
        public override string Author => "Pryaxis";

        [ExcludeFromCodeCoverage]
        public override string Name => "Orion Chest Service";

        public int Count => _chests.Count;

        public IChest this[int index] {
            get {
                if (index < 0 || index >= Count) {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                /*
                 * Some chests in _terrariaChests may be null, so we need to handle this properly by also returning
                 * null.
                 */
                if (_chests[index] == null || _chests[index].Wrapped != _terrariaChests[index]) {
                    if (_terrariaChests[index] == null) {
                        return null;
                    } else {
                        _chests[index] = new OrionChest(_terrariaChests[index]);
                    }
                }

                var chest = _chests[index];
                Debug.Assert(chest != null, $"{nameof(chest)} should not be null.");
                Debug.Assert(chest.Wrapped != null, $"{nameof(chest.Wrapped)} should not be null.");
                return chest;
            }
        }

        public OrionChestService() {
            _terrariaChests = Terraria.Main.chest;
            _chests = new OrionChest[_terrariaChests.Count];
        }

        public IEnumerator<IChest> GetEnumerator() {
            for (var i = 0; i < Count; ++i) {
                yield return this[i];
            }
        }
        
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
