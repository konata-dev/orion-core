﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Orion.Players.Events;
using OTAPI;

namespace Orion.Players {
    internal sealed class OrionPlayerService : OrionService, IPlayerService {
        private readonly IList<Terraria.Player> _terrariaPlayers;
        private readonly IList<OrionPlayer> _players;
        
        [ExcludeFromCodeCoverage]
        public override string Author => "Pryaxis";
        
        [ExcludeFromCodeCoverage]
        public override string Name => "Orion Player Service";
        
        /*
         * We need to subtract 1 from the count. This is because Terraria actually has an extra slot which is reserved
         * as a failure index.
         */
        public int Count => _players.Count - 1;

        public IPlayer this[int index] {
            get {
                if (index < 0 || index >= Count) {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                if (_players[index]?.Wrapped != _terrariaPlayers[index]) {
                    _players[index] = new OrionPlayer(_terrariaPlayers[index]);
                }

                var player = _players[index];
                Debug.Assert(player != null, $"{nameof(player)} should not be null.");
                Debug.Assert(player.Wrapped != null, $"{nameof(player.Wrapped)} should not be null.");
                return player;
            }
        }

        public event EventHandler<GreetingPlayerEventArgs> GreetingPlayer;
        public event EventHandler<UpdatingPlayerEventArgs> UpdatingPlayer;
        public event EventHandler<UpdatedPlayerEventArgs> UpdatedPlayer;

        public OrionPlayerService() {
            _terrariaPlayers = Terraria.Main.player;
            _players = new OrionPlayer[_terrariaPlayers.Count];
            
            Hooks.Player.PreGreet = PreGreetHandler;
            Hooks.Player.PreUpdate = PreUpdateHandler;
            Hooks.Player.PostUpdate = PostUpdateHandler;
        }

        protected override void Dispose(bool disposeManaged) {
            if (!disposeManaged) {
                return;
            }

            Hooks.Player.PreGreet = null;
            Hooks.Player.PreUpdate = null;
            Hooks.Player.PostUpdate = null;
        }

        public IEnumerator<IPlayer> GetEnumerator() {
            for (var i = 0; i < Count; ++i) {
                yield return this[i];
            }
        }
        
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        private HookResult PreGreetHandler(ref int playerIndex) {
            Debug.Assert(playerIndex >= 0 && playerIndex < Count, $"{nameof(playerIndex)} should be a valid index.");

            var player = this[playerIndex];
            var args = new GreetingPlayerEventArgs(player);
            GreetingPlayer?.Invoke(this, args);

            return args.Handled ? HookResult.Cancel : HookResult.Continue;
        }

        private HookResult PreUpdateHandler(Terraria.Player terrariaPlayer, ref int playerIndex) {
            Debug.Assert(playerIndex >= 0 && playerIndex < Count, $"{nameof(playerIndex)} should be a valid index.");

            var player = this[playerIndex];
            var args = new UpdatingPlayerEventArgs(player);
            UpdatingPlayer?.Invoke(this, args);

            return args.Handled ? HookResult.Cancel : HookResult.Continue;
        }

        private void PostUpdateHandler(Terraria.Player terrariaPlayer, int playerIndex) {
            Debug.Assert(playerIndex >= 0 && playerIndex < Count, $"{nameof(playerIndex)} should be a valid index.");

            var player = this[playerIndex];
            var args = new UpdatedPlayerEventArgs(player);
            UpdatedPlayer?.Invoke(this, args);
        }
    }
}
