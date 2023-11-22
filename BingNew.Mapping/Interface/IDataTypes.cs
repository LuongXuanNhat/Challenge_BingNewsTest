using Newtonsoft.Json.Linq;
using System;
namespace BingNew.Mapping.Interface
{
    public interface IDataTypes
    {
       string DataType { get; }
    }

    public abstract class DataTypes : IDataTypes
    {
        public abstract string DataType { get; }
    }
    public class Text : DataTypes 
    {
        public override string DataType => "string";
    }
    public class Number : DataTypes 
    {
        public override string DataType => "int";
    }
}
