using System;

namespace TegraX.Core
{
    public enum CmdType
    {
        Message, Callback, All
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandType : Attribute
    {
        public CmdType Type { get; set; }
        
        public CommandType(CmdType type)
        {
            Type = type;
        }
    }
}