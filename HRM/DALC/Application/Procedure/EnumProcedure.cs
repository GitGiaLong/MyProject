using System.Runtime.Serialization;

namespace DALC.Application.Procedure
{
    public enum EnumProcedure
    {
        /// <summary>
        /// ViewOrSelectQuery
        /// </summary>
        [EnumMember(Value = "ViewOrSelectQuery")] ViewOrSelectQuery,

        /// <summary>
        /// InsertValueQuery
        /// </summary>
        [EnumMember(Value = "InsertValueQuery")] InsertValueQuery,

        /// <summary>
        /// UpdateValueQuery
        /// </summary>
        [EnumMember(Value = "UpdateValueQuery")] UpdateValueQuery,

        /// <summary>
        /// DeleteQuery
        /// </summary>
        [EnumMember(Value = "DeleteQuery")] DeleteQuery,

    }
}
