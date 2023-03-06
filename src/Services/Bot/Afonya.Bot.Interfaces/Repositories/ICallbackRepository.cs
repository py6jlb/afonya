﻿using Afonya.Bot.Domain.Entities;

namespace Afonya.Bot.Interfaces.Repositories;

public interface ICallbackRepository
{
    public IReadOnlyCollection<Callback>? GetGroup(Guid groupId);
    public Callback? Get(string id);
    Callback Create(Callback callback);
    bool Delete(string id);
}