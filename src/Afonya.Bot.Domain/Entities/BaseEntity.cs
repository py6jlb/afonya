using LiteDB;
#pragma warning disable CS8618

namespace Afonya.Bot.Domain.Entities;

/// <summary>
/// Базовая сущность
/// </summary>
public class BaseEntity
{
    protected BaseEntity() { }

    ObjectId _id;

    /// <summary>
    /// Получить или присвоить идентификатор сущности
    /// </summary>
    public virtual ObjectId Id
    {
        get => _id;
        protected set => _id = value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as BaseEntity);
    }

    private static bool IsTransient(BaseEntity obj)
    {
        return obj != null && Equals(obj.Id, default(ObjectId));
    }

    private Type GetUnproxiedType()
    {
        return GetType();
    }

    public virtual bool Equals(BaseEntity? other)
    {
        if (other == null) return false;

        if (ReferenceEquals(this, other)) return true;

        if (IsTransient(this) || IsTransient(other) || !Equals(Id, other.Id)) return false;

        var otherType = other.GetUnproxiedType();
        var thisType = GetUnproxiedType();
        return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
    }

    public override int GetHashCode()
    {
        if (Equals(Id, default(ObjectId))) return base.GetHashCode();
        return Id.GetHashCode();
    }

    public static bool operator ==(BaseEntity? x, BaseEntity? y)
    {
        return Equals(x, y);
    }

    public static bool operator !=(BaseEntity? x, BaseEntity? y)
    {
        return !(x == y);
    }

}