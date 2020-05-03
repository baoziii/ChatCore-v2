﻿using ChatCore.Interfaces;
using ChatCore.SimpleJSON;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatCore.Models.Twitch
{
    public class TwitchMessage : IChatMessage, ICloneable
    {
        public string Id { get; internal set; }
        public string Message { get; internal set; }
        public bool IsSystemMessage { get; internal set; }
        public bool IsActionMessage { get; internal set; }
        public bool IsHighlighted { get; internal set; }
        public bool IsPing { get; internal set; }
        public IChatUser Sender { get; internal set; }
        public IChatChannel Channel { get; internal set; }
        public IChatEmote[] Emotes { get; internal set; }
        public ReadOnlyDictionary<string, string> Metadata { get; internal set; }
        /// <summary>
        /// The IRC message type for this TwitchMessage
        /// </summary>
        public string Type { get; internal set; }
        /// <summary>
        /// The number of bits in this message, if any.
        /// </summary>
        public int Bits { get; internal set; }

        public TwitchMessage() { }
        public TwitchMessage(string json)
        {
            JSONNode obj = JSON.Parse(json);
            if (obj.HasKey(nameof(Id))) { Id = obj[nameof(Id)].Value; }
            if (obj.HasKey(nameof(IsSystemMessage))) { IsSystemMessage = obj[nameof(IsSystemMessage)].AsBool; }
            if (obj.HasKey(nameof(IsActionMessage))) { IsActionMessage = obj[nameof(IsActionMessage)].AsBool; }
            if (obj.HasKey(nameof(IsHighlighted))) { IsHighlighted = obj[nameof(IsHighlighted)].AsBool; }
            if (obj.HasKey(nameof(IsPing))) { IsPing = obj[nameof(IsPing)].AsBool; }
            if (obj.HasKey(nameof(Message))) { Message = obj[nameof(Message)].Value; }
            if (obj.HasKey(nameof(Sender))) { Sender = new TwitchUser(obj[nameof(Sender)].ToString()); }
            if (obj.HasKey(nameof(Channel))) { Channel = new TwitchChannel(obj[nameof(Channel)].ToString()); }
            if (obj.HasKey(nameof(Emotes)))
            {
                List<IChatEmote> emotes = new List<IChatEmote>();
                foreach (var emote in obj[nameof(Emotes)].AsArray)
                {
                    if(emote.Value.HasKey(nameof(IChatEmote.Id)))
                    {
                        var id = emote.Value[nameof(IChatEmote.Id)].Value;
                        if (id.StartsWith("Twitch") || id.StartsWith("BTTV") || id.StartsWith("FFZ"))
                        {
                            emotes.Add(new TwitchEmote(emote.Value.ToString()));
                        }
                        else if (id.StartsWith("Emoji"))
                        {
                            emotes.Add(new Emoji(emote.Value.ToString()));
                        }
                        else
                        {
                            emotes.Add(new UnknownChatEmote(emote.Value.ToString()));
                        }
                    }
                }
                Emotes = emotes.ToArray();
            }
            if (obj.HasKey(nameof(Type))) { Type = obj[nameof(Type)].Value; }
            if (obj.HasKey(nameof(Bits))) { Bits = obj[nameof(Bits)].AsInt; }
        }
        public JSONObject ToJson()
        {
            JSONObject obj = new JSONObject();
            obj.Add(nameof(Id), new JSONString(Id));
            obj.Add(nameof(IsSystemMessage), new JSONBool(IsSystemMessage));
            obj.Add(nameof(IsActionMessage), new JSONBool(IsActionMessage));
            obj.Add(nameof(IsActionMessage), new JSONBool(IsActionMessage));
            obj.Add(nameof(IsHighlighted), new JSONBool(IsHighlighted));
            obj.Add(nameof(IsPing), new JSONBool(IsPing));
            obj.Add(nameof(Message), new JSONString(Message));
            obj.Add(nameof(Sender), Sender.ToJson());
            obj.Add(nameof(Channel), Channel.ToJson());
            JSONArray emotes = new JSONArray();
            foreach (var emote in Emotes)
            {
                emotes.Add(emote.ToJson());
            }
            obj.Add(nameof(Emotes), emotes);
            obj.Add(nameof(Type), new JSONString(Type));
            obj.Add(nameof(Bits), new JSONNumber(Bits));
            return obj;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
