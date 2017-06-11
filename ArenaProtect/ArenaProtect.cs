using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace ArenaProtect
{
    public class ArenaProtect : RocketPlugin<Configuration>
    {
        private DateTime startTime = DateTime.Now;
        private bool Finished = true;

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "protected", "You have been protected for {0} seconds by arena spawn protection!" },
            { "unprotected", "You are no longer protected by arena spawn protection!" }
        };

        public override void LoadPlugin()
        {
            LevelManager.onLevelNumberUpdated += new LevelNumberUpdated(OnPlayersAmount);
        }

        private void OnPlayersAmount(int players)
        {
            if (players != Provider.clients.Count)
                return;

            foreach (ArenaPlayer player in LevelManager.arenaPlayers)
            {
                UnturnedPlayer uPlayer = UnturnedPlayer.FromSteamPlayer(player.steamPlayer);

                if (Configuration.Instance.GodMode)
                    uPlayer.GodMode = true;
                if (Configuration.Instance.Vanish)
                    uPlayer.VanishMode = true;

                ChatManager.say(player.steamPlayer.playerID.steamID, Translate("protected", Configuration.Instance.Seconds), Configuration.Instance.color);
            }
            startTime = DateTime.Now;
            Finished = false;
        }

        void Update()
        {
            if (Finished)
                return;

            if((DateTime.Now - startTime).TotalSeconds >= Configuration.Instance.Seconds)
            {
                foreach (ArenaPlayer player in LevelManager.arenaPlayers)
                {
                    UnturnedPlayer uPlayer = UnturnedPlayer.FromSteamPlayer(player.steamPlayer);

                    uPlayer.GodMode = false;
                    uPlayer.VanishMode = false;

                    ChatManager.say(player.steamPlayer.playerID.steamID, Translate("unprotected"), Configuration.Instance.color);
                }
                Finished = true;
            }
        }
    }
}
