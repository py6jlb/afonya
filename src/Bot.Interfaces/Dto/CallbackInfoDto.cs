﻿using Newtonsoft.Json;

namespace Bot.Interfaces.Dto;

public class CallbackInfoDto
{
    public string Id { get; set; }
    public string Ctg { get; set; }

    public override string ToString()
    {
        var str = JsonConvert.SerializeObject(this);
        return str;
    }
}