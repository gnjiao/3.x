using System;

namespace Hdc.Mv.Inspection
{
    [Serializable]
    public class DataCodeSearchingResult
    {
        public int Index { get; set; }

        public string Name
        {
            get { return Definition.Name; }
            set { Definition.Name = value; }
        }

        public bool HasError { get; set; }

        public bool IsNotFound { get; set; }

        public DataCodeSearchingDefinition Definition { get; set; }

        public DataCodeSearchingResult()
        {
        }

        public DataCodeSearchingResult(int index)
        {
            Index = index;
        }

        public DataCodeSearchingResult(int index, DataCodeSearchingDefinition definition)
        {
            Index = index;
            Definition = definition;
        }

        public DataCodeSearchingResult(string name)
        {
            Name = name;
        }

        public string DecodeString { get; set; }
    }
}