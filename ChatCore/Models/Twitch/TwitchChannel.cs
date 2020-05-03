﻿using ChatCore.Interfaces;
using ChatCore.SimpleJSON;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatCore.Models.Twitch
{
    public class TwitchChannel : IChatChannel
    {
        public string Id { get; internal set; }
        public TwitchRoomstate Roomstate { get; internal set; }

        public TwitchChannel() { }
        public TwitchChannel(string json)
        {
            JSONNode obj = JSON.Parse(json);
            if (obj.HasKey(nameof(Id))) { Id = obj[nameof(Id)].Value; }
            if (obj.HasKey(nameof(Roomstate))) { Roomstate = new TwitchRoomstate(obj["Roomstate"].ToString()); }
        }
        public JSONObject ToJson()
        {
            JSONObject obj = new JSONObject();
            obj.Add(nameof(Id), new JSONString(Id));
            obj.Add(nameof(Roomstate), Roomstate.ToJson());
            return obj;
        }
    }
}
