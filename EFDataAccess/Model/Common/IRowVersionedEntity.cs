namespace EFDataAccess.Model.Common;

public interface IRowVersionedEntity
{
    byte[] _RowVersion{ get; set; }
}