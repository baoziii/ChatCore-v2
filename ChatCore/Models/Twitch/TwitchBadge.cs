﻿using ChatCore.Interfaces;
using ChatCore.Utilities;

namespace ChatCore.Models.Twitch
{
	public class TwitchBadge : IChatBadge
	{
		public string Id { get; internal set; } = null!;
		public string Name { get; internal set; } = null!;
		public string Uri { get; internal set; } = null!;

		public TwitchBadge() { }
		public TwitchBadge(string json)
		{
			var obj = JSON.Parse(json);
			if (obj.TryGetKey(nameof(Id), out var id))
			{ Id = id.Value; }
			if (obj.TryGetKey(nameof(Name), out var name))
			{ Name = name.Value; }
			if (obj.TryGetKey(nameof(Uri), out var uri))
			{ Uri = uri.Value; }
		}

		public JSONObject ToJson()
		{
			var obj = new JSONObject();
			obj.Add(nameof(Id), new JSONString(Id));
			obj.Add(nameof(Name), new JSONString(Name));
			obj.Add(nameof(Uri), new JSONString(Uri));
			return obj;
		}
	}
}
