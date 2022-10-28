// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;

namespace osu.Game.Online.Notifications
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EndChatRequest : SocketMessage
    {
        public EndChatRequest()
        {
            Event = "chat.end";
        }
    }
}
