namespace BuildingBlocks.Domain;

public interface IBusinessRule
{
    Task<bool> IsBroken();
    string Message { get; }
}